import 'package:flutter/material.dart';
import 'package:functional_widget_annotation/functional_widget_annotation.dart';
import 'route.dart';

part 'route_test.g.dart';

class HomeRoute extends NestedRoute<void> {
  HomeRoute({TemplatedRoute parent}): super(title: 'Home', parent: parent);
  @override
  Widget build(BuildContext context) => HomeView();
}

class DialogRoute extends NestedRoute<String> {
  DialogRoute(this.name, this.id,
      {String title = 'Dialog',
      TemplatedRoute parent,
      NestedRouteType type = NestedRouteType.popup,
      OnModalResult<String> onModalResult,
      OnModalError onModalError})
      : super(
            parent: parent,
            title: title,
            type: type,
            onModalResult: onModalResult,
            onModalError: onModalError);
  @override
  Widget build(BuildContext context) => DialogView(this);
  final String name;
  final int id;
}

class Template extends TemplatedRoute {
  Template({this.title}) : super();

  String title;

  @override
  Widget build(BuildContext context, Widget childWidget) => Scaffold(
      appBar: AppBar(
        title: Text(title),
      ),
      body: childWidget);
}

void main() => runApp(MyApp());

@widget
Widget myApp(BuildContext context) => MaterialApp(
    navigatorObservers: [RouterHelper.navigatorObserver], // !!!!
    onGenerateRoute: RouterHelper.onGenerateRoute(HomeRoute()), // !!!!
    title: 'Flutter Navig Demo',
    builder: (context, child) {
      return Scaffold(
        key: RouterHelper.scaffoldKey, // !!!!
        drawer: MyDrawer(),
        body: child,
      );
    });

@widget
Widget homeView(BuildContext context) => Column(children: [
      RouteLink<DialogRoute, String>(
          route: DialogRoute('User11', 11,
              onModalResult: (r) => showDialog<dynamic>(
                  context: context,
                  builder: (context) => AlertDialog(
                        content: Text(r ?? 'Canceled'),
                      )))),
      RouteLink<DialogRoute, String>(
          route: DialogRoute('User22', 22, type: null, title: 'Dialog as main')),
    ]);

@widget
Widget dialogView(BuildContext context, DialogRoute par) => Column(children: [
      Text(par.name),
      Text(par.id.toString()),
      FlatButton(
          onPressed: par.type != null
              ? par.returnModalValue('Dialog modal result')
              : RouterHelper.homeRoute.navigate(context),
          child: Text('Close')),
    ]);

@widget
Widget myDrawer(BuildContext context) => Drawer(
      child: Column(
          mainAxisAlignment: MainAxisAlignment.center, children: const []),
    );
