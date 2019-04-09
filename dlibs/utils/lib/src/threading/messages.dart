import 'dart:isolate' show SendPort;

void initMessages() {
  if (_called) return;
  messageDecoders.addAll(<String, DecodeProc>{
    Msg.id: (list) => Msg.decode(list),
    WorkerFinished.id: (list) => WorkerFinished.decode(list),
    FinishWorker.id: (list) => FinishWorker.decode(list),
    ErrorMsg.id: (list) => ErrorMsg.decode(list),
    ContinueMsg.id: (list) => ContinueMsg.decode(list),
    StringMsg.id: (list) => StringMsg.decode(list),
  });
  _called = true;
}

bool _called = false;

Msg decodeMessage(List list) {
  assert(list != null && list.length > 0);
  final dec = messageDecoders[list[0]];
  assert(dec != null);
  return dec(list);
}

const _namespace = 'common.';
final messageDecoders = Map<String, DecodeProc>();

typedef Msg DecodeProc(List);

class Msg {
  Msg();
  static const id = _namespace + 'Msg';
  SendPort sendPort;
  int threadId;

  static List encode() => [id];
  Msg.decode(List list):
    sendPort = list[1],
    threadId = list[2];
}

class ContinueMsg extends Msg {
  static const id = _namespace + 'ContinueMsg';
  static List encode() => [id];
  ContinueMsg.decode(List list) : super.decode(list);
}

class WorkerFinished extends Msg {
  static const id = _namespace + 'WorkerFinished';
  static List encode() => [id];
  WorkerFinished.decode(List list) : super.decode(list);
}

class FinishWorker extends Msg {
  static const id = _namespace + 'FinishWorker';
  static List encode() => [id];
  FinishWorker.decode(List list) : super.decode(list);
}

class ErrorMsg extends Msg {
  static const id = _namespace + 'ErrorMsg';
  String error;
  String stackTrace;

  static List encode(String error, String stackTrace) =>
      [id, error, stackTrace];
  ErrorMsg.decode(List list) : super.decode(list) {
    error = list[3];
    stackTrace = list[4];
  }
}

class StringMsg extends Msg {
  StringMsg(this.strValue) : super();
  static const id = _namespace + 'StringMsg';
  String strValue;
  static List encode(String relPath) => [id, relPath];
  StringMsg.decode(List list) : super.decode(list) {
    strValue = list[3];
  }
  toString() => strValue;
}

