using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var app1 = builder.AddProject("app1", "../app1/BasicOpenTelemetry-1.csproj");
    //.WithHttpEndpoint(5000,5000);

var app2 = builder.AddContainer("app2", "aspirePOC/app2")
                  .WithHttpEndpoint(5001, 8080)
                  .WithOtlpExporter();

builder.Build().Run();
