import 'package:test/test.dart';
import 'package:rw_low/code.dart';

main() {
  group("TESTS", () {

    test('REFRESH MESSAGE DART', () {
      // **** 0. step by hand: 
      //         remove server from workspace
      //         delete c:\rewise\dlibs\utils\lib\src\messages\ and C:\rewise\dlibs\utils\lib\rw\
      // **** 1. step:
      //refreshGenCmd();
      // **** 2. step by hand: copy C:\rewise\protobuf\compiler\include\rewise\fragment.cmd to gen.cmd
      // **** 3. step by hand: run C:\rewise\protobuf\compiler\include\rewise\gen.cmd
      // **** 4. step
      refreshServicesCSharp();
      generateMessagesExports();
      expect(0, equals(0));
    }, skip: false);

  }, skip: false);
}