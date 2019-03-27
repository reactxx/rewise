import 'dart:isolate' show SendPort;

void initMessages() {
  if (_called) return;
  messageDecoders.addAll(<String, DecodeProc>{
    MsgLow.id: (list) => MsgLow.decode(list),
    Msg.id: (list) => Msg.decode(list),
    WorkerStartedMsg.id: (list) => WorkerStartedMsg.decode(list),
    WorkerInit.id: (list) => WorkerInit.decode(list),
    WorkerFinished.id: (list) => WorkerFinished.decode(list),
    FinishWorker.id: (list) => FinishWorker.decode(list),
    ErrorMsg.id: (list) => ErrorMsg.decode(list),
  });
  _called = true;
}
bool _called = false;

MsgLow decodeMessage(List list) {
  assert(list != null && list.length > 0);
  final dec = messageDecoders[list[0]];
  assert(dec != null);
  return dec(list);
}

const _namespace = 'common.';
final messageDecoders = Map<String, DecodeProc>();

typedef MsgLow DecodeProc(List);

class MsgLow {
  static String id = _namespace + 'MsgLow';
  static List encode() => [id];
  MsgLow.decode(List list);
}

class Msg extends MsgLow {
  static const id = _namespace + 'Msg';
  SendPort sendPort;
  int threadId;

  static List encode() => [id];
  Msg.decode(List list) : super.decode(list) {
    sendPort = list[1];
    threadId = list[2];
  }
}

class WorkerStartedMsg extends Msg {
  static const id = _namespace + 'WorkerStartedMsg';
  static List encode() => [id];
  WorkerStartedMsg.decode(List list) : super.decode(list);
}

class WorkerInit extends Msg {
  static const id = _namespace + 'WorkerInit';
  List par;
  static List encode(List par) =>
      par == null ? [id] : (<dynamic>[id].followedBy(par)).toList();
  WorkerInit.decode(List list) : super.decode(list) {
    par = list.skip(3).toList();
  }
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
