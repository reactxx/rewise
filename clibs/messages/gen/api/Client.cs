// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: google/api/client.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Google.Api {

  /// <summary>Holder for reflection information generated from google/api/client.proto</summary>
  public static partial class ClientReflection {

    #region Descriptor
    /// <summary>File descriptor for google/api/client.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ClientReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Chdnb29nbGUvYXBpL2NsaWVudC5wcm90bxIKZ29vZ2xlLmFwaRogZ29vZ2xl",
            "L3Byb3RvYnVmL2Rlc2NyaXB0b3IucHJvdG86OQoQbWV0aG9kX3NpZ25hdHVy",
            "ZRIeLmdvb2dsZS5wcm90b2J1Zi5NZXRob2RPcHRpb25zGJsIIAMoCTo2Cgxk",
            "ZWZhdWx0X2hvc3QSHy5nb29nbGUucHJvdG9idWYuU2VydmljZU9wdGlvbnMY",
            "mQggASgJOjYKDG9hdXRoX3Njb3BlcxIfLmdvb2dsZS5wcm90b2J1Zi5TZXJ2",
            "aWNlT3B0aW9ucxiaCCABKAlCaQoOY29tLmdvb2dsZS5hcGlCC0NsaWVudFBy",
            "b3RvUAFaQWdvb2dsZS5nb2xhbmcub3JnL2dlbnByb3RvL2dvb2dsZWFwaXMv",
            "YXBpL2Fubm90YXRpb25zO2Fubm90YXRpb25zogIER0FQSWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.Reflection.DescriptorReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pb::Extension[] { ClientExtensions.MethodSignature, ClientExtensions.DefaultHost, ClientExtensions.OauthScopes }, null));
    }
    #endregion

  }
  /// <summary>Holder for extension identifiers generated from the top level of google/api/client.proto</summary>
  public static partial class ClientExtensions {
    /// <summary>
    /// A definition of a client library method signature.
    ///
    /// In client libraries, each proto RPC corresponds to one or more methods
    /// which the end user is able to call, and calls the underlying RPC.
    /// Normally, this method receives a single argument (a struct or instance
    /// corresponding to the RPC request object). Defining this field will
    /// add one or more overloads providing flattened or simpler method signatures
    /// in some languages.
    ///
    /// The fields on the method signature are provided as a comma-separated
    /// string.
    ///
    /// For example, the proto RPC and annotation:
    ///
    ///   rpc CreateSubscription(CreateSubscriptionRequest)
    ///       returns (Subscription) {
    ///     option (google.api.method_signature) = "name,topic";
    ///   }
    ///
    /// Would add the following Java overload (in addition to the method accepting
    /// the request object):
    ///
    ///   public final Subscription createSubscription(String name, String topic)
    ///
    /// The following backwards-compatibility guidelines apply:
    ///
    ///   * Adding this annotation to an unannotated method is backwards
    ///     compatible.
    ///   * Adding this annotation to a method which already has existing
    ///     method signature annotations is backwards compatible if and only if
    ///     the new method signature annotation is last in the sequence.
    ///   * Modifying or removing an existing method signature annotation is
    ///     a breaking change.
    ///   * Re-ordering existing method signature annotations is a breaking
    ///     change.
    /// </summary>
    public static readonly pb::RepeatedExtension<global::Google.Protobuf.Reflection.MethodOptions, string> MethodSignature =
      new pb::RepeatedExtension<global::Google.Protobuf.Reflection.MethodOptions, string>(1051, pb::FieldCodec.ForString(8410));
    /// <summary>
    /// The hostname for this service.
    /// This should be specified with no prefix or protocol.
    ///
    /// Example:
    ///
    ///   service Foo {
    ///     option (google.api.default_host) = "foo.googleapi.com";
    ///     ...
    ///   }
    /// </summary>
    public static readonly pb::Extension<global::Google.Protobuf.Reflection.ServiceOptions, string> DefaultHost =
      new pb::Extension<global::Google.Protobuf.Reflection.ServiceOptions, string>(1049, pb::FieldCodec.ForString(8394, ""));
    /// <summary>
    /// OAuth scopes needed for the client.
    ///
    /// Example:
    ///
    ///   service Foo {
    ///     option (google.api.oauth_scopes) = \
    ///       "https://www.googleapis.com/auth/cloud-platform";
    ///     ...
    ///   }
    ///
    /// If there is more than one scope, use a comma-separated string:
    ///
    /// Example:
    ///
    ///   service Foo {
    ///     option (google.api.oauth_scopes) = \
    ///       "https://www.googleapis.com/auth/cloud-platform,"
    ///       "https://www.googleapis.com/auth/monitoring";
    ///     ...
    ///   }
    /// </summary>
    public static readonly pb::Extension<global::Google.Protobuf.Reflection.ServiceOptions, string> OauthScopes =
      new pb::Extension<global::Google.Protobuf.Reflection.ServiceOptions, string>(1050, pb::FieldCodec.ForString(8402, ""));
  }

}

#endregion Designer generated code
