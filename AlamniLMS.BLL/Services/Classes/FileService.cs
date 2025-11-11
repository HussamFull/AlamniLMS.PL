using AlamniLMS.BLL.Services.Interfacese;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Classes
{
    public class FileService : IFileService
    {
        public async Task<string> UploadAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images" , fileName);


                // **الخطوة 1: تحديد مسار المجلد**
                //var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                // **الخطوة 2: إنشاء المجلد إذا لم يكن موجودًا**
                //if (!Directory.Exists(folderPath))
                //{
                //    Directory.CreateDirectory(folderPath);
                //}

                // **الخطوة 3: تحديد مسار الملف الكامل**
               // var filePath = Path.Combine(folderPath, fileName);

                using (var stream = File.Create(folderPath))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }

            throw new Exception("File is null or empty");


        }


    }
}
