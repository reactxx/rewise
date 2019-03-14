import 'dart:io';
import 'package:path/path.dart' as p;
import 'package:tuple/tuple.dart';
//https://pub.dartlang.org/packages/recase
import 'package:recase/recase.dart' show ReCase;

import 'package:server_dart/utils.dart';
import 'package:rewise_low_utils/utils.dart';

void refreshMessagesDart() {
  final relFiles = fileSystem.codeDartUtils
      .list(from: 'lib/src/messages', relTo: 'lib')
      .map((f) => Tuple2(f, p.split(f)));
  //var x = List<Tuple2<String, List<String>>>.from(relFiles);
  final grps =
      group<Tuple2<String, List<String>>, String, String>(relFiles, (t) {
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

void refreshServicesCSharp() {
  final relFiles = List<Tuple2<String, List<String>>>.from(fileSystem.protobufs
      .list(from: 'rewise', relTo: 'rewise')
      .map((f) => Tuple2(f, p.split(f))));
  //var x = List<Tuple2<String, List<String>>>.from(relFiles);
  final grps =
      group<Tuple2<String, List<String>>, String, String>(relFiles, (t) {
    if (t.item2.length < 2) return '';
    return t.item2[0];
  }, valuesAs: (t) => t.item1);
  final services = List<_Parsed>();
  for (final grp in grps) {
    if (grp.key == '') continue;
    services.addAll(grp.values.map((f) {
      final lines = fileSystem.protobufs.readAsLines(r'rewise\' + f);
      return _parseProto(lines);
    }).where((s) => s != null));
  }
  final cont = StringBuffer();
  cont.write(constImport);
  for (final imp
      in Set<String>.from(selectMany(services, (_Parsed s) => s.imports)))
    cont.writeln(imp);
  for (final pars in services) cont.writeln(_importMask(pars));
  cont.writeln();
  for (final pars in services)
    for (final serv in pars.services)
      cont.writeln(_codeMask(serv, pars.pascalCase));
  fileSystem.codeDartUtils.writeAsString('lib\\rw\\client.dart', cont.toString());
}

final constImport = '''
//***** generated code
import 'package:rewise_low_utils/utils.dart' show getHost, MakeRequest;
''';

String _importMask(_Parsed parsed) =>
    "import '${parsed.snakeCase}.dart' as ${parsed.pascalCase};";
//    "import 'package:rewise_low_utils/rw/${parsed.snakeCase}.dart' as ${parsed.pascalCase};";

String _codeMask(_Service service, String namespace) => '''
Future<${service.fixResponse}> ${namespace}_${service.name}(${service.fixRequest} request) => 
  MakeRequest<${service.fixResponse}>(
      (channel) => ${namespace}.CSharpServiceClient(channel).${service.cammelCase}(request),
      getHost('${namespace}'));
''';

class _Parsed {
  String snakeCase; // to_raw
  String pascalCase; // ToRaw
  final imports = List<String>();
  final services = List<_Service>();
}

class _Service {
  _Service(String namespace, this.name, this.fixRequest, this.fixResponse) {
    if (fixResponse.indexOf('.') < 0)
      fixResponse = namespace + '.' + fixResponse;
    if (fixRequest.indexOf('.') < 0) fixRequest = namespace + '.' + fixRequest;
    cammelCase = ReCase(name).camelCase;
  }
  String name;
  String cammelCase;
  String fixResponse;
  String fixRequest;
}

_Parsed _parseProto(List<String> lines) {
  final res = _Parsed();
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
    final match = callServiceRx.firstMatch(line);
    res.services.add(_Service(
        res.pascalCase, match.group(1), match.group(3), match.group(4)));
  }
  return hasService ? res : null;
}

// parse etc. 'rpc HackToJson (HackJsonBytes) returns (x.y.HackJsonString)'
final callServiceRx = RegExp(
    r'^\s\srpc\s((\w|\.)*)\s\((\w*)\)\sreturns\s\(((\w|\.)*)\)',
    caseSensitive: false);

void refreshMessagesCmd() {
  final relFiles =
      fileSystem.protobufs.list(from: 'rewise', regExp: r'\.proto$');
  final lines = relFiles.map((f) => ' ${p.withoutExtension(f)}^');
  fileSystem.protobufs.writeAsLines(r'rewise/fragment.cmd', lines);
}
