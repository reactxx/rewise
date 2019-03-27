//https://stackoverflow.com/questions/14930950/what-are-the-differences-between-the-different-map-implementations-in-dart
//https://stackoverflow.com/questions/14536437/how-do-you-create-a-stream-in-dart
import 'package:test/test.dart' as test;

class Const {
  const Const(this.id);
  final id;
}

const c1 = Const(1);
const c2 = Const(1);
const c3 = Const(2);

enum ConstEnum { c1, c2, c3 }

class Static {
  static get i => p2();
  final j = p2();
}

p2() {
  return 2;
}

main() {
  test.test('fake-test', () {
    int Run(Const c) {
      switch (c) {
        case c1:
          return 1;
        case c2:
          return 2;
        case c3:
          return 3;
        default:
          return 0;
      }
    }

    final st = Static();
    if (Static.i * st.j != 4) return;


    test.expect(Run(c1), test.equals(1));
    test.expect(Run(c2), test.equals(1));
    test.expect(Run(c3), test.equals(3));
    test.expect(ConstEnum.c1.toString(), test.equals('ConstEnum.c1'));
    test.expect(
        ConstEnum.values.map((v) => v.index).join(','), test.equals('0,1,2'));
  });
}
