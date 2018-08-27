using App.Core.Interfaces;
using App.Core.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Fabric.Web.Abstractions
{
    public abstract class AbstractCrudController<TKey, TEntity> : Controller
        where TEntity : class, IEntity<TKey>, new()
    {
        private readonly ICrudRepo<TKey, TEntity> _crudRepo;

        protected AbstractCrudController(ICrudRepo<TKey, TEntity> crudRepo)
        {
            _crudRepo = crudRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var models = await _crudRepo
                .GetAllAsync(cancellationToken).ConfigureAwait(false);

            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            var model = await _crudRepo
                .GetByKeyAsync(id.ParseKey<TKey>(), cancellationToken).ConfigureAwait(false);

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TEntity entity, CancellationToken cancellationToken)
        {
            var model = await _crudRepo
                .AddAsync(entity, cancellationToken).ConfigureAwait(false);

            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] string id, [FromBody] TEntity entity, CancellationToken cancellationToken)
        {
            var model = await _crudRepo
                .UpdateAsync(id.ParseKey<TKey>(), entity, cancellationToken).ConfigureAwait(false);

            return Ok(model);
        }
    }
}
