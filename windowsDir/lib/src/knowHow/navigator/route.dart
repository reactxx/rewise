import 'package:flutter/material.dart';
import 'package:functional_widget_annotation/functional_widget_annotation.dart';

part 'route.g.dart';

enum NestedRouteType { popup, fullscreenDialog }

typedef OnModalResult<TOut> = void Function(TOut result);
typedef OnModalError = dynamic Function(dynamic error, StackTrace stackTrace);

typedef LinkRouteBuilder<TIn extends NestedRoute<TOut>, TOut> = Widget Function(
    BuildContext context, TIn nestedRoute);

// class NestedRouteArguments<TOut> {
//   const NestedRouteArguments(this.title,
//       {this.type, this.onModalResult, this.onModalError});

//   final String title;
//   final NestedRouteType type;
//   final OnModalResult<TOut> onModalResult;
//   final OnModalError onModalError;

//   VoidCallback onReturnModalValue(TOut value) =>
//       () => RouterHelper.navigatorState.pop(value);

// }

abstract class NestedRoute<TOut> {
  const NestedRoute(
      {this.parent,
      this.type,
      this.onModalResult,
      this.onModalError,
      this.title});

  final String title;
  final NestedRouteType type;
  final OnModalResult<TOut> onModalResult;
  final OnModalError onModalError;
  //final NestedRouteArguments<TOut> arguments;
  final TemplatedRoute parent;

  Widget build(BuildContext context);

  Route<TOut> buildRoute() {
    // MaterialPageRoute.builder function
    Widget routeBuilder(BuildContext context) {
      Widget w = build(context);
      TemplatedRoute p = parent;
      while (p != null) {
        w = p.build(context, w);
        p = p.parent;
      }
      return w;
    }

    return MaterialPageRoute<TOut>(
      settings: RouteSettings(arguments: this),
      builder: routeBuilder,
      fullscreenDialog: type == NestedRouteType.fullscreenDialog,
    );
  }

  VoidCallback returnModalValue(TOut value) =>
      () => RouterHelper.navigatorState.pop(value);

  VoidCallback navigate(BuildContext context) => () {
        // close drawer if opened
        final DrawerControllerState drawerState =
            context.ancestorStateOfType(TypeMatcher<DrawerControllerState>());
        drawerState?.close();
        // navigate
        final navigator = RouterHelper.navigatorState;
        Future<dynamic> modalResult;
        final route = buildRoute();
        (type == NestedRouteType.fullscreenDialog ||
                type == NestedRouteType.popup)
            ? modalResult = navigator.pushNamed<dynamic>('', arguments: route)
            : modalResult = navigator.pushNamedAndRemoveUntil<dynamic>(
                '', (r) => false,
                arguments: route);
        // process modal result
        if (onModalResult != null)
          modalResult.then((dynamic r) => onModalResult(r),
              onError: onModalError);
      };
}

abstract class TemplatedRoute {
  const TemplatedRoute({
    this.parent,
  });
  final TemplatedRoute parent;
  Widget build(BuildContext context, Widget childWidget);
}

class RouterHelper {
  RouterHelper._();

  static NestedRoute<dynamic> homeRoute;
  static final navigatorObserver = History();
  static final scaffoldKey = GlobalKey<ScaffoldState>();

  static RouteFactory onGenerateRoute(NestedRoute<dynamic> home) {
    homeRoute = home;
    return (RouteSettings settings) => settings.arguments ?? home.buildRoute();
  }

  static ScaffoldState get scaffoldState => scaffoldKey.currentState;
  static NavigatorState get navigatorState => navigatorObserver.navigator;
}

@widget
Widget routeLink<TIn extends NestedRoute<TOut>, TOut>(BuildContext context,
        {@required TIn route, LinkRouteBuilder<TIn, TOut> builder}) =>
    builder != null
        ? builder(context, route)
        : FlatButton(
            onPressed: route.navigate(context),
            child: Text(route.title?.toUpperCase()));

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
