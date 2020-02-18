using System;
using Dal.Model;

namespace Pos.Services.Web.Domain
{
    public class MemberCardResponse
    {
        public static explicit operator MemberCardResponse(MemberCard mc)
        {
            if (mc == null)
                return (MemberCardResponse)null;
            return new MemberCardResponse()
            {
                ActiveMember = mc.ActiveMember,
                Blocked = mc.Blocked,
                CreationTime = mc.CreationTime,
                ExpireDate = mc.ExpireDate,
                Id = mc.Id,
                Member = (MemberResponse)mc.Member,
                Printed = mc.Printed,
                SmartCardId = mc.SmartCardId
            };
        }

        public Guid Id { get; set; }

        public MemberResponse Member { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime ExpireDate { get; set; }

        public bool ActiveMember { get; set; }

        public string SmartCardId { get; set; }

        public bool Printed { get; set; }

        public bool Blocked { get; set; }
    }
}