using System;
using System.Web.Http;
using System.Web.Http.Description;
using Dal;
using Pos.Services.Web.Domain;

namespace Pos.Services.Web.Controllers
{
    [Authorize]
    public class MembersController : ApiController
    {
        private UnitOfWork UnitOfWork = new UnitOfWork();

        [ResponseType(typeof(MemberResponse))]
        public IHttpActionResult GetMember(Guid id)
        {
            MemberResponse byId = (MemberResponse)this.UnitOfWork.MemberRepository.GetById(id);
            return byId == null ? (IHttpActionResult)this.NotFound() : (IHttpActionResult)this.Ok<MemberResponse>(byId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.UnitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}