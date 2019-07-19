import 'package:flutter_web_ui/ui.dart' as ui;
import 'package:flutter_web_ui/src/engine.dart' as engine;
import 'package:rw_web/main.dart' as app;

main() async {
  await ui.webOnlyInitializePlatform(
    assetManager: engine.AssetManager(
      assetsDir: 'assets'      // Name of my assets folder
    )
  );
  app.main();
}
