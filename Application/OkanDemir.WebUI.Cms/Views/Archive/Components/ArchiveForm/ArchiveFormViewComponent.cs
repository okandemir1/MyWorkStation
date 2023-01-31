using Microsoft.AspNetCore.Mvc;
using OkanDemir.Business;
using OkanDemir.Dto;
using OkanDemir.WebUI.Cms.Helpers;

namespace OkanDemir.WebUI.Cms.Views.Archive.Components.ArchiveForm
{
    public class ArchiveFormViewComponent : ViewComponent
    {
        ArchiveCategoryBusiness archiveCategoryBusiness;
        public ArchiveFormViewComponent(ArchiveCategoryBusiness archiveCategoryBusiness)
        {
            this.archiveCategoryBusiness = archiveCategoryBusiness;
        }

        public IViewComponentResult Invoke(string action,
            ArchiveDto model)
        {
            ViewBag.Action = action;
            ViewBag.Categories = archiveCategoryBusiness.GetAllActiveList(User.GetUserId());
            return View(model);
        }

    }
}
