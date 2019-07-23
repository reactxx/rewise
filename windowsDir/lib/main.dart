import 'package:flutter/foundation.dart'
    show debugDefaultTargetPlatformOverride;
import 'package:flutter/material.dart';

//import 'src/knowHow/navigator/main.dart' as navig;
//import 'src/knowHow/navigator/main.1.dart' as navig1;
import 'src/knowHow/navigator/route_test.dart' as route_test;

void main() {
  // See https://github.com/flutter/flutter/wiki/Desktop-shells#target-platform-override
  debugDefaultTargetPlatformOverride = TargetPlatform.fuchsia;

  route_test.main();
  //navig.main();
  //navig1.main(); 
  //runApp(Text('XXX'));
}
