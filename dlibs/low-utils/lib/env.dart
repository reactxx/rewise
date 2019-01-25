bool DEV__ = false;

final _traceLog = DEV__ ? List<String>() : null;

void traceFunc(String getMsg()) {
  if (!DEV__) return;
  trace(getMsg());
}

void trace(String msg) {
  if (_traceLog == null) return;
  _traceLog.add(msg);
}

void clearTrace() {
  if (_traceLog == null) return;
  _traceLog.clear();
}

String getTrace() {
  if (_traceLog == null) return null;
  return _traceLog.join('\n');
}
