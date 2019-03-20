class StateMachine {
  StateMachine(this.input);

  String input;
  int pos = 0;
  final stack = List<IState>();

  void moveAhead() => pos += pos == input.length ? 0 : 1;
  String read() => pos == input.length ? throw Exception() : input[pos++];
  int get act => pos == input.length ? -1 : input.codeUnitAt(pos);
  String get actChar => pos == input.length ? eosChar : input[pos];

  start(IState trans) {
    push(trans);
    while (stack.isNotEmpty) stack.last.run();
    assert(pos == input.length);
  }

  pop() {
    stack.last
      ..end = pos
      ..popped();
    stack.removeLast();
  }

  push(IState tr) {
    tr.idx = stack.length;
    tr.start = pos;
    stack.add(tr);
  }

  bool get eos => pos >= input.length;
}

//https://en.wikipedia.org/wiki/Private_Use_Areas
const String eosChar = '\uF8FF';
const String errorChar = '\uF8FE';

class StrPart {
  int start;
  int end;
}

abstract class IState extends StrPart {
  run();
  popped() {}
  int idx;
}
