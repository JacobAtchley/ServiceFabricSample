using App.Core.Interfaces;
using Grains.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Threading;
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
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var model = await UseGrain(Guid.Empty, x => x.GetAllAsync(cancellationToken));

            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            var entityKey = id.ParseKey<TKey>();

            var model = await UseGrain(Guid.Empty, x => x.GetByKeyAsync(entityKey, cancellationToken));

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TEntity entity, CancellationToken cancellationToken)
        {
            var model = await UseGrain(Guid.Empty, x => x.AddAsync(entity, cancellationToken));

            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] string id, [FromBody] TEntity entity, CancellationToken cancellationToken)
        {
            var model = await UseGrain(Guid.Empty, x => x.UpdateAsync(id.ParseKey<TKey>(), entity, cancellationToken));

            return Ok(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            await UseGrain(Guid.Empty, x => x.DeleteAsync(id.ParseKey<TKey>(), cancellationToken));

            return Ok("Deleted");
        }
    }
}
