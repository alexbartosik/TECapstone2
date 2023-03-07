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
    }
}
