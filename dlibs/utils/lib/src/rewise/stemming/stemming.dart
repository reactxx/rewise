import 'package:rw_utils/utils.dart' as utils;
import 'package:rw_utils/rewise.dart' as rewise;
import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/langs.dart' show Langs;
import 'package:rw_utils/dom/stemming.dart' as stemm;
import 'package:rw_utils/stemming.dart' as stemm;

const _devFilter = r'goetheverlag\.msg';

Future toStemmCache(rewise.ParseBookResult parsed) async {
  final stemmLangs =
      Set.from(Langs.meta.where((m) => m.hasStemming).map((m) => m.id));

  for (final fn in utils.fileSystem.parsed.list(regExp: _devFilter)) {
    final books =
        toPars.ParsedBooks.fromBuffer(utils.fileSystem.parsed.readAsBytes(fn));
    for (var book in books.books.where((b) => stemmLangs.contains(b.lang))) {
      final request = stemm.Request().writeToBuffer();
    }
  }
}

utils.Msg msgDecoder(List list) {
  switch (list[0]) {
    default:
      return utils.ThreadPool.decodeMessage(list);
  }
}

class TThread extends utils.Thread {
  TThread.px(utils.ThreadPool pool) : super.px(pool);
  TThread.wk(utils.MsgDecode decoder, List list) : super.wk(decoder, list);

  // ------------- PROXY ON POOL SIDE
  @override
  utils.EntryPoint get pxWkCode => wkCode;

  // ------------- WORKER
  static void wkCode(List l) => TThread.wk(msgDecoder, l).wkRun();

  @override
  Future wkOnStream(Stream<utils.Msg> stream) async {
    await for (final msg in stream) {
      if (msg is utils.FinishWorker) break;
    }
  }

  final utils.PXOnMsg pxOnMsg = (pool, msg, proxy) => pool.pxOnMsg(msg, proxy);
}
