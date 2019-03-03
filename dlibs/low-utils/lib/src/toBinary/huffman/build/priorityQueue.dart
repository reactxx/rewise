class PriorityQueue<T extends Comparable> {
  final _lstHeap = new List<T>();

  int get Count => _lstHeap.length;

  void Add(T val) {
    _lstHeap.add(val);
    SetAt(_lstHeap.length - 1, val);
    UpHeap(_lstHeap.length - 1);
  }

  T Peek() {
    if (_lstHeap.length == 0) 
      throw new Exception("Peeking at an empty priority queue");
    return _lstHeap[0];
  }

  T Pop() {
    if (_lstHeap.length == 0) {
      throw new Exception("Popping an empty priority queue");
    }

    T valRet = _lstHeap[0];

    SetAt(0, _lstHeap[_lstHeap.length - 1]);
    _lstHeap.removeAt(_lstHeap.length - 1);
    DownHeap(0);
    return valRet;
  }

  void SetAt(int i, T val) {
    _lstHeap[i] = val;
  }

  bool RightSonExists(int i) {
    return RightChildIndex(i) < _lstHeap.length;
  }

  bool LeftSonExists(int i) {
    return LeftChildIndex(i) < _lstHeap.length;
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
    return _lstHeap[i];
  }

  T Parent(int i) {
    return _lstHeap[ParentIndex(i)];
  }

  T Left(int i) {
    return _lstHeap[LeftChildIndex(i)];
  }

  T Right(int i) {
    return _lstHeap[RightChildIndex(i)];
  }

  void Swap(int i, int j) {
    T valHold = ArrayVal(i);
    SetAt(i, _lstHeap[j]);
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

      if (iContinue >= 0 && iContinue < _lstHeap.length) {
        Swap(i, iContinue);
      }

      i = iContinue;
    }
  }
}
