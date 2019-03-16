///
//  Generated code. Do not modify.
//  source: rewise/to_parsed/to_parsed_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

const RawBooks$json = const {
  '1': 'RawBooks',
  '2': const [
    const {'1': 'name', '3': 1, '4': 1, '5': 9, '10': 'name'},
    const {'1': 'books', '3': 2, '4': 3, '5': 11, '6': '.rw.to_parsed.RawBook', '10': 'books'},
    const {'1': 'lessons', '3': 3, '4': 3, '5': 5, '10': 'lessons'},
  ],
};

const RawBook$json = const {
  '1': 'RawBook',
  '2': const [
    const {'1': 'lang', '3': 1, '4': 1, '5': 9, '10': 'lang'},
    const {'1': 'facts', '3': 2, '4': 3, '5': 9, '10': 'facts'},
  ],
};

const ParsedBooks$json = const {
  '1': 'ParsedBooks',
  '2': const [
    const {'1': 'name', '3': 1, '4': 1, '5': 9, '10': 'name'},
    const {'1': 'books', '3': 2, '4': 3, '5': 11, '6': '.rw.to_parsed.ParsedBook', '10': 'books'},
  ],
};

const ParsedBook$json = const {
  '1': 'ParsedBook',
  '2': const [
    const {'1': 'lang', '3': 1, '4': 1, '5': 9, '10': 'lang'},
    const {'1': 'facts', '3': 2, '4': 3, '5': 11, '6': '.rw.to_parsed.ParsedFact', '10': 'facts'},
  ],
};

const ParsedFact$json = const {
  '1': 'ParsedFact',
  '2': const [
    const {'1': 'idx', '3': 1, '4': 1, '5': 5, '10': 'idx'},
    const {'1': 'lesson_id', '3': 2, '4': 1, '5': 5, '10': 'lessonId'},
    const {'1': 'childs', '3': 3, '4': 3, '5': 11, '6': '.rw.to_parsed.ParsedSubFact', '10': 'childs'},
    const {'1': 'brackets', '3': 4, '4': 3, '5': 11, '6': '.rw.to_parsed.Bracket', '10': 'brackets'},
  ],
};

const ParsedSubFact$json = const {
  '1': 'ParsedSubFact',
  '2': const [
    const {'1': 'text', '3': 1, '4': 1, '5': 9, '10': 'text'},
    const {'1': 'break_text', '3': 2, '4': 1, '5': 9, '10': 'breakText'},
    const {'1': 'word_class', '3': 3, '4': 1, '5': 9, '10': 'wordClass'},
  ],
};

const BracketBooks$json = const {
  '1': 'BracketBooks',
  '2': const [
    const {'1': 'name', '3': 1, '4': 1, '5': 9, '10': 'name'},
    const {'1': 'books', '3': 2, '4': 3, '5': 11, '6': '.rw.to_parsed.BracketBook', '10': 'books'},
  ],
};

const BracketBook$json = const {
  '1': 'BracketBook',
  '2': const [
    const {'1': 'lang', '3': 1, '4': 1, '5': 9, '10': 'lang'},
    const {'1': 'facts', '3': 2, '4': 3, '5': 11, '6': '.rw.to_parsed.Bracket', '10': 'facts'},
  ],
};

const Bracket$json = const {
  '1': 'Bracket',
  '2': const [
    const {'1': 'type', '3': 1, '4': 1, '5': 9, '10': 'type'},
    const {'1': 'value', '3': 2, '4': 1, '5': 9, '10': 'value'},
  ],
};

const ErrorBooks$json = const {
  '1': 'ErrorBooks',
  '2': const [
    const {'1': 'name', '3': 1, '4': 1, '5': 9, '10': 'name'},
    const {'1': 'books', '3': 2, '4': 3, '5': 11, '6': '.rw.to_parsed.ErrorBook', '10': 'books'},
  ],
};

const ErrorBook$json = const {
  '1': 'ErrorBook',
  '2': const [
    const {'1': 'lang', '3': 1, '4': 1, '5': 9, '10': 'lang'},
    const {'1': 'facts', '3': 2, '4': 3, '5': 11, '6': '.rw.to_parsed.Error', '10': 'facts'},
  ],
};

const Error$json = const {
  '1': 'Error',
  '2': const [
    const {'1': 'id', '3': 1, '4': 1, '5': 9, '10': 'id'},
    const {'1': 'code', '3': 2, '4': 1, '5': 5, '10': 'code'},
  ],
};

