import 'dart:typed_data';
import 'dart:convert';

import 'package:rewise_low_utils/toBinary.dart' as binary;

import 'node.dart';
import 'priorityQueue.dart';

class BuildInput<T extends Comparable> {
  int countAll = 0;
  Map<T, int> counts = Map<T, int>();
}

class BuildResult<T extends Comparable> {
  BuildResult(this.encodingMap, this.decodingTree, this.dump);
  Map<T, NodeEnc> encodingMap;
  String dump;
  Uint8List decodingTree;
}

class NodeEnc extends binary.BitData {
  NodeEnc(Uint8List bits, int bitsCount, this.dump) : super(bits, bitsCount);
  String dump;
}

abstract class KeyHandler<T extends Comparable> {
  String keyToDump(T key);
  void keyToBits(T key, binary.BitWriter wr);
}


BuildResult<T> build<T extends Comparable>(
    BuildInput<T> input, KeyHandler<T> handler) {
  final leafDictionary = Map<T, NodeDesign<T>>();
  final priorityQueue = PriorityQueue<NodeDesign<T>>();

  for (T value in input.counts.keys) {
    var node = NodeDesign<T>.leaf(input.counts[value] / input.countAll, value);
    priorityQueue.Add(node);
    leafDictionary[value] = node;
  }

  while (priorityQueue.Count > 1) {
    NodeDesign<T> leftSon = priorityQueue.Pop();
    NodeDesign<T> rightSon = priorityQueue.Pop();
    var parent = new NodeDesign<T>(leftSon, rightSon);
    priorityQueue.Add(parent);
  }

  Map<T, NodeEnc> encodingMap = Map.fromIterable(
    leafDictionary.entries,
    key: (item) => item.key,
    value: (item) => item.value.toBits(),
  );

  final root = priorityQueue.Pop();
  root.isZero = false;

  final wr = binary.BitWriter();
  root.encode(wr, handler.keyToBits);
  Uint8List decodingTree = wr.toBytes();

  String dump;
  assert(() {
    final dumpMap = Map.fromIterable(
      encodingMap.entries,
      key: (item) => handler.keyToDump(item.key),
      value: (item) => item.value.dump,
    );
    dump = jsonEncode(dumpMap);
    return true;
  }());

  return BuildResult(encodingMap, decodingTree, dump);
}
