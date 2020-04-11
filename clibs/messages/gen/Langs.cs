// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: rewise/utils/langs.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace RewiseDom {

  /// <summary>Holder for reflection information generated from rewise/utils/langs.proto</summary>
  public static partial class LangsReflection {

    #region Descriptor
    /// <summary>File descriptor for rewise/utils/langs.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static LangsReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChhyZXdpc2UvdXRpbHMvbGFuZ3MucHJvdG8SCXJld2lzZURvbSIvCglDbGRy",
            "TGFuZ3MSIgoFbGFuZ3MYASADKAsyEy5yZXdpc2VEb20uQ2xkckxhbmci4QEK",
            "CENsZHJMYW5nEgoKAmlkGAEgASgJEgwKBGxhbmcYAiABKAkSEQoJc2NyaXB0",
            "X2lkGAMgASgJEhYKDmRlZmF1bHRfcmVnaW9uGAQgASgJEhgKEGhhc19tb3Jl",
            "X3NjcmlwdHMYBSABKAgSFAoMaGFzX3N0ZW1taW5nGAYgASgIEhAKCGFscGhh",
            "YmV0GAcgASgJEhYKDmFscGhhYmV0X3VwcGVyGAggASgJEh0KFXdvcmRfc3Bl",
            "bGxfY2hlY2tfbGNpZBgJIAEoBRIXCg9nb29nbGVfdHJhbnNfaWQYCiABKAki",
            "MwoIVW5jUmFuZ2USDQoFc3RhcnQYASABKAUSCwoDZW5kGAIgASgFEgsKA2lk",
            "eBgDIAEoBSJCCglVbmNCbG9ja3MSEAoISVNPMTU5MjQYASADKAkSIwoGcmFu",
            "Z2VzGAIgAygLMhMucmV3aXNlRG9tLlVuY1JhbmdlYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::RewiseDom.CldrLangs), global::RewiseDom.CldrLangs.Parser, new[]{ "Langs" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::RewiseDom.CldrLang), global::RewiseDom.CldrLang.Parser, new[]{ "Id", "Lang", "ScriptId", "DefaultRegion", "HasMoreScripts", "HasStemming", "Alphabet", "AlphabetUpper", "WordSpellCheckLcid", "GoogleTransId" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::RewiseDom.UncRange), global::RewiseDom.UncRange.Parser, new[]{ "Start", "End", "Idx" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::RewiseDom.UncBlocks), global::RewiseDom.UncBlocks.Parser, new[]{ "ISO15924", "Ranges" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class CldrLangs : pb::IMessage<CldrLangs> {
    private static readonly pb::MessageParser<CldrLangs> _parser = new pb::MessageParser<CldrLangs>(() => new CldrLangs());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CldrLangs> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RewiseDom.LangsReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CldrLangs() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CldrLangs(CldrLangs other) : this() {
      langs_ = other.langs_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CldrLangs Clone() {
      return new CldrLangs(this);
    }

    /// <summary>Field number for the "langs" field.</summary>
    public const int LangsFieldNumber = 1;
    private static readonly pb::FieldCodec<global::RewiseDom.CldrLang> _repeated_langs_codec
        = pb::FieldCodec.ForMessage(10, global::RewiseDom.CldrLang.Parser);
    private readonly pbc::RepeatedField<global::RewiseDom.CldrLang> langs_ = new pbc::RepeatedField<global::RewiseDom.CldrLang>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::RewiseDom.CldrLang> Langs {
      get { return langs_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CldrLangs);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CldrLangs other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!langs_.Equals(other.langs_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= langs_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      langs_.WriteTo(output, _repeated_langs_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += langs_.CalculateSize(_repeated_langs_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CldrLangs other) {
      if (other == null) {
        return;
      }
      langs_.Add(other.langs_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            langs_.AddEntriesFrom(input, _repeated_langs_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class CldrLang : pb::IMessage<CldrLang> {
    private static readonly pb::MessageParser<CldrLang> _parser = new pb::MessageParser<CldrLang>(() => new CldrLang());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CldrLang> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RewiseDom.LangsReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CldrLang() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CldrLang(CldrLang other) : this() {
      id_ = other.id_;
      lang_ = other.lang_;
      scriptId_ = other.scriptId_;
      defaultRegion_ = other.defaultRegion_;
      hasMoreScripts_ = other.hasMoreScripts_;
      hasStemming_ = other.hasStemming_;
      alphabet_ = other.alphabet_;
      alphabetUpper_ = other.alphabetUpper_;
      wordSpellCheckLcid_ = other.wordSpellCheckLcid_;
      googleTransId_ = other.googleTransId_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CldrLang Clone() {
      return new CldrLang(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private string id_ = "";
    /// <summary>
    /// e.g. cs-CZ, sr-Latn, 1 for invariant locale
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Id {
      get { return id_; }
      set {
        id_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "lang" field.</summary>
    public const int LangFieldNumber = 2;
    private string lang_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Lang {
      get { return lang_; }
      set {
        lang_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "script_id" field.</summary>
    public const int ScriptIdFieldNumber = 3;
    private string scriptId_ = "";
    /// <summary>
    /// unicode script, e.g. Latn, Arab etc.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ScriptId {
      get { return scriptId_; }
      set {
        scriptId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "default_region" field.</summary>
    public const int DefaultRegionFieldNumber = 4;
    private string defaultRegion_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string DefaultRegion {
      get { return defaultRegion_; }
      set {
        defaultRegion_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "has_more_scripts" field.</summary>
    public const int HasMoreScriptsFieldNumber = 5;
    private bool hasMoreScripts_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMoreScripts {
      get { return hasMoreScripts_; }
      set {
        hasMoreScripts_ = value;
      }
    }

    /// <summary>Field number for the "has_stemming" field.</summary>
    public const int HasStemmingFieldNumber = 6;
    private bool hasStemming_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasStemming {
      get { return hasStemming_; }
      set {
        hasStemming_ = value;
      }
    }

    /// <summary>Field number for the "alphabet" field.</summary>
    public const int AlphabetFieldNumber = 7;
    private string alphabet_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Alphabet {
      get { return alphabet_; }
      set {
        alphabet_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "alphabet_upper" field.</summary>
    public const int AlphabetUpperFieldNumber = 8;
    private string alphabetUpper_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string AlphabetUpper {
      get { return alphabetUpper_; }
      set {
        alphabetUpper_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "word_spell_check_lcid" field.</summary>
    public const int WordSpellCheckLcidFieldNumber = 9;
    private int wordSpellCheckLcid_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int WordSpellCheckLcid {
      get { return wordSpellCheckLcid_; }
      set {
        wordSpellCheckLcid_ = value;
      }
    }

    /// <summary>Field number for the "google_trans_id" field.</summary>
    public const int GoogleTransIdFieldNumber = 10;
    private string googleTransId_ = "";
    /// <summary>
    /// repeated string regions = 7; // other regions for given &lt;id>
    /// int64 LCID = 8;
    /// string stemmer_class = 9;
    /// string breaker_class = 10;
    /// bool is_euro_talk = 11;
    /// bool is_goethe = 12;
    /// bool is_lingea = 13;
    /// string google_trans_id = 14;
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string GoogleTransId {
      get { return googleTransId_; }
      set {
        googleTransId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CldrLang);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CldrLang other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (Lang != other.Lang) return false;
      if (ScriptId != other.ScriptId) return false;
      if (DefaultRegion != other.DefaultRegion) return false;
      if (HasMoreScripts != other.HasMoreScripts) return false;
      if (HasStemming != other.HasStemming) return false;
      if (Alphabet != other.Alphabet) return false;
      if (AlphabetUpper != other.AlphabetUpper) return false;
      if (WordSpellCheckLcid != other.WordSpellCheckLcid) return false;
      if (GoogleTransId != other.GoogleTransId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id.Length != 0) hash ^= Id.GetHashCode();
      if (Lang.Length != 0) hash ^= Lang.GetHashCode();
      if (ScriptId.Length != 0) hash ^= ScriptId.GetHashCode();
      if (DefaultRegion.Length != 0) hash ^= DefaultRegion.GetHashCode();
      if (HasMoreScripts != false) hash ^= HasMoreScripts.GetHashCode();
      if (HasStemming != false) hash ^= HasStemming.GetHashCode();
      if (Alphabet.Length != 0) hash ^= Alphabet.GetHashCode();
      if (AlphabetUpper.Length != 0) hash ^= AlphabetUpper.GetHashCode();
      if (WordSpellCheckLcid != 0) hash ^= WordSpellCheckLcid.GetHashCode();
      if (GoogleTransId.Length != 0) hash ^= GoogleTransId.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Id.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Id);
      }
      if (Lang.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Lang);
      }
      if (ScriptId.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(ScriptId);
      }
      if (DefaultRegion.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(DefaultRegion);
      }
      if (HasMoreScripts != false) {
        output.WriteRawTag(40);
        output.WriteBool(HasMoreScripts);
      }
      if (HasStemming != false) {
        output.WriteRawTag(48);
        output.WriteBool(HasStemming);
      }
      if (Alphabet.Length != 0) {
        output.WriteRawTag(58);
        output.WriteString(Alphabet);
      }
      if (AlphabetUpper.Length != 0) {
        output.WriteRawTag(66);
        output.WriteString(AlphabetUpper);
      }
      if (WordSpellCheckLcid != 0) {
        output.WriteRawTag(72);
        output.WriteInt32(WordSpellCheckLcid);
      }
      if (GoogleTransId.Length != 0) {
        output.WriteRawTag(82);
        output.WriteString(GoogleTransId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Id);
      }
      if (Lang.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Lang);
      }
      if (ScriptId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ScriptId);
      }
      if (DefaultRegion.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(DefaultRegion);
      }
      if (HasMoreScripts != false) {
        size += 1 + 1;
      }
      if (HasStemming != false) {
        size += 1 + 1;
      }
      if (Alphabet.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Alphabet);
      }
      if (AlphabetUpper.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(AlphabetUpper);
      }
      if (WordSpellCheckLcid != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(WordSpellCheckLcid);
      }
      if (GoogleTransId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(GoogleTransId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CldrLang other) {
      if (other == null) {
        return;
      }
      if (other.Id.Length != 0) {
        Id = other.Id;
      }
      if (other.Lang.Length != 0) {
        Lang = other.Lang;
      }
      if (other.ScriptId.Length != 0) {
        ScriptId = other.ScriptId;
      }
      if (other.DefaultRegion.Length != 0) {
        DefaultRegion = other.DefaultRegion;
      }
      if (other.HasMoreScripts != false) {
        HasMoreScripts = other.HasMoreScripts;
      }
      if (other.HasStemming != false) {
        HasStemming = other.HasStemming;
      }
      if (other.Alphabet.Length != 0) {
        Alphabet = other.Alphabet;
      }
      if (other.AlphabetUpper.Length != 0) {
        AlphabetUpper = other.AlphabetUpper;
      }
      if (other.WordSpellCheckLcid != 0) {
        WordSpellCheckLcid = other.WordSpellCheckLcid;
      }
      if (other.GoogleTransId.Length != 0) {
        GoogleTransId = other.GoogleTransId;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Id = input.ReadString();
            break;
          }
          case 18: {
            Lang = input.ReadString();
            break;
          }
          case 26: {
            ScriptId = input.ReadString();
            break;
          }
          case 34: {
            DefaultRegion = input.ReadString();
            break;
          }
          case 40: {
            HasMoreScripts = input.ReadBool();
            break;
          }
          case 48: {
            HasStemming = input.ReadBool();
            break;
          }
          case 58: {
            Alphabet = input.ReadString();
            break;
          }
          case 66: {
            AlphabetUpper = input.ReadString();
            break;
          }
          case 72: {
            WordSpellCheckLcid = input.ReadInt32();
            break;
          }
          case 82: {
            GoogleTransId = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class UncRange : pb::IMessage<UncRange> {
    private static readonly pb::MessageParser<UncRange> _parser = new pb::MessageParser<UncRange>(() => new UncRange());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UncRange> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RewiseDom.LangsReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UncRange() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UncRange(UncRange other) : this() {
      start_ = other.start_;
      end_ = other.end_;
      idx_ = other.idx_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UncRange Clone() {
      return new UncRange(this);
    }

    /// <summary>Field number for the "start" field.</summary>
    public const int StartFieldNumber = 1;
    private int start_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Start {
      get { return start_; }
      set {
        start_ = value;
      }
    }

    /// <summary>Field number for the "end" field.</summary>
    public const int EndFieldNumber = 2;
    private int end_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int End {
      get { return end_; }
      set {
        end_ = value;
      }
    }

    /// <summary>Field number for the "idx" field.</summary>
    public const int IdxFieldNumber = 3;
    private int idx_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Idx {
      get { return idx_; }
      set {
        idx_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UncRange);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UncRange other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Start != other.Start) return false;
      if (End != other.End) return false;
      if (Idx != other.Idx) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Start != 0) hash ^= Start.GetHashCode();
      if (End != 0) hash ^= End.GetHashCode();
      if (Idx != 0) hash ^= Idx.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Start != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Start);
      }
      if (End != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(End);
      }
      if (Idx != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Idx);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Start != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Start);
      }
      if (End != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(End);
      }
      if (Idx != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Idx);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UncRange other) {
      if (other == null) {
        return;
      }
      if (other.Start != 0) {
        Start = other.Start;
      }
      if (other.End != 0) {
        End = other.End;
      }
      if (other.Idx != 0) {
        Idx = other.Idx;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Start = input.ReadInt32();
            break;
          }
          case 16: {
            End = input.ReadInt32();
            break;
          }
          case 24: {
            Idx = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class UncBlocks : pb::IMessage<UncBlocks> {
    private static readonly pb::MessageParser<UncBlocks> _parser = new pb::MessageParser<UncBlocks>(() => new UncBlocks());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UncBlocks> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RewiseDom.LangsReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UncBlocks() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UncBlocks(UncBlocks other) : this() {
      iSO15924_ = other.iSO15924_.Clone();
      ranges_ = other.ranges_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UncBlocks Clone() {
      return new UncBlocks(this);
    }

    /// <summary>Field number for the "ISO15924" field.</summary>
    public const int ISO15924FieldNumber = 1;
    private static readonly pb::FieldCodec<string> _repeated_iSO15924_codec
        = pb::FieldCodec.ForString(10);
    private readonly pbc::RepeatedField<string> iSO15924_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> ISO15924 {
      get { return iSO15924_; }
    }

    /// <summary>Field number for the "ranges" field.</summary>
    public const int RangesFieldNumber = 2;
    private static readonly pb::FieldCodec<global::RewiseDom.UncRange> _repeated_ranges_codec
        = pb::FieldCodec.ForMessage(18, global::RewiseDom.UncRange.Parser);
    private readonly pbc::RepeatedField<global::RewiseDom.UncRange> ranges_ = new pbc::RepeatedField<global::RewiseDom.UncRange>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::RewiseDom.UncRange> Ranges {
      get { return ranges_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UncBlocks);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UncBlocks other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!iSO15924_.Equals(other.iSO15924_)) return false;
      if(!ranges_.Equals(other.ranges_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= iSO15924_.GetHashCode();
      hash ^= ranges_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      iSO15924_.WriteTo(output, _repeated_iSO15924_codec);
      ranges_.WriteTo(output, _repeated_ranges_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += iSO15924_.CalculateSize(_repeated_iSO15924_codec);
      size += ranges_.CalculateSize(_repeated_ranges_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UncBlocks other) {
      if (other == null) {
        return;
      }
      iSO15924_.Add(other.iSO15924_);
      ranges_.Add(other.ranges_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            iSO15924_.AddEntriesFrom(input, _repeated_iSO15924_codec);
            break;
          }
          case 18: {
            ranges_.AddEntriesFrom(input, _repeated_ranges_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
