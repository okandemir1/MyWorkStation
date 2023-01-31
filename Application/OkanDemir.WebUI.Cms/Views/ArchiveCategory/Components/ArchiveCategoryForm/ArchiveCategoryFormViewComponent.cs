using Microsoft.AspNetCore.Mvc;
using OkanDemir.Dto;

namespace OkanDemir.WebUI.Cms.Views.ArchiveCategory.Components.ArchiveCategoryForm
{
    public class ArchiveCategoryFormViewComponent : ViewComponent
    {
        
        public IViewComponentResult Invoke(string action,
            ArchiveCategoryDto model)
        {
            ViewBag.Action = action;
            return View(model);
        }

    }
}
