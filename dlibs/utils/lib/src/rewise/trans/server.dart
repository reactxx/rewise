import 'dart:io' as io;
import 'package:shelf_router/shelf_router.dart';
import 'package:shelf/shelf.dart' as shelf;
import 'package:shelf/shelf_io.dart' as shelfIO;
import 'package:shelf_static/shelf_static.dart' as st;
import 'package:rw_utils/utils.dart' show fileSystem;

main() {
  var link = io.Link(fileSystem.current.absolute('build/trans_tasks'));
  if (!link.existsSync())
    link.createSync(fileSystem.transTasks.absolute(''), recursive: true);

  var app = Router();

  // action inside root page
  app.get('/worker/init', (shelf.Request request) {
    var json =
        ''; // TODO - array of file, without extension, relative to rewise\data\trans_tasks
    return shelf.Response.ok(json,
        headers: {'Content-Type': 'application/json'});
  });
  app.get('/worker/isworking', (shelf.Request request) {
    var working = io.File(fileSystem.transTasks.absolute('transWorking.txt'))
        .existsSync();
    return shelf.Response.ok(working ? 'yes' : 'no', headers: {});
  });
  app.get('/worker/startfile', (shelf.Request request) {
    fileSystem.transTasks.writeAsString('transWorking.txt', '');
    return shelf.Response.ok(null);
  });

  // action inside translation page
  app.get('/worker/endfile', (shelf.Request request) {
    io.File(fileSystem.transTasks.absolute('transWorking.txt')).deleteSync();
    return shelf.Response.ok(null);
  });

  Future<shelf.Response> routerHack(shelf.Request request) async {
    var resp = await app.handler(request);
    return Future.value(resp == null ? shelf.Response.notFound('') : resp);
  }

  var handler = shelf.Cascade()
      .add(st.createStaticHandler('data'))
      .add(routerHack)
      .handler;

  shelfIO.serve(handler, 'localhost', 8080).then((server) {
    print('Serving at http://${server.address.host}:${server.port}');
  });
}
