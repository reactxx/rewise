// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: google/api/documentation.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Google.Api {

  /// <summary>Holder for reflection information generated from google/api/documentation.proto</summary>
  public static partial class DocumentationReflection {

    #region Descriptor
    /// <summary>File descriptor for google/api/documentation.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static DocumentationReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch5nb29nbGUvYXBpL2RvY3VtZW50YXRpb24ucHJvdG8SCmdvb2dsZS5hcGki",
            "oQEKDURvY3VtZW50YXRpb24SDwoHc3VtbWFyeRgBIAEoCRIfCgVwYWdlcxgF",
            "IAMoCzIQLmdvb2dsZS5hcGkuUGFnZRIsCgVydWxlcxgDIAMoCzIdLmdvb2ds",
            "ZS5hcGkuRG9jdW1lbnRhdGlvblJ1bGUSHgoWZG9jdW1lbnRhdGlvbl9yb290",
            "X3VybBgEIAEoCRIQCghvdmVydmlldxgCIAEoCSJbChFEb2N1bWVudGF0aW9u",
            "UnVsZRIQCghzZWxlY3RvchgBIAEoCRITCgtkZXNjcmlwdGlvbhgCIAEoCRIf",
            "ChdkZXByZWNhdGlvbl9kZXNjcmlwdGlvbhgDIAEoCSJJCgRQYWdlEgwKBG5h",
            "bWUYASABKAkSDwoHY29udGVudBgCIAEoCRIiCghzdWJwYWdlcxgDIAMoCzIQ",
            "Lmdvb2dsZS5hcGkuUGFnZUJ0Cg5jb20uZ29vZ2xlLmFwaUISRG9jdW1lbnRh",
            "dGlvblByb3RvUAFaRWdvb2dsZS5nb2xhbmcub3JnL2dlbnByb3RvL2dvb2ds",
            "ZWFwaXMvYXBpL3NlcnZpY2Vjb25maWc7c2VydmljZWNvbmZpZ6ICBEdBUEli",
            "BnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Api.Documentation), global::Google.Api.Documentation.Parser, new[]{ "Summary", "Pages", "Rules", "DocumentationRootUrl", "Overview" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Api.DocumentationRule), global::Google.Api.DocumentationRule.Parser, new[]{ "Selector", "Description", "DeprecationDescription" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Api.Page), global::Google.Api.Page.Parser, new[]{ "Name", "Content", "Subpages" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// `Documentation` provides the information for describing a service.
  ///
  /// Example:
  /// &lt;pre>&lt;code>documentation:
  ///   summary: >
  ///     The Google Calendar API gives access
  ///     to most calendar features.
  ///   pages:
  ///   - name: Overview
  ///     content: &amp;#40;== include google/foo/overview.md ==&amp;#41;
  ///   - name: Tutorial
  ///     content: &amp;#40;== include google/foo/tutorial.md ==&amp;#41;
  ///     subpages;
  ///     - name: Java
  ///       content: &amp;#40;== include google/foo/tutorial_java.md ==&amp;#41;
  ///   rules:
  ///   - selector: google.calendar.Calendar.Get
  ///     description: >
  ///       ...
  ///   - selector: google.calendar.Calendar.Put
  ///     description: >
  ///       ...
  /// &lt;/code>&lt;/pre>
  /// Documentation is provided in markdown syntax. In addition to
  /// standard markdown features, definition lists, tables and fenced
  /// code blocks are supported. Section headers can be provided and are
  /// interpreted relative to the section nesting of the context where
  /// a documentation fragment is embedded.
  ///
  /// Documentation from the IDL is merged with documentation defined
  /// via the config at normalization time, where documentation provided
  /// by config rules overrides IDL provided.
  ///
  /// A number of constructs specific to the API platform are supported
  /// in documentation text.
  ///
  /// In order to reference a proto element, the following
  /// notation can be used:
  /// &lt;pre>&lt;code>&amp;#91;fully.qualified.proto.name]&amp;#91;]&lt;/code>&lt;/pre>
  /// To override the display text used for the link, this can be used:
  /// &lt;pre>&lt;code>&amp;#91;display text]&amp;#91;fully.qualified.proto.name]&lt;/code>&lt;/pre>
  /// Text can be excluded from doc using the following notation:
  /// &lt;pre>&lt;code>&amp;#40;-- internal comment --&amp;#41;&lt;/code>&lt;/pre>
  ///
  /// A few directives are available in documentation. Note that
  /// directives must appear on a single line to be properly
  /// identified. The `include` directive includes a markdown file from
  /// an external source:
  /// &lt;pre>&lt;code>&amp;#40;== include path/to/file ==&amp;#41;&lt;/code>&lt;/pre>
  /// The `resource_for` directive marks a message to be the resource of
  /// a collection in REST view. If it is not specified, tools attempt
  /// to infer the resource from the operations in a collection:
  /// &lt;pre>&lt;code>&amp;#40;== resource_for v1.shelves.books ==&amp;#41;&lt;/code>&lt;/pre>
  /// The directive `suppress_warning` does not directly affect documentation
  /// and is documented together with service config validation.
  /// </summary>
  public sealed partial class Documentation : pb::IMessage<Documentation> {
    private static readonly pb::MessageParser<Documentation> _parser = new pb::MessageParser<Documentation>(() => new Documentation());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Documentation> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Api.DocumentationReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Documentation() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Documentation(Documentation other) : this() {
      summary_ = other.summary_;
      pages_ = other.pages_.Clone();
      rules_ = other.rules_.Clone();
      documentationRootUrl_ = other.documentationRootUrl_;
      overview_ = other.overview_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Documentation Clone() {
      return new Documentation(this);
    }

    /// <summary>Field number for the "summary" field.</summary>
    public const int SummaryFieldNumber = 1;
    private string summary_ = "";
    /// <summary>
    /// A short summary of what the service does. Can only be provided by
    /// plain text.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Summary {
      get { return summary_; }
      set {
        summary_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "pages" field.</summary>
    public const int PagesFieldNumber = 5;
    private static readonly pb::FieldCodec<global::Google.Api.Page> _repeated_pages_codec
        = pb::FieldCodec.ForMessage(42, global::Google.Api.Page.Parser);
    private readonly pbc::RepeatedField<global::Google.Api.Page> pages_ = new pbc::RepeatedField<global::Google.Api.Page>();
    /// <summary>
    /// The top level pages for the documentation set.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Google.Api.Page> Pages {
      get { return pages_; }
    }

    /// <summary>Field number for the "rules" field.</summary>
    public const int RulesFieldNumber = 3;
    private static readonly pb::FieldCodec<global::Google.Api.DocumentationRule> _repeated_rules_codec
        = pb::FieldCodec.ForMessage(26, global::Google.Api.DocumentationRule.Parser);
    private readonly pbc::RepeatedField<global::Google.Api.DocumentationRule> rules_ = new pbc::RepeatedField<global::Google.Api.DocumentationRule>();
    /// <summary>
    /// A list of documentation rules that apply to individual API elements.
    ///
    /// **NOTE:** All service configuration rules follow "last one wins" order.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Google.Api.DocumentationRule> Rules {
      get { return rules_; }
    }

    /// <summary>Field number for the "documentation_root_url" field.</summary>
    public const int DocumentationRootUrlFieldNumber = 4;
    private string documentationRootUrl_ = "";
    /// <summary>
    /// The URL to the root of documentation.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string DocumentationRootUrl {
      get { return documentationRootUrl_; }
      set {
        documentationRootUrl_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "overview" field.</summary>
    public const int OverviewFieldNumber = 2;
    private string overview_ = "";
    /// <summary>
    /// Declares a single overview page. For example:
    /// &lt;pre>&lt;code>documentation:
    ///   summary: ...
    ///   overview: &amp;#40;== include overview.md ==&amp;#41;
    /// &lt;/code>&lt;/pre>
    /// This is a shortcut for the following declaration (using pages style):
    /// &lt;pre>&lt;code>documentation:
    ///   summary: ...
    ///   pages:
    ///   - name: Overview
    ///     content: &amp;#40;== include overview.md ==&amp;#41;
    /// &lt;/code>&lt;/pre>
    /// Note: you cannot specify both `overview` field and `pages` field.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Overview {
      get { return overview_; }
      set {
        overview_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Documentation);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Documentation other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Summary != other.Summary) return false;
      if(!pages_.Equals(other.pages_)) return false;
      if(!rules_.Equals(other.rules_)) return false;
      if (DocumentationRootUrl != other.DocumentationRootUrl) return false;
      if (Overview != other.Overview) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Summary.Length != 0) hash ^= Summary.GetHashCode();
      hash ^= pages_.GetHashCode();
      hash ^= rules_.GetHashCode();
      if (DocumentationRootUrl.Length != 0) hash ^= DocumentationRootUrl.GetHashCode();
      if (Overview.Length != 0) hash ^= Overview.GetHashCode();
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
      if (Summary.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Summary);
      }
      if (Overview.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Overview);
      }
      rules_.WriteTo(output, _repeated_rules_codec);
      if (DocumentationRootUrl.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(DocumentationRootUrl);
      }
      pages_.WriteTo(output, _repeated_pages_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Summary.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Summary);
      }
      size += pages_.CalculateSize(_repeated_pages_codec);
      size += rules_.CalculateSize(_repeated_rules_codec);
      if (DocumentationRootUrl.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(DocumentationRootUrl);
      }
      if (Overview.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Overview);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Documentation other) {
      if (other == null) {
        return;
      }
      if (other.Summary.Length != 0) {
        Summary = other.Summary;
      }
      pages_.Add(other.pages_);
      rules_.Add(other.rules_);
      if (other.DocumentationRootUrl.Length != 0) {
        DocumentationRootUrl = other.DocumentationRootUrl;
      }
      if (other.Overview.Length != 0) {
        Overview = other.Overview;
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
            Summary = input.ReadString();
            break;
          }
          case 18: {
            Overview = input.ReadString();
            break;
          }
          case 26: {
            rules_.AddEntriesFrom(input, _repeated_rules_codec);
            break;
          }
          case 34: {
            DocumentationRootUrl = input.ReadString();
            break;
          }
          case 42: {
            pages_.AddEntriesFrom(input, _repeated_pages_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// A documentation rule provides information about individual API elements.
  /// </summary>
  public sealed partial class DocumentationRule : pb::IMessage<DocumentationRule> {
    private static readonly pb::MessageParser<DocumentationRule> _parser = new pb::MessageParser<DocumentationRule>(() => new DocumentationRule());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DocumentationRule> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Api.DocumentationReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DocumentationRule() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DocumentationRule(DocumentationRule other) : this() {
      selector_ = other.selector_;
      description_ = other.description_;
      deprecationDescription_ = other.deprecationDescription_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DocumentationRule Clone() {
      return new DocumentationRule(this);
    }

    /// <summary>Field number for the "selector" field.</summary>
    public const int SelectorFieldNumber = 1;
    private string selector_ = "";
    /// <summary>
    /// The selector is a comma-separated list of patterns. Each pattern is a
    /// qualified name of the element which may end in "*", indicating a wildcard.
    /// Wildcards are only allowed at the end and for a whole component of the
    /// qualified name, i.e. "foo.*" is ok, but not "foo.b*" or "foo.*.bar". A
    /// wildcard will match one or more components. To specify a default for all
    /// applicable elements, the whole pattern "*" is used.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Selector {
      get { return selector_; }
      set {
        selector_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "description" field.</summary>
    public const int DescriptionFieldNumber = 2;
    private string description_ = "";
    /// <summary>
    /// Description of the selected API(s).
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Description {
      get { return description_; }
      set {
        description_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "deprecation_description" field.</summary>
    public const int DeprecationDescriptionFieldNumber = 3;
    private string deprecationDescription_ = "";
    /// <summary>
    /// Deprecation description of the selected element(s). It can be provided if
    /// an element is marked as `deprecated`.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string DeprecationDescription {
      get { return deprecationDescription_; }
      set {
        deprecationDescription_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DocumentationRule);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DocumentationRule other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Selector != other.Selector) return false;
      if (Description != other.Description) return false;
      if (DeprecationDescription != other.DeprecationDescription) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Selector.Length != 0) hash ^= Selector.GetHashCode();
      if (Description.Length != 0) hash ^= Description.GetHashCode();
      if (DeprecationDescription.Length != 0) hash ^= DeprecationDescription.GetHashCode();
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
      if (Selector.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Selector);
      }
      if (Description.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Description);
      }
      if (DeprecationDescription.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(DeprecationDescription);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Selector.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Selector);
      }
      if (Description.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Description);
      }
      if (DeprecationDescription.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(DeprecationDescription);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DocumentationRule other) {
      if (other == null) {
        return;
      }
      if (other.Selector.Length != 0) {
        Selector = other.Selector;
      }
      if (other.Description.Length != 0) {
        Description = other.Description;
      }
      if (other.DeprecationDescription.Length != 0) {
        DeprecationDescription = other.DeprecationDescription;
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
            Selector = input.ReadString();
            break;
          }
          case 18: {
            Description = input.ReadString();
            break;
          }
          case 26: {
            DeprecationDescription = input.ReadString();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Represents a documentation page. A page can contain subpages to represent
  /// nested documentation set structure.
  /// </summary>
  public sealed partial class Page : pb::IMessage<Page> {
    private static readonly pb::MessageParser<Page> _parser = new pb::MessageParser<Page>(() => new Page());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Page> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Api.DocumentationReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Page() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Page(Page other) : this() {
      name_ = other.name_;
      content_ = other.content_;
      subpages_ = other.subpages_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Page Clone() {
      return new Page(this);
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 1;
    private string name_ = "";
    /// <summary>
    /// The name of the page. It will be used as an identity of the page to
    /// generate URI of the page, text of the link to this page in navigation,
    /// etc. The full page name (start from the root page name to this page
    /// concatenated with `.`) can be used as reference to the page in your
    /// documentation. For example:
    /// &lt;pre>&lt;code>pages:
    /// - name: Tutorial
    ///   content: &amp;#40;== include tutorial.md ==&amp;#41;
    ///   subpages:
    ///   - name: Java
    ///     content: &amp;#40;== include tutorial_java.md ==&amp;#41;
    /// &lt;/code>&lt;/pre>
    /// You can reference `Java` page using Markdown reference link syntax:
    /// `[Java][Tutorial.Java]`.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "content" field.</summary>
    public const int ContentFieldNumber = 2;
    private string content_ = "";
    /// <summary>
    /// The Markdown content of the page. You can use &lt;code>&amp;#40;== include {path}
    /// ==&amp;#41;&lt;/code> to include content from a Markdown file.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Content {
      get { return content_; }
      set {
        content_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "subpages" field.</summary>
    public const int SubpagesFieldNumber = 3;
    private static readonly pb::FieldCodec<global::Google.Api.Page> _repeated_subpages_codec
        = pb::FieldCodec.ForMessage(26, global::Google.Api.Page.Parser);
    private readonly pbc::RepeatedField<global::Google.Api.Page> subpages_ = new pbc::RepeatedField<global::Google.Api.Page>();
    /// <summary>
    /// Subpages of this page. The order of subpages specified here will be
    /// honored in the generated docset.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Google.Api.Page> Subpages {
      get { return subpages_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Page);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Page other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Name != other.Name) return false;
      if (Content != other.Content) return false;
      if(!subpages_.Equals(other.subpages_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Content.Length != 0) hash ^= Content.GetHashCode();
      hash ^= subpages_.GetHashCode();
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
      if (Content.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Content);
      }
      subpages_.WriteTo(output, _repeated_subpages_codec);
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
      if (Content.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Content);
      }
      size += subpages_.CalculateSize(_repeated_subpages_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Page other) {
      if (other == null) {
        return;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.Content.Length != 0) {
        Content = other.Content;
      }
      subpages_.Add(other.subpages_);
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
          case 18: {
            Content = input.ReadString();
            break;
          }
          case 26: {
            subpages_.AddEntriesFrom(input, _repeated_subpages_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
