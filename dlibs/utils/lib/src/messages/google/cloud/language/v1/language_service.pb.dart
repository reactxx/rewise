///
//  Generated code. Do not modify.
//  source: google/cloud/language/v1/language_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

import 'language_service.pbenum.dart';

export 'language_service.pbenum.dart';

enum Document_Source {
  content, 
  gcsContentUri, 
  notSet
}

class Document extends $pb.GeneratedMessage {
  static const Map<int, Document_Source> _Document_SourceByTag = {
    2 : Document_Source.content,
    3 : Document_Source.gcsContentUri,
    0 : Document_Source.notSet
  };
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Document', package: const $pb.PackageName('google.cloud.language.v1'))
    ..e<Document_Type>(1, 'type', $pb.PbFieldType.OE, Document_Type.TYPE_UNSPECIFIED, Document_Type.valueOf, Document_Type.values)
    ..aOS(2, 'content')
    ..aOS(3, 'gcsContentUri')
    ..aOS(4, 'language')
    ..oo(0, [2, 3])
    ..hasRequiredFields = false
  ;

  Document() : super();
  Document.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Document.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Document clone() => new Document()..mergeFromMessage(this);
  Document copyWith(void Function(Document) updates) => super.copyWith((message) => updates(message as Document));
  $pb.BuilderInfo get info_ => _i;
  static Document create() => new Document();
  Document createEmptyInstance() => create();
  static $pb.PbList<Document> createRepeated() => new $pb.PbList<Document>();
  static Document getDefault() => _defaultInstance ??= create()..freeze();
  static Document _defaultInstance;

  Document_Source whichSource() => _Document_SourceByTag[$_whichOneof(0)];
  void clearSource() => clearField($_whichOneof(0));

  Document_Type get type => $_getN(0);
  set type(Document_Type v) { setField(1, v); }
  bool hasType() => $_has(0);
  void clearType() => clearField(1);

  String get content => $_getS(1, '');
  set content(String v) { $_setString(1, v); }
  bool hasContent() => $_has(1);
  void clearContent() => clearField(2);

  String get gcsContentUri => $_getS(2, '');
  set gcsContentUri(String v) { $_setString(2, v); }
  bool hasGcsContentUri() => $_has(2);
  void clearGcsContentUri() => clearField(3);

  String get language => $_getS(3, '');
  set language(String v) { $_setString(3, v); }
  bool hasLanguage() => $_has(3);
  void clearLanguage() => clearField(4);
}

class Sentence extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Sentence', package: const $pb.PackageName('google.cloud.language.v1'))
    ..a<TextSpan>(1, 'text', $pb.PbFieldType.OM, TextSpan.getDefault, TextSpan.create)
    ..a<Sentiment>(2, 'sentiment', $pb.PbFieldType.OM, Sentiment.getDefault, Sentiment.create)
    ..hasRequiredFields = false
  ;

  Sentence() : super();
  Sentence.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Sentence.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Sentence clone() => new Sentence()..mergeFromMessage(this);
  Sentence copyWith(void Function(Sentence) updates) => super.copyWith((message) => updates(message as Sentence));
  $pb.BuilderInfo get info_ => _i;
  static Sentence create() => new Sentence();
  Sentence createEmptyInstance() => create();
  static $pb.PbList<Sentence> createRepeated() => new $pb.PbList<Sentence>();
  static Sentence getDefault() => _defaultInstance ??= create()..freeze();
  static Sentence _defaultInstance;

  TextSpan get text => $_getN(0);
  set text(TextSpan v) { setField(1, v); }
  bool hasText() => $_has(0);
  void clearText() => clearField(1);

  Sentiment get sentiment => $_getN(1);
  set sentiment(Sentiment v) { setField(2, v); }
  bool hasSentiment() => $_has(1);
  void clearSentiment() => clearField(2);
}

class Entity extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Entity', package: const $pb.PackageName('google.cloud.language.v1'))
    ..aOS(1, 'name')
    ..e<Entity_Type>(2, 'type', $pb.PbFieldType.OE, Entity_Type.UNKNOWN, Entity_Type.valueOf, Entity_Type.values)
    ..m<String, String>(3, 'metadata', 'Entity.MetadataEntry',$pb.PbFieldType.OS, $pb.PbFieldType.OS, null, null, null , const $pb.PackageName('google.cloud.language.v1'))
    ..a<double>(4, 'salience', $pb.PbFieldType.OF)
    ..pc<EntityMention>(5, 'mentions', $pb.PbFieldType.PM,EntityMention.create)
    ..a<Sentiment>(6, 'sentiment', $pb.PbFieldType.OM, Sentiment.getDefault, Sentiment.create)
    ..hasRequiredFields = false
  ;

  Entity() : super();
  Entity.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Entity.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Entity clone() => new Entity()..mergeFromMessage(this);
  Entity copyWith(void Function(Entity) updates) => super.copyWith((message) => updates(message as Entity));
  $pb.BuilderInfo get info_ => _i;
  static Entity create() => new Entity();
  Entity createEmptyInstance() => create();
  static $pb.PbList<Entity> createRepeated() => new $pb.PbList<Entity>();
  static Entity getDefault() => _defaultInstance ??= create()..freeze();
  static Entity _defaultInstance;

  String get name => $_getS(0, '');
  set name(String v) { $_setString(0, v); }
  bool hasName() => $_has(0);
  void clearName() => clearField(1);

  Entity_Type get type => $_getN(1);
  set type(Entity_Type v) { setField(2, v); }
  bool hasType() => $_has(1);
  void clearType() => clearField(2);

  Map<String, String> get metadata => $_getMap(2);

  double get salience => $_getN(3);
  set salience(double v) { $_setFloat(3, v); }
  bool hasSalience() => $_has(3);
  void clearSalience() => clearField(4);

  List<EntityMention> get mentions => $_getList(4);

  Sentiment get sentiment => $_getN(5);
  set sentiment(Sentiment v) { setField(6, v); }
  bool hasSentiment() => $_has(5);
  void clearSentiment() => clearField(6);
}

