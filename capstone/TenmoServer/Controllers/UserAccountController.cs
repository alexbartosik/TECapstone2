using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("account")]
    [ApiController]
    [Authorize]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountDAO dao;
        private readonly IUserDAO userDAO;

        public UserAccountController(IUserAccountDAO accountDao)
        {
            this.dao = accountDao;
        }

        [HttpGet("balance")]
        public ActionResult GetBalance()
        {
            return Ok(dao.GetMyAccountBalance(User.Identity.Name));
        }

        [HttpGet("users")]
        public ActionResult GetUsers()
        {
            return Ok(dao.GetUsers(User.Identity.Name));
        }

        [HttpPut("transfer")]
        public ActionResult TransferTEbucks(int accountTo, decimal amountToSend) 
        {
            decimal currentBalance = dao.GetMyAccountBalance(User.Identity.Name);
            string username = User.Identity.Name;

            if (amountToSend < currentBalance)
            {
                dao.DecreaseAccountBalance(username, amountToSend);
                dao.IncreaseAccountBalance(accountTo, amountToSend);
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
