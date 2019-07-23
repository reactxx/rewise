import 'package:flutter/material.dart';
import 'package:functional_widget_annotation/functional_widget_annotation.dart';
import 'route.dart';

part 'route_test.g.dart';

class HomeProxy extends RouteProxy<void> {
  HomeProxy({RouteTemplate parent}) : super(linkTitle: 'Home', parent: parent);

  @override
  Widget build(BuildContext context) => HomeView();
}

class DialogProxy extends RouteProxy<String> {
  DialogProxy(this.name, this.id,
      {String linkTitle,
      RouteTemplate parent,
      RouteType type,
      OnModalResult<String> onModalResult,
      OnModalError onModalError})
      : super(
            parent: parent,
            linkTitle: linkTitle,
            type: type,
            onModalResult: onModalResult,
            onModalError: onModalError);

  final String name;
  final int id;
  @override
  String get appBarTitle => '$name ${super.appBarTitle}';

  @override
  Widget build(BuildContext context) => DialogView(this);
}

void main() => runApp(MyApp());

@widget
Widget myApp(BuildContext context) => MaterialApp(
    //navigatorObservers: [RouteHelper.history], // !!!!
    onGenerateRoute:
        RouteHelper.onGenerateRoute(RouteTemplate(HomeProxy()).proxy), // !!!!
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
Widget homeView(BuildContext context) => Column(children: [
      RouteTemplate(DialogProxy('Modal', 11,
          type: RouteType.popup,
          linkTitle: 'Modal dialog',
          onModalResult: (r) => showDialog<dynamic>(
              context: context,
              builder: (context) => AlertDialog(
                    content: Text(r ?? 'Canceled'),
                  )))).proxy.link(),
      RouteTemplate(DialogProxy('FullScreen', 22,
              type: RouteType.fullscreenDialog, linkTitle: 'Fullscreen dialog'))
          .proxy
          .link(),
      RouteTemplate(DialogProxy('Plain level0', 33, linkTitle: 'Dialog level0'))
          .proxy
          .link(),
      RouteTemplate(DialogProxy('Plain level 1', 44,
              linkTitle: 'Dialog level1', type: RouteType.level1))
          .proxy
          .link(),
    ]);

@widget
Widget dialogView(BuildContext context, DialogProxy par) => Column(children: [
      Text(par.name),
      Text(par.id.toString()),
      RouteTemplate(DialogProxy('Modal', 66,
              type: RouteType.popup, linkTitle: 'in modal dialog'))
          .proxy
          .link(),
      if (par.type == RouteType.level0)
        RouteTemplate(DialogProxy('Plain level1', 55,
                linkTitle: 'Dialog level1', type: RouteType.level1))
            .proxy
            .link(),
      if (par.isModal)
        FlatButton(
            onPressed: par.returnModalValue('Dialog modal result'),
            child: Text('Close')),
      RouteHelper.homeRoute.link(),
    ]);

@widget
Widget myDrawer(BuildContext context) => Drawer(
      child: Column(
          mainAxisAlignment: MainAxisAlignment.center, children: const []),
    );
