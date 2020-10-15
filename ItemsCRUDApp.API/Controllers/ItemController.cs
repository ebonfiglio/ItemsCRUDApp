using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItemsCRUDApp.Domain.Services;
using ItemsCRUDApp.Shared.Requests;
using ItemsCRUDApp.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ItemsCRUDApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }
        // GET api/<ItemController>/5
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var item = await _itemService.GetAsync(id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/<ItemController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await _itemService.GetAllAsync();
                return Ok(items);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<ItemController>
        [HttpPost]
        public async Task<IActionResult> Post(ItemRequest request)
        {
            try
            {
                var item = await _itemService.AddAsync(request);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<ItemController>/
        [HttpPut]
        public async Task<IActionResult> Put(ItemRequest request)
        {
            try
            {
               var item = await _itemService.UpdateAsync(request);
               return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<ItemController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _itemService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
