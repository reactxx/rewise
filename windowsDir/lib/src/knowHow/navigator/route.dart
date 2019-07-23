import 'package:flutter/material.dart';
import 'package:functional_widget_annotation/functional_widget_annotation.dart';

part 'route.g.dart';

enum RouteType { level0, level1, popup, fullscreenDialog }

typedef OnModalResult<TOut> = void Function(TOut result);
typedef OnModalError = dynamic Function(dynamic error, StackTrace stackTrace);
typedef SetParent<T extends RouteProxy<dynamic>> = RouteTemplate Function(
    T self);

typedef RouteLinkBuilder<TIn extends RouteProxy<TOut>, TOut> = Widget Function(
    BuildContext context, TIn proxy);

abstract class RouteProxy<TOut> {
  RouteProxy(
      {this.parent,
      this.type,
      this.onModalResult,
      this.onModalError,
      this.linkTitle}) {
    type ??= RouteType.level0;
  }

  final String linkTitle;
  String get appBarTitle => linkTitle;
  final OnModalResult<TOut> onModalResult;
  final OnModalError onModalError;
  RouteType type;
  RouteTemplate parent;

  bool get isModal =>
      type == RouteType.popup || type == RouteType.fullscreenDialog;

  // ?? obsolete
  RouteProxy setParent<T extends RouteProxy<dynamic>>(SetParent<T> setter) {
    parent = setter(this as dynamic);
    return this;
  }

  Widget build(BuildContext context);

  RouteLink link() => RouteLink<RouteProxy<TOut>, TOut>(this);

  Route<TOut> buildRoute() {
    // MaterialPageRoute.builder function
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
        RouteHelper.closeDrawer(context);
        // navigate to newRoute
        final newRoute = buildRoute();
        final navigator = RouteHelper.navigatorState;
        Future<dynamic> modalResult;
        if (isModal)
          modalResult = navigator.pushNamed<dynamic>('', arguments: newRoute);
        else
          // delete all routes or preserve level0 route (when this.type==level1)
          modalResult = navigator.pushNamedAndRemoveUntil<dynamic>('', (r) {
            // unknown proxy or this.type==level0 => remove (by returning false)
            if (r.settings.arguments == null ||
                r.settings.arguments is! RouteProxy ||
                type == RouteType.level0) {
              return false;
            }
            // now this.type==level1
            final RouteProxy proxy = r.settings.arguments;
            // preserve level0 route (by returning true)
            return proxy.type == RouteType.level0;
          }, arguments: newRoute);
        // process modal result
        if (onModalResult != null)
          modalResult.then((dynamic r) => onModalResult(r),
              onError: onModalError);
      };
}

class RouteTemplate {
  // as proxy parent
  RouteTemplate(this.proxy) : assert(proxy != null) {
    proxy.parent = this;
  }
  // as template parent
  RouteTemplate.subTemplate(this.subTemplate) : assert(subTemplate != null) {
    subTemplate.parent = this;
  }
  RouteProxy proxy; // proxy child
  RouteTemplate subTemplate; // template child
  RouteTemplate parent; // template parent
  Widget build(BuildContext context, Widget childWidget) => Scaffold(
      appBar: AppBar(
        title: Text(proxy?.appBarTitle ?? 'Unknown appBarTitle'),
        leading: proxy?.isModal == true ? null : OpenDrawerButton(),
      ),
      body: childWidget);
}

class RouteHelper {
  RouteHelper._();

  static RouteProxy<dynamic> homeRoute;
  // ?? obsolete
  static final navigatorObserver = History();
  static final scaffoldKey = GlobalKey<ScaffoldState>();
  static final navigatorKey = GlobalKey<NavigatorState>();
  static void closeDrawer(BuildContext context) {
    // close drawer if opened
    final DrawerControllerState drawerState =
        context.ancestorStateOfType(TypeMatcher<DrawerControllerState>());
    drawerState?.close();
  }

  static RouteFactory onGenerateRoute(RouteProxy<dynamic> home) {
    assert(home != null && home.type == RouteType.level0);
    homeRoute = home;
    return (RouteSettings settings) => settings.arguments ?? home.buildRoute();
  }

  static ScaffoldState get scaffoldState => scaffoldKey.currentState;
  static NavigatorState get navigatorState => navigatorKey.currentState;
}

@widget
Widget routeLink<TIn extends RouteProxy<TOut>, TOut>(
        BuildContext context, TIn proxy,
        {RouteLinkBuilder<TIn, TOut> builder}) =>
    builder != null
        ? builder(context, proxy)
        : FlatButton(
            textColor: Theme.of(context).primaryColor,
            onPressed: proxy.navigate(context),
            child:
                Text((proxy.linkTitle ?? 'unknown link title').toUpperCase()));

@widget
Widget openDrawerButton(BuildContext context) => IconButton(
      icon: Icon(Icons.menu),
      onPressed: RouteHelper.scaffoldState.openDrawer,
    );

// ?? obsolete
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
