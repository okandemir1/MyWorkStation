using OkanDemir.WebUI.Cms.Models;
using System.Linq;
using System.Text;

namespace OkanDemir.WebUI.Cms.Helpers
{
    public class ViewResultHelperExtension
    {
        public static string ViewResult(ViewModelResult viewModelResult)
        {
            if (viewModelResult == null)
                return "";

            if (viewModelResult.Errors.Count() > 0 && !viewModelResult.IsSucceed)
            {
                var return_html = new StringBuilder($"<div class='alert alert-danger'><p>{viewModelResult.Message}</p><ul>");
                foreach (var item in viewModelResult.Errors)
                {
                    return_html.Append($"<li>{item}</li>");
                }
                return_html.Append("</ul></div>");

                return return_html.ToString();
            }

            if (!viewModelResult.IsSucceed && !string.IsNullOrEmpty(viewModelResult.Message))
            {
                var return_html = new StringBuilder($"<div class='alert alert-danger'>Hata : {viewModelResult.Message}</div>");
                return return_html.ToString();
            }

            if (viewModelResult.IsSucceed && !string.IsNullOrEmpty(viewModelResult.Message))
            {
                var return_html = new StringBuilder($"<div class='alert alert-success'>{viewModelResult.Message}</div>");
                return return_html.ToString();
            }

            return "";
        }
    }
}
