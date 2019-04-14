import 'dart:isolate' show Isolate, ReceivePort;
import 'messages.dart';
import 'proxy.dart';

typedef CreateProxies = List<Proxy> Function(WorkersPool pool);

const trace = false;

class WorkersPool {
  WorkersPool(CreateProxies createProxies) {
    proxies = Map<int, Proxy>.fromIterable(createProxies(this),
        key: (t) => (t as Proxy).threadId);
    initMessageCreators();
  }

  final receivePort = ReceivePort();

  Map<int, Proxy> proxies;
  final result = List<Msg>();

  Future<List<Msg>> run({void traceMsg(int count, msg)}) async {
    if (trace) print('MAIN STARTED');

    // start isolates
    await Future.wait(proxies.values.map((proxy) async {
      final initMsg = proxy.createMsg(proxy.initPar ?? Msg());
      proxy.isolate = await Isolate.spawn(
          proxy.entryPoint, initMsg);
      proxy.isolate.addOnExitListener(receivePort.sendPort,
          response: proxy.createMsg(WorkerFinished()));
      return Future.value();
    }));
    if (trace) print('MAIN PROXIES STARTED');

    // run client message queue
    final msgStream = receivePort.map((list) => decodeMessage(list.iterator));
    await for (var msg in msgStream) {
      if (trace) print('MAIN RUN: ${msg.threadId}-$msg');

      final proxy = proxies[msg.threadId];
      if (proxy == null) continue;

      // save sendPort for sending messages to Worker
      proxy.sendPort = msg.sendPort;

      var res = await mainStreamMsg(msg, proxy, traceMsg: traceMsg);

      if (res == true) break;
      if (res is Msg) result.add(msg);
      if (proxies.length == 0) break;
    }
    return Future.value(result);
  }

  Future mainStreamMsg(Msg msg, Proxy proxy, {void traceMsg(int count, msg)}) {
    if (proxies.length == 0) return futureTrue;
    if (msg is WorkerFinished) {
      proxies.remove(msg.threadId);
      if (proxies.length == 0) return futureTrue;
    } else if (msg is ErrorMsg)
      return Future.value(msg);
    else
      throw Exception(
          'Server: unknown thread mesage: ${msg.runtimeType.toString()}');
    return futureFalse; // continue main message loop
  }
}

final futureFalse = Future.value(false);
final futureTrue = Future.value(true);
