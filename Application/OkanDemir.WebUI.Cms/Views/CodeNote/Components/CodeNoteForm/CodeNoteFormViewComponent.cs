using Microsoft.AspNetCore.Mvc;
using OkanDemir.Business;
using OkanDemir.Dto;
using OkanDemir.WebUI.Cms.Helpers;

namespace OkanDemir.WebUI.Cms.Views.CodeNote.Components.CodeNoteForm
{
    public class CodeNoteFormViewComponent : ViewComponent
    {
        CodeCategoryBusiness _codeCategoryBusiness;
        public CodeNoteFormViewComponent(CodeCategoryBusiness _codeCategoryBusiness)
        {
            this._codeCategoryBusiness = _codeCategoryBusiness;
        }

        public IViewComponentResult Invoke(string action,
            CodeNoteDto model)
        {
            ViewBag.Action = action;
            ViewBag.Categories = _codeCategoryBusiness.GetAllActiveList(User.GetUserId());
            return View(model);
        }

    }
}
