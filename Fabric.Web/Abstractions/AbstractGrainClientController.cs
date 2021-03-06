﻿using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Fabric.Web.Abstractions
{
    /// <summary>
    /// This is an controller that provides scaffolding for
    ///  interacting wtiha an orleans grain via a cluster client
    /// </summary>
    /// <typeparam name="TGrain">The type of grain to interact with</typeparam>
    public class AbstractGrainClientController<TGrain> : Controller
        where TGrain : IGrainWithGuidKey
    {
        private readonly IClusterClient _clusterClient;

        public AbstractGrainClientController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        protected async Task<T> UseGrain<T>(Guid grainKey, Func<TGrain, Task<T>> func)
        {
            await _clusterClient.Connect();

            var grain = _clusterClient.GetGrain<TGrain>(grainKey);

            return await func(grain);
        }

        protected async Task UseGrain(Guid grainKey, Func<TGrain, Task> func)
        {
            await _clusterClient.Connect();

            var grain = _clusterClient.GetGrain<TGrain>(grainKey);

            await func(grain);
        }
    }
}
