///
//  Generated code. Do not modify.
//  source: rewise/dom/dom.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

const Fact$json = const {
  '1': 'Fact',
  '2': const [
    const {'1': 'id', '3': 1, '4': 1, '5': 5, '10': 'id'},
    const {'1': 'text', '3': 2, '4': 1, '5': 9, '10': 'text'},
    const {'1': 'breaks', '3': 3, '4': 1, '5': 12, '10': 'breaks'},
    const {'1': 'right_ids', '3': 4, '4': 3, '5': 5, '10': 'rightIds'},
    const {'1': 'word_class', '3': 5, '4': 1, '5': 9, '10': 'wordClass'},
    const {'1': 'lesson_id', '3': 6, '4': 1, '5': 5, '10': 'lessonId'},
  ],
};

const BookMeta$json = const {
  '1': 'BookMeta',
  '2': const [
    const {'1': 'id', '3': 1, '4': 1, '5': 9, '10': 'id'},
    const {'1': 'title', '3': 2, '4': 1, '5': 9, '10': 'title'},
  ],
};

const WordMsg$json = const {
  '1': 'WordMsg',
  '2': const [
    const {'1': 'text', '3': 1, '4': 1, '5': 9, '10': 'text'},
    const {'1': 'before', '3': 2, '4': 1, '5': 9, '10': 'before'},
    const {'1': 'after', '3': 3, '4': 1, '5': 9, '10': 'after'},
    const {'1': 'flags', '3': 4, '4': 1, '5': 5, '10': 'flags'},
    const {'1': 'flags_data', '3': 5, '4': 1, '5': 9, '10': 'flagsData'},
  ],
};

const FactMsg$json = const {
  '1': 'FactMsg',
  '2': const [
    const {'1': 'word_class', '3': 1, '4': 1, '5': 9, '10': 'wordClass'},
    const {'1': 'flags', '3': 2, '4': 1, '5': 5, '10': 'flags'},
    const {'1': 'words', '3': 3, '4': 3, '5': 11, '6': '.rw.dom.WordMsg', '10': 'words'},
  ],
};

const FactsMsg$json = const {
  '1': 'FactsMsg',
  '2': const [
    const {'1': 'crc', '3': 1, '4': 1, '5': 5, '10': 'crc'},
    const {'1': 'as_string', '3': 2, '4': 1, '5': 9, '10': 'asString'},
    const {'1': 'facts', '3': 3, '4': 3, '5': 11, '6': '.rw.dom.FactMsg', '10': 'facts'},
    const {'1': 'lesson', '3': 4, '4': 1, '5': 9, '10': 'lesson'},
  ],
};

const FileMsg$json = const {
  '1': 'FileMsg',
  '2': const [
    const {'1': 'left_lang', '3': 1, '4': 1, '5': 9, '10': 'leftLang'},
    const {'1': 'book_name', '3': 2, '4': 1, '5': 9, '10': 'bookName'},
    const {'1': 'lang', '3': 3, '4': 1, '5': 9, '10': 'lang'},
    const {'1': 'book_type', '3': 4, '4': 1, '5': 14, '6': '.rw.dom.FileMsg.BookType', '10': 'bookType'},
    const {'1': 'file_type', '3': 5, '4': 1, '5': 14, '6': '.rw.dom.FileMsg.FileType', '10': 'fileType'},
    const {'1': 'factss', '3': 6, '4': 3, '5': 11, '6': '.rw.dom.FactsMsg', '10': 'factss'},
  ],
  '4': const [FileMsg_BookType$json, FileMsg_FileType$json],
};

const FileMsg_BookType$json = const {
  '1': 'BookType',
  '2': const [
    const {'1': 'KDICT', '2': 0},
    const {'1': 'DICT', '2': 1},
    const {'1': 'ETALK', '2': 2},
    const {'1': 'BOOK', '2': 3},
  ],
};

const FileMsg_FileType$json = const {
  '1': 'FileType',
  '2': const [
    const {'1': 'LEFT', '2': 0},
    const {'1': 'LANGLEFT', '2': 1},
    const {'1': 'LANG', '2': 2},
  ],
};

