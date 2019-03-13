// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: rewise/books_import/books_import_fromrj.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace RewiseDom {

  /// <summary>Holder for reflection information generated from rewise/books_import/books_import_fromrj.proto</summary>
  public static partial class BooksImportFromrjReflection {

    #region Descriptor
    /// <summary>File descriptor for rewise/books_import/books_import_fromrj.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BooksImportFromrjReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ci1yZXdpc2UvYm9va3NfaW1wb3J0L2Jvb2tzX2ltcG9ydF9mcm9tcmoucHJv",
            "dG8SCXJld2lzZURvbSI8ChBGaWxlTmFtZXNSZXF1ZXN0EigKCmZpbGVfbmFt",
            "ZXMYASADKAsyFC5yZXdpc2VEb20uRmlsZU5hbWVzIigKCUZpbGVOYW1lcxIO",
            "CgZtYXRyaXgYASABKAkSCwoDYmluGAIgASgJImYKB0Jvb2tPdXQSDAoEbmFt",
            "ZRgBIAEoCRIZChFlcnJvcl93cm9uZ19sYW5ncxgEIAMoCRIhCgVmYWN0cxgF",
            "IAMoCzISLnJld2lzZURvbS5GYWN0T3V0Eg8KB2xlc3NvbnMYBiADKAUiJgoH",
            "RmFjdE91dBIMCgRsYW5nGAEgASgJEg0KBXdvcmRzGAIgAygJIgoKCEJvb2tN",
            "ZXRhYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::RewiseDom.FileNamesRequest), global::RewiseDom.FileNamesRequest.Parser, new[]{ "FileNames" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::RewiseDom.FileNames), global::RewiseDom.FileNames.Parser, new[]{ "Matrix", "Bin" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::RewiseDom.BookOut), global::RewiseDom.BookOut.Parser, new[]{ "Name", "ErrorWrongLangs", "Facts", "Lessons" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::RewiseDom.FactOut), global::RewiseDom.FactOut.Parser, new[]{ "Lang", "Words" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::RewiseDom.BookMeta), global::RewiseDom.BookMeta.Parser, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class FileNamesRequest : pb::IMessage<FileNamesRequest> {
    private static readonly pb::MessageParser<FileNamesRequest> _parser = new pb::MessageParser<FileNamesRequest>(() => new FileNamesRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FileNamesRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RewiseDom.BooksImportFromrjReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FileNamesRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FileNamesRequest(FileNamesRequest other) : this() {
      fileNames_ = other.fileNames_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FileNamesRequest Clone() {
      return new FileNamesRequest(this);
    }

    /// <summary>Field number for the "file_names" field.</summary>
    public const int FileNamesFieldNumber = 1;
    private static readonly pb::FieldCodec<global::RewiseDom.FileNames> _repeated_fileNames_codec
        = pb::FieldCodec.ForMessage(10, global::RewiseDom.FileNames.Parser);
    private readonly pbc::RepeatedField<global::RewiseDom.FileNames> fileNames_ = new pbc::RepeatedField<global::RewiseDom.FileNames>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::RewiseDom.FileNames> FileNames {
      get { return fileNames_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FileNamesRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FileNamesRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!fileNames_.Equals(other.fileNames_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= fileNames_.GetHashCode();
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
      fileNames_.WriteTo(output, _repeated_fileNames_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += fileNames_.CalculateSize(_repeated_fileNames_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FileNamesRequest other) {
      if (other == null) {
        return;
      }
      fileNames_.Add(other.fileNames_);
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
            fileNames_.AddEntriesFrom(input, _repeated_fileNames_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class FileNames : pb::IMessage<FileNames> {
    private static readonly pb::MessageParser<FileNames> _parser = new pb::MessageParser<FileNames>(() => new FileNames());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FileNames> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RewiseDom.BooksImportFromrjReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FileNames() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FileNames(FileNames other) : this() {
      matrix_ = other.matrix_;
      bin_ = other.bin_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FileNames Clone() {
      return new FileNames(this);
    }

    /// <summary>Field number for the "matrix" field.</summary>
    public const int MatrixFieldNumber = 1;
    private string matrix_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Matrix {
      get { return matrix_; }
      set {
        matrix_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "bin" field.</summary>
    public const int BinFieldNumber = 2;
    private string bin_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Bin {
      get { return bin_; }
      set {
        bin_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FileNames);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FileNames other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Matrix != other.Matrix) return false;
      if (Bin != other.Bin) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Matrix.Length != 0) hash ^= Matrix.GetHashCode();
      if (Bin.Length != 0) hash ^= Bin.GetHashCode();
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
      if (Matrix.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Matrix);
      }
      if (Bin.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Bin);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Matrix.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Matrix);
      }
      if (Bin.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Bin);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FileNames other) {
      if (other == null) {
        return;
      }
      if (other.Matrix.Length != 0) {
        Matrix = other.Matrix;
      }
      if (other.Bin.Length != 0) {
        Bin = other.Bin;
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
            Matrix = input.ReadString();
            break;
          }
          case 18: {
            Bin = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class BookOut : pb::IMessage<BookOut> {
    private static readonly pb::MessageParser<BookOut> _parser = new pb::MessageParser<BookOut>(() => new BookOut());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<BookOut> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RewiseDom.BooksImportFromrjReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public BookOut() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public BookOut(BookOut other) : this() {
      name_ = other.name_;
      errorWrongLangs_ = other.errorWrongLangs_.Clone();
      facts_ = other.facts_.Clone();
      lessons_ = other.lessons_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public BookOut Clone() {
      return new BookOut(this);
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 1;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "error_wrong_langs" field.</summary>
    public const int ErrorWrongLangsFieldNumber = 4;
    private static readonly pb::FieldCodec<string> _repeated_errorWrongLangs_codec
        = pb::FieldCodec.ForString(34);
    private readonly pbc::RepeatedField<string> errorWrongLangs_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> ErrorWrongLangs {
      get { return errorWrongLangs_; }
    }

    /// <summary>Field number for the "facts" field.</summary>
    public const int FactsFieldNumber = 5;
    private static readonly pb::FieldCodec<global::RewiseDom.FactOut> _repeated_facts_codec
        = pb::FieldCodec.ForMessage(42, global::RewiseDom.FactOut.Parser);
    private readonly pbc::RepeatedField<global::RewiseDom.FactOut> facts_ = new pbc::RepeatedField<global::RewiseDom.FactOut>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::RewiseDom.FactOut> Facts {
      get { return facts_; }
    }

    /// <summary>Field number for the "lessons" field.</summary>
    public const int LessonsFieldNumber = 6;
    private static readonly pb::FieldCodec<int> _repeated_lessons_codec
        = pb::FieldCodec.ForInt32(50);
    private readonly pbc::RepeatedField<int> lessons_ = new pbc::RepeatedField<int>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<int> Lessons {
      get { return lessons_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as BookOut);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(BookOut other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Name != other.Name) return false;
      if(!errorWrongLangs_.Equals(other.errorWrongLangs_)) return false;
      if(!facts_.Equals(other.facts_)) return false;
      if(!lessons_.Equals(other.lessons_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      hash ^= errorWrongLangs_.GetHashCode();
      hash ^= facts_.GetHashCode();
      hash ^= lessons_.GetHashCode();
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
      if (Name.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Name);
      }
      errorWrongLangs_.WriteTo(output, _repeated_errorWrongLangs_codec);
      facts_.WriteTo(output, _repeated_facts_codec);
      lessons_.WriteTo(output, _repeated_lessons_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      size += errorWrongLangs_.CalculateSize(_repeated_errorWrongLangs_codec);
      size += facts_.CalculateSize(_repeated_facts_codec);
      size += lessons_.CalculateSize(_repeated_lessons_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(BookOut other) {
      if (other == null) {
        return;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      errorWrongLangs_.Add(other.errorWrongLangs_);
      facts_.Add(other.facts_);
      lessons_.Add(other.lessons_);
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
            Name = input.ReadString();
            break;
          }
          case 34: {
            errorWrongLangs_.AddEntriesFrom(input, _repeated_errorWrongLangs_codec);
            break;
          }
          case 42: {
            facts_.AddEntriesFrom(input, _repeated_facts_codec);
            break;
          }
          case 50:
          case 48: {
            lessons_.AddEntriesFrom(input, _repeated_lessons_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class FactOut : pb::IMessage<FactOut> {
    private static readonly pb::MessageParser<FactOut> _parser = new pb::MessageParser<FactOut>(() => new FactOut());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FactOut> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RewiseDom.BooksImportFromrjReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FactOut() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FactOut(FactOut other) : this() {
      lang_ = other.lang_;
      words_ = other.words_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FactOut Clone() {
      return new FactOut(this);
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
      return Equals(other as FactOut);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FactOut other) {
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
    public void MergeFrom(FactOut other) {
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

  public sealed partial class BookMeta : pb::IMessage<BookMeta> {
    private static readonly pb::MessageParser<BookMeta> _parser = new pb::MessageParser<BookMeta>(() => new BookMeta());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<BookMeta> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RewiseDom.BooksImportFromrjReflection.Descriptor.MessageTypes[4]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public BookMeta() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public BookMeta(BookMeta other) : this() {
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public BookMeta Clone() {
      return new BookMeta(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as BookMeta);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(BookMeta other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
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
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(BookMeta other) {
      if (other == null) {
        return;
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
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
