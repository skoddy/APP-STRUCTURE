﻿using System.Threading.Tasks;

namespace GPB.Services
{
    public class ImagePickerResult
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] ImageBytes { get; set; }
        public object ImageSource { get; set; }
    }

    public interface IFilePickerService
    {
        Task<ImagePickerResult> OpenImagePickerAsync();
    }
}
