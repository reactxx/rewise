// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'login.dart';

// **************************************************************************
// FunctionalWidgetGenerator
// **************************************************************************

class Logger<T extends UserInfo> extends StatelessWidget {
  const Logger(this.status, {Key key, this.child}) : super(key: key);

  final LoginStatus<T> status;

  final Widget child;

  @override
  Widget build(BuildContext _context) =>
      logger<T>(_context, status, child: child);
}

class LoginView<T extends UserInfo> extends StatelessWidget {
  const LoginView(this.par, {Key key}) : super(key: key);

  final LoginProxy<T> par;

  @override
  Widget build(BuildContext _context) => loginView<T>(_context, par);
}

class LoginBtn extends StatelessWidget {
  const LoginBtn({Key key}) : super(key: key);

  @override
  Widget build(BuildContext _context) => loginBtn(_context);
}
