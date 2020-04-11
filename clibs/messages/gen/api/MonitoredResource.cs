// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: google/api/monitored_resource.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Google.Api {

  /// <summary>Holder for reflection information generated from google/api/monitored_resource.proto</summary>
  public static partial class MonitoredResourceReflection {

    #region Descriptor
    /// <summary>File descriptor for google/api/monitored_resource.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static MonitoredResourceReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiNnb29nbGUvYXBpL21vbml0b3JlZF9yZXNvdXJjZS5wcm90bxIKZ29vZ2xl",
            "LmFwaRoWZ29vZ2xlL2FwaS9sYWJlbC5wcm90bxodZ29vZ2xlL2FwaS9sYXVu",
            "Y2hfc3RhZ2UucHJvdG8aHGdvb2dsZS9wcm90b2J1Zi9zdHJ1Y3QucHJvdG8i",
            "wAEKG01vbml0b3JlZFJlc291cmNlRGVzY3JpcHRvchIMCgRuYW1lGAUgASgJ",
            "EgwKBHR5cGUYASABKAkSFAoMZGlzcGxheV9uYW1lGAIgASgJEhMKC2Rlc2Ny",
            "aXB0aW9uGAMgASgJEisKBmxhYmVscxgEIAMoCzIbLmdvb2dsZS5hcGkuTGFi",
            "ZWxEZXNjcmlwdG9yEi0KDGxhdW5jaF9zdGFnZRgHIAEoDjIXLmdvb2dsZS5h",
            "cGkuTGF1bmNoU3RhZ2UiiwEKEU1vbml0b3JlZFJlc291cmNlEgwKBHR5cGUY",
            "ASABKAkSOQoGbGFiZWxzGAIgAygLMikuZ29vZ2xlLmFwaS5Nb25pdG9yZWRS",
            "ZXNvdXJjZS5MYWJlbHNFbnRyeRotCgtMYWJlbHNFbnRyeRILCgNrZXkYASAB",
            "KAkSDQoFdmFsdWUYAiABKAk6AjgBIsoBChlNb25pdG9yZWRSZXNvdXJjZU1l",
            "dGFkYXRhEi4KDXN5c3RlbV9sYWJlbHMYASABKAsyFy5nb29nbGUucHJvdG9i",
            "dWYuU3RydWN0EkoKC3VzZXJfbGFiZWxzGAIgAygLMjUuZ29vZ2xlLmFwaS5N",
            "b25pdG9yZWRSZXNvdXJjZU1ldGFkYXRhLlVzZXJMYWJlbHNFbnRyeRoxCg9V",
            "c2VyTGFiZWxzRW50cnkSCwoDa2V5GAEgASgJEg0KBXZhbHVlGAIgASgJOgI4",
            "AUJ5Cg5jb20uZ29vZ2xlLmFwaUIWTW9uaXRvcmVkUmVzb3VyY2VQcm90b1AB",
            "WkNnb29nbGUuZ29sYW5nLm9yZy9nZW5wcm90by9nb29nbGVhcGlzL2FwaS9t",
            "b25pdG9yZWRyZXM7bW9uaXRvcmVkcmVz+AEBogIER0FQSWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Api.LabelReflection.Descriptor, global::Google.Api.LaunchStageReflection.Descriptor, global::Google.Protobuf.WellKnownTypes.StructReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Api.MonitoredResourceDescriptor), global::Google.Api.MonitoredResourceDescriptor.Parser, new[]{ "Name", "Type", "DisplayName", "Description", "Labels", "LaunchStage" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Api.MonitoredResource), global::Google.Api.MonitoredResource.Parser, new[]{ "Type", "Labels" }, null, null, null, new pbr::GeneratedClrTypeInfo[] { null, }),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Api.MonitoredResourceMetadata), global::Google.Api.MonitoredResourceMetadata.Parser, new[]{ "SystemLabels", "UserLabels" }, null, null, null, new pbr::GeneratedClrTypeInfo[] { null, })
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// An object that describes the schema of a [MonitoredResource][google.api.MonitoredResource] object using a
  /// type name and a set of labels.  For example, the monitored resource
  /// descriptor for Google Compute Engine VM instances has a type of
  /// `"gce_instance"` and specifies the use of the labels `"instance_id"` and
  /// `"zone"` to identify particular VM instances.
  ///
  /// Different APIs can support different monitored resource types. APIs generally
  /// provide a `list` method that returns the monitored resource descriptors used
  /// by the API.
  /// </summary>
  public sealed partial class MonitoredResourceDescriptor : pb::IMessage<MonitoredResourceDescriptor> {
    private static readonly pb::MessageParser<MonitoredResourceDescriptor> _parser = new pb::MessageParser<MonitoredResourceDescriptor>(() => new MonitoredResourceDescriptor());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MonitoredResourceDescriptor> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Api.MonitoredResourceReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MonitoredResourceDescriptor() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MonitoredResourceDescriptor(MonitoredResourceDescriptor other) : this() {
      name_ = other.name_;
      type_ = other.type_;
      displayName_ = other.displayName_;
      description_ = other.description_;
      labels_ = other.labels_.Clone();
      launchStage_ = other.launchStage_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MonitoredResourceDescriptor Clone() {
      return new MonitoredResourceDescriptor(this);
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 5;
    private string name_ = "";
    /// <summary>
    /// Optional. The resource name of the monitored resource descriptor:
    /// `"projects/{project_id}/monitoredResourceDescriptors/{type}"` where
    /// {type} is the value of the `type` field in this object and
    /// {project_id} is a project ID that provides API-specific context for
    /// accessing the type.  APIs that do not use project information can use the
    /// resource name format `"monitoredResourceDescriptors/{type}"`.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "type" field.</summary>
    public const int TypeFieldNumber = 1;
    private string type_ = "";
    /// <summary>
    /// Required. The monitored resource type. For example, the type
    /// `"cloudsql_database"` represents databases in Google Cloud SQL.
    /// The maximum length of this value is 256 characters.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Type {
      get { return type_; }
      set {
        type_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "display_name" field.</summary>
    public const int DisplayNameFieldNumber = 2;
    private string displayName_ = "";
    /// <summary>
    /// Optional. A concise name for the monitored resource type that might be
    /// displayed in user interfaces. It should be a Title Cased Noun Phrase,
    /// without any article or other determiners. For example,
    /// `"Google Cloud SQL Database"`.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string DisplayName {
      get { return displayName_; }
      set {
        displayName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "description" field.</summary>
    public const int DescriptionFieldNumber = 3;
    private string description_ = "";
    /// <summary>
    /// Optional. A detailed description of the monitored resource type that might
    /// be used in documentation.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Description {
      get { return description_; }
      set {
        description_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "labels" field.</summary>
    public const int LabelsFieldNumber = 4;
    private static readonly pb::FieldCodec<global::Google.Api.LabelDescriptor> _repeated_labels_codec
        = pb::FieldCodec.ForMessage(34, global::Google.Api.LabelDescriptor.Parser);
    private readonly pbc::RepeatedField<global::Google.Api.LabelDescriptor> labels_ = new pbc::RepeatedField<global::Google.Api.LabelDescriptor>();
    /// <summary>
    /// Required. A set of labels used to describe instances of this monitored
    /// resource type. For example, an individual Google Cloud SQL database is
    /// identified by values for the labels `"database_id"` and `"zone"`.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Google.Api.LabelDescriptor> Labels {
      get { return labels_; }
    }

    /// <summary>Field number for the "launch_stage" field.</summary>
    public const int LaunchStageFieldNumber = 7;
    private global::Google.Api.LaunchStage launchStage_ = global::Google.Api.LaunchStage.Unspecified;
    /// <summary>
    /// Optional. The launch stage of the monitored resource definition.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Api.LaunchStage LaunchStage {
      get { return launchStage_; }
      set {
        launchStage_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MonitoredResourceDescriptor);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MonitoredResourceDescriptor other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Name != other.Name) return false;
      if (Type != other.Type) return false;
      if (DisplayName != other.DisplayName) return false;
      if (Description != other.Description) return false;
      if(!labels_.Equals(other.labels_)) return false;
      if (LaunchStage != other.LaunchStage) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Type.Length != 0) hash ^= Type.GetHashCode();
      if (DisplayName.Length != 0) hash ^= DisplayName.GetHashCode();
      if (Description.Length != 0) hash ^= Description.GetHashCode();
      hash ^= labels_.GetHashCode();
      if (LaunchStage != global::Google.Api.LaunchStage.Unspecified) hash ^= LaunchStage.GetHashCode();
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
      if (Type.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Type);
      }
      if (DisplayName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(DisplayName);
      }
      if (Description.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Description);
      }
      labels_.WriteTo(output, _repeated_labels_codec);
      if (Name.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(Name);
      }
      if (LaunchStage != global::Google.Api.LaunchStage.Unspecified) {
        output.WriteRawTag(56);
        output.WriteEnum((int) LaunchStage);
      }
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
      if (Type.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Type);
      }
      if (DisplayName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(DisplayName);
      }
      if (Description.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Description);
      }
      size += labels_.CalculateSize(_repeated_labels_codec);
      if (LaunchStage != global::Google.Api.LaunchStage.Unspecified) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) LaunchStage);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MonitoredResourceDescriptor other) {
      if (other == null) {
        return;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.Type.Length != 0) {
        Type = other.Type;
      }
      if (other.DisplayName.Length != 0) {
        DisplayName = other.DisplayName;
      }
      if (other.Description.Length != 0) {
        Description = other.Description;
      }
      labels_.Add(other.labels_);
      if (other.LaunchStage != global::Google.Api.LaunchStage.Unspecified) {
        LaunchStage = other.LaunchStage;
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
            Type = input.ReadString();
            break;
          }
          case 18: {
            DisplayName = input.ReadString();
            break;
          }
          case 26: {
            Description = input.ReadString();
            break;
          }
          case 34: {
            labels_.AddEntriesFrom(input, _repeated_labels_codec);
            break;
          }
          case 42: {
            Name = input.ReadString();
            break;
          }
          case 56: {
            LaunchStage = (global::Google.Api.LaunchStage) input.ReadEnum();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// An object representing a resource that can be used for monitoring, logging,
  /// billing, or other purposes. Examples include virtual machine instances,
  /// databases, and storage devices such as disks. The `type` field identifies a
  /// [MonitoredResourceDescriptor][google.api.MonitoredResourceDescriptor] object that describes the resource's
  /// schema. Information in the `labels` field identifies the actual resource and
  /// its attributes according to the schema. For example, a particular Compute
  /// Engine VM instance could be represented by the following object, because the
  /// [MonitoredResourceDescriptor][google.api.MonitoredResourceDescriptor] for `"gce_instance"` has labels
  /// `"instance_id"` and `"zone"`:
  ///
  ///     { "type": "gce_instance",
  ///       "labels": { "instance_id": "12345678901234",
  ///                   "zone": "us-central1-a" }}
  /// </summary>
  public sealed partial class MonitoredResource : pb::IMessage<MonitoredResource> {
    private static readonly pb::MessageParser<MonitoredResource> _parser = new pb::MessageParser<MonitoredResource>(() => new MonitoredResource());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MonitoredResource> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Api.MonitoredResourceReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MonitoredResource() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MonitoredResource(MonitoredResource other) : this() {
      type_ = other.type_;
      labels_ = other.labels_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MonitoredResource Clone() {
      return new MonitoredResource(this);
    }

    /// <summary>Field number for the "type" field.</summary>
    public const int TypeFieldNumber = 1;
    private string type_ = "";
    /// <summary>
    /// Required. The monitored resource type. This field must match
    /// the `type` field of a [MonitoredResourceDescriptor][google.api.MonitoredResourceDescriptor] object. For
    /// example, the type of a Compute Engine VM instance is `gce_instance`.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Type {
      get { return type_; }
      set {
        type_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "labels" field.</summary>
    public const int LabelsFieldNumber = 2;
    private static readonly pbc::MapField<string, string>.Codec _map_labels_codec
        = new pbc::MapField<string, string>.Codec(pb::FieldCodec.ForString(10, ""), pb::FieldCodec.ForString(18, ""), 18);
    private readonly pbc::MapField<string, string> labels_ = new pbc::MapField<string, string>();
    /// <summary>
    /// Required. Values for all of the labels listed in the associated monitored
    /// resource descriptor. For example, Compute Engine VM instances use the
    /// labels `"project_id"`, `"instance_id"`, and `"zone"`.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::MapField<string, string> Labels {
      get { return labels_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MonitoredResource);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MonitoredResource other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Type != other.Type) return false;
      if (!Labels.Equals(other.Labels)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Type.Length != 0) hash ^= Type.GetHashCode();
      hash ^= Labels.GetHashCode();
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
      if (Type.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Type);
      }
      labels_.WriteTo(output, _map_labels_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Type.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Type);
      }
      size += labels_.CalculateSize(_map_labels_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MonitoredResource other) {
      if (other == null) {
        return;
      }
      if (other.Type.Length != 0) {
        Type = other.Type;
      }
      labels_.Add(other.labels_);
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
            Type = input.ReadString();
            break;
          }
          case 18: {
            labels_.AddEntriesFrom(input, _map_labels_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Auxiliary metadata for a [MonitoredResource][google.api.MonitoredResource] object.
  /// [MonitoredResource][google.api.MonitoredResource] objects contain the minimum set of information to
  /// uniquely identify a monitored resource instance. There is some other useful
  /// auxiliary metadata. Monitoring and Logging use an ingestion
  /// pipeline to extract metadata for cloud resources of all types, and store
  /// the metadata in this message.
  /// </summary>
  public sealed partial class MonitoredResourceMetadata : pb::IMessage<MonitoredResourceMetadata> {
    private static readonly pb::MessageParser<MonitoredResourceMetadata> _parser = new pb::MessageParser<MonitoredResourceMetadata>(() => new MonitoredResourceMetadata());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MonitoredResourceMetadata> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Api.MonitoredResourceReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MonitoredResourceMetadata() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MonitoredResourceMetadata(MonitoredResourceMetadata other) : this() {
      systemLabels_ = other.systemLabels_ != null ? other.systemLabels_.Clone() : null;
      userLabels_ = other.userLabels_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MonitoredResourceMetadata Clone() {
      return new MonitoredResourceMetadata(this);
    }

    /// <summary>Field number for the "system_labels" field.</summary>
    public const int SystemLabelsFieldNumber = 1;
    private global::Google.Protobuf.WellKnownTypes.Struct systemLabels_;
    /// <summary>
    /// Output only. Values for predefined system metadata labels.
    /// System labels are a kind of metadata extracted by Google, including
    /// "machine_image", "vpc", "subnet_id",
    /// "security_group", "name", etc.
    /// System label values can be only strings, Boolean values, or a list of
    /// strings. For example:
    ///
    ///     { "name": "my-test-instance",
    ///       "security_group": ["a", "b", "c"],
    ///       "spot_instance": false }
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Protobuf.WellKnownTypes.Struct SystemLabels {
      get { return systemLabels_; }
      set {
        systemLabels_ = value;
      }
    }

    /// <summary>Field number for the "user_labels" field.</summary>
    public const int UserLabelsFieldNumber = 2;
    private static readonly pbc::MapField<string, string>.Codec _map_userLabels_codec
        = new pbc::MapField<string, string>.Codec(pb::FieldCodec.ForString(10, ""), pb::FieldCodec.ForString(18, ""), 18);
    private readonly pbc::MapField<string, string> userLabels_ = new pbc::MapField<string, string>();
    /// <summary>
    /// Output only. A map of user-defined metadata labels.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::MapField<string, string> UserLabels {
      get { return userLabels_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MonitoredResourceMetadata);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MonitoredResourceMetadata other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(SystemLabels, other.SystemLabels)) return false;
      if (!UserLabels.Equals(other.UserLabels)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (systemLabels_ != null) hash ^= SystemLabels.GetHashCode();
      hash ^= UserLabels.GetHashCode();
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
      if (systemLabels_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(SystemLabels);
      }
      userLabels_.WriteTo(output, _map_userLabels_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (systemLabels_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(SystemLabels);
      }
      size += userLabels_.CalculateSize(_map_userLabels_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MonitoredResourceMetadata other) {
      if (other == null) {
        return;
      }
      if (other.systemLabels_ != null) {
        if (systemLabels_ == null) {
          SystemLabels = new global::Google.Protobuf.WellKnownTypes.Struct();
        }
        SystemLabels.MergeFrom(other.SystemLabels);
      }
      userLabels_.Add(other.userLabels_);
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
            if (systemLabels_ == null) {
              SystemLabels = new global::Google.Protobuf.WellKnownTypes.Struct();
            }
            input.ReadMessage(SystemLabels);
            break;
          }
          case 18: {
            userLabels_.AddEntriesFrom(input, _map_userLabels_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
