import 'package:tuple/tuple.dart';
//BETTER: https://github.com/mezoni/queries

const int maxInt = 0x7fffffff;
const int minInt = -maxInt;

class Linq {
  static Iterable<int> range(int from, [int length]) sync* {
    for (var i = from; i < (length == null ? maxInt : from + length); i++)
      yield i;
  }

  static num sum<T extends num>(Iterable<T> seq, [T fn(T x)]) => seq.fold(
      0, (prev, element) => prev + (fn != null ? fn(element) : element));

  static num min<T extends num>(Iterable<T> seq) =>
      seq.fold(maxInt, (prev, element) => element < prev ? element : prev);

  static num max<T extends num>(Iterable<T> seq) =>
      seq.fold(minInt, (prev, element) => element > prev ? element : prev);

  static Iterable<T> concat<T>(Iterable<T> seq, Iterable<T> withSeq) sync* {
    yield* seq;
    yield* withSeq;
  }

  static Iterable<V> selectMany<T, V>(
      Iterable<T> seq, Iterable<V> fn(T x)) sync* {
    if (seq != null) for (final x in seq) yield* fn(x);
  }

  static Iterable<Tuple2<T1, T2>> zip<T1, T2>(
      Iterable<T1> seq1, Iterable<T2> seq2) sync* {
    final iter1 = seq1.iterator, iter2 = seq2.iterator;
    bool canNext1, canNext2;
    while (true) {
      canNext1 = iter1.moveNext();
      canNext2 = iter2.moveNext();
      if (!canNext1 && !canNext2) break;
      yield Tuple2(
          canNext1 ? iter1.current : null, canNext2 ? iter2.current : null);
    }
  }

  static Iterable<T> distinct<T>(Iterable<T> seq) => Set<T>.from(seq);
  // from https://github.com/mezoni/queries
  // static Iterable<E> distinct<E>(Iterable<E> source,
  //     {bool equals(E e1, E e2), int hashCode(E e)}) {
  //   Iterable<E> generator() sync* {
  //     var hashSet = HashSet(equals: equals, hashCode: hashCode);
  //     var it = source.iterator;
  //     while (it.moveNext()) {
  //       var current = it.current;
  //       if (hashSet.add(current)) yield current;
  //     }
  //   }

  //   return generator();
  // }

  static List<Group<TKey, TValue>> group<TKeyValue, TKey, TValue>(
      Iterable<TKeyValue> seq, TKey by(TKeyValue kv),
      {TValue valuesAs(TKeyValue kv)}) {
    var map = Map<TKey, Group<TKey, TValue>>();
    for (final x in seq) {
      final key = by(x);
      TValue value = valuesAs == null ? x : valuesAs(x);
      map.update(key, (group) => group..values.add(value),
          ifAbsent: () => Group(key)..values.add(value));
    }
    return map.values.toList();
  }
}

class Group<TKey, TValue> {
  TKey key;
  final values = List<TValue>();
  Group(this.key);
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
