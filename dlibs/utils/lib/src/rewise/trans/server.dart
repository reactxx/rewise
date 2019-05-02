import 'dart:io' as io;
import 'package:shelf_router/shelf_router.dart';
import 'package:shelf/shelf.dart' as shelf;
import 'package:shelf/shelf_io.dart' as shelfIO;
import 'package:shelf_static/shelf_static.dart' as st;
import 'package:rw_utils/utils.dart' show fileSystem;

main() {
  print('ROOT: ${fileSystem.current.absolute(r'build\trans_tasks')}');
  var link = io.Link(fileSystem.current.absolute(r'build\trans_tasks'));
  if (!link.existsSync())
    link.createSync(fileSystem.transTasks.absolute(''), recursive: true);

  var app = Router();

  // action inside root page
  app.get('/worker/isworking', (shelf.Request request) {
    var working = io.File(fileSystem.transTasks.absolute('_transWorking.txt'))
        .existsSync();
    return shelf.Response.ok(working ? 'yes' : 'no', headers: {});
  });
  app.get('/worker/startfile', (shelf.Request request) {
    fileSystem.transTasks.writeAsString('_transWorking.txt', '');
    return shelf.Response.ok(null);
  });

  // action inside translation page
  app.get('/worker/endfile', (shelf.Request request) {
    var file = io.File(fileSystem.transTasks.absolute('_transWorking.txt'));
    if (file.existsSync()) file.deleteSync();
    return shelf.Response.ok(null);
  });

  Future<shelf.Response> routerHack(shelf.Request request) async {
    var resp = await app.handler(request);
    return Future.value(resp == null ? shelf.Response.notFound('') : resp);
  }

  var handler = shelf.Cascade()
      //.add(st.createStaticHandler(fileSystem.ntb ? 'web' : 'build', serveFilesOutsidePath: true))
      .add(st.createStaticHandler('build',
          serveFilesOutsidePath: true, defaultDocument: 'trans_index.html'))
      .add(routerHack)
      .handler;

  shelfIO.serve(handler, 'localhost', 8080).then((server) {
    print('Serving at http://${server.address.host}:${server.port}');
  });
}