class Token extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Token', package: const $pb.PackageName('google.cloud.language.v1'))
    ..a<TextSpan>(1, 'text', $pb.PbFieldType.OM, TextSpan.getDefault, TextSpan.create)
    ..a<PartOfSpeech>(2, 'partOfSpeech', $pb.PbFieldType.OM, PartOfSpeech.getDefault, PartOfSpeech.create)
    ..a<DependencyEdge>(3, 'dependencyEdge', $pb.PbFieldType.OM, DependencyEdge.getDefault, DependencyEdge.create)
    ..aOS(4, 'lemma')
    ..hasRequiredFields = false
  ;

  Token() : super();
  Token.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Token.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Token clone() => new Token()..mergeFromMessage(this);
  Token copyWith(void Function(Token) updates) => super.copyWith((message) => updates(message as Token));
  $pb.BuilderInfo get info_ => _i;
  static Token create() => new Token();
  Token createEmptyInstance() => create();
  static $pb.PbList<Token> createRepeated() => new $pb.PbList<Token>();
  static Token getDefault() => _defaultInstance ??= create()..freeze();
  static Token _defaultInstance;

  TextSpan get text => $_getN(0);
  set text(TextSpan v) { setField(1, v); }
  bool hasText() => $_has(0);
  void clearText() => clearField(1);

  PartOfSpeech get partOfSpeech => $_getN(1);
  set partOfSpeech(PartOfSpeech v) { setField(2, v); }
  bool hasPartOfSpeech() => $_has(1);
  void clearPartOfSpeech() => clearField(2);

  DependencyEdge get dependencyEdge => $_getN(2);
  set dependencyEdge(DependencyEdge v) { setField(3, v); }
  bool hasDependencyEdge() => $_has(2);
  void clearDependencyEdge() => clearField(3);

  String get lemma => $_getS(3, '');
  set lemma(String v) { $_setString(3, v); }
  bool hasLemma() => $_has(3);
  void clearLemma() => clearField(4);
}

class Sentiment extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Sentiment', package: const $pb.PackageName('google.cloud.language.v1'))
    ..a<double>(2, 'magnitude', $pb.PbFieldType.OF)
    ..a<double>(3, 'score', $pb.PbFieldType.OF)
    ..hasRequiredFields = false
  ;

  Sentiment() : super();
  Sentiment.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Sentiment.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Sentiment clone() => new Sentiment()..mergeFromMessage(this);
  Sentiment copyWith(void Function(Sentiment) updates) => super.copyWith((message) => updates(message as Sentiment));
  $pb.BuilderInfo get info_ => _i;
  static Sentiment create() => new Sentiment();
  Sentiment createEmptyInstance() => create();
  static $pb.PbList<Sentiment> createRepeated() => new $pb.PbList<Sentiment>();
  static Sentiment getDefault() => _defaultInstance ??= create()..freeze();
  static Sentiment _defaultInstance;

  double get magnitude => $_getN(0);
  set magnitude(double v) { $_setFloat(0, v); }
  bool hasMagnitude() => $_has(0);
  void clearMagnitude() => clearField(2);

  double get score => $_getN(1);
  set score(double v) { $_setFloat(1, v); }
  bool hasScore() => $_has(1);
  void clearScore() => clearField(3);
}

