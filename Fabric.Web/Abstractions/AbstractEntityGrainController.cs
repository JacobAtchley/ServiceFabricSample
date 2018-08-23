using App.Core.Interfaces;
using Grains.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Fabric.Web.Abstractions
{
    public class AbstractEntityGrainController<TKey, TEntity>
        : AbstractGrainClientController<IEntityGrain<TKey, TEntity>>
        where TEntity : class, IEntity<TKey>, new()
    {
        /// <inheritdoc />
        public AbstractEntityGrainController(IClusterClient clusterClient)
            : base(clusterClient)
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var model = await UseGrain(Guid.Empty, x => x.GetAllAsync());

            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var entityKey = id.ParseKey<TKey>();

            var model = await UseGrain(Guid.Empty, x => x.GetByKeyAsync(entityKey));

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TEntity entity)
        {
            var model = await UseGrain(Guid.Empty, x => x.AddAsync(entity));

            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] string id, [FromBody] TEntity entity)
        {
            var model = await UseGrain(Guid.Empty, x => x.UpdateAsync(id.ParseKey<TKey>(), entity));

            return Ok(model);
        }
    }
}
