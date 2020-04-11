// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: rewise/stemming/stemming_service.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Rw.Stemming {

  /// <summary>Holder for reflection information generated from rewise/stemming/stemming_service.proto</summary>
  public static partial class StemmingServiceReflection {

    #region Descriptor
    /// <summary>File descriptor for rewise/stemming/stemming_service.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static StemmingServiceReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiZyZXdpc2Uvc3RlbW1pbmcvc3RlbW1pbmdfc2VydmljZS5wcm90bxILcncu",
            "c3RlbW1pbmciJgoHUmVxdWVzdBIMCgRsYW5nGAEgASgJEg0KBXdvcmRzGAIg",
            "AygJIjoKCFJlc3BvbnNlEgwKBGxhbmcYASABKAkSIAoFd29yZHMYAiADKAsy",
            "ES5ydy5zdGVtbWluZy5Xb3JkIjYKBFdvcmQSDgoGc3RlbW1zGAEgAygJEg4K",
            "Bm93bkxlbhgCIAEoBRIOCgZzb3VyY2UYAyABKAkyRwoNQ1NoYXJwU2Vydmlj",
            "ZRI2CgVTdGVtbRIULnJ3LnN0ZW1taW5nLlJlcXVlc3QaFS5ydy5zdGVtbWlu",
            "Zy5SZXNwb25zZSIAYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Rw.Stemming.Request), global::Rw.Stemming.Request.Parser, new[]{ "Lang", "Words" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Rw.Stemming.Response), global::Rw.Stemming.Response.Parser, new[]{ "Lang", "Words" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Rw.Stemming.Word), global::Rw.Stemming.Word.Parser, new[]{ "Stemms", "OwnLen", "Source" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Request : pb::IMessage<Request> {
    private static readonly pb::MessageParser<Request> _parser = new pb::MessageParser<Request>(() => new Request());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Request> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Rw.Stemming.StemmingServiceReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Request() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Request(Request other) : this() {
      lang_ = other.lang_;
      words_ = other.words_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Request Clone() {
      return new Request(this);
    }

    /// <summary>Field number for the "lang" field.</summary>
    public const int LangFieldNumber = 1;
    private string lang_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Lang {
      get { return lang_; }
      set {
        lang_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "words" field.</summary>
    public const int WordsFieldNumber = 2;
    private static readonly pb::FieldCodec<string> _repeated_words_codec
        = pb::FieldCodec.ForString(18);
    private readonly pbc::RepeatedField<string> words_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> Words {
      get { return words_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Request);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Request other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Lang != other.Lang) return false;
      if(!words_.Equals(other.words_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Lang.Length != 0) hash ^= Lang.GetHashCode();
      hash ^= words_.GetHashCode();
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
      if (Lang.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Lang);
      }
      words_.WriteTo(output, _repeated_words_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Lang.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Lang);
      }
      size += words_.CalculateSize(_repeated_words_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Request other) {
      if (other == null) {
        return;
      }
      if (other.Lang.Length != 0) {
        Lang = other.Lang;
      }
      words_.Add(other.words_);
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
            Lang = input.ReadString();
            break;
          }
          case 18: {
            words_.AddEntriesFrom(input, _repeated_words_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class Response : pb::IMessage<Response> {
    private static readonly pb::MessageParser<Response> _parser = new pb::MessageParser<Response>(() => new Response());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Response> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Rw.Stemming.StemmingServiceReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Response() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Response(Response other) : this() {
      lang_ = other.lang_;
      words_ = other.words_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Response Clone() {
      return new Response(this);
    }

    /// <summary>Field number for the "lang" field.</summary>
    public const int LangFieldNumber = 1;
    private string lang_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Lang {
      get { return lang_; }
      set {
        lang_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "words" field.</summary>
    public const int WordsFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Rw.Stemming.Word> _repeated_words_codec
        = pb::FieldCodec.ForMessage(18, global::Rw.Stemming.Word.Parser);
    private readonly pbc::RepeatedField<global::Rw.Stemming.Word> words_ = new pbc::RepeatedField<global::Rw.Stemming.Word>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Rw.Stemming.Word> Words {
      get { return words_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Response);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Response other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Lang != other.Lang) return false;
      if(!words_.Equals(other.words_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Lang.Length != 0) hash ^= Lang.GetHashCode();
      hash ^= words_.GetHashCode();
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
      if (Lang.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Lang);
      }
      words_.WriteTo(output, _repeated_words_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Lang.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Lang);
      }
      size += words_.CalculateSize(_repeated_words_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Response other) {
      if (other == null) {
        return;
      }
      if (other.Lang.Length != 0) {
        Lang = other.Lang;
      }
      words_.Add(other.words_);
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
            Lang = input.ReadString();
            break;
          }
          case 18: {
            words_.AddEntriesFrom(input, _repeated_words_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class Word : pb::IMessage<Word> {
    private static readonly pb::MessageParser<Word> _parser = new pb::MessageParser<Word>(() => new Word());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Word> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Rw.Stemming.StemmingServiceReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Word() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Word(Word other) : this() {
      stemms_ = other.stemms_.Clone();
      ownLen_ = other.ownLen_;
      source_ = other.source_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Word Clone() {
      return new Word(this);
    }

    /// <summary>Field number for the "stemms" field.</summary>
    public const int StemmsFieldNumber = 1;
    private static readonly pb::FieldCodec<string> _repeated_stemms_codec
        = pb::FieldCodec.ForString(10);
    private readonly pbc::RepeatedField<string> stemms_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> Stemms {
      get { return stemms_; }
    }

    /// <summary>Field number for the "ownLen" field.</summary>
    public const int OwnLenFieldNumber = 2;
    private int ownLen_;
    /// <summary>
    /// words[0..ownLen-1] are words, with stemms's stemming result
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int OwnLen {
      get { return ownLen_; }
      set {
        ownLen_ = value;
      }
    }

    /// <summary>Field number for the "source" field.</summary>
    public const int SourceFieldNumber = 3;
    private string source_ = "";
    /// <summary>
    /// not empty if source word is not within stemms
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Source {
      get { return source_; }
      set {
        source_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Word);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Word other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!stemms_.Equals(other.stemms_)) return false;
      if (OwnLen != other.OwnLen) return false;
      if (Source != other.Source) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= stemms_.GetHashCode();
      if (OwnLen != 0) hash ^= OwnLen.GetHashCode();
      if (Source.Length != 0) hash ^= Source.GetHashCode();
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
      stemms_.WriteTo(output, _repeated_stemms_codec);
      if (OwnLen != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(OwnLen);
      }
      if (Source.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Source);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += stemms_.CalculateSize(_repeated_stemms_codec);
      if (OwnLen != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(OwnLen);
      }
      if (Source.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Source);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Word other) {
      if (other == null) {
        return;
      }
      stemms_.Add(other.stemms_);
      if (other.OwnLen != 0) {
        OwnLen = other.OwnLen;
      }
      if (other.Source.Length != 0) {
        Source = other.Source;
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
            stemms_.AddEntriesFrom(input, _repeated_stemms_codec);
            break;
          }
          case 16: {
            OwnLen = input.ReadInt32();
            break;
          }
          case 26: {
            Source = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
