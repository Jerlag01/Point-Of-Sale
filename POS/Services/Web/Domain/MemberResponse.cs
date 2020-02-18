using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dal.Model;
using Pos.Services.Web.Domain;

namespace Pos.Services.Web.Domain
{
    public class MemberResponse
    {
        public MemberResponse()
        {
            this.MemberCards = new List<MemberCardResponse>();
        }

        public static explicit operator MemberResponse(Member member)
        {
            if (member == null)
                return (MemberResponse)null;
            MemberResponse memberResponse = new MemberResponse()
            {
                Address = member.Address,
                BirthDate = member.BirthDate,
                City = member.City,
                Country = member.Country,
                Email = member.Email,
                FirstName = member.FirstName,
                Id = member.Id,
                LastName = member.LastName,
                TelephoneNr = member.TelephoneNr,
                ZipCode = member.ZipCode
            };
            if (member.Gender == Member.Genders.Male)
                memberResponse.Gender = MemberResponse.Genders.Male;
            else if (member.Gender == Member.Genders.Female)
                memberResponse.Gender = MemberResponse.Genders.Female;
            if (member.MemberCards == null)
            {
                memberResponse.MemberCards = (List<MemberCardResponse>)null;
                return memberResponse;
            }
            foreach (MemberCard memberCard in (Collection<MemberCard>)member.MemberCards)
            {
                MemberCardResponse memberCardResponse = new MemberCardResponse()
                {
                    ActiveMember = memberCard.ActiveMember,
                    Blocked = memberCard.Blocked,
                    CreationTime = memberCard.CreationTime,
                    ExpireDate = memberCard.ExpireDate,
                    Id = memberCard.Id,
                    Member = memberResponse,
                    Printed = memberCard.Printed,
                    SmartCardId = memberCard.SmartCardId
                };
                memberResponse.MemberCards.Add(memberCardResponse);
            }
            return memberResponse;
        }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public MemberResponse.Genders Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Email { get; set; }

        public string TelephoneNr { get; set; }

        public string Country { get; set; }

        public List<MemberCardResponse> MemberCards { get; set; }

        public enum Genders
        {
            Male,
            Female,
        }
    }
}
