using Microsoft.AspNetCore.Mvc;
using OkanDemir.Dto;

namespace OkanDemir.WebUI.Cms.Views.CodeCategory.Components.CodeCategoryForm
{
    public class CodeCategoryFormViewComponent : ViewComponent
    {
        
        public IViewComponentResult Invoke(string action,
            CodeCategoryDto model)
        {
            ViewBag.Action = action;
            return View(model);
        }

    }
}
