using System;
using System.Web.Http;
using System.Web.Http.Description;
using Dal;
using Dal.Model;
using Pos.Services.Web.Domain;

namespace Pos.Services.Web.Controllers
{
    [Authorize]
    public class ValidateMembercardController : ApiController
    {
        private UnitOfWork UnitOfWork = new UnitOfWork();

        [ResponseType(typeof(MembercardStatusResponse.MembercardStatus))]
        public IHttpActionResult GetValidateMembercard(Guid id)
        {
            MemberCard byId = this.UnitOfWork.MemberCardRepository.GetById(id);
            if (byId == null)
                return (IHttpActionResult)this.NotFound();
            if (byId.Blocked)
                return (IHttpActionResult)this.Ok<MembercardStatusResponse>(new MembercardStatusResponse(MembercardStatusResponse.MembercardStatus.Blocked, byId.Member.Fullname));
            return byId.ExpireDate < DateTime.Now ? (IHttpActionResult)this.Ok<MembercardStatusResponse>(new MembercardStatusResponse(MembercardStatusResponse.MembercardStatus.Expired, byId.Member.Fullname)) : (IHttpActionResult)this.Ok<MembercardStatusResponse>(new MembercardStatusResponse(MembercardStatusResponse.MembercardStatus.Valid, byId.Member.Fullname));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.UnitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}