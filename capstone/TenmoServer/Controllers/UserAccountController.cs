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
        private readonly ITransferDAO transferDao;

        public UserAccountController(IUserAccountDAO accountDao, ITransferDAO transferDAO)
        {
            this.dao = accountDao;
            this.transferDao = transferDAO;
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

        [HttpPost("transfer")]
        public ActionResult TransferTEbucks([FromBody] Transfer transfer) 
        {
            transfer.AccountFrom = int.Parse(User.FindFirst("sub").Value);

            Transfer newTransfer = transferDao.CreateTransfer(transfer);

            decimal currentBalance = dao.GetMyAccountBalance(User.Identity.Name);

            if (newTransfer.Amount < currentBalance)
            {
                dao.DecreaseAccountBalance(transfer.AccountFrom, newTransfer.Amount);
                dao.IncreaseAccountBalance(newTransfer.AccountTo, newTransfer.Amount);
                return Created($"account/transfer/{newTransfer.Id}", newTransfer);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet("myTransfers")]
        public ActionResult GetMyTransfers() 
        {
            List<TransferRecord> allTransfers = new List<TransferRecord>();

            List<TransferRecord> toTransfers = transferDao.ListToTransfersByUserId(int.Parse(User.FindFirst("sub").Value));
            List<TransferRecord> fromTransfers = transferDao.ListFromTransfersByUserId(int.Parse(User.FindFirst("sub").Value));

            foreach (TransferRecord t in fromTransfers)
            {
                allTransfers.Add(t);
            }

            foreach (TransferRecord t in toTransfers)
            {
                allTransfers.Add(t);
            }

            return Ok(allTransfers);
        }
    }
}