class PartOfSpeech extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('PartOfSpeech', package: const $pb.PackageName('google.cloud.language.v1'))
    ..e<PartOfSpeech_Tag>(1, 'tag', $pb.PbFieldType.OE, PartOfSpeech_Tag.UNKNOWN, PartOfSpeech_Tag.valueOf, PartOfSpeech_Tag.values)
    ..e<PartOfSpeech_Aspect>(2, 'aspect', $pb.PbFieldType.OE, PartOfSpeech_Aspect.ASPECT_UNKNOWN, PartOfSpeech_Aspect.valueOf, PartOfSpeech_Aspect.values)
    ..e<PartOfSpeech_Case>(3, 'case_3', $pb.PbFieldType.OE, PartOfSpeech_Case.CASE_UNKNOWN, PartOfSpeech_Case.valueOf, PartOfSpeech_Case.values)
    ..e<PartOfSpeech_Form>(4, 'form', $pb.PbFieldType.OE, PartOfSpeech_Form.FORM_UNKNOWN, PartOfSpeech_Form.valueOf, PartOfSpeech_Form.values)
    ..e<PartOfSpeech_Gender>(5, 'gender', $pb.PbFieldType.OE, PartOfSpeech_Gender.GENDER_UNKNOWN, PartOfSpeech_Gender.valueOf, PartOfSpeech_Gender.values)
    ..e<PartOfSpeech_Mood>(6, 'mood', $pb.PbFieldType.OE, PartOfSpeech_Mood.MOOD_UNKNOWN, PartOfSpeech_Mood.valueOf, PartOfSpeech_Mood.values)
    ..e<PartOfSpeech_Number>(7, 'number', $pb.PbFieldType.OE, PartOfSpeech_Number.NUMBER_UNKNOWN, PartOfSpeech_Number.valueOf, PartOfSpeech_Number.values)
    ..e<PartOfSpeech_Person>(8, 'person', $pb.PbFieldType.OE, PartOfSpeech_Person.PERSON_UNKNOWN, PartOfSpeech_Person.valueOf, PartOfSpeech_Person.values)
    ..e<PartOfSpeech_Proper>(9, 'proper', $pb.PbFieldType.OE, PartOfSpeech_Proper.PROPER_UNKNOWN, PartOfSpeech_Proper.valueOf, PartOfSpeech_Proper.values)
    ..e<PartOfSpeech_Reciprocity>(10, 'reciprocity', $pb.PbFieldType.OE, PartOfSpeech_Reciprocity.RECIPROCITY_UNKNOWN, PartOfSpeech_Reciprocity.valueOf, PartOfSpeech_Reciprocity.values)
    ..e<PartOfSpeech_Tense>(11, 'tense', $pb.PbFieldType.OE, PartOfSpeech_Tense.TENSE_UNKNOWN, PartOfSpeech_Tense.valueOf, PartOfSpeech_Tense.values)
    ..e<PartOfSpeech_Voice>(12, 'voice', $pb.PbFieldType.OE, PartOfSpeech_Voice.VOICE_UNKNOWN, PartOfSpeech_Voice.valueOf, PartOfSpeech_Voice.values)
    ..hasRequiredFields = false
  ;

  PartOfSpeech() : super();
  PartOfSpeech.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  PartOfSpeech.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  PartOfSpeech clone() => new PartOfSpeech()..mergeFromMessage(this);
  PartOfSpeech copyWith(void Function(PartOfSpeech) updates) => super.copyWith((message) => updates(message as PartOfSpeech));
  $pb.BuilderInfo get info_ => _i;
  static PartOfSpeech create() => new PartOfSpeech();
  PartOfSpeech createEmptyInstance() => create();
  static $pb.PbList<PartOfSpeech> createRepeated() => new $pb.PbList<PartOfSpeech>();
  static PartOfSpeech getDefault() => _defaultInstance ??= create()..freeze();
  static PartOfSpeech _defaultInstance;

  PartOfSpeech_Tag get tag => $_getN(0);
  set tag(PartOfSpeech_Tag v) { setField(1, v); }
  bool hasTag() => $_has(0);
  void clearTag() => clearField(1);

  PartOfSpeech_Aspect get aspect => $_getN(1);
  set aspect(PartOfSpeech_Aspect v) { setField(2, v); }
  bool hasAspect() => $_has(1);
  void clearAspect() => clearField(2);

  PartOfSpeech_Case get case_3 => $_getN(2);
  set case_3(PartOfSpeech_Case v) { setField(3, v); }
  bool hasCase_3() => $_has(2);
  void clearCase_3() => clearField(3);

  PartOfSpeech_Form get form => $_getN(3);
  set form(PartOfSpeech_Form v) { setField(4, v); }
  bool hasForm() => $_has(3);
  void clearForm() => clearField(4);

  PartOfSpeech_Gender get gender => $_getN(4);
  set gender(PartOfSpeech_Gender v) { setField(5, v); }
  bool hasGender() => $_has(4);
  void clearGender() => clearField(5);

  PartOfSpeech_Mood get mood => $_getN(5);
  set mood(PartOfSpeech_Mood v) { setField(6, v); }
  bool hasMood() => $_has(5);
  void clearMood() => clearField(6);

  PartOfSpeech_Number get number => $_getN(6);
  set number(PartOfSpeech_Number v) { setField(7, v); }
  bool hasNumber() => $_has(6);
  void clearNumber() => clearField(7);

  PartOfSpeech_Person get person => $_getN(7);
  set person(PartOfSpeech_Person v) { setField(8, v); }
  bool hasPerson() => $_has(7);
  void clearPerson() => clearField(8);

  PartOfSpeech_Proper get proper => $_getN(8);
  set proper(PartOfSpeech_Proper v) { setField(9, v); }
  bool hasProper() => $_has(8);
  void clearProper() => clearField(9);

  PartOfSpeech_Reciprocity get reciprocity => $_getN(9);
  set reciprocity(PartOfSpeech_Reciprocity v) { setField(10, v); }
  bool hasReciprocity() => $_has(9);
  void clearReciprocity() => clearField(10);

  PartOfSpeech_Tense get tense => $_getN(10);
  set tense(PartOfSpeech_Tense v) { setField(11, v); }
  bool hasTense() => $_has(10);
  void clearTense() => clearField(11);

  PartOfSpeech_Voice get voice => $_getN(11);
  set voice(PartOfSpeech_Voice v) { setField(12, v); }
  bool hasVoice() => $_has(11);
  void clearVoice() => clearField(12);
}

class DependencyEdge extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('DependencyEdge', package: const $pb.PackageName('google.cloud.language.v1'))
    ..a<int>(1, 'headTokenIndex', $pb.PbFieldType.O3)
    ..e<DependencyEdge_Label>(2, 'label', $pb.PbFieldType.OE, DependencyEdge_Label.UNKNOWN, DependencyEdge_Label.valueOf, DependencyEdge_Label.values)
    ..hasRequiredFields = false
  ;

  DependencyEdge() : super();
  DependencyEdge.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  DependencyEdge.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  DependencyEdge clone() => new DependencyEdge()..mergeFromMessage(this);
  DependencyEdge copyWith(void Function(DependencyEdge) updates) => super.copyWith((message) => updates(message as DependencyEdge));
  $pb.BuilderInfo get info_ => _i;
  static DependencyEdge create() => new DependencyEdge();
  DependencyEdge createEmptyInstance() => create();
  static $pb.PbList<DependencyEdge> createRepeated() => new $pb.PbList<DependencyEdge>();
  static DependencyEdge getDefault() => _defaultInstance ??= create()..freeze();
  static DependencyEdge _defaultInstance;

  int get headTokenIndex => $_get(0, 0);
  set headTokenIndex(int v) { $_setSignedInt32(0, v); }
  bool hasHeadTokenIndex() => $_has(0);
  void clearHeadTokenIndex() => clearField(1);

  DependencyEdge_Label get label => $_getN(1);
  set label(DependencyEdge_Label v) { setField(2, v); }
  bool hasLabel() => $_has(1);
  void clearLabel() => clearField(2);
}

