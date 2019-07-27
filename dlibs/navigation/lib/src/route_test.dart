import 'package:flutter/material.dart';
import 'package:functional_widget_annotation/functional_widget_annotation.dart';
//import 'package:provider/provider.dart';
import 'package:navigation/main.dart';
//import 'login.dart';
//import 'route.dart';

part 'route_test.g.dart';

void main() => runApp(MyApp());

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
  DialogProxy(
    this.id, {
    String linkTitle,
    OnModalResult<String> onModalResult,
    OnModalError onModalError,
    RouteType type,
  }) : super(
          linkTitle: linkTitle,
          onModalResult: onModalResult,
          onModalError: onModalError,
          type: type,
        ) {
    parent = Template(this);
  }

  final int id;

  @override
  String get appBarTitle => '${super.appBarTitle} $id';
  @override
  Widget build(BuildContext context) => DialogView(this);
}

@widget
Widget myApp(BuildContext context) => Logger<UserInfo>(LoginStatus<UserInfo>(),
    child: MaterialApp(
        navigatorObservers: [RouteHelper.navigatorObserver], // !!!!
        onGenerateRoute: RouteHelper.onGenerateRoute(HomeProxy(),
            loginApiCreator: (context) {
          return LoginStatus.of(context);
        }), // !!!!
        navigatorKey: RouteHelper.navigatorKey, // !!!!
        title: 'Flutter Navig Demo',
        builder: (context, child) => Scaffold(
              key: RouteHelper.scaffoldKey, // !!!!
              drawer: DrawerContainer(child: MyDrawer()), // !!!!
              body: child,
            )));

@widget
Widget homeView(BuildContext context, HomeProxy par) => Column(crossAxisAlignment: CrossAxisAlignment.stretch, children: [
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
      NeedsLoginProxy().link(),
    ]);

@widget
Widget dialogView(BuildContext context, DialogProxy par) => Column(crossAxisAlignment: CrossAxisAlignment.stretch, children: [
      Text(par.id.toString()),
      DialogProxy(5, type: RouteType.popup, linkTitle: 'Popup dialog').link(),
      if (par.type == RouteType.level0)
        DialogProxy(6, type: RouteType.level1, linkTitle: 'Dialog level1')
            .link(),
      NeedsLoginProxy().link(),
      if (par.isModal)
        FlatButton(
            textColor: Theme.of(context).primaryColor,
            onPressed: par.returnModalValue('Dialog modal result'),
            child: Text('RETURN VALUE')),
      RouteHelper.homeRoute.link(),
    ]);

@widget
Widget myDrawer(BuildContext context) => Drawer(
        child: Column(crossAxisAlignment: CrossAxisAlignment.stretch, mainAxisAlignment: MainAxisAlignment.center, children: [
      NeedsLoginProxy().link(),
      DialogProxy(7, type: RouteType.popup, linkTitle: 'Popup dialog').link(),
      DialogProxy(8, linkTitle: 'Dialog level0').link(),
      DialogProxy(9, type: RouteType.level1, linkTitle: 'Dialog level1').link(),
      LoginBtn(),
      RouteHelper.homeRoute.link(),
    ]));

// *********** LOGIN

class NeedsLoginProxy extends RouteProxy<void> {
  NeedsLoginProxy()
      : super(
            linkTitle: 'Needs login',
            needsLogin: true,
            type: RouteType.level1) {
    parent = Template(this);
  }

  @override
  Widget build(BuildContext context) => NeedsLoginView(this);
}

@widget
Widget needsLoginView(BuildContext context, NeedsLoginProxy par) =>
    Text(LoginStatus.of(context).logged ? 'LOGGED' : 'NOT LOGGED');
