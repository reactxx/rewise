// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: rewise/word_breaking/word_breaking_service.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Rw.WordBreaking {
  public static partial class CSharpService
  {
    static readonly string __ServiceName = "rw.word_breaking.CSharpService";

    static readonly grpc::Marshaller<global::Rw.WordBreaking.Request2> __Marshaller_rw_word_breaking_Request2 = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Rw.WordBreaking.Request2.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Rw.WordBreaking.Response2> __Marshaller_rw_word_breaking_Response2 = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Rw.WordBreaking.Response2.Parser.ParseFrom);

    static readonly grpc::Method<global::Rw.WordBreaking.Request2, global::Rw.WordBreaking.Response2> __Method_Run2 = new grpc::Method<global::Rw.WordBreaking.Request2, global::Rw.WordBreaking.Response2>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Run2",
        __Marshaller_rw_word_breaking_Request2,
        __Marshaller_rw_word_breaking_Response2);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Rw.WordBreaking.WordBreakingServiceReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of CSharpService</summary>
    [grpc::BindServiceMethod(typeof(CSharpService), "BindService")]
    public abstract partial class CSharpServiceBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Rw.WordBreaking.Response2> Run2(global::Rw.WordBreaking.Request2 request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for CSharpService</summary>
    public partial class CSharpServiceClient : grpc::ClientBase<CSharpServiceClient>
    {
      /// <summary>Creates a new client for CSharpService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public CSharpServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for CSharpService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public CSharpServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected CSharpServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected CSharpServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Rw.WordBreaking.Response2 Run2(global::Rw.WordBreaking.Request2 request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Run2(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Rw.WordBreaking.Response2 Run2(global::Rw.WordBreaking.Request2 request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Run2, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Rw.WordBreaking.Response2> Run2Async(global::Rw.WordBreaking.Request2 request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Run2Async(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Rw.WordBreaking.Response2> Run2Async(global::Rw.WordBreaking.Request2 request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Run2, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override CSharpServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new CSharpServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(CSharpServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Run2, serviceImpl.Run2).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, CSharpServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_Run2, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Rw.WordBreaking.Request2, global::Rw.WordBreaking.Response2>(serviceImpl.Run2));
    }

  }
}
#endregion