class EntityMention extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('EntityMention', package: const $pb.PackageName('google.cloud.language.v1'))
    ..a<TextSpan>(1, 'text', $pb.PbFieldType.OM, TextSpan.getDefault, TextSpan.create)
    ..e<EntityMention_Type>(2, 'type', $pb.PbFieldType.OE, EntityMention_Type.TYPE_UNKNOWN, EntityMention_Type.valueOf, EntityMention_Type.values)
    ..a<Sentiment>(3, 'sentiment', $pb.PbFieldType.OM, Sentiment.getDefault, Sentiment.create)
    ..hasRequiredFields = false
  ;

  EntityMention() : super();
  EntityMention.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  EntityMention.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  EntityMention clone() => new EntityMention()..mergeFromMessage(this);
  EntityMention copyWith(void Function(EntityMention) updates) => super.copyWith((message) => updates(message as EntityMention));
  $pb.BuilderInfo get info_ => _i;
  static EntityMention create() => new EntityMention();
  EntityMention createEmptyInstance() => create();
  static $pb.PbList<EntityMention> createRepeated() => new $pb.PbList<EntityMention>();
  static EntityMention getDefault() => _defaultInstance ??= create()..freeze();
  static EntityMention _defaultInstance;

  TextSpan get text => $_getN(0);
  set text(TextSpan v) { setField(1, v); }
  bool hasText() => $_has(0);
  void clearText() => clearField(1);

  EntityMention_Type get type => $_getN(1);
  set type(EntityMention_Type v) { setField(2, v); }
  bool hasType() => $_has(1);
  void clearType() => clearField(2);

  Sentiment get sentiment => $_getN(2);
  set sentiment(Sentiment v) { setField(3, v); }
  bool hasSentiment() => $_has(2);
  void clearSentiment() => clearField(3);
}

class TextSpan extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('TextSpan', package: const $pb.PackageName('google.cloud.language.v1'))
    ..aOS(1, 'content')
    ..a<int>(2, 'beginOffset', $pb.PbFieldType.O3)
    ..hasRequiredFields = false
  ;

  TextSpan() : super();
  TextSpan.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  TextSpan.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  TextSpan clone() => new TextSpan()..mergeFromMessage(this);
  TextSpan copyWith(void Function(TextSpan) updates) => super.copyWith((message) => updates(message as TextSpan));
  $pb.BuilderInfo get info_ => _i;
  static TextSpan create() => new TextSpan();
  TextSpan createEmptyInstance() => create();
  static $pb.PbList<TextSpan> createRepeated() => new $pb.PbList<TextSpan>();
  static TextSpan getDefault() => _defaultInstance ??= create()..freeze();
  static TextSpan _defaultInstance;

  String get content => $_getS(0, '');
  set content(String v) { $_setString(0, v); }
  bool hasContent() => $_has(0);
  void clearContent() => clearField(1);

  int get beginOffset => $_get(1, 0);
  set beginOffset(int v) { $_setSignedInt32(1, v); }
  bool hasBeginOffset() => $_has(1);
  void clearBeginOffset() => clearField(2);
}

class ClassificationCategory extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('ClassificationCategory', package: const $pb.PackageName('google.cloud.language.v1'))
    ..aOS(1, 'name')
    ..a<double>(2, 'confidence', $pb.PbFieldType.OF)
    ..hasRequiredFields = false
  ;

  ClassificationCategory() : super();
  ClassificationCategory.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  ClassificationCategory.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  ClassificationCategory clone() => new ClassificationCategory()..mergeFromMessage(this);
  ClassificationCategory copyWith(void Function(ClassificationCategory) updates) => super.copyWith((message) => updates(message as ClassificationCategory));
  $pb.BuilderInfo get info_ => _i;
  static ClassificationCategory create() => new ClassificationCategory();
  ClassificationCategory createEmptyInstance() => create();
  static $pb.PbList<ClassificationCategory> createRepeated() => new $pb.PbList<ClassificationCategory>();
  static ClassificationCategory getDefault() => _defaultInstance ??= create()..freeze();
  static ClassificationCategory _defaultInstance;

  String get name => $_getS(0, '');
  set name(String v) { $_setString(0, v); }
  bool hasName() => $_has(0);
  void clearName() => clearField(1);

  double get confidence => $_getN(1);
  set confidence(double v) { $_setFloat(1, v); }
  bool hasConfidence() => $_has(1);
  void clearConfidence() => clearField(2);
}

class AnalyzeSentimentRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('AnalyzeSentimentRequest', package: const $pb.PackageName('google.cloud.language.v1'))
    ..a<Document>(1, 'document', $pb.PbFieldType.OM, Document.getDefault, Document.create)
    ..e<EncodingType>(2, 'encodingType', $pb.PbFieldType.OE, EncodingType.NONE, EncodingType.valueOf, EncodingType.values)
    ..hasRequiredFields = false
  ;

  AnalyzeSentimentRequest() : super();
  AnalyzeSentimentRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  AnalyzeSentimentRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  AnalyzeSentimentRequest clone() => new AnalyzeSentimentRequest()..mergeFromMessage(this);
  AnalyzeSentimentRequest copyWith(void Function(AnalyzeSentimentRequest) updates) => super.copyWith((message) => updates(message as AnalyzeSentimentRequest));
  $pb.BuilderInfo get info_ => _i;
  static AnalyzeSentimentRequest create() => new AnalyzeSentimentRequest();
  AnalyzeSentimentRequest createEmptyInstance() => create();
  static $pb.PbList<AnalyzeSentimentRequest> createRepeated() => new $pb.PbList<AnalyzeSentimentRequest>();
  static AnalyzeSentimentRequest getDefault() => _defaultInstance ??= create()..freeze();
  static AnalyzeSentimentRequest _defaultInstance;

  Document get document => $_getN(0);
  set document(Document v) { setField(1, v); }
  bool hasDocument() => $_has(0);
  void clearDocument() => clearField(1);

  EncodingType get encodingType => $_getN(1);
  set encodingType(EncodingType v) { setField(2, v); }
  bool hasEncodingType() => $_has(1);
  void clearEncodingType() => clearField(2);
}

class AnalyzeSentimentResponse extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('AnalyzeSentimentResponse', package: const $pb.PackageName('google.cloud.language.v1'))
    ..a<Sentiment>(1, 'documentSentiment', $pb.PbFieldType.OM, Sentiment.getDefault, Sentiment.create)
    ..aOS(2, 'language')
    ..pc<Sentence>(3, 'sentences', $pb.PbFieldType.PM,Sentence.create)
    ..hasRequiredFields = false
  ;

  AnalyzeSentimentResponse() : super();
  AnalyzeSentimentResponse.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  AnalyzeSentimentResponse.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  AnalyzeSentimentResponse clone() => new AnalyzeSentimentResponse()..mergeFromMessage(this);
  AnalyzeSentimentResponse copyWith(void Function(AnalyzeSentimentResponse) updates) => super.copyWith((message) => updates(message as AnalyzeSentimentResponse));
  $pb.BuilderInfo get info_ => _i;
  static AnalyzeSentimentResponse create() => new AnalyzeSentimentResponse();
  AnalyzeSentimentResponse createEmptyInstance() => create();
  static $pb.PbList<AnalyzeSentimentResponse> createRepeated() => new $pb.PbList<AnalyzeSentimentResponse>();
  static AnalyzeSentimentResponse getDefault() => _defaultInstance ??= create()..freeze();
  static AnalyzeSentimentResponse _defaultInstance;

  Sentiment get documentSentiment => $_getN(0);
  set documentSentiment(Sentiment v) { setField(1, v); }
  bool hasDocumentSentiment() => $_has(0);
  void clearDocumentSentiment() => clearField(1);

  String get language => $_getS(1, '');
  set language(String v) { $_setString(1, v); }
  bool hasLanguage() => $_has(1);
  void clearLanguage() => clearField(2);

  List<Sentence> get sentences => $_getList(2);
}

class AnalyzeEntitySentimentRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('AnalyzeEntitySentimentRequest', package: const $pb.PackageName('google.cloud.language.v1'))
    ..a<Document>(1, 'document', $pb.PbFieldType.OM, Document.getDefault, Document.create)
    ..e<EncodingType>(2, 'encodingType', $pb.PbFieldType.OE, EncodingType.NONE, EncodingType.valueOf, EncodingType.values)
    ..hasRequiredFields = false
  ;

  AnalyzeEntitySentimentRequest() : super();
  AnalyzeEntitySentimentRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  AnalyzeEntitySentimentRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  AnalyzeEntitySentimentRequest clone() => new AnalyzeEntitySentimentRequest()..mergeFromMessage(this);
  AnalyzeEntitySentimentRequest copyWith(void Function(AnalyzeEntitySentimentRequest) updates) => super.copyWith((message) => updates(message as AnalyzeEntitySentimentRequest));
  $pb.BuilderInfo get info_ => _i;
  static AnalyzeEntitySentimentRequest create() => new AnalyzeEntitySentimentRequest();
  AnalyzeEntitySentimentRequest createEmptyInstance() => create();
  static $pb.PbList<AnalyzeEntitySentimentRequest> createRepeated() => new $pb.PbList<AnalyzeEntitySentimentRequest>();
  static AnalyzeEntitySentimentRequest getDefault() => _defaultInstance ??= create()..freeze();
  static AnalyzeEntitySentimentRequest _defaultInstance;

  Document get document => $_getN(0);
  set document(Document v) { setField(1, v); }
  bool hasDocument() => $_has(0);
  void clearDocument() => clearField(1);

  EncodingType get encodingType => $_getN(1);
  set encodingType(EncodingType v) { setField(2, v); }
  bool hasEncodingType() => $_has(1);
  void clearEncodingType() => clearField(2);
}

