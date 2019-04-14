import 'dart:isolate' show Isolate, ReceivePort, SendPort;
import 'messages.dart';
import 'worker.dart';
import 'threading.dart';

class WorkerProxyCommon {
  SendPort sendPort; // for sending back, with receivePort.sendPort
  ReceivePort receivePort; // listening
  int threadId;

  sendMsg(Msg msg) => sendPort.send(createMsg(msg));
  List createMsg(Msg msg) => (msg..sendPort = receivePort.sendPort..threadId = threadId).toList();

  //static List createMsgLow(SendPort sendPort, int threadId, List list) =>
      //[list[0], sendPort, threadId].followedBy(list.skip(1)).toList();
  //List createMsg(List list) => createMsgLow(receivePort.sendPort, threadId, list);
  //sendMsg(List list) => sendPort.send(createMsg(list));
  sendError(exp, StackTrace stacktrace) =>
      sendMsg(ErrorMsg(exp.toString(), stacktrace.toString()));
}

class Proxy extends WorkerProxyCommon {
  Proxy(this.pool, this.entryPoint, {this.initPar}) {
    threadId = _idCounter++;
    receivePort = pool.receivePort;
  }

  void mainFinishWorker() {
    pool.proxies.remove(threadId);
    sendMsg(FinishWorker());
  }

  final Msg initPar;
  WorkerEntryPoint entryPoint;
  WorkersPool pool;
  Isolate isolate;

  static int _idCounter = 0;

}

