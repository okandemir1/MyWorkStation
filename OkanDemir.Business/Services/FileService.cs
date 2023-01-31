using ImageMagick;
using ImageMagick.Formats;
using OkanDemir.Dto;
using OkanDemir.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Buffers.Text;
using System.Net;
using System.Text;

namespace OkanDemir.Business.Services
{
    public class FileService
    {
        public FileUploadDto UploadBase64(string evnoPath, string base64)
        {
            string[] extensions = base64.Split('/');
            extensions = extensions[1].Split(';');
            var extension = extensions[0];

            if (!(new[] { "jpg", "png", "jpeg" }.Contains(extension)))
                return new Dto.FileUploadDto(false, "Resim uzantýsý .jpg, .png veya .jpeg olabilir");

            try
            {
                var guid = Guid.NewGuid().ToString();
                string[] baseSplit = base64.Split(',');
                var bytes = Convert.FromBase64String(baseSplit[1]);

                var localImageDir = $"_uploads/post";
                var localImagePath = $"{localImageDir}/{guid}.{extension}";

                string contentRootPath = $"{evnoPath}\\wwwroot\\{localImagePath.Replace("/", "\\")}";

                if (!Directory.Exists(Path.Combine(contentRootPath.Replace($"{guid}.{extension}", ""))))
                    Directory.CreateDirectory(Path.Combine(contentRootPath.Replace($"{guid}.{extension}", "")));

                var model = new FileUploadDto(false, "") { HasAvif = false, HasWebp = false, WebpPath = "", AvifPath = "", };

                using (var imageFile = new FileStream(contentRootPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                Resize(contentRootPath, 400, 0);

                var webpConvertResult = ToWebP(contentRootPath);
                if (webpConvertResult.IsSucceed)
                {
                    model.HasWebp = webpConvertResult.IsSucceed;
                    model.WebpPath = $"/_uploads/post/{guid}.webp";
                }
                
                var avifConvertResult = ToAvif(contentRootPath);
                if (avifConvertResult.IsSucceed)
                {
                    model.HasAvif = avifConvertResult.IsSucceed;
                    model.AvifPath = $"/_uploads/post/{guid}.avif";
                }

                using (var image = new MagickImage(contentRootPath))
                {
                    model.Height = image.Height;
                    model.Width = image.Width;
                }

                model.FilePath = $"/{localImagePath}";
                model.SmallFilePath = $"";
                model.IsSucceed = true;
                model.Message = "";
                return model;
            }
            catch (Exception ex)
            {
                return new Dto.FileUploadDto(false, ex.Message);
            }
        }

        public DbOperationResult UploadAvatar(string evnoPath, string base64, string username)
        {
            string[] extensions = base64.Split('/');
            extensions = extensions[1].Split(';');
            var extension = extensions[0];

            if (!(new[] { "jpg", "png", "jpeg" }.Contains(extension)))
                return new DbOperationResult(false, "Resim uzantýsý .jpg, .png veya .jpeg olabilir");

            try
            {
                var localImageDir = $"_uploads/avatar";
                var localImagePath = $"{localImageDir}/{username}.jpg";
                string contentRootPath = $"{evnoPath}\\wwwroot\\{localImagePath.Replace("/", "\\")}";

                if (!Directory.Exists(Path.Combine(contentRootPath.Replace($"{username}.jpg", ""))))
                    Directory.CreateDirectory(Path.Combine(contentRootPath.Replace($"{username}.jpg", "")));

                string[] baseSplit = base64.Split(',');
                var bytes = Convert.FromBase64String(baseSplit[1]);

                using (var imageFile = new FileStream(contentRootPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                Resize(contentRootPath, 80, 80);
                return new DbOperationResult(true, "Baþarýlý");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }

        public string GenerateImage(string evnoPath, string fullname, string username)
        {
            try
            {
                var name_split = fullname.Split(' ');
                var name = "";
                foreach (var item in name_split)
                {
                    if (!string.IsNullOrEmpty(item))
                        name += item[0].ToString();
                }

                var fileList = new List<string>() { "avatar_1.jpg", "avatar_2.jpg", "avatar_3.jpg", "avatar_4.jpg", "avatar_5.jpg" };
                var fileName = fileList.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

                string image = $"{evnoPath}\\wwwroot\\img\\{fileName}";
                var newImage = $"{evnoPath}\\wwwroot\\_uploads\\avatar\\{username}.jpg";
                File.Copy(image, $"{evnoPath}\\wwwroot\\_uploads\\avatar\\{username}.jpg", true);

                WatermarkImage(newImage, "white", name);
                return $"/_uploads/avatar/{username}.jpg";
            }
            catch (Exception ex)
            {
                return $"/img/avatar_1.jpg";
            }
        }

        public void DeleteImages(string evnoPath, string path)
        {
            try
            {
                string contentRootPath = $"{evnoPath}\\wwwroot\\{path.Replace("/", "\\")}";

                var existImage = File.Exists($"{contentRootPath}");
                if (existImage)
                {
                    var extension = Path.GetExtension(contentRootPath);
                    var avifPath = $"{contentRootPath.Replace(extension, ".avif")}";
                    var avifExist = File.Exists(avifPath);
                    if (avifExist)
                        File.Delete(avifPath);

                    var webpPath = $"{contentRootPath.Replace(extension, ".webp")}";
                    var webpExist = File.Exists(webpPath);
                    if (webpExist)
                        File.Delete(webpPath);

                    File.Delete($"{contentRootPath}");
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string WatermarkImage(string path, string color, string text)
        {
            int x = 0;
            int y = 0;
            double font = 70;
            if (text.Length >= 3)
                font = 40;

            using (var image = new MagickImage(path))
            {
                using (var copyright = new MagickImage("xc:none", image.Width, image.Height))
                {
                    image.Draw(new Drawables()
                        .FillColor(new MagickColor(color))
                        .Gravity(Gravity.Center)
                        .Rotation(0)
                        .FontPointSize(font)
                        .Text(x, y, text));

                    image.Tile(copyright, CompositeOperator.Over);
                    image.Write(path);
                }
            }

            return path;
        }

        public string GetCroppedMedia(string path, int width, int height, bool generateWebFormats = true)
        {
            if (string.IsNullOrEmpty(path))
                return "";


            path = path.TrimStart('/');
            if (!File.Exists(path))
                return "";

            var directory = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path).TrimStart('.');
            var croppedFileName = $"{fileName}-{width}x{height}";
            var croppedFilePath = $"{directory}\\{croppedFileName}.{extension}";

            bool isFileExists = File.Exists(croppedFilePath);
            if (!isFileExists)
            {
                croppedFilePath = Crop(path, croppedFilePath, width, height);

                //crop edemedi
                if (croppedFilePath == "")
                    return "";

                Optimize(path);
            }

            if (generateWebFormats)
            {

                var webpPathExists = File.Exists(croppedFilePath.Replace(extension, ".webp"));
                if (!webpPathExists)
                {
                    var webpPath = ToWebP(croppedFilePath);
                    Optimize(webpPath.Path);
                }

                var avifPathExists = File.Exists(croppedFilePath.Replace(extension, ".avif"));
                if (!avifPathExists)
                {
                    ToAvif(croppedFilePath);
                }
            }

            var relativeRequestedFilePath = "\\" + croppedFilePath.Replace(path, "");

            return relativeRequestedFilePath.Replace("\\", "/");
        }

        public string Optimize(string path)
        {
            if (!File.Exists(path))
                return "";
            try
            {
                using (var image = new MagickImage(path))
                {

                    image.Settings.SetDefines(new JpegWriteDefines()
                    {
                        SamplingFactor = JpegSamplingFactor.Ratio420,
                    });

                    image.Strip();
                    image.Quality = 85;
                    image.Interlace = Interlace.Jpeg;
                    image.ColorSpace = ColorSpace.sRGB;

                    image.Write(path);
                }


            }
            catch (Exception exc)
            {
                return "";
            }

            return path;
        }

        public string Crop(string path, string newFilePath, int width, int height)
        {
            if (!File.Exists(path))
                return "";
            try
            {
                using (var image = new MagickImage(path))
                {
                    var requestFileRatio = (double)width / (double)height;

                    var currentFileRatio = (double)image.Width / (double)image.Height;

                    if (requestFileRatio > currentFileRatio)
                    {
                        var newHeight = image.Width * height / width;


                        image.Crop(image.Width, newHeight);


                    }
                    else
                    {
                        var newWidth = image.Height * width / height;
                        image.Crop(newWidth, image.Height);
                    }

                    image.Resize(width, height);
                    image.Write(newFilePath);
                }
            }
            catch (Exception exc)
            {
                return "";
            }

            return newFilePath;
        }

        public string Quality(string path, int quality)
        {
            using (var image = new MagickImage(path))
            {
                image.Strip();
                image.Quality = quality;
                image.Write(path);
            }

            return path;
        }

        public string Resize(string path, int width, int height)
        {
            using (var image = new MagickImage(path))
            {
                image.Resize(width, height);
                image.Write(path);
            }

            return path;
        }

        public ConvertResponseDto ToWebP(string path)
        {
            var convertResponse = new ConvertResponseDto();

            if (!File.Exists(path))
            {
                convertResponse.IsSucceed = false;
                convertResponse.Message = "Path not found";
                convertResponse.Path = "";

                return convertResponse;
            }

            var directory = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var requestedFilePath = $"{directory}\\{fileName}.webp";
            convertResponse.Path = requestedFilePath;

            if (File.Exists(requestedFilePath))
            {
                convertResponse.IsSucceed = true;
                convertResponse.Message = "already exists";

                return convertResponse;
            }

            try
            {
                using (var images = new MagickImageCollection(path))
                {
                    var defines = new WebPWriteDefines
                    {
                        Lossless = true,
                        Method = 6,
                    };

                    images.Write(requestedFilePath, defines);

                    convertResponse.IsSucceed = true;
                    convertResponse.Message = "converted";

                    return convertResponse;
                }
            }
            catch (Exception exc)
            {
                convertResponse.IsSucceed = false;
                convertResponse.Message = exc.Message;
                convertResponse.Path = "";

                return convertResponse;
            }
        }

        public ConvertResponseDto ToAvif(string path)
        {
            var convertResponse = new ConvertResponseDto();

            if (!File.Exists(path))
            {
                convertResponse.IsSucceed = false;
                convertResponse.Message = "Path not found";
                convertResponse.Path = "";

                return convertResponse;
            }

            var directory = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var requestedFilePath = $"{directory}\\{fileName}.avif";
            convertResponse.Path = requestedFilePath;

            if (File.Exists(requestedFilePath))
            {
                convertResponse.IsSucceed = true;
                convertResponse.Message = "already exists";

                return convertResponse;
            }

            try
            {
                using (var images = new MagickImage(path))
                {
                    images.Write(requestedFilePath, MagickFormat.Avif);

                    convertResponse.IsSucceed = true;
                    convertResponse.Message = "converted";

                    return convertResponse;
                }
            }
            catch (Exception exc)
            {
                convertResponse.IsSucceed = false;
                convertResponse.Message = exc.Message;
                convertResponse.Path = "";

                return convertResponse;
            }
        }
    }

}