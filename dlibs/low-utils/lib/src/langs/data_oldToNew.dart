
// design\langsDesign\cldr\cldr.cs generated code
import 'dart:convert' as convert;

oldToNewData() {
  if (_oldToNewData==null) {
    final res = convert.jsonDecode('''{
"-":"xal-US","":"xal-US","pa-in":"pa-Guru","quz-pe":"qu-PE","sw-ke":"sw-TZ","bn-in":"bn-BD","ha-latn-ng":"ha-NG","az-latn-az":"az-Latn","uz-latn-uz":"uz-Latn","bs-latn-ba":"bs-Latn","zh-cn":"zh-Hans","zh-sg":"zh-Hans-SG","zh-tw":"zh-Hant","zh-hk":"zh-Hant-HK","zh-mo":"zh-Hant-MO","sr-latn-cs":"sr-Latn","sr-cyrl-cs":"sr-Cyrl"
    }
      ''');
    _oldToNewData = Map<String, String>.from(res);
  }
  return _oldToNewData;
}
Map<String, String> _oldToNewData;
