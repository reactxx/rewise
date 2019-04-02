import 'dart:isolate' show Isolate, ReceivePort, SendPort;
import 'messages.dart';
import 'worker.dart';
import 'threading.dart';

class WorkerProxyCommon {
  SendPort sendPort; // for sending back, with receivePort.sendPort
  ReceivePort receivePort; // listening
  int id;

  static List createMsgLow(SendPort sendPort, int threadId, List list) =>
      [list[0], sendPort, threadId].followedBy(list.skip(1)).toList();
  List createMsg(List list) => createMsgLow(receivePort.sendPort, id, list);
  sendMsg(List list) => sendPort.send(createMsg(list));
  sendError(exp, StackTrace stacktrace) =>
      sendMsg(ErrorMsg.encode(exp.toString(), stacktrace.toString()));
}

class Proxy extends WorkerProxyCommon {
  Proxy(this.pool, this.entryPoint, {this.initPar}) {
    id = _idCounter++;
    receivePort = pool.receivePort;
  }

  void mainFinishWorker() {
    pool.proxies.remove(id);
    sendMsg(FinishWorker.encode());
  }

  final List initPar;
  WorkerEntryPoint entryPoint;
  WorkersPool pool;
  Isolate isolate;

  static int _idCounter = 0;

}

