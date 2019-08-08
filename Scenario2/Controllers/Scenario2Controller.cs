using DotNetCoreLoggingDemoAPI.Scenario2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreLoggingDemoAPI.Scenario2.Controllers
{

    /// <summary>
    /// A basic in-memory CRUD controller.
    /// </summary>
    /// <remarks>
    /// The implementation is still not perfect!.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class Scenario2Controller : ControllerBase
    {

        /// <summary>
        /// Our in memory data collection (for the sake of simpleness).
        /// </summary>
        private static IDictionary<int, Scenario2Model> _data = new Dictionary<int, Scenario2Model>();

        /// <summary>
        /// A helper giving us the next id to use.
        /// </summary>
        private static int NextId { get { return _data.Any() ? _data.Max(kvp => kvp.Key) : 1; } }

        /// <summary>
        /// Gets the list of all models.
        /// </summary>
        /// <response code="200">OK: returned when the operation was success and retuns the list of all models.</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Scenario2Model[]))]
        public IActionResult GetModelList()
        {

            var list = _data.Select(kvp => kvp.Value).ToList();

            return Ok(list);
            
        }

        /// <summary>
        /// Gets the model with the given id.
        /// </summary>
        /// <param name="id">The id of the model to be returned.</param>
        /// <response code="200">OK: returned when the operation was success and returns the model with the given id.</response>
        /// <response code="404">Not Found: returned when no model with the given id exists.</response>
        [HttpGet("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Scenario2Model))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetModel(int id)
        {

            if (!_data.ContainsKey(id)) return NotFound();

            return Ok(_data[id]);

        }

        /// <summary>
        /// Creates a new model.
        /// </summary>
        /// <param name="request">The request holding the values to be created.</param>
        /// <response code="201">Created: returned when the operation was success and returns the created model.</response>
        /// <response code="400">Bad Request: returned when the client sent invalid data.</response>
        /// <response code="404">Not Found: returned when no model with the given id exists.</response>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Scenario2Model))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PostModel([FromBody] Scenario2RequestModel request)
        {

            // for testing exceptions
            if (request.Value < 0) throw new Exception("the test exception because item.Value < 0");

            var item = new Scenario2Model
            {
                Id = NextId,
                Name = request.Name,
                Date = request.Date,
                Value = request.Value
            };

            _data.Add(item.Id, item);

            return CreatedAtAction(nameof(GetModel), new { Id = item.Id }, item);

        }

        /// <summary>
        /// Updates the model with the given id setting it to the values from the request.
        /// </summary>
        /// <param name="id">The id of the model to be updated.</param>
        /// <param name="request">The request holding the new values.</param>
        /// <response code="200">OK: returned when the operation was success and returns the model with the updated values.</response>
        /// <response code="400">Bad Request: returned when the client sent invalid data.</response>
        /// <response code="404">Not Found: returned when no model with the given id exists.</response>
        [HttpPut("{id:int}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Scenario2Model))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PutModel(int id, [FromBody] Scenario2RequestModel request)
        {

            if (!_data.ContainsKey(id)) return NotFound();

            var item = _data[id];
            item.Name = request.Name;
            item.Date = request.Date;
            item.Value = request.Value;

            return Ok(item);
           
        }

        /// <summary>
        /// Deletes the model with the given id.
        /// </summary>
        /// <param name="id">The id of the model to be deleted.</param>
        /// <response code="204">No Content: returned when the operation was success.</response>
        /// <response code="404">Not Found: returned when no model with the given id exists.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteModel(int id)
        {

            if (!_data.ContainsKey(id)) return NotFound();

            _data.Remove(id);

            return NoContent();

        }

    }

}
