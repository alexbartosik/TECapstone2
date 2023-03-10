using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Models;
using System.Linq;

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

        [HttpPost("send")]
        public ActionResult SendTEbucks([FromBody] Transfer transfer) 
        {
            transfer.AccountFrom = int.Parse(User.FindFirst("sub").Value);

            decimal currentBalance = dao.GetMyAccountBalance(User.Identity.Name);

            List<User> users = dao.GetUsers(User.Identity.Name);

            if (transfer.Amount < currentBalance && transfer.Amount > 0 && users.Any(u => u.UserId == transfer.AccountTo))
            {
                Transfer newTransfer = transferDao.CreateSendTransfer(transfer);

                //following code to be run after transfer is approved for RequestTEBucks()
                dao.DecreaseAccountBalance(transfer.AccountFrom, newTransfer.Amount);
                dao.IncreaseAccountBalance(newTransfer.AccountTo, newTransfer.Amount);

                return Created($"account/send/{newTransfer.Id}", newTransfer);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("request")]
        public ActionResult RequestTEbucks([FromBody] Transfer transfer)
        {
            transfer.AccountTo = int.Parse(User.FindFirst("sub").Value);

            List<User> users = dao.GetUsers(User.Identity.Name);

            if (transfer.Amount > 0 && users.Any(u => u.UserId == transfer.AccountFrom))
            {
                Transfer newTransfer = transferDao.CreateRequestTransfer(transfer);

                return Created($"account/request/{newTransfer.Id}", newTransfer);
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

        [HttpGet("myTransfers/{transferId}")]
        public ActionResult GetTransferById(int transferId)
        {
            return Ok(transferDao.GetTransferInfo(transferId));
        }

        [HttpGet("pending")]
        public ActionResult ListPendingTransferForCurrentUser()
        {
            return Ok(transferDao.ListPendingTransfersByUserId(int.Parse(User.FindFirst("sub").Value)));
        }
    }
}
