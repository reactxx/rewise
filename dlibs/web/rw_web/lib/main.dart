import 'package:flutter_web/material.dart';
import 'package:provider/provider.dart' as prov;
import 'package:functional_widget_annotation/functional_widget_annotation.dart';
import 'package:flutter_hooks/flutter_hooks.dart';

part 'main.g.dart';

void main() {
  runApp(
    // Provide the model to all widgets within the app. We're using
    // ChangeNotifierProvider because that's a simple way to rebuild
    // widgets when a model changes. We could also just use
    // Provider, but then we would have to listen to Counter ourselves.
    //pub global activate webdev
    // Read Provider's docs to learn about all the available providers.
    prov.ChangeNotifierProvider(
      // Initialize the model in the builder. That way, Provider
      // can own Counter's lifecycle, making sure to call `dispose`
      // when not needed anymore.
      builder: (context) => Counter(),
      child: MyApp(),
    ),
  );
}

/// Simplest possible model, with just one field.
///
/// [ChangeNotifier] is a class in `flutter:foundation`. [Counter] does
/// _not_ depend on Provider.
class Counter with ChangeNotifier {
  int value = 0;

  void increment() {
    value += 1;
    notifyListeners();
  }

  void decrement() {
    value -= 1;
    notifyListeners();
  }
}

@widget
Widget myApp() => MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: MyHomePage(),
    );

@widget
Widget couterText(BuildContext _context, int value) => Text(
      '${value}',
      style: Theme.of(_context).textTheme.display2,
    );

@hwidget
Widget myHomePage(BuildContext context) {
  final val = useState(0);
  return Scaffold(
    appBar: AppBar(
      title: RaisedButton(
          onPressed: () {
            //val.value += 1;
            prov.Provider.of<Counter>(context, listen: false).decrement();
          },
          child: Icon(Icons.remove)),
    ),
    body: Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Text('You have pushed the button this many times (${val.value}):'),
          // Consumer looks for an ancestor Provider widget
          // and retrieves its model (Counter, in this case).
          // Then it uses that model to build widgets, and will trigger
          // rebuilds if the model is updated.
          prov.Consumer<Counter>(
            builder: (_, counter, __) => CouterText(counter.value),
          ),
        ],
      ),
    ),
    floatingActionButton: FloatingActionButton(
      // Provider.of is another way to access the model object held
      // by an ancestor Provider. By default, even this listens to
      // changes in the model, and rebuilds the whole encompassing widget
      // when notified.
      //
      // By using `listen: false` below, we are disabling that
      // behavior. We are only calling a function here, and so we don't care
      // about the current value. Without `listen: false`, we'd be rebuilding
      // the whole MyHomePage whenever Counter notifies listeners.
      onPressed: () =>
          prov.Provider.of<Counter>(context, listen: false).increment(),
      tooltip: 'Increment',
      child: Icon(Icons.add),
    ),
  );
}
