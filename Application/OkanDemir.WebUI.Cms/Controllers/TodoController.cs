using Microsoft.AspNetCore.Mvc;
using OkanDemir.Business;
using OkanDemir.Dto;
using OkanDemir.WebUI.Cms.Authorize;
using OkanDemir.WebUI.Cms.Helpers;
using OkanDemir.WebUI.Cms.Models;

namespace OkanDemir.WebUI.Cms.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        TodoBusiness _todoBusiness;
        CacheHelper _cache;

        public TodoController(TodoBusiness _todoBusiness, CacheHelper _cache)
        {
            this._todoBusiness = _todoBusiness;
            this._cache = _cache;
        }

        [HttpGet]
        public IActionResult Index(int projectId)
        {
            var tables = _todoBusiness.GetAllTodoTables(User.GetUserId(), projectId);
            var todos = _todoBusiness.GetAllTodoList(User.GetUserId(), projectId);
            var types = _todoBusiness.GetTodoListTypes();

            var model = new TodoViewModel()
            {
                TodoLists = todos,
                TodoListTypes = types,
                TodoTables = tables,
            };

            return View(model);
        }
    }
}
