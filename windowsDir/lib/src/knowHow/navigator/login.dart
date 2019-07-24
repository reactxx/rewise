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

  static LoginStatus<UserInfo> of(BuildContext context, [bool listen]) {
    final st = Provider.of<LoginStatus>(context, listen: listen ?? false);
    assert(st != null);
    return st;
  }

  bool get logged => _userInfo!=null;

  T get userInfo => _userInfo;
  set userInfo(T value) {
    if (_userInfo == value) {
      return;
    }
    _userInfo = value;
    notifyListeners();
  }

  T _userInfo;

  @override
  bool login(BuildContext context,
      {route.RouteProxy<dynamic> fromRoute,
      route.RouteProxy<dynamic> fallBackRoute}) {
    if (_userInfo != null) {
      return false;
    }
    final proxy = LoginProxy<T>();
    proxy.navigate(context);
    proxy.done.future.then((u) {
      userInfo = u;
      if (fromRoute != null) {
        fromRoute.navigate();
      }
    }).catchError((dynamic err, StackTrace stack) {
      if (fallBackRoute != null) {
        fallBackRoute.navigateMethod(context);
      }
    });
    return true;
  }

  void logout() {
    if (_userInfo == null) {
      return;
    }
    final logoutCompleter = Completer<void>();
    doLogout(logoutCompleter);
    logoutCompleter.future.then<void>((_) => userInfo = null);
  }

  void doLogout(Completer<dynamic> done) {
    Timer(Duration(seconds: 1), done.complete);
  }
}

class LoginUtils {
  LoginUtils._();
}

@widget
Widget logger<T extends UserInfo>(
        BuildContext context, LoginStatus<T> status, {Widget child}) =>
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
          onPressed: () => Timer(Duration(seconds: 1), () => par.done.complete(UserInfo() as T)),
          child: Text('FACEBOOK')),
      FlatButton(
          onPressed: () => Timer(Duration(seconds: 1), () => par.done.complete(UserInfo() as T)),
          child: Text('GOOGLE')),
    ]));
