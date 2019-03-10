import 'data_langsData.dart' show getLangsData;
import 'package:rewise_low_utils/messages.dart' show CldrLang;


class Langs {
  static List<CldrLang> get meta  => _meta ?? (_meta = getLangsData().langs);
  static List<CldrLang> _meta;
  static Map<String, CldrLang> get nameToMeta => _nameToMeta ?? (_nameToMeta = Map<String, CldrLang>.fromIterable(meta,
    key: (item) => item.id,
    value: (item) => item
    ));
  static Map<String, CldrLang> _nameToMeta;
}