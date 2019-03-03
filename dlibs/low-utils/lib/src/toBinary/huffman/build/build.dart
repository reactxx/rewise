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
  Map<T, binary.NodeEnc> encodingMap;
  String dump;
  Uint8List decodingTree;
}

abstract class KeyHandler<T extends Comparable> {
  String keyToDump(T key) => key.toString();
  void keyToBits(T key, binary.BitWriter wr);
  BuildInput<T> getBuildInput<T extends Comparable>(Iterable<T> data) {
    final input = BuildInput<T>();
    for(var d in data) {
      input
      ..counts.update(d, (v) => v++, ifAbsent: () => 1)
      ..countAll = input.countAll + 1;
    }
    return input;
  }
}

class StringKeyHandler extends KeyHandler<int> {
  @override
  String keyToDump(int key) => String.fromCharCodes([key]);
  @override
  void keyToBits(int key, binary.BitWriter wr) {
    binary.encode_8_16(key, wr);
  }
  static BuildInput<int> getStringInput(String data) {
    final counts = Map<int, int>();
    for (var i = 0; i < data.length; i++)
      counts.update(data.codeUnitAt(i), (v) => v + 1, ifAbsent: () => 1);
    return BuildInput<int>()
    ..counts = counts
    ..countAll = data.length;
  }

  static final instance = StringKeyHandler();
}

BuildResult<int> stringBuildFromData(String input) =>
    build(StringKeyHandler.getStringInput(input), StringKeyHandler.instance);

BuildResult<int> stringBuild(BuildInput<int> input) =>
    build(input, StringKeyHandler.instance);

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

  Map<T, binary.NodeEnc> encodingMap = Map.fromIterable(
    leafDictionary.entries,
    key: (item) => item.key,
    value: (item) => item.value.toBits(),
  );

  final root = priorityQueue.Pop();
  root.isZero = false;

  final wr = binary.BitWriter();
  root.binaryEncode(handler.keyToBits, wr);
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
