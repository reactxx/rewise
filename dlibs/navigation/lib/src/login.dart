import 'dart:async';

import 'package:flutter/material.dart';
import 'package:functional_widget_annotation/functional_widget_annotation.dart';
import 'package:provider/provider.dart';
import 'route.dart' as route;

part 'login.g.dart'; 

class UserInfo {}

class LoginStatus<T extends UserInfo> 
    with ChangeNotifier
    implements route.LoginApi {
  static LoginStatus<UserInfo> of(BuildContext context, [bool listen]) =>
      Provider.of<LoginStatus>(context, listen: listen ?? false);

  bool get logged => _userInfo != null;

  T get userInfo => _userInfo;
  set userInfo(T value) {
    if (_userInfo == value) {
      return;
    }
    _userInfo = value;
    notifyListeners();
  }

  T _userInfo;

  void toggle(
      {route.RouteProxy<dynamic> fromRoute,
      route.RouteProxy<dynamic> fallBackRoute}) {
    if (logged)
      logout(fallBackRoute: fallBackRoute);
    else
      login(fallBackRoute: fallBackRoute, fromRoute: fromRoute);
  }

  @override
  bool login(
      {route.RouteProxy<dynamic> fromRoute,
      route.RouteProxy<dynamic> fallBackRoute}) {
    route.RouteHelper.closeDrawer();
    if (logged) {
      return false;
    }
    final proxy = LoginProxy<T>();
    proxy.navigate();
    proxy.done.future.then((u) {
      userInfo = u;
      if (fromRoute != null) {
        fromRoute.navigate();
      }
    }).catchError((dynamic err, StackTrace stack) {
      if (fallBackRoute != null) {
        fallBackRoute.navigate();
      }
    });
    return true;
  }

  void logout({route.RouteProxy<dynamic> fallBackRoute}) {
    route.RouteHelper.closeDrawer();
    if (!logged) {
      return;
    }
    final logoutCompleter = Completer<void>();
    doLogout(logoutCompleter);
    logoutCompleter.future.then<void>((_) {
      userInfo = null;
      if (route.RouteHelper.navigatorObserver.anyNeedsLogin)
        fallBackRoute.navigate();
    });
  }

  void doLogout(Completer<dynamic> done) {
    Timer(Duration(seconds: 1), done.complete);
  }
}

class LoginUtils {
  LoginUtils._();
}

@widget
Widget logger<T extends UserInfo>(BuildContext context, LoginStatus<T> status,
        {Widget child}) =>
    ChangeNotifierProvider<LoginStatus<T>>.value(
      value: status,
      child: child,
    );

class LoginProxy<T extends UserInfo> extends route.RouteProxy<T> {
  LoginProxy()
      : super(linkTitle: 'Login', type: route.RouteType.fullscreenDialog);

  final done = Completer<T>();
  @override
  Widget build(BuildContext context) => LoginView(this);
}

@widget
Widget loginView<T extends UserInfo>(BuildContext context, LoginProxy<T> par) =>
    Center(
        child: Row(children: [
      FlatButton(
          onPressed: () => Timer(
              Duration(seconds: 1), () => par.done.complete(UserInfo() as T)),
          textColor: Theme.of(context).primaryColor,
          child: Text('FACEBOOK')),
      FlatButton(
          onPressed: () => Timer(
              Duration(seconds: 1), () => par.done.complete(UserInfo() as T)),
          textColor: Theme.of(context).primaryColor,
          child: Text('GOOGLE')),
    ]));

@widget
Widget loginBtn(BuildContext context) => FlatButton(
    onPressed: () => LoginStatus.of(context).toggle(
        fromRoute: route.RouteHelper.currentProxy(),
        fallBackRoute: route.RouteHelper.homeRoute),
    textColor: Theme.of(context).primaryColor,
    child: Text(LoginStatus.of(context).logged ? 'LOGGOF' : 'LOGIN'));
