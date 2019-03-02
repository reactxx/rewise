import 'dart:convert';

import 'encoderNode.dart';
import 'priorityQueue.dart';

class Tree<T extends Comparable> {
  Map<T, NodeDesign<T>> leafDictionary;
  NodeDesign<T> root;
  Map<T, NodeEnc> encodings;

  Tree.fromCounts(Map<T, int> counts) {
    leafDictionary = Map<T, NodeDesign<T>>();
    var priorityQueue = new PriorityQueue<NodeDesign<T>>();
    int valueCount = 0;

    for (T value in counts.keys) {
      var node = NodeDesign<T>.leaf(counts[value] / valueCount, value);
      priorityQueue.Add(node);
      leafDictionary[value] = node;
    }

    while (priorityQueue.Count > 1) {
      NodeDesign<T> leftSon = priorityQueue.Pop();
      NodeDesign<T> rightSon = priorityQueue.Pop();
      var parent = new NodeDesign<T>(leftSon, rightSon);
      priorityQueue.Add(parent);
    }

    encodings = Map.fromIterable(
      leafDictionary.entries,
      key: (item) => item.key,
      value:(item) => item.value.adjustBits(),
    );

    root = priorityQueue.Pop();
    root.IsZero = false;
  }
  factory Tree(Iterable<T> data) {
    final counts = Map<T, int>();
    for (var d in data) counts.update(d, (v) => v++, ifAbsent: () => 1);
    return Tree<T>.fromCounts(counts);
  }

  String dump() {
    final m = Map.fromIterable(
      encodings.entries,
      key: (item) => keyToDump(item.key),
      value:(item) => item.value.dump,
    );
    return jsonEncode(m);
  }

  String keyToDump(T key) {
    return key.toString();
  }

}

class StringTree extends Tree<int> {
  factory StringTree(String data) {
    final counts = Map<int, int>();
    for (var i = 0; i < data.length; i++)
      counts.update(data.codeUnitAt(i), (v) => v + 1, ifAbsent: () => 1);
    return StringTree.fromCounts(counts);
  }
  StringTree.fromCounts(Map<int, int> counts) : super.fromCounts(counts) {}

  @override
  String keyToDump(int key) {
    return String.fromCharCodes([key]);
  }


}
