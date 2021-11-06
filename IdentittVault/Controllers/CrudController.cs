using IdentittVault.Entities;
using IdentittVault.Models;
using IdentittVault.Repositories;
using IdentittVault.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentittVault.Controllers
{
    [ApiController]
    public class CrudController<TModel, TEntity> : ControllerBase
        where TModel : Model
        where TEntity : Entity
    {
        protected readonly ICrudService<TEntity> service;

        /// <summary>
        /// Constructor of CRUD controller
        /// </summary>
        /// <param name="service">The non-null service</param>
        public CrudController(ICrudService<TEntity> service)
        {
            this.service = service;
        }

        protected virtual void OnCreate(TModel model)
        {

        }

        /// <summary>
        /// API for create an entity
        /// </summary>
        /// <param name="model">A non-null entity data</param>
        /// <returns>The created entity with it's id</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TModel model, [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            OnCreate(model);

            if (ModelState.IsValid)
            {
                //if the name already exists, throws an Conflict exception and return 409.
                TEntity entity = (TEntity)model.ToEntity();
                await service.CreateAsync(entity);
                model = (TModel)entity.ToModel();

                return base.CreatedAtAction(nameof(Read), new { id = model.Id }, model);
            }

            return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        /// <summary>
        /// API for get an entity by it's ID
        /// </summary>
        /// <param name="id">The id of the entity</param>
        /// <returns>A non-null entity</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TModel>> Read(Guid id)
        {
            //If not found, throws an exception NotFound and returns 404.
            TEntity entity = await service.ReadAsync(id);

            TModel model = (TModel)entity.ToModel(expand: true);

            return Ok(model);
        }

        /// <summary>
        /// API for get entity with or without name filter
        /// </summary>
        /// <param name="name">The name of the institution. Can be combined with operation field EQUAL or LIKE</param>
        /// <param name="op">The operation field. Can be EQUAL or LIKE</param>
        /// <returns>A non-null list of entities.</returns>
        [HttpGet(Order = 1)]
        public ActionResult<List<TModel>> Read([FromQuery] string name, [FromQuery] ReadOperation op = ReadOperation.Equal, [FromQuery] bool expand = false)
        {
            List<TEntity> entities;

            //Get all entities
            if (string.IsNullOrEmpty(name))
                entities = service.ReadAll();
            else
                entities = service.ReadByName(name, op);

            return Ok(entities
                .Select(x => (TModel)x.ToModel(expand))
                .ToList());
        }

        /// <summary>
        /// API for update the selected entity
        /// </summary>
        /// <param name="id">The id of the entity</param>
        /// <param name="model">A non-null entity data updated</param>
        /// <param name="apiBehaviorOptions">A non-null service to control validation model</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TModel model, [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            if (id != model.Id)
            {
                ModelState.AddModelError("Id", "The id field values is mismatch. Check the values at body and/or route.");
                return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }

            await service.UpdateAsync((TEntity)model.ToEntity());

            return Ok(model);
        }


        /// <summary>
        /// API for delete the entity
        /// </summary>
        /// <param name="id">The id of the entity</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            //If not found, throws an exception NotFound and returns 404.
            await service.DeleteAsync(id);

            return NoContent();
        }
    }
}
