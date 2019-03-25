import 'dart:isolate' show SendPort;

MsgLow decodeMessage(List list) {
  switch (list[0]) {
    case Msg.id:
      return Msg.decode(list);
    case WorkerStartedMsg.id:
      return WorkerStartedMsg.decode(list);
    case WorkerInit.id:
      return WorkerInit.decode(list);
    case WorkerFinished.id:
      return WorkerFinished.decode(list);
    case FinishWorker.id:
      return FinishWorker.decode(list);
    case ErrorMsg.id:
      return ErrorMsg.decode(list);
    default:
      throw Exception('Server: unknown thread mesage: ${list[0]}');
  }
}

class MsgLow {
  static const id = 'th.common.MsgLow';

  static List encode() => [id];
  MsgLow.decode(List list);
}

class Msg extends MsgLow {
  static const id = 'th.common.Msg';
  SendPort sendPort;
  int threadId;

  static List encode() => [id];
  Msg.decode(List list) : super.decode(list) {
    sendPort = list[1];
    threadId = list[2];
  }
}

class WorkerStartedMsg extends Msg {
  static const id = 'th.common.WorkerStartedMsg';
  static List encode() => [id];
  WorkerStartedMsg.decode(List list) : super.decode(list);
}

class WorkerInit extends Msg {
  static const id = 'th.common.WorkerInit';
  List par;
  static List encode(List par) => par==null ? [id] : (([id] as List).followedBy(par)).toList();
  WorkerInit.decode(List list) : super.decode(list) {
    par = list.skip(3).toList();
  }
}

class WorkerFinished extends Msg {
  static const id = 'th.common.WorkerFinished';
  static List encode() => [id];
  WorkerFinished.decode(List list) : super.decode(list);
}

class FinishWorker extends Msg {
  static const id = 'th.common.FinishWorker';
  static List encode() => [id];
  FinishWorker.decode(List list) : super.decode(list);
}

class ErrorMsg extends Msg {
  static const id = 'th.common.ErrorMsg';
  String error;
  String stackTrace;

  static List encode(String error, String stackTrace) =>
      [id, error, stackTrace];
  ErrorMsg.decode(List list) : super.decode(list) {
    error = list[3];
    stackTrace = list[4];
  }
}
