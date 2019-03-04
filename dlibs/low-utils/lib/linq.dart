const int maxInt = 0xffffff;
const int minInt = -2147483648;

Iterable<int> range(int from, [int to]) sync* {
  for(var i=from; i<(to==null ? maxInt : to); i++)
  yield i;
}

num sum<T extends num>(Iterable<T> seq, [T fn(T x)]) =>
    seq.fold(0, (prev, element) => prev + (fn != null ? fn(element) : element));

num min<T extends num>(Iterable<T> seq) => seq.fold(maxInt,
    (prev, element) => element < prev ? element : prev);

num max<T extends num>(Iterable<T> seq)  => seq.fold(minInt,
    (prev, element) => element > prev ? element : prev);

Iterable<T> concat<T>(Iterable<T> seq, Iterable<T> withSeq) sync* {
  yield* seq;
  yield* withSeq;
}

// import 'dart:collection';
// //import 'dart:mirrors';

// wrap(value, fn(x)) => fn(value);

// order(List seq,
//         {Comparator by, List<Comparator> byAll, on(x), List<Function> onAll}) =>
//     by != null
//         ? (seq..sort(by))
//         : byAll != null
//             ? (seq
//               ..sort((a, b) => byAll.firstWhere((compare) => compare(a, b) != 0,
//                   orElse: () => (x, y) => 0)(a, b)))
//             : on != null
//                 ? (seq..sort((a, b) => on(a).compareTo(on(b))))
//                 : onAll != null
//                     ? (seq
//                       ..sort((a, b) => wrap(
//                           onAll.firstWhere(
//                               (_on) => _on(a).compareTo(_on(b)) != 0,
//                               orElse: () => (x) => 0),
//                           (_on) => _on(a).compareTo(_on(b)))))
//                     : (seq..sort());

// caseInsensitiveComparer(a, b) => a.toUpperCase().compareTo(b.toUpperCase());

// List<Group> group(Iterable seq,
//     {by(x): null, Comparator matchWith: null, valuesAs(x): null}) {
//   var map = Map<dynamic, Group>();
//   seq.forEach((x) {
//     var val = by(x);
//     var key = matchWith != null
//         ? map.keys.firstWhere((k) => matchWith(val, k) == 0, orElse: () => val)
//         : val;

//     if (!map.containsKey(key)) map[key] = Group(val);

//     if (valuesAs != null) x = valuesAs(x);

//     map[key].add(x);
//   });
//   return map.values.toList();
// }

// class Group extends IterableBase {
//   var key;
//   List _list;
//   Group(this.key) : _list = [];

//   get iterator => _list.iterator;
//   void add(e) => _list.add(e);
//   get values => _list;
// }

// toMap(List seq, f(x)) {
//   var map = {};
//   seq.forEach((x) => map[f(x)] = x);
//   return map;
// }

// // ofType(List seq, type) => seq.where(
// //     (x) => reflect(x).type.qualifiedName == reflectClass(type).qualifiedName);

// avg(Iterable seq) => sum(seq) / seq.length;

// concat(Iterable seq, Iterable withSeq) => seq.toList()..addAll(withSeq);

// bool seqEq(Iterable seq, Iterable withSeq) =>
//     seq.length == withSeq.length &&
//     range(seq.length).every((i) => seq.elementAt(i) == withSeq.elementAt(i));

// join(Iterable seq, Iterable withSeq, bool match(x, y)) =>
//     seq.expand((x) => withSeq.where((y) => match(x, y)).map((y) => [x, y]));

// joinGroup(Iterable seq, Iterable withSeq, bool match(x, y)) =>
//     group(join(seq, withSeq, match), by: (j) => j[0]);

