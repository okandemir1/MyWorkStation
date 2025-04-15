using Microsoft.AspNetCore.Mvc;
using OkanDemir.Dto;

namespace OkanDemir.WebUI.Cms.Views.TodoProject.Components.TodoProjectForm
{
    public class TodoProjectFormViewComponent : ViewComponent
    {
        
        public IViewComponentResult Invoke(string action,
            TodoProjectDto model)
        {
            ViewBag.Action = action;
            return View(model);
        }

    }
}
