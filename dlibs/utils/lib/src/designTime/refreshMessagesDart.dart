import 'package:path/path.dart' as p;
import 'package:tuple/tuple.dart';
//https://pub.dartlang.org/packages/recase
import 'package:recase/recase.dart' show ReCase;
import 'package:rewise_low_utils/utils.dart';
import 'fileSystem.dart';


// generate C:\rewise\protobuf\compiler\include\rewise\fragment.cmd
void refreshGenCmd() {
  final relFiles =
      fileSystem.protobufs.list(from: 'rewise', regExp: r'\.proto$');
  final lines = relFiles.map((f) => ' ${p.withoutExtension(f)}^');
  fileSystem.protobufs.writeAsLines(r'rewise/fragment.cmd', lines);
}

// generate content of C:\rewise\dlibs\utils\lib\rw\ dir (except the client.dart
void generateMessagesExports() {
  final relFiles = fileSystem.codeDartUtils
      .list(from: 'lib/src/messages', relTo: 'lib')
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
        "export 'package:rewise_low_utils/${f.replaceAll(RegExp(r'\\'), '/')}';");
    fileSystem.codeDartUtils.writeAsLines('lib\\rw\\${grp.key}.dart', lines);
  }
}

// generate content of C:\rewise\dlibs\utils\lib\rw\client.dart
void refreshServicesCSharp() {
  final relFiles = List<Tuple2<String, List<String>>>.from(fileSystem.protobufs
      .list(from: 'rewise', relTo: 'rewise')
      .map((f) => Tuple2(f, p.split(f))));
  //var x = List<Tuple2<String, List<String>>>.from(relFiles);
  final grps =
      Linq.group<Tuple2<String, List<String>>, String, String>(relFiles, (t) {
    if (t.item2.length < 2) return '';
    return t.item2[0];
  }, valuesAs: (t) => t.item1);
  final services = List<_Service>();
  for (final grp in grps) {
    if (grp.key == '') continue;
    services.addAll(grp.values.map((f) {
      final lines = fileSystem.protobufs.readAsLines(r'rewise\' + f);
      return _parseProtoFile(lines);
    }).where((s) => s != null));
  }
  final cont = StringBuffer();
  cont.write(_constImport);
  for (final imp
      in Set<String>.from(Linq.selectMany(services, (_Service s) => s.imports)))
    cont.writeln(imp);
  for (final pars in services) cont.writeln(_importMask(pars));
  cont.writeln();
  for (final pars in services)
    for (final serv in pars.services)
      cont.writeln(_codeMask(serv, pars.pascalCase));
  fileSystem.codeDartUtils.writeAsString('lib\\rw\\client.dart', cont.toString());
}

//*******       PRIVATE     ***************/

final _constImport = '''
//***** generated code
import 'package:rewise_low_utils/utils.dart' show getHost, MakeRequest;
''';

String _importMask(_Service parsed) =>
    "import '${parsed.snakeCase}.dart' as ${parsed.pascalCase};";
//    "import 'package:rewise_low_utils/rw/${parsed.snakeCase}.dart' as ${parsed.pascalCase};";

String _codeMask(_Request req, String namespace) => '''
Future<${req.response}> ${namespace}_${req.name}(${req.request} request) => 
  MakeRequest<${req.response}>(
      (channel) => ${namespace}.CSharpServiceClient(channel).${req.cammelCase}(request),
      getHost('${namespace}'));
''';

class _Service {
  String snakeCase; // to_raw
  String pascalCase; // ToRaw
  final imports = List<String>();
  final services = List<_Request>();
}

class _Request {
  _Request(String namespace, this.name, this.request, this.response) {
    if (response.indexOf('.') < 0)
      response = namespace + '.' + response;
    if (request.indexOf('.') < 0) request = namespace + '.' + request;
    cammelCase = ReCase(name).camelCase;
  }
  String name;
  String cammelCase;
  String response;
  String request;
}

_Service _parseProtoFile(List<String> lines) {
  final res = _Service();
  bool inService = false;
  bool hasService = false;
  for (final line in lines) {
    if (line.startsWith('import "rewise/')) res.imports.add(line);
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
        res.pascalCase, match.group(1), match.group(3), match.group(4)));
  }
  return hasService ? res : null;
}

// parse etc. 'rpc HackToJson (HackJsonBytes) returns (x.y.HackJsonString)'
final _callRequestRx = RegExp(
    r'^\s\srpc\s((\w|\.)*)\s\((\w*)\)\sreturns\s\(((\w|\.)*)\)',
    caseSensitive: false);

