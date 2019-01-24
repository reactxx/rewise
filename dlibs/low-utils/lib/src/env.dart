final bool DEV__ = true;

final _traceLog = DEV__ ? List<String>() : null;

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
