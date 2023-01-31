using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OkanDemir.Business;
using OkanDemir.Business.Filters;
using OkanDemir.Data;
using OkanDemir.Dto;
using OkanDemir.WebUI.Cms.Authorize;
using OkanDemir.WebUI.Cms.Helpers;
using OkanDemir.WebUI.Cms.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OkanDemir.WebUI.Cms.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        NoteBusiness _noteBusiness;

        public NoteController(NoteBusiness _noteBusiness)
        {
            this._noteBusiness = _noteBusiness;
        }

        [HttpGet]
        public IActionResult Index(DateTime? createDate)
        {
            ViewBag.Action = "Index";
            if (createDate == null)
                createDate = DateTime.Now;

            var notes = _noteBusiness.GetNotesWithDates(createDate.Value, User.GetUserId());

            var model = new NoteViewModel()
            {
                Notes = notes,
                Date = createDate.Value,
                Note = new NoteDto() { Description = "", UserId = User.GetUserId() }
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(NoteViewModel viewModel)
        {
            viewModel.Note.CreateDate = viewModel.Date;
            viewModel.Note.UserId = User.GetUserId();

            var response = _noteBusiness.Create(viewModel.Note);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var note = _noteBusiness.GetNoteById(id, User.GetUserId());
            if (note == null)
                return RedirectToAction("Index", new { q = "not_found_note" });

            //AlertTime'ı kullanabilir güncelleyelim kafa karıştırmasın
            if (note.AlertTime < DateTime.Now)
                note.AlertTime = note.AlertTime.AddMinutes(20);

            return View(note);
        }

        [HttpPost]
        public IActionResult Edit(NoteDto model)
        {
            model.UserId = User.GetUserId();
            var response = _noteBusiness.Update(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var response = _noteBusiness.Delete(id, User.GetUserId());
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        public ActionResult Metion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetMetionList(DataTableParameters dataTableParameters)
        {
            dataTableParameters.UserId = User.GetUserId();
            var response = _noteBusiness.GetMetions(new MetionFilterModel(dataTableParameters));

            return Json(
            new
            {
                draw = dataTableParameters.Draw,
                recordsFiltered = response.RecordsFiltered,
                recordsTotal = response.TotalCount,
                data = response.Data
            });
        }

        public IActionResult MetionNotes(int id)
        {
            var metionNotes = _noteBusiness.MetionNotesById(id, User.GetUserId());
            return View(metionNotes);
        }

        public IActionResult Hashtag()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetHashtagList(DataTableParameters dataTableParameters)
        {
            dataTableParameters.UserId = User.GetUserId();
            var response = _noteBusiness.GetHashtags(new HashtagFilterModel(dataTableParameters));

            return Json(
            new
            {
                draw = dataTableParameters.Draw,
                recordsFiltered = response.RecordsFiltered,
                recordsTotal = response.TotalCount,
                data = response.Data
            });
        }

        public IActionResult HashtagNotes(int id)
        {
            var hashtagNotes = _noteBusiness.HashtagNotesById(id, User.GetUserId());
            return View(hashtagNotes);
        }

        [HttpGet]
        public IActionResult MetionDelete(int id)
        {
            var response = _noteBusiness.DeleteMetion(id, User.GetUserId());
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public IActionResult HashtagDelete(int id)
        {
            var response = _noteBusiness.DeleteHashtag(id, User.GetUserId());
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }
    }
}
