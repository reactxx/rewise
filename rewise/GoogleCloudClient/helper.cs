using System;

public static class GoogleCloudHelper {
  // proto: https://github.com/googleapis/googleapis/tree/master/google
  // c# code: https://github.com/googleapis/google-api-dotnet-client/tree/master/Src
  public static Grpc.Core.CallInvoker create(string url) {
    if (!initialized) {
      initialized = true;
      var path = System.Configuration.ConfigurationManager.AppSettings["google-cloud-service-account"];
      Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
    }
    var ev = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
    var token = Google.Apis.Auth.OAuth2.GoogleCredential.GetApplicationDefault();
    var channelCredential = Grpc.Auth.GoogleGrpcCredentials.ToChannelCredentials(token);
    var channel = new Grpc.Core.Channel(url, channelCredential);
    return new Grpc.Core.DefaultCallInvoker(channel);
  }
  static bool initialized = false;
}
