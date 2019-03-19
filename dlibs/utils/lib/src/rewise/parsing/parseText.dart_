class Bracket {
  Bracket(this.type, this.value);
  String type;
  String value;
}

class ParseResult {
  ParseResult(this.brakets, this.str, this.breakStr, this.valid, this.wCls);
  List<Bracket> brakets;
  String str;
  String breakStr;
  bool valid;
  String wCls;
}

ParseResult parse(String str) {
  final sb = StringBuffer();
  final sbBreak = StringBuffer();
  final res = List<Bracket>();
  String wCls;
  var state = 0; // 0..out of bracket, 1..in (, 2..in [, 3..in {
  int openBrPos;
  var copyPos = 0;
  var brCopyPos = 0;
  void createBr(String type, int i) {
    if (type == "[") {
      sbBreak.write(str.substring(brCopyPos, openBrPos));
      sb.write(str.substring(copyPos, openBrPos));
      copyPos = i + 1;
      brCopyPos = i + 1;
      wCls = str.substring(openBrPos + 1, i);
      res.add(Bracket(type, wCls));
    } else if (type != null) {
      sbBreak.write(str.substring(brCopyPos, openBrPos));
      sbBreak.write(''.padRight(i - openBrPos + 1));
      brCopyPos = i + 1;
      final val = str.substring(openBrPos + 1, i);
      res.add(Bracket(type, val));
    } else {
      sbBreak.write(str.substring(brCopyPos, i));
      sb.write(str.substring(copyPos, i));
    }
  }

  final codes = str.codeUnits;
  for (var i = 0; i < codes.length; i++) {
    final ch = codes[i];
    switch (state) {
      case 999:
        break;
      case 0: //out of bracket
        switch (ch) {
          case bro:
            openBrPos = i;
            state = 1;
            break;
          case brSqo:
            openBrPos = i;
            state = 2;
            break;
          case brCuro:
            openBrPos = i;
            state = 3;
            break;
          case brc:
          case brSqc:
          case brCurc:
            state = 999;
            break;
        }
        break;
      case 1: //in (
        switch (ch) {
          case brc:
            state = 0;
            createBr('(', i);
            break;
          case bro:
          case brSqc:
          case brCurc:
            state = 999;
            break;
        }
        break;
      case 2: //in [
        switch (ch) {
          case brSqc:
            state = 0;
            createBr('[', i);
            break;
          case brc:
          case brCurc:
          case brSqo:
            state = 999;
            break;
        }
        break;
      case 3: //in [
        switch (ch) {
          case brCurc:
            state = 0;
            createBr('{', i);
            break;
          case brc:
          case brSqo:
          case brCuro:
            state = 999;
            break;
        }
        break;
    }
  }
  createBr(null, str.length);
  final t = sb.toString(), b = sbBreak.toString();
  return ParseResult(res, t, t == b ? null : b, state == 0, wCls);
}

const bro = 40;
const brc = 41;
const brSqo = 91;
const brSqc = 93;
const brCuro = 123;
const brCurc = 125;
const wCls = 124;
const wMean = 94;
const wSyn = 44;
const esc = 92;
// http://www.mauvecloud.net/charsets/CharCodeFinder.html
// ()[]{}|^,\
// 40, 41, 91, 93, 123, 125, 124, 94, 44, 92

class FSM {
  String input;
  int idx;
  String read() => input[idx++];

  update([data]) {
    stack.last.run();
  }

  final stack = List<IFS>();
  pop([x]) {
    stack.removeLast().run(popData: x);
    update();
  }
  push(IFS st) {
    stack.add(st);
    st.run();
  }

  root() {
    switch (read()) {
      case '(':
        push(Br('('));
        break;
      case ')':
        pop('x');
        break;
    }
  }
}

abstract class IFS {
  int idx;
  FSM fsm;
  run({popData});
}

class Br extends IFS {
  Br(this.type);
  String type;
  run({popData}) {}
}
