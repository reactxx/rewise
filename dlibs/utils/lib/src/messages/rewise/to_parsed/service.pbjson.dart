///
//  Generated code. Do not modify.
//  source: rewise/to_parsed/service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

const Request$json = const {
  '1': 'Request',
  '2': const [
    const {'1': 'book', '3': 1, '4': 1, '5': 11, '6': '.rw.to_parsed.RawBook', '10': 'book'},
  ],
};

const RawBook$json = const {
  '1': 'RawBook',
  '2': const [
    const {'1': 'name', '3': 1, '4': 1, '5': 9, '10': 'name'},
    const {'1': 'facts', '3': 5, '4': 3, '5': 11, '6': '.rw.to_parsed.RawFact', '10': 'facts'},
    const {'1': 'lessons', '3': 6, '4': 3, '5': 5, '10': 'lessons'},
  ],
};

const RawFact$json = const {
  '1': 'RawFact',
  '2': const [
    const {'1': 'lang', '3': 1, '4': 1, '5': 9, '10': 'lang'},
    const {'1': 'words', '3': 2, '4': 3, '5': 9, '10': 'words'},
  ],
};

const Response$json = const {
  '1': 'Response',
  '2': const [
    const {'1': 'book', '3': 1, '4': 1, '5': 11, '6': '.rw.to_parsed.ParsedBook', '10': 'book'},
    const {'1': 'error', '3': 2, '4': 1, '5': 9, '10': 'error'},
  ],
};

const ParsedBook$json = const {
  '1': 'ParsedBook',
  '2': const [
    const {'1': 'lang', '3': 1, '4': 1, '5': 9, '10': 'lang'},
    const {'1': 'book_id', '3': 2, '4': 1, '5': 9, '10': 'bookId'},
    const {'1': 'facts', '3': 3, '4': 3, '5': 11, '6': '.rw.to_parsed.ParsedFact', '10': 'facts'},
  ],
};

const ParsedFact$json = const {
  '1': 'ParsedFact',
  '2': const [
    const {'1': 'id', '3': 1, '4': 1, '5': 5, '10': 'id'},
    const {'1': 'stemm_text', '3': 2, '4': 1, '5': 9, '10': 'stemmText'},
    const {'1': 'fact', '3': 3, '4': 1, '5': 11, '6': '.rw.dom.Fact', '10': 'fact'},
  ],
};

