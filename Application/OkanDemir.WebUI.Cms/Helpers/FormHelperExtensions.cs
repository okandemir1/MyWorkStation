using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OkanDemir.WebUI.Cms.Helpers
{
    public enum Length
    {
        XSmall,
        Small,
        Medium,
        Long,
        XLong
    };

    public enum TextAreaRows
    {
        XSmall = 2,
        Small = 3,
        Medium = 5,
        Long = 8,
        Longer = 12,
        Longest = 16,
        Max = 20
    }

    public class FormHelperExtensions
    {
        public StringBuilder InputTextRow(string labelValue, object value
            , string name, string id = "", Length length = Length.XLong, string placeholder = "")
        {
            var input_text_html = new StringBuilder();
            var input_start_html = "<div class='form-group mb-3 row'>" +
                                        $"<label class='control-label text-md-right col-sm-2 m-0' style='line-height:36px;'>{labelValue}</label>" +
                                        "<div class='col-sm-10'>";
            var input_html = $"<input type='text' class='form-control' placeholer='{placeholder}' id='{id}' name='{name}' value='{value}' />";
            var input_finish_html = "</div></div>";

            input_text_html.Append(input_start_html);
            input_text_html.Append(input_html);
            input_text_html.Append(input_finish_html);
            return input_text_html;
        }

        public StringBuilder InputPasswordRow(string labelValue, object value
            , string name, string id = "", Length length = Length.XLong, string placeholder = "")
        {
            var input_text_html = new StringBuilder();
            var input_start_html = "<div class='form-group mb-3 row'>" +
                                        $"<label class='control-label text-md-right col-sm-2 m-0' style='line-height:36px;'>{labelValue}</label>" +
                                        "<div class='col-sm-10'>";
            var input_html = $"<input type='password' class='form-control' placeholer='{placeholder}' id='{id}' name='{name}' value='{value}' />";
            var input_finish_html = "</div></div>";

            input_text_html.Append(input_start_html);
            input_text_html.Append(input_html);
            input_text_html.Append(input_finish_html);
            return input_text_html;
        }

        public StringBuilder FileRow(string labelValue, string name)
        {
            var input_text_html = new StringBuilder();
            var input_start_html = "<div class='form-group mb-3 row'>" +
                                        $"<label class='control-label text-md-right col-sm-2 m-0' style='line-height:36px;'>{labelValue}</label>" +
                                        "<div class='col-sm-10'>";
            var input_html = $"<input type='file' class='form-control' name='{name}' />";
            var input_finish_html = "</div></div>";

            input_text_html.Append(input_start_html);
            input_text_html.Append(input_html);
            input_text_html.Append(input_finish_html);
            return input_text_html;
        }
        
        public StringBuilder TextAreaTextRow(string labelValue, object value
            , string name, string id = "", Length length = Length.XLong, string placeholder = "")
        {
            var input_text_html = new StringBuilder();

            var input_start_html = "<div class='form-group mb-3 row'>" +
                                        $"<label class='control-label text-md-right col-sm-2 m-0' style='line-height:36px;'>{labelValue}</label>" +
                                        "<div class='col-sm-10'>";
            var input_html = $"<textarea class='form-control' name='{name}' placeholer='{placeholder}' id='{id}'>{value}</textarea>";
            var input_finish_html = "</div></div>";

            input_text_html.Append(input_start_html);
            input_text_html.Append(input_html);
            input_text_html.Append(input_finish_html);
            return input_text_html;
        }

        public StringBuilder InputDateRow(string labelValue, object value
            , string name, string id = "", Length length = Length.XLong, string placeholder = "")
        {

            /*
             <div class="form-group row">
                <label class="col-form-label col-lg-2">Yayınlanma Tarihi:</label>
                <div class="col-lg-10">
                    <div class="input-group">
                        <span class="input-group-prepend">
                            <button type="button" class="btn btn-light btn-icon" id="publishButton"><i class="icon-calendar3"></i></button>
                        </span>
                        <input type="text" class="form-control" id="publishInput" name="PublishDate" value="@Model.Content.PublishDate" placeholder="Yayınlanma Tarihi Seçin">
                    </div>
                    <span class="form-text text-muted">Format must be YYYY-MM-DD HH:MM:SS</span>
                </div>
            </div>
             */

            var input_text_html = new StringBuilder();

            if (!string.IsNullOrEmpty(labelValue))
                input_text_html.Append($"<div class='form-group mb-3 row'><label class='col-form-label col-lg-2'>{labelValue}</label><div class='col-lg-10'>");
            else
                input_text_html.Append("<div class='col-lg-10 px-0'>");

            input_text_html.Append("<div class='input-group'>" +
                                                "<span class='input-group-text'>" +
                                                    "<i class='ph-calendar'></i>" +
                                                "</span>" +
                                                $"<input type='text' name='{name}' value='{value}' class='form-control datepicker-start-day datepicker-input' placeholder='{labelValue}'>" +
                                            "</div>" +
                                        "</div>");

            if (!string.IsNullOrEmpty(labelValue))
                input_text_html.Append("</div>");
            //var input_start_html = "<div class='form-group mb-3 row'>" +
            //                            $"<label class='col-form-label col-lg-2'>{labelValue}</label>"+
            //                            "<div class='col-lg-10'>" +
            //                                "<div class='input-group'>"+
											 //   "<span class='input-group-text'>"+
												//    "<i class='ph-calendar'></i>"+
											 //   "</span>"+
            //                                    $"<input type='text' name='{name}' value='{value}' class='form-control datepicker-start-day datepicker-input' placeholder='{labelValue}'>" +
										  //  "</div>"+
            //                            "</div>"+
            //                        "</div>";

            return input_text_html;
        }
        
        public StringBuilder SubmitButtonRow(string value)
        {
            var input_text_html = new StringBuilder();
            var input_start_html = "<div class='form-group'>"+
                                        "<div class='offset-sm-2'>"+
                                            $"<button type='submit' class='btn btn-success ml-3'>{value}</button>"+
                                        "</div>"+
                                    "</div>";

            input_text_html.Append(input_start_html);
            return input_text_html;
        }

        public StringBuilder UpdateButtonRow(string value)
        {
            var input_text_html = new StringBuilder();
            var input_start_html = "<div class='form-group'>" +
                                        "<div class='offset-sm-2'>" +
                                            $"<button type='submit' class='btn btn-primary ml-3'>{value}</button>" +
                                        "</div>" +
                                    "</div>";

            input_text_html.Append(input_start_html);
            return input_text_html;
        }
        public StringBuilder CheckBoxRow(string labelValue, bool value, string name)
        {
            var input_text_html = new StringBuilder();
            var input_start_html = "<div class='form-check form-switch mb-2'>" +
                                        $"<label class='form-check-label' for='{name}'>{labelValue}</label>" +
                                        "<div class='col-sm-10'>";
            var input_html = $"<input type='checkbox' class='form-check-input' id='{name}' name='{name}' value='{value}' />";
            var input_finish_html = "</div></div>";

            input_text_html.Append(input_start_html);
            input_text_html.Append(input_html);
            input_text_html.Append(input_finish_html);
            return input_text_html;
        }
    }
}