class AnalyzeEntitySentimentResponse extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('AnalyzeEntitySentimentResponse', package: const $pb.PackageName('google.cloud.language.v1'))
    ..pc<Entity>(1, 'entities', $pb.PbFieldType.PM,Entity.create)
    ..aOS(2, 'language')
    ..hasRequiredFields = false
  ;

  AnalyzeEntitySentimentResponse() : super();
  AnalyzeEntitySentimentResponse.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  AnalyzeEntitySentimentResponse.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  AnalyzeEntitySentimentResponse clone() => new AnalyzeEntitySentimentResponse()..mergeFromMessage(this);
  AnalyzeEntitySentimentResponse copyWith(void Function(AnalyzeEntitySentimentResponse) updates) => super.copyWith((message) => updates(message as AnalyzeEntitySentimentResponse));
  $pb.BuilderInfo get info_ => _i;
  static AnalyzeEntitySentimentResponse create() => new AnalyzeEntitySentimentResponse();
  AnalyzeEntitySentimentResponse createEmptyInstance() => create();
  static $pb.PbList<AnalyzeEntitySentimentResponse> createRepeated() => new $pb.PbList<AnalyzeEntitySentimentResponse>();
  static AnalyzeEntitySentimentResponse getDefault() => _defaultInstance ??= create()..freeze();
  static AnalyzeEntitySentimentResponse _defaultInstance;

  List<Entity> get entities => $_getList(0);

  String get language => $_getS(1, '');
  set language(String v) { $_setString(1, v); }
  bool hasLanguage() => $_has(1);
  void clearLanguage() => clearField(2);
}

class AnalyzeEntitiesRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('AnalyzeEntitiesRequest', package: const $pb.PackageName('google.cloud.language.v1'))
    ..a<Document>(1, 'document', $pb.PbFieldType.OM, Document.getDefault, Document.create)
    ..e<EncodingType>(2, 'encodingType', $pb.PbFieldType.OE, EncodingType.NONE, EncodingType.valueOf, EncodingType.values)
    ..hasRequiredFields = false
  ;

  AnalyzeEntitiesRequest() : super();
  AnalyzeEntitiesRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  AnalyzeEntitiesRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  AnalyzeEntitiesRequest clone() => new AnalyzeEntitiesRequest()..mergeFromMessage(this);
  AnalyzeEntitiesRequest copyWith(void Function(AnalyzeEntitiesRequest) updates) => super.copyWith((message) => updates(message as AnalyzeEntitiesRequest));
  $pb.BuilderInfo get info_ => _i;
  static AnalyzeEntitiesRequest create() => new AnalyzeEntitiesRequest();
  AnalyzeEntitiesRequest createEmptyInstance() => create();
  static $pb.PbList<AnalyzeEntitiesRequest> createRepeated() => new $pb.PbList<AnalyzeEntitiesRequest>();
  static AnalyzeEntitiesRequest getDefault() => _defaultInstance ??= create()..freeze();
  static AnalyzeEntitiesRequest _defaultInstance;

  Document get document => $_getN(0);
  set document(Document v) { setField(1, v); }
  bool hasDocument() => $_has(0);
  void clearDocument() => clearField(1);

  EncodingType get encodingType => $_getN(1);
  set encodingType(EncodingType v) { setField(2, v); }
  bool hasEncodingType() => $_has(1);
  void clearEncodingType() => clearField(2);
}

class AnalyzeEntitiesResponse extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('AnalyzeEntitiesResponse', package: const $pb.PackageName('google.cloud.language.v1'))
    ..pc<Entity>(1, 'entities', $pb.PbFieldType.PM,Entity.create)
    ..aOS(2, 'language')
    ..hasRequiredFields = false
  ;

  AnalyzeEntitiesResponse() : super();
  AnalyzeEntitiesResponse.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  AnalyzeEntitiesResponse.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  AnalyzeEntitiesResponse clone() => new AnalyzeEntitiesResponse()..mergeFromMessage(this);
  AnalyzeEntitiesResponse copyWith(void Function(AnalyzeEntitiesResponse) updates) => super.copyWith((message) => updates(message as AnalyzeEntitiesResponse));
  $pb.BuilderInfo get info_ => _i;
  static AnalyzeEntitiesResponse create() => new AnalyzeEntitiesResponse();
  AnalyzeEntitiesResponse createEmptyInstance() => create();
  static $pb.PbList<AnalyzeEntitiesResponse> createRepeated() => new $pb.PbList<AnalyzeEntitiesResponse>();
  static AnalyzeEntitiesResponse getDefault() => _defaultInstance ??= create()..freeze();
  static AnalyzeEntitiesResponse _defaultInstance;

  List<Entity> get entities => $_getList(0);

  String get language => $_getS(1, '');
  set language(String v) { $_setString(1, v); }
  bool hasLanguage() => $_has(1);
  void clearLanguage() => clearField(2);
}

class AnalyzeSyntaxRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('AnalyzeSyntaxRequest', package: const $pb.PackageName('google.cloud.language.v1'))
    ..a<Document>(1, 'document', $pb.PbFieldType.OM, Document.getDefault, Document.create)
    ..e<EncodingType>(2, 'encodingType', $pb.PbFieldType.OE, EncodingType.NONE, EncodingType.valueOf, EncodingType.values)
    ..hasRequiredFields = false
  ;

  AnalyzeSyntaxRequest() : super();
  AnalyzeSyntaxRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  AnalyzeSyntaxRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  AnalyzeSyntaxRequest clone() => new AnalyzeSyntaxRequest()..mergeFromMessage(this);
  AnalyzeSyntaxRequest copyWith(void Function(AnalyzeSyntaxRequest) updates) => super.copyWith((message) => updates(message as AnalyzeSyntaxRequest));
  $pb.BuilderInfo get info_ => _i;
  static AnalyzeSyntaxRequest create() => new AnalyzeSyntaxRequest();
  AnalyzeSyntaxRequest createEmptyInstance() => create();
  static $pb.PbList<AnalyzeSyntaxRequest> createRepeated() => new $pb.PbList<AnalyzeSyntaxRequest>();
  static AnalyzeSyntaxRequest getDefault() => _defaultInstance ??= create()..freeze();
  static AnalyzeSyntaxRequest _defaultInstance;

  Document get document => $_getN(0);
  set document(Document v) { setField(1, v); }
  bool hasDocument() => $_has(0);
  void clearDocument() => clearField(1);

  EncodingType get encodingType => $_getN(1);
  set encodingType(EncodingType v) { setField(2, v); }
  bool hasEncodingType() => $_has(1);
  void clearEncodingType() => clearField(2);
}

