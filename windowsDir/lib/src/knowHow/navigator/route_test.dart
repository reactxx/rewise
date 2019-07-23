import 'package:flutter/material.dart';
import 'package:functional_widget_annotation/functional_widget_annotation.dart';
import 'route.dart';

part 'route_test.g.dart';

class Template extends RouteTemplate {
  Template(RouteProxy proxy) : super(proxy);
}

class HomeProxy extends RouteProxy<void> {
  HomeProxy() : super(linkTitle: 'Home') {
    parent = Template(this);
  }

  @override
  Widget build(BuildContext context) => HomeView(this);
}

class DialogProxy extends RouteProxy<String> {
  DialogProxy(this.id,
      {String linkTitle,
      RouteType type,
      OnModalResult<String> onModalResult,
      OnModalError onModalError})
      : super(
            linkTitle: linkTitle,
            type: type,
            onModalResult: onModalResult,
            onModalError: onModalError) {
    parent = Template(this);
  }

  final int id;

  @override
  String get appBarTitle => '${super.appBarTitle} $id';
  @override
  Widget build(BuildContext context) => DialogView(this);
}

void main() => runApp(MyApp());

@widget
Widget myApp(BuildContext context) => MaterialApp(
    // ?? obsolete
    navigatorObservers: [RouteHelper.navigatorObserver], // !!!!
    onGenerateRoute: RouteHelper.onGenerateRoute(HomeProxy()), // !!!!
    navigatorKey: RouteHelper.navigatorKey, // !!!!
    title: 'Flutter Navig Demo',
    builder: (context, child) {
      return Scaffold(
        key: RouteHelper.scaffoldKey, // !!!!
        drawer: MyDrawer(),
        body: child,
      );
    });

@widget
Widget homeView(BuildContext context, HomeProxy par) => Column(children: [
      DialogProxy(1,
          type: RouteType.popup,
          linkTitle: 'Modal dialog',
          onModalResult: (r) => showDialog<dynamic>(
              context: context,
              builder: (context) => AlertDialog(
                    content: Text(r ?? 'Canceled'),
                  ))).link(),
      DialogProxy(2,
              type: RouteType.fullscreenDialog, linkTitle: 'Fullscreen dialog')
          .link(),
      DialogProxy(3, linkTitle: 'Dialog level0').link(),
      DialogProxy(4, linkTitle: 'Dialog level1', type: RouteType.level1).link(),
    ]);

@widget
Widget dialogView(BuildContext context, DialogProxy par) => Column(children: [
      Text(par.id.toString()),
      DialogProxy(5, type: RouteType.popup, linkTitle: 'Popup dialog').link(),
      if (par.type == RouteType.level0)
        DialogProxy(6, type: RouteType.level1, linkTitle: 'Dialog level1')
            .link(),
      if (par.isModal)
        FlatButton(
            textColor: Theme.of(context).primaryColor,
            onPressed: par.returnModalValue('Dialog modal result'),
            child: Text('RETURN VALUE')),
      RouteHelper.homeRoute.link(),
    ]);

@widget
Widget myDrawer(BuildContext context) => Drawer(
      child: Column(mainAxisAlignment: MainAxisAlignment.center, children: [
        DialogProxy(7, type: RouteType.popup, linkTitle: 'Popup dialog').link(),
        DialogProxy(8, linkTitle: 'Dialog level0').link(),
        DialogProxy(9, type: RouteType.level1, linkTitle: 'Dialog level1')
            .link(),
        RouteHelper.homeRoute.link(),
      ]),
    );
