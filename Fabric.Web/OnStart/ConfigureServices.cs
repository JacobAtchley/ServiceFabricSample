using App.Core.Interfaces.Data;
using App.Core.Models;
using App.Data;
using App.Data.Implementations;
using App.Data.Interfaces;
using Fabric.Web.Observers;
using Grains.Interfaces.Observers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oreleans.Observers.Implementations;
using Oreleans.Observers.Interfaces;
using System;

namespace Fabric.Web.OnStart
{
    public static class ConfigureServices
    {
        public static IServiceCollection ConfigureMyAppServices(this IServiceCollection source, IConfiguration configuration)
        {
            source.AddSingleton(configuration);

            //orleans subscribers
            source.AddSingleton<IHelloSubscriber, HelloSubscriber>();
            source.AddSingleton<IEntityModifiedSubscriber, PeopleEntityModifedSubscriber>();

            //orleans observers
            source.AddScoped<IHelloObserver, HelloObserver>();
            source.AddScoped<IEntityModifiedObserver<Guid, Person>, PeopleObserver>();

            //data access
            source.AddScoped<IAppDbContext, AppDbContext>();
            source.AddScoped<ICrudRepo<Guid, Person>, PeopleRepository>();

            return source;
        }
    }
}
