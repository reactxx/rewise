// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'main.dart';

// **************************************************************************
// FunctionalWidgetGenerator
// **************************************************************************

class MyApp extends StatelessWidget {
  const MyApp({Key key}) : super(key: key);

  @override
  Widget build(BuildContext _context) => myApp(_context);
}

class MyDrawer extends StatelessWidget {
  const MyDrawer({Key key}) : super(key: key);

  @override
  Widget build(BuildContext _context) => myDrawer(_context);
}

class OpenDrawerButton extends StatelessWidget {
  const OpenDrawerButton({Key key}) : super(key: key);

  @override
  Widget build(BuildContext _context) => openDrawerButton(_context);
}

class RouteLink extends StatelessWidget {
  const RouteLink(this.routeName, this.routeTitle, this.isPopup,
      {Key key, this.arguments})
      : super(key: key);

  final String routeName;

  final String routeTitle;

  final bool isPopup;

  final Object arguments;

  @override
  Widget build(BuildContext _context) =>
      routeLink(_context, routeName, routeTitle, isPopup, arguments: arguments);
}

class HomeView extends StatelessWidget {
  const HomeView({Key key}) : super(key: key);

  @override
  Widget build(BuildContext _context) => homeView(_context);
}

class SubPageView extends StatelessWidget {
  const SubPageView({Key key}) : super(key: key);

  @override
  Widget build(BuildContext _context) => subPageView(_context);
}

class DialogView extends StatelessWidget {
  const DialogView({Key key}) : super(key: key);

  @override
  Widget build(BuildContext _context) => dialogView(_context);
}
