class PriorityQueue<T extends Comparable> {
  final LstHeap = new List<T>();

  int get Count => LstHeap.length;

  void Add(T val) {
    LstHeap.add(val);
    SetAt(LstHeap.length - 1, val);
    UpHeap(LstHeap.length - 1);
  }

  T Peek() {
    if (LstHeap.length == 0) 
      throw new Exception("Peeking at an empty priority queue");
    return LstHeap[0];
  }

  T Pop() {
    if (LstHeap.length == 0) {
      throw new Exception("Popping an empty priority queue");
    }

    T valRet = LstHeap[0];

    SetAt(0, LstHeap[LstHeap.length - 1]);
    LstHeap.removeAt(LstHeap.length - 1);
    DownHeap(0);
    return valRet;
  }

  void SetAt(int i, T val) {
    LstHeap[i] = val;
  }

  bool RightSonExists(int i) {
    return RightChildIndex(i) < LstHeap.length;
  }

  bool LeftSonExists(int i) {
    return LeftChildIndex(i) < LstHeap.length;
  }

  int ParentIndex(int i) {
    return (i - 1) ~/ 2;
  }

  int LeftChildIndex(int i) {
    return 2 * i + 1;
  }

  int RightChildIndex(int i) {
    return 2 * (i + 1);
  }

  T ArrayVal(int i) {
    return LstHeap[i];
  }

  T Parent(int i) {
    return LstHeap[ParentIndex(i)];
  }

  T Left(int i) {
    return LstHeap[LeftChildIndex(i)];
  }

  T Right(int i) {
    return LstHeap[RightChildIndex(i)];
  }

  void Swap(int i, int j) {
    T valHold = ArrayVal(i);
    SetAt(i, LstHeap[j]);
    SetAt(j, valHold);
  }

  void UpHeap(int i) {
    while (i > 0 && ArrayVal(i).compareTo(Parent(i)) > 0) {
      Swap(i, ParentIndex(i));
      i = ParentIndex(i);
    }
  }

  void DownHeap(int i) {
    while (i >= 0) {
      int iContinue = -1;

      if (RightSonExists(i) && Right(i).compareTo(ArrayVal(i)) > 0) {
        iContinue = Left(i).compareTo(Right(i)) < 0
            ? RightChildIndex(i)
            : LeftChildIndex(i);
      } else if (LeftSonExists(i) && Left(i).compareTo(ArrayVal(i)) > 0) {
        iContinue = LeftChildIndex(i);
      }

      if (iContinue >= 0 && iContinue < LstHeap.length) {
        Swap(i, iContinue);
      }

      i = iContinue;
    }
  }
}
