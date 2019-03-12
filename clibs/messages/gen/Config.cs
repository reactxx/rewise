// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: rewise/utils/config.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace RewiseDom {

  /// <summary>Holder for reflection information generated from rewise/utils/config.proto</summary>
  public static partial class ConfigReflection {

    #region Descriptor
    /// <summary>File descriptor for rewise/utils/config.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ConfigReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChlyZXdpc2UvdXRpbHMvY29uZmlnLnByb3RvEglyZXdpc2VEb20iiQEKBkNv",
            "bmZpZxI2Cgt3b3JrX3NwYWNlcxgBIAMoCzIhLnJld2lzZURvbS5Db25maWcu",
            "V29ya1NwYWNlc0VudHJ5GkcKD1dvcmtTcGFjZXNFbnRyeRILCgNrZXkYASAB",
            "KAkSIwoFdmFsdWUYAiABKAsyFC5yZXdpc2VEb20uV29ya1NwYWNlOgI4ASJj",
            "CglXb3JrU3BhY2USKQoKZGFydFNlcnZlchgBIAEoCzIVLnJld2lzZURvbS5D",
            "b25uZWN0aW9uEisKDGNzaGFycFNlcnZlchgCIAEoCzIVLnJld2lzZURvbS5D",
            "b25uZWN0aW9uIigKCkNvbm5lY3Rpb24SDAoEaG9zdBgBIAEoCRIMCgRwb3J0",
            "GAIgASgFYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::RewiseDom.Config), global::RewiseDom.Config.Parser, new[]{ "WorkSpaces" }, null, null, new pbr::GeneratedClrTypeInfo[] { null, }),
            new pbr::GeneratedClrTypeInfo(typeof(global::RewiseDom.WorkSpace), global::RewiseDom.WorkSpace.Parser, new[]{ "DartServer", "CsharpServer" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::RewiseDom.Connection), global::RewiseDom.Connection.Parser, new[]{ "Host", "Port" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Config : pb::IMessage<Config> {
    private static readonly pb::MessageParser<Config> _parser = new pb::MessageParser<Config>(() => new Config());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Config> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RewiseDom.ConfigReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Config() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Config(Config other) : this() {
      workSpaces_ = other.workSpaces_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Config Clone() {
      return new Config(this);
    }

    /// <summary>Field number for the "work_spaces" field.</summary>
    public const int WorkSpacesFieldNumber = 1;
    private static readonly pbc::MapField<string, global::RewiseDom.WorkSpace>.Codec _map_workSpaces_codec
        = new pbc::MapField<string, global::RewiseDom.WorkSpace>.Codec(pb::FieldCodec.ForString(10), pb::FieldCodec.ForMessage(18, global::RewiseDom.WorkSpace.Parser), 10);
    private readonly pbc::MapField<string, global::RewiseDom.WorkSpace> workSpaces_ = new pbc::MapField<string, global::RewiseDom.WorkSpace>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::MapField<string, global::RewiseDom.WorkSpace> WorkSpaces {
      get { return workSpaces_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Config);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Config other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!WorkSpaces.Equals(other.WorkSpaces)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= WorkSpaces.GetHashCode();
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
      workSpaces_.WriteTo(output, _map_workSpaces_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += workSpaces_.CalculateSize(_map_workSpaces_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Config other) {
      if (other == null) {
        return;
      }
      workSpaces_.Add(other.workSpaces_);
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
            workSpaces_.AddEntriesFrom(input, _map_workSpaces_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class WorkSpace : pb::IMessage<WorkSpace> {
    private static readonly pb::MessageParser<WorkSpace> _parser = new pb::MessageParser<WorkSpace>(() => new WorkSpace());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<WorkSpace> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RewiseDom.ConfigReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public WorkSpace() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public WorkSpace(WorkSpace other) : this() {
      dartServer_ = other.dartServer_ != null ? other.dartServer_.Clone() : null;
      csharpServer_ = other.csharpServer_ != null ? other.csharpServer_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public WorkSpace Clone() {
      return new WorkSpace(this);
    }

    /// <summary>Field number for the "dartServer" field.</summary>
    public const int DartServerFieldNumber = 1;
    private global::RewiseDom.Connection dartServer_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::RewiseDom.Connection DartServer {
      get { return dartServer_; }
      set {
        dartServer_ = value;
      }
    }

    /// <summary>Field number for the "csharpServer" field.</summary>
    public const int CsharpServerFieldNumber = 2;
    private global::RewiseDom.Connection csharpServer_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::RewiseDom.Connection CsharpServer {
      get { return csharpServer_; }
      set {
        csharpServer_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as WorkSpace);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(WorkSpace other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(DartServer, other.DartServer)) return false;
      if (!object.Equals(CsharpServer, other.CsharpServer)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (dartServer_ != null) hash ^= DartServer.GetHashCode();
      if (csharpServer_ != null) hash ^= CsharpServer.GetHashCode();
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
      if (dartServer_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(DartServer);
      }
      if (csharpServer_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(CsharpServer);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (dartServer_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(DartServer);
      }
      if (csharpServer_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(CsharpServer);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(WorkSpace other) {
      if (other == null) {
        return;
      }
      if (other.dartServer_ != null) {
        if (dartServer_ == null) {
          dartServer_ = new global::RewiseDom.Connection();
        }
        DartServer.MergeFrom(other.DartServer);
      }
      if (other.csharpServer_ != null) {
        if (csharpServer_ == null) {
          csharpServer_ = new global::RewiseDom.Connection();
        }
        CsharpServer.MergeFrom(other.CsharpServer);
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
            if (dartServer_ == null) {
              dartServer_ = new global::RewiseDom.Connection();
            }
            input.ReadMessage(dartServer_);
            break;
          }
          case 18: {
            if (csharpServer_ == null) {
              csharpServer_ = new global::RewiseDom.Connection();
            }
            input.ReadMessage(csharpServer_);
            break;
          }
        }
      }
    }

  }

  public sealed partial class Connection : pb::IMessage<Connection> {
    private static readonly pb::MessageParser<Connection> _parser = new pb::MessageParser<Connection>(() => new Connection());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Connection> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RewiseDom.ConfigReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Connection() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Connection(Connection other) : this() {
      host_ = other.host_;
      port_ = other.port_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Connection Clone() {
      return new Connection(this);
    }

    /// <summary>Field number for the "host" field.</summary>
    public const int HostFieldNumber = 1;
    private string host_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Host {
      get { return host_; }
      set {
        host_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "port" field.</summary>
    public const int PortFieldNumber = 2;
    private int port_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Port {
      get { return port_; }
      set {
        port_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Connection);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Connection other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Host != other.Host) return false;
      if (Port != other.Port) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Host.Length != 0) hash ^= Host.GetHashCode();
      if (Port != 0) hash ^= Port.GetHashCode();
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
      if (Host.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Host);
      }
      if (Port != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Port);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Host.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Host);
      }
      if (Port != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Port);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Connection other) {
      if (other == null) {
        return;
      }
      if (other.Host.Length != 0) {
        Host = other.Host;
      }
      if (other.Port != 0) {
        Port = other.Port;
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
            Host = input.ReadString();
            break;
          }
          case 16: {
            Port = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
