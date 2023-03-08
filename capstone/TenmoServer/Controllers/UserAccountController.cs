using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;

namespace TenmoServer.Controllers
{
    [Route("account")]
    [ApiController]
    [Authorize]
    public class UserAccountController : ControllerBase
    {
        public readonly IUserAccountDAO dao;

        public UserAccountController(IUserAccountDAO accountDao)
        {
            this.dao = accountDao;
        }

        [HttpGet("balance")]
        public ActionResult GetBalance()
        {
            return Ok(dao.GetMyAccountBalance(User.Identity.Name));
        }
    }
}
