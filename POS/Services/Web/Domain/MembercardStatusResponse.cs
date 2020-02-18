namespace Pos.Services.Web.Domain
{
    public class MembercardStatusResponse
    {
        public MembercardStatusResponse(MembercardStatusResponse.MembercardStatus status, string name)
        {
            this.MemcardStatus = status;
            this.Name = name;
        }

        public MembercardStatusResponse.MembercardStatus MemcardStatus { get; set; }

        public string Name { get; set; }

        public enum MembercardStatus
        {
            Valid,
            Blocked,
            Expired,
        }
    }
}