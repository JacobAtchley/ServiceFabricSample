using App.Core.Interfaces;
using App.Core.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<IActionResult> GetAsync()
        {
            var models = await _crudRepo
                .GetAllAsync().ConfigureAwait(false);

            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var model = await _crudRepo
                .GetByKeyAsync(ParseKey(id)).ConfigureAwait(false);

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TEntity entity)
        {
            var model = await _crudRepo
                .AddAsync(entity).ConfigureAwait(false);

            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] string id, [FromBody] TEntity entity)
        {
            var model = await _crudRepo
                .UpdateAsync(ParseKey(id), entity).ConfigureAwait(false);

            return Ok(model);
        }

        private static TKey ParseKey(string source)
        {
            var tKeyType = typeof(TKey);

            if (tKeyType == typeof(long))
            {
                if (long.TryParse(source, out var l))
                {
                    return (TKey)Convert.ChangeType(l, typeof(long));
                }
            }
            else if (tKeyType == typeof(int))
            {
                if (int.TryParse(source, out var i))
                {
                    return (TKey)Convert.ChangeType(i, typeof(int));
                }

            }
            else if (tKeyType == typeof(Guid))
            {
                if (Guid.TryParse(source, out var g))
                {
                    return (TKey)Convert.ChangeType(g, typeof(long));
                }

            }
            else if (tKeyType == typeof(string))
            {
                return (TKey)Convert.ChangeType(source, typeof(string));
            }

            throw new Exception("Could not parse key");
        }
    }
}
