using Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddSingleton<ICallFunction, CallFunction>();
        services.AddSingleton<ICreateResponse, CreateResponse>();
    })
    .Build();

host.Run();
