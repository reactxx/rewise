// https://stackoverflow.com/questions/51659805/persisting-appbar-drawer-across-all-pages-flutter
// https://github.com/ayalma/flutter_multi_page_drawer
// https://stackoverflow.com/questions/48098085/nesting-routes-with-flutter/48227916#48227916
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:functional_widget_annotation/functional_widget_annotation.dart';
// import 'package:json_annotation/json_annotation.dart';

// import 'uri_test.dart' as uriTest;

part 'main.g.dart';

void main() => runApp(MyApp());

class RouteData {
  RouteData(this._name, this._title, this._builder,
      {this.isPopup = false, this.fullscreenDialog = false})
      : assert(_routeDatas[_name] == null),
        assert(!fullscreenDialog || isPopup) {
    _routeDatas[_name] = this;
  }
  final String _name;
  final String _title;
  final bool isPopup;
  final bool fullscreenDialog;
  final WidgetBuilder _builder;
  MaterialPageRoute toMaterialPageRoute() => MaterialPageRoute<void>(
      builder: _builder,
      fullscreenDialog: fullscreenDialog,
      maintainState: false); // true => Close dialog icon
  RouteLink toLink({Object arguments}) =>
      RouteLink(_name, _title, isPopup, arguments: arguments);

  static final _routeDatas = <String, RouteData>{};
  static Route<dynamic> generateRoute(RouteSettings settings) {
    // uriTest.test();
    return (_routeDatas[settings.name] ?? homeRoute).toMaterialPageRoute();
  }
}

final homeRoute = RouteData('/', 'Home', (context) => HomeView());
final dialogRoute = RouteData('/dialog', 'Dialog', (context) => DialogView(),
    isPopup: true, fullscreenDialog: true);
final subPageRoute =
    RouteData('/subPage', 'SubPage', (context) => SubPageView());

class _NavigGlobals {
  GlobalKey<NavigatorState> navigKey = GlobalKey<NavigatorState>();
  GlobalKey<ScaffoldState> scaffoldKey = GlobalKey<ScaffoldState>();
  static _NavigGlobals of(BuildContext context) =>
      Provider.of<_NavigGlobals>(context, listen: false);
  static DrawerControllerState drawerControllerState(BuildContext context) =>
      context.rootAncestorStateOfType(TypeMatcher<DrawerControllerState>());
}

@widget
Widget myApp(BuildContext context) => Provider<_NavigGlobals>.value(
    value: _NavigGlobals(),
    child: Builder(
        builder: (context) => MaterialApp(
              title: 'Flutter Navig Demo',
              navigatorKey: _NavigGlobals.of(context).navigKey,
              builder: (context, child) {
                return Scaffold(
                  key: _NavigGlobals.of(context).scaffoldKey,
                  drawer: MyDrawer(),
                  body: child,
                );
              },
              onGenerateRoute: RouteData.generateRoute,
            )));

@widget
Widget myDrawer(BuildContext context) => Drawer(
      child: Column(mainAxisAlignment: MainAxisAlignment.center, children: [
        homeRoute.toLink(),
        subPageRoute.toLink(),
        dialogRoute.toLink(),
      ]),
    );
//final DrawerKey = GlobalKey();
@widget
Widget openDrawerButton(BuildContext context) => IconButton(
      icon: Icon(Icons.menu),
      onPressed: _NavigGlobals.of(context).scaffoldKey.currentState.openDrawer,
    );

@widget
Widget routeLink(
        BuildContext context, String routeName, String routeTitle, bool isPopup,
        {Object arguments}) =>
    FlatButton(
      onPressed: () {
        // close drawer if opened
        final DrawerControllerState drawerState = context.ancestorStateOfType(TypeMatcher<DrawerControllerState>());
        drawerState?.close();
        final navig = _NavigGlobals.of(context).navigKey.currentState;
        //navig.pop();
        isPopup
            ? navig.pushNamed(routeName, arguments: arguments)
            : navig.pushNamedAndRemoveUntil(routeName, (route) => false,
                arguments: arguments);
      },
      child: Text(routeTitle.toUpperCase()),
    );

@widget
Widget homeView(BuildContext context) => Scaffold(
      appBar: AppBar(
        title: Text('Home'),
        leading: OpenDrawerButton(),
      ),
      body: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [subPageRoute.toLink(), dialogRoute.toLink()]),
    );

@widget
Widget subPageView(BuildContext context) => Scaffold(
      appBar: AppBar(title: Text('SubPage'), leading: OpenDrawerButton()),
      body: Column(mainAxisAlignment: MainAxisAlignment.center, children: [
        homeRoute.toLink(),
        dialogRoute.toLink(),
      ]),
    );

@widget
Widget dialogView(BuildContext context) => Scaffold(
      appBar: AppBar(
        title: Text('Dialog'),
        actions: [
          FlatButton(
            onPressed: () =>
                _NavigGlobals.of(context).navigKey.currentState.pop(true),
            child: Text('SAVE'),
          ),
        ],
      ),
      body: Column(mainAxisAlignment: MainAxisAlignment.center, children: [
        homeRoute.toLink(),
        subPageRoute.toLink(),
        dialogRoute.toLink(),
        FlatButton(
          onPressed: _NavigGlobals.of(context).navigKey.currentState.pop,
          child: Text('CLOSE'),
        ),
      ]),
    );
