import 'dart:isolate' show SendPort;

void initMessageCreators() {
  if (_called) return;
  messageDecoders.addAll(<String, DecodeProc>{
    Msg.id: (list) => Msg.fromIter(list),
    WorkerFinished.id: (list) => WorkerFinished.fromIter(list),
    FinishWorker.id: (list) => FinishWorker.fromIter(list),
    ErrorMsg.id: (list) => ErrorMsg.fromIter(list),
    ContinueMsg.id: (list) => ContinueMsg.fromIter(list),
    DataMsg.id: (list) => DataMsg.fromIter(list),
    InitMsg.id: (list) => InitMsg.fromIter(list),
  });
  _called = true;
}

bool _called = false;

Msg decodeMessage(Iterator list) {
  assert(list != null);
  final id = (list..moveNext()).current;
  final dec = messageDecoders[id];
  assert(dec != null);
  return dec(list)..msgId = id;
}

const _namespace = 'common.';
final messageDecoders = Map<String, DecodeProc>();

typedef Msg DecodeProc(List);

class Msg {
  static const id = _namespace + 'Msg';
  SendPort sendPort;
  int threadId;
  String msgId;
  List listValue;

  Msg() : this._create(id);
  Msg._create(this.msgId, [this.listValue]);
  Msg.fromIter(Iterator iter) {
    sendPort = (iter..moveNext()).current;
    threadId = (iter..moveNext()).current;
    while (iter.moveNext())
      (listValue ?? (listValue = List())).add(iter.current);
  }

  List toList() => listValue == null
      ? [msgId, sendPort, threadId]
      : List.from([msgId, sendPort, threadId].followedBy(listValue));
}

class ContinueMsg extends Msg {
  static const id = _namespace + 'ContinueMsg';
  ContinueMsg() : super._create(id);
  ContinueMsg.fromIter(Iterator iter) : super.fromIter(iter);
}

class WorkerFinished extends Msg {
  static const id = _namespace + 'WorkerFinished';
  WorkerFinished() : super._create(id);
  WorkerFinished.fromIter(Iterator iter) : super.fromIter(iter);
}

class FinishWorker extends Msg {
  static const id = _namespace + 'FinishWorker';
  FinishWorker.fromIter(Iterator iter) : super.fromIter(iter);
  FinishWorker() : super._create(id);
}

class ErrorMsg extends Msg {
  static const id = _namespace + 'ErrorMsg';
  ErrorMsg(String error, String stackTrace)
      : super._create(id, [error, stackTrace]);
  ErrorMsg.fromIter(Iterator iter) : super.fromIter(iter);

  String get error => listValue[3];
  String get stackTrace => listValue[4];
}

class DataMsg extends Msg {
  static const id = _namespace + 'DataMsg';
  DataMsg(List listValue) : super._create(id, listValue);
  DataMsg.fromIter(Iterator iter) : super.fromIter(iter);
}

class InitMsg extends Msg {
  static const id = _namespace + 'InitMsg';
  InitMsg([List listValue]) : super._create(id, listValue);
  InitMsg.fromIter(Iterator iter) : super.fromIter(iter);
}
