using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.DAO;
using Server.Models;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CityController : ControllerBase
    {
        private readonly ICityDao dao;

        public CityController(ICityDao cityDao)
        {
            this.dao = cityDao;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult List()
        {
            return Ok(dao.ListCities());
        }

        /*
        [HttpGet("{id}")]
        public ActionResult GetCity(int id)
        {
            return Ok(dao.ListCities());
        } */

        [HttpPost]
        public ActionResult AddCity([FromBody] City city)
        {
            City newCity = dao.AddCity(city);

            return Created($"api/city/{newCity.Id}", newCity);
        }
    }
}
