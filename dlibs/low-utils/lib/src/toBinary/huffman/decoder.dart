class Node<T> {
  Node<T> LeftSon;
  Node<T> RightSon;
  T Value;
  bool get IsLeaf => LeftSon==null && RightSon==null;
}
