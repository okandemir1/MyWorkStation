using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Html;

namespace OkanDemir.WebUI.Cms.Helpers
{
    public class PageHeaderHelperExtensions
    {
        public StringBuilder MainHeader(string title, string description
            , string actionLabel = "", string actionUrl = "", string actionLabel2 = "", string actionUrl2 = "", string actionLabel3 = "", string actionUrl3 = "")
        {
            if (string.IsNullOrEmpty(description))
                description = "&nbsp;";

            var page_header_html = new StringBuilder();
            var start_tag = "<div class='page-header-content d-lg-flex border-top border-bottom px-4'>" +
                            "<div class='d-flex'>" +
                                "<div class='breadcrumb py-2'>"+
                                    $"<span class='font-weight-semibold'>{title}</span>"+
                                    $"<div class='col mt-1 text-muted'><span>{description}</span></div>"+
                                "</div>" +
                            "</div>" +
                            "<div class='collapse d-lg-block ms-lg-auto'>";
            page_header_html.Append(start_tag);
            if(!string.IsNullOrEmpty(actionLabel))
            {
                var action = $"<a href='{actionUrl}' class='btn btn-success btn-sm text-default text-white mt-1'><span>{actionLabel}</span></a>";
                page_header_html.Append(action);
            }
            if (!string.IsNullOrEmpty(actionLabel2))
            {
                var action = $"<a href='{actionUrl2}' class='btn btn-warning btn-sm ms-2 text-default text-white mt-1'><span>{actionLabel2}</span></a>";
                page_header_html.Append(action);
            }
            if (!string.IsNullOrEmpty(actionLabel3))
            {
                var action = $"<a href='{actionUrl3}' class='btn btn-info ms-2 btn-sm text-default text-white mt-1'><span>{actionLabel3}</span></a>";
                page_header_html.Append(action);
            }
            var finish_tag = "</div></div>";
            page_header_html.Append(finish_tag);
            return page_header_html;
        }
    }
}
