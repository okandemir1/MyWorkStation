namespace OkanDemir.Dto
{
    public class FileUploadDto
    {
        public FileUploadDto(bool isSucceed, string message)
        {
            IsSucceed = isSucceed;
            Message = message;
        }

        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public string FilePath { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string SmallFilePath { get; set; }
        public bool HasAvif { get; set; }
        public bool HasWebp { get; set; }
        public string WebpPath { get; set; }
        public string AvifPath { get; set; }
    }
}
