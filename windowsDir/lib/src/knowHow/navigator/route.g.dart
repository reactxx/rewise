// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'route.dart';

// **************************************************************************
// FunctionalWidgetGenerator
// **************************************************************************

class RouteLink<TIn extends NestedRoute<TOut>, TOut> extends StatelessWidget {
  const RouteLink({Key key, @required this.route, this.builder})
      : super(key: key);

  final TIn route;

  final Widget Function(BuildContext, TIn) builder;

  @override
  Widget build(BuildContext _context) =>
      routeLink<TIn, TOut>(_context, route: route, builder: builder);
}
