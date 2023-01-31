using FluentValidation;

namespace OkanDemir.WebUI.Cms.Models
{
    public class CmsHomeViewModel
    {
        public int TotalVerifiedUser { get; set; }
        public int TotalUser { get; set; }
        public int BannedUser { get; set; }
        public int TotalPost { get; set; }
    }
}
