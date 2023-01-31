using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Html;

namespace OkanDemir.WebUI.Cms.Helpers
{
    public class BreadCrumbHelperExtensions
    {
        public StringBuilder BreadCrumb(List<Bc> items)
        {
            var bread_crumb_html = new StringBuilder();
            var start_tag = "<div class='page-header-content d-lg-flex px-4'>" +
                            "<div class='d-flex'>" +
                                "<div class='breadcrumb py-2'>";

            bread_crumb_html.Append(start_tag);

            foreach (var item in items)
            {
                var bread_content_html = "";
                if (string.IsNullOrEmpty(item.Path))
                    bread_content_html = $"<span class='breadcrumb-item active'>{item.Name}</span>";
                else
                    bread_content_html = $"<a href='{item.Path}' class='breadcrumb-item'>{item.Name}</a>";

                bread_crumb_html.Append(bread_content_html);
            }
                                    
            var finish_tag =    "</div>"+
                                "</div>" +
                                "</div>";

            bread_crumb_html.Append(finish_tag);
            return bread_crumb_html;
        }
    }

    public class Bc
    {
        public Bc(string _name, string _path, bool _isActive = false)
        {
            Name = _name;
            Path = _path;
            IsActive = _isActive;
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsActive { get; set; }
    }
}
