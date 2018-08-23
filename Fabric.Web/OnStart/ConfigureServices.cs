using App.Core.Interfaces.Data;
using App.Core.Models;
using App.Data;
using App.Data.Implementations;
using App.Data.Interfaces;
using Fabric.Web.Observers;
using Grains.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oreleans.Observers;
using Orleans.Client;
using System;

namespace Fabric.Web.OnStart
{
    public static class ConfigureServices
    {
        public static IServiceCollection ConfigureMyAppServices(this IServiceCollection source, IConfiguration configuration)
        {
            source.AddSingleton(configuration);
            source.AddSingleton<IHelloSubscriber, HelloSubscriber>();
            source.AddScoped<IHelloObserver, HelloObserver>();
            source.AddScoped<IAppDbContext, AppDbContext>();
            source.AddScoped<IAppContextSettings, AppContextSettings>();
            source.AddScoped<ICrudRepo<Guid, Person>, PeopleRepository>();

            source.AddScoped(provider => OrleansClientFactory.Get(
                "fabric:/ServiceFabricSample/MyStatelessService",
                "UseDevelopmentStorage=true"));

            return source;
        }
    }
}
