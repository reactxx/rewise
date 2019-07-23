// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'route.dart';

// **************************************************************************
// FunctionalWidgetGenerator
// **************************************************************************

class RouteLink<TIn extends RouteProxy<TOut>, TOut> extends StatelessWidget {
  const RouteLink(this.route, {Key key, this.builder}) : super(key: key);

  final TIn route;

  final Widget Function(BuildContext, TIn) builder;

  @override
  Widget build(BuildContext _context) =>
      routeLink<TIn, TOut>(_context, route, builder: builder);
}

class OpenDrawerButton extends StatelessWidget {
  const OpenDrawerButton({Key key}) : super(key: key);

  @override
  Widget build(BuildContext _context) => openDrawerButton(_context);
}
