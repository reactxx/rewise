import 'package:flutter/material.dart';
import 'package:functional_widget_annotation/functional_widget_annotation.dart';
import 'route.dart';

part 'route_test.g.dart';

class HomeProxy extends RouteProxy<void> {
  HomeProxy({RouteTemplate parent}) : super(title: 'Home', parent: parent);

  @override
  Widget build(BuildContext context) => HomeView();
}

class DialogProxy extends RouteProxy<String> {
  DialogProxy(this.name, this.id,
      {String title,
      RouteTemplate parent,
      RouteType type,
      OnModalResult<String> onModalResult,
      OnModalError onModalError})
      : super(
            parent: parent,
            title: title,
            type: type,
            onModalResult: onModalResult,
            onModalError: onModalError);

  final String name;
  final int id;

  @override
  Widget build(BuildContext context) => DialogView(this);
  // not needed when https://github.com/dart-lang/sdk/issues/28477 solved
  @override
  void setParent(RouteTemplate setter(DialogProxy self)) =>
      parent = setter(this);
}

void main() => runApp(MyApp());

@widget
Widget myApp(BuildContext context) => MaterialApp(
    navigatorObservers: [RouteHelper.navigatorObserver], // !!!!
    onGenerateRoute: RouteHelper.onGenerateRoute(
        HomeProxy()..setParent((self) => RouteTemplate(self))), // !!!!
    title: 'Flutter Navig Demo',
    builder: (context, child) {
      return Scaffold(
        key: RouteHelper.scaffoldKey, // !!!!
        drawer: MyDrawer(),
        body: child,
      );
    });

@widget
Widget homeView(BuildContext context) => Column(children: [
      RouteLink<DialogProxy, String>(DialogProxy('Modal', 11,
          type: RouteType.popup,
          title: 'Modal dialog',
          onModalResult: (r) => showDialog<dynamic>(
              context: context,
              builder: (context) => AlertDialog(
                    content: Text(r ?? 'Canceled'),
                  )))
        ..setParent((self) =>
            RouteTemplate(self, appBarTitle: '${self.name} ${self.title}'))),
      RouteLink<DialogProxy, String>(DialogProxy('FullScreen', 22,
          type: RouteType.fullscreenDialog, title: 'Fullscreen dialog')
        ..setParent((self) =>
            RouteTemplate(self, appBarTitle: '${self.name} ${self.title}'))),
      RouteLink<DialogProxy, String>(DialogProxy('Plain level0', 33,
          title: 'Dialog level0')
        ..setParent((self) =>
            RouteTemplate(self, appBarTitle: '${self.name} ${self.title}'))),
      RouteLink<DialogProxy, String>(DialogProxy('Plain level 1', 44,
          title: 'Dialog level1', type: RouteType.level1)
        ..setParent((self) =>
            RouteTemplate(self, appBarTitle: '${self.name} ${self.title}'))),
    ]);

@widget
Widget dialogView(BuildContext context, DialogProxy par) => Column(children: [
      Text(par.name),
      Text(par.id.toString()),
      FlatButton(
          onPressed: par.isPopupType
              ? par.returnModalValue('Dialog modal result')
              : RouteHelper.homeRoute.navigate(context),
          child: Text(par.isPopupType ? 'Close' : 'Goto Home')),
      if (par.type == RouteType.level0)
        RouteLink<DialogProxy, String>(DialogProxy('Plain level1', 55,
            title: 'Dialog level1', type: RouteType.level1)
          ..setParent((self) =>
              RouteTemplate(self, appBarTitle: '${self.name} ${self.title}'))),
    ]);

@widget
Widget myDrawer(BuildContext context) => Drawer(
      child: Column(
          mainAxisAlignment: MainAxisAlignment.center, children: const []),
    );