class AnalyzeSyntaxResponse extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('AnalyzeSyntaxResponse', package: const $pb.PackageName('google.cloud.language.v1'))
    ..pc<Sentence>(1, 'sentences', $pb.PbFieldType.PM,Sentence.create)
    ..pc<Token>(2, 'tokens', $pb.PbFieldType.PM,Token.create)
    ..aOS(3, 'language')
    ..hasRequiredFields = false
  ;

  AnalyzeSyntaxResponse() : super();
  AnalyzeSyntaxResponse.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  AnalyzeSyntaxResponse.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  AnalyzeSyntaxResponse clone() => new AnalyzeSyntaxResponse()..mergeFromMessage(this);
  AnalyzeSyntaxResponse copyWith(void Function(AnalyzeSyntaxResponse) updates) => super.copyWith((message) => updates(message as AnalyzeSyntaxResponse));
  $pb.BuilderInfo get info_ => _i;
  static AnalyzeSyntaxResponse create() => new AnalyzeSyntaxResponse();
  AnalyzeSyntaxResponse createEmptyInstance() => create();
  static $pb.PbList<AnalyzeSyntaxResponse> createRepeated() => new $pb.PbList<AnalyzeSyntaxResponse>();
  static AnalyzeSyntaxResponse getDefault() => _defaultInstance ??= create()..freeze();
  static AnalyzeSyntaxResponse _defaultInstance;

  List<Sentence> get sentences => $_getList(0);

  List<Token> get tokens => $_getList(1);

  String get language => $_getS(2, '');
  set language(String v) { $_setString(2, v); }
  bool hasLanguage() => $_has(2);
  void clearLanguage() => clearField(3);
}

class ClassifyTextRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('ClassifyTextRequest', package: const $pb.PackageName('google.cloud.language.v1'))
    ..a<Document>(1, 'document', $pb.PbFieldType.OM, Document.getDefault, Document.create)
    ..hasRequiredFields = false
  ;

  ClassifyTextRequest() : super();
  ClassifyTextRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  ClassifyTextRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  ClassifyTextRequest clone() => new ClassifyTextRequest()..mergeFromMessage(this);
  ClassifyTextRequest copyWith(void Function(ClassifyTextRequest) updates) => super.copyWith((message) => updates(message as ClassifyTextRequest));
  $pb.BuilderInfo get info_ => _i;
  static ClassifyTextRequest create() => new ClassifyTextRequest();
  ClassifyTextRequest createEmptyInstance() => create();
  static $pb.PbList<ClassifyTextRequest> createRepeated() => new $pb.PbList<ClassifyTextRequest>();
  static ClassifyTextRequest getDefault() => _defaultInstance ??= create()..freeze();
  static ClassifyTextRequest _defaultInstance;

  Document get document => $_getN(0);
  set document(Document v) { setField(1, v); }
  bool hasDocument() => $_has(0);
  void clearDocument() => clearField(1);
}

class ClassifyTextResponse extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('ClassifyTextResponse', package: const $pb.PackageName('google.cloud.language.v1'))
    ..pc<ClassificationCategory>(1, 'categories', $pb.PbFieldType.PM,ClassificationCategory.create)
    ..hasRequiredFields = false
  ;

  ClassifyTextResponse() : super();
  ClassifyTextResponse.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  ClassifyTextResponse.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  ClassifyTextResponse clone() => new ClassifyTextResponse()..mergeFromMessage(this);
  ClassifyTextResponse copyWith(void Function(ClassifyTextResponse) updates) => super.copyWith((message) => updates(message as ClassifyTextResponse));
  $pb.BuilderInfo get info_ => _i;
  static ClassifyTextResponse create() => new ClassifyTextResponse();
  ClassifyTextResponse createEmptyInstance() => create();
  static $pb.PbList<ClassifyTextResponse> createRepeated() => new $pb.PbList<ClassifyTextResponse>();
  static ClassifyTextResponse getDefault() => _defaultInstance ??= create()..freeze();
  static ClassifyTextResponse _defaultInstance;

  List<ClassificationCategory> get categories => $_getList(0);
}

