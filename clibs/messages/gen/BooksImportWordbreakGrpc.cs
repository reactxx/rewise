// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: rewise/books_import_wordbreak.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace RewiseDom {
  public static partial class WordBreak
  {
    static readonly string __ServiceName = "rewiseDom.WordBreak";

    static readonly grpc::Marshaller<global::RewiseDom.WordBreakRequest> __Marshaller_rewiseDom_WordBreakRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RewiseDom.WordBreakRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::RewiseDom.BytesList> __Marshaller_rewiseDom_BytesList = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RewiseDom.BytesList.Parser.ParseFrom);

    static readonly grpc::Method<global::RewiseDom.WordBreakRequest, global::RewiseDom.BytesList> __Method_CallWordBreaks = new grpc::Method<global::RewiseDom.WordBreakRequest, global::RewiseDom.BytesList>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CallWordBreaks",
        __Marshaller_rewiseDom_WordBreakRequest,
        __Marshaller_rewiseDom_BytesList);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::RewiseDom.BooksImportWordbreakReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of WordBreak</summary>
    public abstract partial class WordBreakBase
    {
      public virtual global::System.Threading.Tasks.Task<global::RewiseDom.BytesList> CallWordBreaks(global::RewiseDom.WordBreakRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for WordBreak</summary>
    public partial class WordBreakClient : grpc::ClientBase<WordBreakClient>
    {
      /// <summary>Creates a new client for WordBreak</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public WordBreakClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for WordBreak that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public WordBreakClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected WordBreakClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected WordBreakClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::RewiseDom.BytesList CallWordBreaks(global::RewiseDom.WordBreakRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CallWordBreaks(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RewiseDom.BytesList CallWordBreaks(global::RewiseDom.WordBreakRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CallWordBreaks, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RewiseDom.BytesList> CallWordBreaksAsync(global::RewiseDom.WordBreakRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CallWordBreaksAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RewiseDom.BytesList> CallWordBreaksAsync(global::RewiseDom.WordBreakRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CallWordBreaks, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override WordBreakClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new WordBreakClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(WordBreakBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_CallWordBreaks, serviceImpl.CallWordBreaks).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, WordBreakBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_CallWordBreaks, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::RewiseDom.WordBreakRequest, global::RewiseDom.BytesList>(serviceImpl.CallWordBreaks));
    }

  }
}
#endregion
