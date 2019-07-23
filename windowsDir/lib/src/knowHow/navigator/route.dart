import 'package:flutter/material.dart';
import 'package:functional_widget_annotation/functional_widget_annotation.dart';

part 'route.g.dart';

enum RouteType { level0, level1, popup, fullscreenDialog }

typedef OnModalResult<TOut> = void Function(TOut result);
typedef OnModalError = dynamic Function(dynamic error, StackTrace stackTrace);

typedef RouteLinkBuilder<TIn extends RouteProxy<TOut>, TOut> = Widget Function(
    BuildContext context, TIn proxy);

abstract class RouteProxy<TOut> {
  RouteProxy(
      {this.parent,
      this.type,
      this.onModalResult,
      this.onModalError,
      this.title}) {
    type ??= RouteType.level0;
  }

  final String title;
  RouteType type;
  final OnModalResult<TOut> onModalResult;
  final OnModalError onModalError;
  RouteTemplate parent;

  bool get isPopupType =>
      type == RouteType.popup || type == RouteType.fullscreenDialog;

  void setParent(RouteTemplate setter(RouteProxy self)) =>
      parent = setter(this);

  Widget build(BuildContext context);

  // MaterialPageRoute.builder function
  Route<TOut> buildRoute() {
    Widget routeBuilder(BuildContext context) {
      Widget w = build(context);
      RouteTemplate p = parent;
      while (p != null) {
        w = p.build(context, w);
        p = p.parent;
      }
      return w;
    }

    return MaterialPageRoute<TOut>(
      settings: RouteSettings(arguments: this),
      builder: routeBuilder,
      fullscreenDialog: type == RouteType.fullscreenDialog,
    );
  }

  VoidCallback returnModalValue(TOut value) =>
      () => RouteHelper.navigatorState.pop(value);

  VoidCallback navigate(BuildContext context) => () {
        // close drawer if opened
        final DrawerControllerState drawerState =
            context.ancestorStateOfType(TypeMatcher<DrawerControllerState>());
        drawerState?.close();
        // navigate
        final navigator = RouteHelper.navigatorState;
        Future<dynamic> modalResult;
        final route = buildRoute();
        if (isPopupType)
          modalResult = navigator.pushNamed<dynamic>('', arguments: route);
        else
          modalResult = navigator.pushNamedAndRemoveUntil<dynamic>('', (r) {
            // unknown route or new route has level0 => remove (returning false for it)
            if (r.settings.arguments == null ||
                r.settings.arguments is! RouteProxy ||
                type == RouteType.level0) {
              return false;
            }
            // now, new route has level1
            final RouteProxy proxy = r.settings.arguments;
            // preserve level0 route (returning true for it)
            return proxy.type == RouteType.level0;
          }, arguments: route);
        // process modal result
        if (onModalResult != null)
          modalResult.then((dynamic r) => onModalResult(r),
              onError: onModalError);
      };
}

class RouteTemplate {
  const RouteTemplate(
    this.proxy, {
    this.parent,
    this.appBarTitle,
  });
  final RouteProxy proxy;
  final String appBarTitle;
  final RouteTemplate parent;
  Widget build(BuildContext context, Widget childWidget) => Scaffold(
      appBar: AppBar(
        title: Text(appBarTitle ?? proxy.title),
        leading: proxy?.type == null ? OpenDrawerButton() : null,
      ),
      body: childWidget);
}

class RouteHelper {
  RouteHelper._();

  static RouteProxy<dynamic> homeRoute;
  static final navigatorObserver = History();
  static final scaffoldKey = GlobalKey<ScaffoldState>();

  static RouteFactory onGenerateRoute(RouteProxy<dynamic> home) {
    assert(home != null && home.type == RouteType.level0);
    homeRoute = home;
    return (RouteSettings settings) => settings.arguments ?? home.buildRoute();
  }

  static ScaffoldState get scaffoldState => scaffoldKey.currentState;
  static NavigatorState get navigatorState => navigatorObserver.navigator;
}

@widget
Widget routeLink<TIn extends RouteProxy<TOut>, TOut>(
        BuildContext context, TIn route,
        {RouteLinkBuilder<TIn, TOut> builder}) =>
    builder != null
        ? builder(context, route)
        : FlatButton(
            onPressed: route.navigate(context),
            child: Text((route.title ?? 'unknown title').toUpperCase()));

@widget
Widget openDrawerButton(BuildContext context) => IconButton(
      icon: Icon(Icons.menu),
      onPressed: RouteHelper.scaffoldState.openDrawer,
    );

class History extends NavigatorObserver {
  final history = <Route>[];
  @override
  void didPush(Route<dynamic> route, Route<dynamic> previousRoute) {
    assert(previousRoute == null || history.last == previousRoute);
    history.add(route);
  }

  @override
  void didPop(Route<dynamic> route, Route<dynamic> previousRoute) {
    assert(history.last == route);
    history.removeLast();
  }

  @override
  void didRemove(Route<dynamic> route, Route<dynamic> previousRoute) {
    final removed = history.remove(route);
    assert(removed);
  }

  @override
  void didReplace({Route<dynamic> newRoute, Route<dynamic> oldRoute}) {
    final idx = history.indexOf(oldRoute);
    assert(idx >= 0);
    history[idx] = newRoute;
  }
}
