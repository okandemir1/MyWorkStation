using OkanDemir.WebUI.Cms.Helpers.Dto;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace OkanDemir.WebUI.Cms.Helpers
{
    public static class ImageHelper
    {
        public static string GetUserId(FileStream file)
        {
            return "";
        }

        public static string GetFilePath(string evnoPath, string extension)
        {
            if (!(new[] { "jpg", "png", "jpeg" }.Contains(extension)))
                return "Resim uzantýsý .jpg, .png veya .jpeg olabilir";

            var fileName = Guid.NewGuid().ToString("N");
            var localImageDir = $"_uploads";
            var localImagePath = $"{localImageDir}/{fileName}.{extension}";

            string contentRootPath = $"{evnoPath}\\wwwroot\\{localImagePath.Replace("/", "\\")}";
            contentRootPath = contentRootPath.Replace("main.kuryeyolda.com", "kuryeyolda.com");
            contentRootPath = contentRootPath.Replace("Waffle.Application.CMS", "Waffle.Application.WebUI");

            if (!Directory.Exists(Path.Combine(contentRootPath.Replace($"{fileName}.{extension}", ""))))
                Directory.CreateDirectory(Path.Combine(contentRootPath.Replace($"{fileName}.{extension}", "")));

            return contentRootPath;
        }

        public static ImageUploadDto UploadFile(string folderName, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).Trim('.').ToLower();
            if (!(new[] { "jpg", "png", "jpeg", "pdf", "doc", "docx", "xlx", "xlxs" }.Contains(extension)))
                return new ImageUploadDto() { IsSucceed = false, Message = "Geçersiz dosya", Path = "", };

            var localFileDir = $"wwwroot/_uploads/{folderName}";
            var localFilePath = $"{localFileDir}/{file.FileName}";

            try
            {
                if (!Directory.Exists(Path.Combine(localFileDir)))
                    Directory.CreateDirectory(Path.Combine(localFileDir));

                using (Stream fileStream = new FileStream(localFilePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return new ImageUploadDto() { IsSucceed = true, Message = "Dosya yüklendi", Path = localFilePath.Replace("wwwroot/", "") };
            }
            catch (Exception ex)
            {
                return new ImageUploadDto() { IsSucceed = false, Message = "Dosya yüklenemedi", Path = "" };
            }
        }
    }
}