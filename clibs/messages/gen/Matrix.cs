// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: rewise/utils/matrix.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Rw.Common {

  /// <summary>Holder for reflection information generated from rewise/utils/matrix.proto</summary>
  public static partial class MatrixReflection {

    #region Descriptor
    /// <summary>File descriptor for rewise/utils/matrix.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static MatrixReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChlyZXdpc2UvdXRpbHMvbWF0cml4LnByb3RvEglydy5jb21tb24iNAoGTWF0",
            "cml4EhwKBHJvd3MYASADKAsyDi5ydy5jb21tb24uUm93EgwKBGNvbHMYAiAD",
            "KAkiJAoDUm93Eg0KBWxhbmdzGAEgAygJEg4KBnZhbHVlcxgCIAMoCWIGcHJv",
            "dG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Rw.Common.Matrix), global::Rw.Common.Matrix.Parser, new[]{ "Rows", "Cols" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Rw.Common.Row), global::Rw.Common.Row.Parser, new[]{ "Langs", "Values" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Matrix : pb::IMessage<Matrix> {
    private static readonly pb::MessageParser<Matrix> _parser = new pb::MessageParser<Matrix>(() => new Matrix());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Matrix> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Rw.Common.MatrixReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Matrix() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Matrix(Matrix other) : this() {
      rows_ = other.rows_.Clone();
      cols_ = other.cols_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Matrix Clone() {
      return new Matrix(this);
    }

    /// <summary>Field number for the "rows" field.</summary>
    public const int RowsFieldNumber = 1;
    private static readonly pb::FieldCodec<global::Rw.Common.Row> _repeated_rows_codec
        = pb::FieldCodec.ForMessage(10, global::Rw.Common.Row.Parser);
    private readonly pbc::RepeatedField<global::Rw.Common.Row> rows_ = new pbc::RepeatedField<global::Rw.Common.Row>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Rw.Common.Row> Rows {
      get { return rows_; }
    }

    /// <summary>Field number for the "cols" field.</summary>
    public const int ColsFieldNumber = 2;
    private static readonly pb::FieldCodec<string> _repeated_cols_codec
        = pb::FieldCodec.ForString(18);
    private readonly pbc::RepeatedField<string> cols_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> Cols {
      get { return cols_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Matrix);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Matrix other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!rows_.Equals(other.rows_)) return false;
      if(!cols_.Equals(other.cols_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= rows_.GetHashCode();
      hash ^= cols_.GetHashCode();
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
      rows_.WriteTo(output, _repeated_rows_codec);
      cols_.WriteTo(output, _repeated_cols_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += rows_.CalculateSize(_repeated_rows_codec);
      size += cols_.CalculateSize(_repeated_cols_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Matrix other) {
      if (other == null) {
        return;
      }
      rows_.Add(other.rows_);
      cols_.Add(other.cols_);
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
            rows_.AddEntriesFrom(input, _repeated_rows_codec);
            break;
          }
          case 18: {
            cols_.AddEntriesFrom(input, _repeated_cols_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class Row : pb::IMessage<Row> {
    private static readonly pb::MessageParser<Row> _parser = new pb::MessageParser<Row>(() => new Row());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Row> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Rw.Common.MatrixReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Row() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Row(Row other) : this() {
      langs_ = other.langs_.Clone();
      values_ = other.values_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Row Clone() {
      return new Row(this);
    }

    /// <summary>Field number for the "langs" field.</summary>
    public const int LangsFieldNumber = 1;
    private static readonly pb::FieldCodec<string> _repeated_langs_codec
        = pb::FieldCodec.ForString(10);
    private readonly pbc::RepeatedField<string> langs_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> Langs {
      get { return langs_; }
    }

    /// <summary>Field number for the "values" field.</summary>
    public const int ValuesFieldNumber = 2;
    private static readonly pb::FieldCodec<string> _repeated_values_codec
        = pb::FieldCodec.ForString(18);
    private readonly pbc::RepeatedField<string> values_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> Values {
      get { return values_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Row);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Row other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!langs_.Equals(other.langs_)) return false;
      if(!values_.Equals(other.values_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= langs_.GetHashCode();
      hash ^= values_.GetHashCode();
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
      values_.WriteTo(output, _repeated_values_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += langs_.CalculateSize(_repeated_langs_codec);
      size += values_.CalculateSize(_repeated_values_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Row other) {
      if (other == null) {
        return;
      }
      langs_.Add(other.langs_);
      values_.Add(other.values_);
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
          case 18: {
            values_.AddEntriesFrom(input, _repeated_values_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code