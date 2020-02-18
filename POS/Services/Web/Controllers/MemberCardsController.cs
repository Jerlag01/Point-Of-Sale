using System;
using System.Web.Http;
using System.Web.Http.Description;
using Dal;
using Pos.Services.Web.Domain;

namespace Pos.Services.Web.Controllers
{
    [Authorize]
    public class MemberCardsController : ApiController
    {
        private UnitOfWork UnitOfWork = new UnitOfWork();

        [ResponseType(typeof(MemberCardResponse))]
        public IHttpActionResult GetMembeCards(Guid id)
        {
            MemberCardResponse byId = (MemberCardResponse)this.UnitOfWork.MemberCardRepository.GetById(id);
            return byId == null ? (IHttpActionResult)this.NotFound() : (IHttpActionResult)this.Ok<MemberCardResponse>(byId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.UnitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}