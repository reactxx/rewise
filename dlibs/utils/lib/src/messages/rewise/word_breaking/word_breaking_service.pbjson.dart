///
//  Generated code. Do not modify.
//  source: rewise/word_breaking/word_breaking_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

const Request2$json = const {
  '1': 'Request2',
  '2': const [
    const {'1': 'lang', '3': 1, '4': 1, '5': 9, '10': 'lang'},
    const {'1': 'facts', '3': 2, '4': 3, '5': 11, '6': '.rw.word_breaking.FactReq', '10': 'facts'},
    const {'1': 'path', '3': 3, '4': 1, '5': 9, '10': 'path'},
  ],
};

const FactReq$json = const {
  '1': 'FactReq',
  '2': const [
    const {'1': 'id', '3': 1, '4': 1, '5': 5, '10': 'id'},
    const {'1': 'text', '3': 2, '4': 1, '5': 9, '10': 'text'},
  ],
};

const Response2$json = const {
  '1': 'Response2',
  '2': const [
    const {'1': 'facts', '3': 1, '4': 3, '5': 11, '6': '.rw.word_breaking.FactResp', '10': 'facts'},
  ],
};

const FactResp$json = const {
  '1': 'FactResp',
  '2': const [
    const {'1': 'id', '3': 1, '4': 1, '5': 5, '10': 'id'},
    const {'1': 'text', '3': 2, '4': 1, '5': 9, '10': 'text'},
    const {'1': 'posLens', '3': 3, '4': 3, '5': 11, '6': '.rw.word_breaking.PosLen', '10': 'posLens'},
  ],
};

const Request$json = const {
  '1': 'Request',
  '2': const [
    const {'1': 'lang', '3': 1, '4': 1, '5': 9, '10': 'lang'},
    const {'1': 'facts', '3': 2, '4': 3, '5': 9, '10': 'facts'},
  ],
};

const Response$json = const {
  '1': 'Response',
  '2': const [
    const {'1': 'facts', '3': 1, '4': 3, '5': 11, '6': '.rw.word_breaking.Breaks', '10': 'facts'},
  ],
};

const Breaks$json = const {
  '1': 'Breaks',
  '2': const [
    const {'1': 'posLens', '3': 1, '4': 3, '5': 11, '6': '.rw.word_breaking.PosLen', '10': 'posLens'},
    const {'1': 'breaks', '3': 2, '4': 1, '5': 12, '10': 'breaks'},
  ],
};

const PosLen$json = const {
  '1': 'PosLen',
  '2': const [
    const {'1': 'pos', '3': 1, '4': 1, '5': 5, '10': 'pos'},
    const {'1': 'end', '3': 2, '4': 1, '5': 5, '10': 'end'},
  ],
};

