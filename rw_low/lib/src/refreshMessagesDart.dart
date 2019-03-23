import 'package:path/path.dart' as p;
import 'package:tuple/tuple.dart';
//https://pub.dartlang.org/packages/recase
import 'package:recase/recase.dart' show ReCase;
import 'fileSystem.dart';
import 'linq.dart';

// generate C:\rewise\protobuf\compiler\include\rewise\fragment.cmd
void refreshGenCmd() {
  final relFiles =
      fileSystem.lowRoot.list(relTo: 'include', from: r'include\rewise', regExp: r'\.proto$');
  final lines = relFiles.map((f) => ' ${p.withoutExtension(f)}^');
  fileSystem.lowRoot.writeAsLines(r'fragment.cmd', lines);
}

// generate content of C:\rewise\dlibs\utils\lib\rw\ dir (except the client.dart
void generateMessagesExports() {
  final relFiles = fileSystem.utilsRoot
      .list(from: r'lib\src\messages', relTo: 'lib')
      .map((f) => Tuple2(f, p.split(f)));
  //var x = List<Tuple2<String, List<String>>>.from(relFiles);
  final grps =
      Linq.group<Tuple2<String, List<String>>, String, String>(relFiles, (t) {
    if (t.item2[2] == 'google') return 'google';
    if (t.item2.length == 4) return 'rewise';
    return t.item2[3];
  }, valuesAs: (t) => t.item1);
  for (final grp in grps) {
    final lines = grp.values.map((f) =>
        "export 'package:rw_utils/${f.replaceAll(RegExp(r'\\'), '/')}';");
    fileSystem.utilsRoot.writeAsLines('lib\\dom\\${grp.key}.dart', lines);
  }
}

// generate content of C:\rewise\dlibs\utils\lib\rw\client.dart
void refreshServicesCSharp() {
  final relFiles = List<Tuple2<String, List<String>>>.from(fileSystem.lowRoot
      .list(relTo: r'include\rewise', from: r'include\rewise')
      .map((f) => Tuple2(f, p.split(f))));
  // group .proto by directory
  final grps =
      Linq.group<Tuple2<String, List<String>>, String, String>(relFiles, (t) {
    if (t.item2.length < 2) return '';
    return t.item2[0];
  }, valuesAs: (t) => t.item1);
  // parse .proto
  final services = List<_Service>();
  for (final grp in grps) {
    if (grp.key == '') continue;
    services.addAll(grp.values.map((f) {
      final lines = fileSystem.lowRoot.readAsLines(r'include\rewise\' + f);
      return _parseProtoFile(lines);
    }).where((s) => s != null));
  }
  // generate client.dart
  final cont = StringBuffer();
  cont.write(_constImport);
  for (final pars in services)
    for (final serv in pars.services)
      cont.writeln(_codeMask(serv, pars.pascalCase));
  fileSystem.utilsRoot
      .writeAsString(r'lib\client.dart', cont.toString());
}

//*******       PRIVATE     ***************/

final _constImport = '''
//***** generated code
import 'package:rw_utils/utils.dart' show getHost, MakeRequest;
import 'package:rw_utils/dom/google.dart' as Google;
import 'package:rw_utils/dom/hack_json.dart' as HackJson;
import 'package:rw_utils/dom/hallo_world.dart' as HalloWorld;
import 'package:rw_utils/dom/to_raw.dart' as ToRaw;
import 'package:rw_utils/dom/word_breaking.dart' as WordBreaking;
import 'package:rw_utils/dom/stemming.dart' as Stemming;

''';
//import 'utils.dart' as Utils;

String _codeMask(_Request req, String namespace) => '''
Future<${req.response}> ${namespace}_${req.name}(${req.request} request) => 
  MakeRequest<${req.response}>(
      (channel) => ${namespace}.CSharpServiceClient(channel).${req.cammelCase}(request),
      getHost('${namespace}'));
''';

class _Service {
  String snakeCase; // to_raw
  String pascalCase; // ToRaw
  final services = List<_Request>();
}

class _Request {
  _Request(String namespace, this.name, this.request, this.response) {
    response = _finishType(namespace, response);
    request = _finishType(namespace, request);
    cammelCase = ReCase(name).camelCase;
  }
  String _finishType(String namespace, String rr) {
    if (rr.indexOf('.') < 0) return namespace + '.' + rr;
    for(final key in dotMap)
      if (rr.startsWith(key.item1)) return key.item2 + rr.substring(key.item1.length);
    throw Exception('Wrong type: $rr');
  }
  String name;
  String cammelCase;
  String response;
  String request;
}

final dotMap = <Tuple2<String,String>>[
  Tuple2('rw.common.', 'Utils.'),
  Tuple2('google.protobuf.', 'Google.')
];

_Service _parseProtoFile(List<String> lines) {
  final res = _Service();
  bool inService = false;
  bool hasService = false;
  for (final line in lines) {
    if (line.startsWith('package rw.')) {
      res.snakeCase = line.substring('package rw.'.length).split(';')[0];
      res.pascalCase = ReCase(res.snakeCase).pascalCase;
    }
    if (line.startsWith('service CSharpService {')) {
      hasService = true;
      inService = true;
      continue;
    }
    if (!inService) continue;
    if (line.startsWith('}')) break;
    // parse etc. 'rpc HackToJson (HackJsonBytes) returns (x.y.HackJsonString)'
    final match = _callRequestRx.firstMatch(line);
    res.services.add(_Request(
        res.pascalCase, match.group(1), match.group(2), match.group(4)));
  }
  return hasService ? res : null;
}

// parse etc. 'rpc HackToJson (HackJsonBytes) returns (x.y.HackJsonString)'
final _callRequestRx = RegExp(
    r'^\s\srpc\s(\w*)\s\(((\w|\.)*)\)\sreturns\s\(((\w|\.)*)\)',
    caseSensitive: false);
