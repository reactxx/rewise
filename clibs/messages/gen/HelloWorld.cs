// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: rewise/utils/hello_world.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace RewiseDom {

  /// <summary>Holder for reflection information generated from rewise/utils/hello_world.proto</summary>
  public static partial class HelloWorldReflection {

    #region Descriptor
    /// <summary>File descriptor for rewise/utils/hello_world.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static HelloWorldReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch5yZXdpc2UvdXRpbHMvaGVsbG9fd29ybGQucHJvdG8SCXJld2lzZURvbSJc",
            "CgxIZWxsb1JlcXVlc3QSFAoMbm9fcmVjdXJzaW9uGAEgASgIEhIKCmRhcnRf",
            "Y291bnQYAiABKAUSDwoHZGFydF9pZBgDIAEoBRIRCgljc2hhcnBfaWQYBCAB",
            "KAUiRAoKSGVsbG9SZXBseRIPCgdkYXJ0X2lkGAEgASgFEhEKCWNzaGFycF9p",
            "ZBgCIAEoBRISCgpkYXJ0X2NvdW50GAMgASgFYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::RewiseDom.HelloRequest), global::RewiseDom.HelloRequest.Parser, new[]{ "NoRecursion", "DartCount", "DartId", "CsharpId" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::RewiseDom.HelloReply), global::RewiseDom.HelloReply.Parser, new[]{ "DartId", "CsharpId", "DartCount" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class HelloRequest : pb::IMessage<HelloRequest> {
    private static readonly pb::MessageParser<HelloRequest> _parser = new pb::MessageParser<HelloRequest>(() => new HelloRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<HelloRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RewiseDom.HelloWorldReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloRequest(HelloRequest other) : this() {
      noRecursion_ = other.noRecursion_;
      dartCount_ = other.dartCount_;
      dartId_ = other.dartId_;
      csharpId_ = other.csharpId_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloRequest Clone() {
      return new HelloRequest(this);
    }

    /// <summary>Field number for the "no_recursion" field.</summary>
    public const int NoRecursionFieldNumber = 1;
    private bool noRecursion_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool NoRecursion {
      get { return noRecursion_; }
      set {
        noRecursion_ = value;
      }
    }

    /// <summary>Field number for the "dart_count" field.</summary>
    public const int DartCountFieldNumber = 2;
    private int dartCount_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int DartCount {
      get { return dartCount_; }
      set {
        dartCount_ = value;
      }
    }

    /// <summary>Field number for the "dart_id" field.</summary>
    public const int DartIdFieldNumber = 3;
    private int dartId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int DartId {
      get { return dartId_; }
      set {
        dartId_ = value;
      }
    }

    /// <summary>Field number for the "csharp_id" field.</summary>
    public const int CsharpIdFieldNumber = 4;
    private int csharpId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CsharpId {
      get { return csharpId_; }
      set {
        csharpId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as HelloRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(HelloRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (NoRecursion != other.NoRecursion) return false;
      if (DartCount != other.DartCount) return false;
      if (DartId != other.DartId) return false;
      if (CsharpId != other.CsharpId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (NoRecursion != false) hash ^= NoRecursion.GetHashCode();
      if (DartCount != 0) hash ^= DartCount.GetHashCode();
      if (DartId != 0) hash ^= DartId.GetHashCode();
      if (CsharpId != 0) hash ^= CsharpId.GetHashCode();
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
      if (NoRecursion != false) {
        output.WriteRawTag(8);
        output.WriteBool(NoRecursion);
      }
      if (DartCount != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(DartCount);
      }
      if (DartId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(DartId);
      }
      if (CsharpId != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(CsharpId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (NoRecursion != false) {
        size += 1 + 1;
      }
      if (DartCount != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(DartCount);
      }
      if (DartId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(DartId);
      }
      if (CsharpId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(CsharpId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(HelloRequest other) {
      if (other == null) {
        return;
      }
      if (other.NoRecursion != false) {
        NoRecursion = other.NoRecursion;
      }
      if (other.DartCount != 0) {
        DartCount = other.DartCount;
      }
      if (other.DartId != 0) {
        DartId = other.DartId;
      }
      if (other.CsharpId != 0) {
        CsharpId = other.CsharpId;
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
            NoRecursion = input.ReadBool();
            break;
          }
          case 16: {
            DartCount = input.ReadInt32();
            break;
          }
          case 24: {
            DartId = input.ReadInt32();
            break;
          }
          case 32: {
            CsharpId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class HelloReply : pb::IMessage<HelloReply> {
    private static readonly pb::MessageParser<HelloReply> _parser = new pb::MessageParser<HelloReply>(() => new HelloReply());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<HelloReply> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RewiseDom.HelloWorldReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloReply() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloReply(HelloReply other) : this() {
      dartId_ = other.dartId_;
      csharpId_ = other.csharpId_;
      dartCount_ = other.dartCount_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloReply Clone() {
      return new HelloReply(this);
    }

    /// <summary>Field number for the "dart_id" field.</summary>
    public const int DartIdFieldNumber = 1;
    private int dartId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int DartId {
      get { return dartId_; }
      set {
        dartId_ = value;
      }
    }

    /// <summary>Field number for the "csharp_id" field.</summary>
    public const int CsharpIdFieldNumber = 2;
    private int csharpId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CsharpId {
      get { return csharpId_; }
      set {
        csharpId_ = value;
      }
    }

    /// <summary>Field number for the "dart_count" field.</summary>
    public const int DartCountFieldNumber = 3;
    private int dartCount_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int DartCount {
      get { return dartCount_; }
      set {
        dartCount_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as HelloReply);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(HelloReply other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (DartId != other.DartId) return false;
      if (CsharpId != other.CsharpId) return false;
      if (DartCount != other.DartCount) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (DartId != 0) hash ^= DartId.GetHashCode();
      if (CsharpId != 0) hash ^= CsharpId.GetHashCode();
      if (DartCount != 0) hash ^= DartCount.GetHashCode();
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
      if (DartId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(DartId);
      }
      if (CsharpId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(CsharpId);
      }
      if (DartCount != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(DartCount);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (DartId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(DartId);
      }
      if (CsharpId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(CsharpId);
      }
      if (DartCount != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(DartCount);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(HelloReply other) {
      if (other == null) {
        return;
      }
      if (other.DartId != 0) {
        DartId = other.DartId;
      }
      if (other.CsharpId != 0) {
        CsharpId = other.CsharpId;
      }
      if (other.DartCount != 0) {
        DartCount = other.DartCount;
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
            DartId = input.ReadInt32();
            break;
          }
          case 16: {
            CsharpId = input.ReadInt32();
            break;
          }
          case 24: {
            DartCount = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
