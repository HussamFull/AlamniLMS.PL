using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Interfacese
{
    public interface IFileService
    {
        // إضافة معامل لتحديد المجلد الهدف
        Task<string> UploadAsync(IFormFile file, string subFolder);
      //  Task<string> UploadAsync(IFormFile file);
        Task<List<string>> UploadManyAsync(List<string> filePaths);

        void Delete(string fileName);
    }
}
