import 'package:test/test.dart' as test;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/dom/word_breaking.dart' as wb;

main() {
  test.group("EXTRACT WORDS", () {
    test.test('breaking', () async {
      final req = wb.Request2()
        ..lang = 'en-GB'
        ..path = 'xxx';
      var emos = emo.split(' ').where((e) =>e.isNotEmpty).toList();
      req.facts.addAll(emos.map((e) => wb.FactReq()
        ..text = e
        ..id = 1));
      final resp = await client.WordBreaking_Run2(req);

      var emjs = resp.facts.map((f) {
        if (f.posLens.length!=1) return null;
        return '${f.text} ===> ${f.text.substring(f.posLens[0].pos, f.posLens[0].end)}';
      }).where((s) => s!=null).join('\n');
      test.expect(emjs, test.equals(emoRes));

    });
  });
}
const emo = ''':‑) :) :-] :] :-3 :3 :-> :> 8-) 8) :-} :} :o) :c) :^) =] =) :‑D :D 8‑D 8D x‑D xD X‑D XD =D =3 B^D :-)) :‑( :( :‑c :c :‑< :< :‑[ :[ :-|| >:[ :{ :@ >:( :'‑( :'( :'‑) :')  D‑': D:< D: D8 D; D= DX :‑O :O :‑o :o :-0 8‑0 >:O :-* :* :× ;‑) ;) *-) *) ;‑] ;] ;^) :‑, ;D :‑P :P X‑P XP x‑p xp :‑p :p :‑Þ :Þ :‑þ :þ :‑b :b  d: =p >:P :‑/ :/ :‑. >:\ >:/ :\ =/ =\ :L =L :S :‑| :| :\$ :‑X :X :‑# :# :‑& :&  O:‑) O:) 0:‑3 0:3 0:‑) 0:) 0;^) >:‑) >:) }:‑) }:) 3:‑) 3:) >;) |;‑) |‑O :‑J #‑) — %‑) %) :‑###.. :###.. <:‑| ',:-| ',:-l <_< >_>''';

const emoRes = ''':‑) ===> ‑
:) ===> :)
:-3 ===> -3
:3 ===> 3
:-> ===> :->
:> ===> :>
8-) ===> 8-)
8) ===> 8
:o) ===> o
:c) ===> c
:^) ===> :^)
:‑D ===> ‑D
:D ===> D
8‑D ===> 8‑D
8D ===> 8D
x‑D ===> x‑D
xD ===> xD
X‑D ===> X‑D
XD ===> XD
=D ===> D
=3 ===> 3
:-)) ===> :-))
:‑( ===> ‑
:( ===> :(
:‑c ===> ‑c
:c ===> c
:‑< ===> ‑
:< ===> :<
:‑[ ===> ‑
:-|| ===> :-|
>:( ===> >:(
:'‑( ===> ‑
:'‑) ===> ‑
D‑': ===> D‑
D: ===> D
D8 ===> D8
D; ===> D
D= ===> D
DX ===> DX
:‑O ===> ‑O
:O ===> O
:‑o ===> ‑o
:o ===> o
:-0 ===> -0
8‑0 ===> 8‑0
>:O ===> O
;‑) ===> ‑
;) ===> ;)
;‑] ===> ‑
;^) ===> ;^)
:‑, ===> ‑
;D ===> D
:‑P ===> ‑P
:P ===> P
X‑P ===> X‑P
XP ===> XP
x‑p ===> x‑p
xp ===> xp
:‑p ===> ‑p
:p ===> p
:‑Þ ===> ‑Þ
:Þ ===> Þ
:‑þ ===> ‑þ
:þ ===> þ
:‑b ===> ‑b
:b ===> b
d: ===> d
=p ===> p
>:P ===> P
:‑/ ===> ‑
:/ ===> :/
:‑. ===> ‑
>:/ ===> >:/
:L ===> L
=L ===> L
:S ===> S
:‑| ===> ‑
:| ===> :|
:\$ ===> \$
:‑X ===> ‑X
:X ===> X
:‑# ===> ‑
:‑& ===> ‑
>:‑) ===> ‑
>:) ===> >:)
}:‑) ===> ‑
}:) ===> }:)
>;) ===> >;)
|;‑) ===> ‑
|‑O ===> ‑O
:‑J ===> ‑J
#‑) ===> ‑
%‑) ===> ‑
%) ===> %)
:‑###.. ===> ‑
<:‑| ===> ‑
',:-| ===> :-|
',:-l ===> l
<_< ===> _
>_> ===> _''';
