// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: rewise/main_dart.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace RewiseDom {
  public static partial class DartMain
  {
    static readonly string __ServiceName = "rewiseDom.DartMain";

    static readonly grpc::Marshaller<global::RewiseDom.HelloRequest> __Marshaller_rewiseDom_HelloRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RewiseDom.HelloRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::RewiseDom.HelloReply> __Marshaller_rewiseDom_HelloReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RewiseDom.HelloReply.Parser.ParseFrom);

    static readonly grpc::Method<global::RewiseDom.HelloRequest, global::RewiseDom.HelloReply> __Method_SayHello = new grpc::Method<global::RewiseDom.HelloRequest, global::RewiseDom.HelloReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SayHello",
        __Marshaller_rewiseDom_HelloRequest,
        __Marshaller_rewiseDom_HelloReply);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::RewiseDom.MainDartReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of DartMain</summary>
    public abstract partial class DartMainBase
    {
      public virtual global::System.Threading.Tasks.Task<global::RewiseDom.HelloReply> SayHello(global::RewiseDom.HelloRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for DartMain</summary>
    public partial class DartMainClient : grpc::ClientBase<DartMainClient>
    {
      /// <summary>Creates a new client for DartMain</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public DartMainClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for DartMain that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public DartMainClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected DartMainClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected DartMainClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::RewiseDom.HelloReply SayHello(global::RewiseDom.HelloRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SayHello(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RewiseDom.HelloReply SayHello(global::RewiseDom.HelloRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_SayHello, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RewiseDom.HelloReply> SayHelloAsync(global::RewiseDom.HelloRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SayHelloAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RewiseDom.HelloReply> SayHelloAsync(global::RewiseDom.HelloRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_SayHello, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override DartMainClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new DartMainClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(DartMainBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_SayHello, serviceImpl.SayHello).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, DartMainBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_SayHello, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::RewiseDom.HelloRequest, global::RewiseDom.HelloReply>(serviceImpl.SayHello));
    }

  }
}
#endregion