class AnnotateTextRequest_Features extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('AnnotateTextRequest.Features', package: const $pb.PackageName('google.cloud.language.v1'))
    ..aOB(1, 'extractSyntax')
    ..aOB(2, 'extractEntities')
    ..aOB(3, 'extractDocumentSentiment')
    ..aOB(4, 'extractEntitySentiment')
    ..aOB(6, 'classifyText')
    ..hasRequiredFields = false
  ;

  AnnotateTextRequest_Features() : super();
  AnnotateTextRequest_Features.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  AnnotateTextRequest_Features.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  AnnotateTextRequest_Features clone() => new AnnotateTextRequest_Features()..mergeFromMessage(this);
  AnnotateTextRequest_Features copyWith(void Function(AnnotateTextRequest_Features) updates) => super.copyWith((message) => updates(message as AnnotateTextRequest_Features));
  $pb.BuilderInfo get info_ => _i;
  static AnnotateTextRequest_Features create() => new AnnotateTextRequest_Features();
  AnnotateTextRequest_Features createEmptyInstance() => create();
  static $pb.PbList<AnnotateTextRequest_Features> createRepeated() => new $pb.PbList<AnnotateTextRequest_Features>();
  static AnnotateTextRequest_Features getDefault() => _defaultInstance ??= create()..freeze();
  static AnnotateTextRequest_Features _defaultInstance;

  bool get extractSyntax => $_get(0, false);
  set extractSyntax(bool v) { $_setBool(0, v); }
  bool hasExtractSyntax() => $_has(0);
  void clearExtractSyntax() => clearField(1);

  bool get extractEntities => $_get(1, false);
  set extractEntities(bool v) { $_setBool(1, v); }
  bool hasExtractEntities() => $_has(1);
  void clearExtractEntities() => clearField(2);

  bool get extractDocumentSentiment => $_get(2, false);
  set extractDocumentSentiment(bool v) { $_setBool(2, v); }
  bool hasExtractDocumentSentiment() => $_has(2);
  void clearExtractDocumentSentiment() => clearField(3);

  bool get extractEntitySentiment => $_get(3, false);
  set extractEntitySentiment(bool v) { $_setBool(3, v); }
  bool hasExtractEntitySentiment() => $_has(3);
  void clearExtractEntitySentiment() => clearField(4);

  bool get classifyText => $_get(4, false);
  set classifyText(bool v) { $_setBool(4, v); }
  bool hasClassifyText() => $_has(4);
  void clearClassifyText() => clearField(6);
}

class AnnotateTextRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('AnnotateTextRequest', package: const $pb.PackageName('google.cloud.language.v1'))
    ..a<Document>(1, 'document', $pb.PbFieldType.OM, Document.getDefault, Document.create)
    ..a<AnnotateTextRequest_Features>(2, 'features', $pb.PbFieldType.OM, AnnotateTextRequest_Features.getDefault, AnnotateTextRequest_Features.create)
    ..e<EncodingType>(3, 'encodingType', $pb.PbFieldType.OE, EncodingType.NONE, EncodingType.valueOf, EncodingType.values)
    ..hasRequiredFields = false
  ;

  AnnotateTextRequest() : super();
  AnnotateTextRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  AnnotateTextRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  AnnotateTextRequest clone() => new AnnotateTextRequest()..mergeFromMessage(this);
  AnnotateTextRequest copyWith(void Function(AnnotateTextRequest) updates) => super.copyWith((message) => updates(message as AnnotateTextRequest));
  $pb.BuilderInfo get info_ => _i;
  static AnnotateTextRequest create() => new AnnotateTextRequest();
  AnnotateTextRequest createEmptyInstance() => create();
  static $pb.PbList<AnnotateTextRequest> createRepeated() => new $pb.PbList<AnnotateTextRequest>();
  static AnnotateTextRequest getDefault() => _defaultInstance ??= create()..freeze();
  static AnnotateTextRequest _defaultInstance;

  Document get document => $_getN(0);
  set document(Document v) { setField(1, v); }
  bool hasDocument() => $_has(0);
  void clearDocument() => clearField(1);

  AnnotateTextRequest_Features get features => $_getN(1);
  set features(AnnotateTextRequest_Features v) { setField(2, v); }
  bool hasFeatures() => $_has(1);
  void clearFeatures() => clearField(2);

  EncodingType get encodingType => $_getN(2);
  set encodingType(EncodingType v) { setField(3, v); }
  bool hasEncodingType() => $_has(2);
  void clearEncodingType() => clearField(3);
}

class AnnotateTextResponse extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('AnnotateTextResponse', package: const $pb.PackageName('google.cloud.language.v1'))
    ..pc<Sentence>(1, 'sentences', $pb.PbFieldType.PM,Sentence.create)
    ..pc<Token>(2, 'tokens', $pb.PbFieldType.PM,Token.create)
    ..pc<Entity>(3, 'entities', $pb.PbFieldType.PM,Entity.create)
    ..a<Sentiment>(4, 'documentSentiment', $pb.PbFieldType.OM, Sentiment.getDefault, Sentiment.create)
    ..aOS(5, 'language')
    ..pc<ClassificationCategory>(6, 'categories', $pb.PbFieldType.PM,ClassificationCategory.create)
    ..hasRequiredFields = false
  ;

  AnnotateTextResponse() : super();
  AnnotateTextResponse.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  AnnotateTextResponse.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  AnnotateTextResponse clone() => new AnnotateTextResponse()..mergeFromMessage(this);
  AnnotateTextResponse copyWith(void Function(AnnotateTextResponse) updates) => super.copyWith((message) => updates(message as AnnotateTextResponse));
  $pb.BuilderInfo get info_ => _i;
  static AnnotateTextResponse create() => new AnnotateTextResponse();
  AnnotateTextResponse createEmptyInstance() => create();
  static $pb.PbList<AnnotateTextResponse> createRepeated() => new $pb.PbList<AnnotateTextResponse>();
  static AnnotateTextResponse getDefault() => _defaultInstance ??= create()..freeze();
  static AnnotateTextResponse _defaultInstance;

  List<Sentence> get sentences => $_getList(0);

  List<Token> get tokens => $_getList(1);

  List<Entity> get entities => $_getList(2);

  Sentiment get documentSentiment => $_getN(3);
  set documentSentiment(Sentiment v) { setField(4, v); }
  bool hasDocumentSentiment() => $_has(3);
  void clearDocumentSentiment() => clearField(4);

  String get language => $_getS(4, '');
  set language(String v) { $_setString(4, v); }
  bool hasLanguage() => $_has(4);
  void clearLanguage() => clearField(5);

  List<ClassificationCategory> get categories => $_getList(5);
}

