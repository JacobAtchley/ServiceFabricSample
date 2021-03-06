﻿using App.Core.Interfaces;
using Fabric.Web.Hubs;
using Grains.Interfaces.Observers;
using Microsoft.AspNetCore.SignalR;

namespace Fabric.Web.Observers.Abstractions
{
    /// <summary>
    /// Provides a base level implementation for sending entity updates to Signal R via
    /// <see cref="IHubContext{THub}"/>.
    /// </summary>
    /// <typeparam name="TKey">The Entity's Key</typeparam>
    /// <typeparam name="TEntity">The Entity's Type</typeparam>
    public class AbstractEntityObserver<TKey, TEntity>
        : IEntityModifiedObserver<TKey, TEntity>
        where TEntity : class, IEntity<TKey>, new()
    {
        private readonly string _entityName;
        private readonly IHubContext<OrleansHub> _hub;

        public AbstractEntityObserver(string entityName, IHubContext<OrleansHub> hub)
        {
            _entityName = entityName;
            _hub = hub;
        }

        /// <inheritdoc />
        public void Modified(string reason, TEntity entity)
        {
            _hub.Clients.All.SendAsync($"EntityModified;{_entityName}",
                new
                {
                    Reason = reason,
                    Entity = entity
                });
        }
    }
}
