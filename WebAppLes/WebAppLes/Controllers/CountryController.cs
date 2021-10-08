using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppLes.Models;

namespace WebAppLes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private ICountryRepository repo;

        public CountryController(ICountryRepository repo)
        {
            this.repo = repo;
        }

        //[HttpGet]       
        //[HttpHead]

        //public IEnumerable<Country> Get()
        //{
        //    return repo.GetAll();
        //}

        [HttpGet("{id}")]
        public ActionResult<Country> Get(int id)
        {
            try
            {
                return repo.GetCountry(id);
            }
            catch (CountryException ex)
            {
                //Response.StatusCode = 400;
                //return null;
                return NotFound(ex.Message);
            }
        }
        [HttpGet]
        [HttpHead]

        public IEnumerable<Country> GetAll([FromQuery] string continent, [FromQuery] string capital)
        {
            if (!string.IsNullOrWhiteSpace(continent))
            {
                if (!string.IsNullOrWhiteSpace(capital.Trim()))
                    return repo.GetAll(continent, capital);
                else
                    return repo.GetAll(continent);
            }
                
            else
                return repo.GetAll();
        }
        [HttpPost]

        public ActionResult<Country> Post([FromBody] Country country)
        {
            repo.AddCountry(country);
            return CreatedAtAction(nameof(Get), new { id = country.Id }, country);
        }
        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            if (!repo.ExistsCountry(id))
            {
                return NotFound();
            }
            repo.RemoveCountry(repo.GetCountry(id));
            return NoContent();
        }

        [HttpPut("{id}")]

        public IActionResult Put(int id, [FromBody] Country country)
        {
            if (country == null || country.Id != id)
            {
                return BadRequest();
            }
            if (!repo.ExistsCountry(id))
            {
                repo.AddCountry(country);
                return CreatedAtAction(nameof(Get), new { id = country.Id }, country);
            }

            repo.UpdateCountry(country);
            return new NoContentResult();
             
            
        }

        [HttpPatch("{id}")]

        public ActionResult<Country> Patch(int id, [FromBody] JsonPatchDocument<Country> countryPatch)
        {
            var countryDB = repo.GetCountry(id);
            if (countryDB == null)
            {
                return NotFound();
            }
            try
            {
                countryPatch.ApplyTo(countryDB);
                return countryDB;
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
