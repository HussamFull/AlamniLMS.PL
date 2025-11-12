using AlamniLMS.BLL.Services.Interfacese;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Classes
{
    // في AlamniLMS.BLL.Services.Classes/FileService.cs

    public class FileService : IFileService
    {
        // ⚠️ تم تغيير التوقيع لقبول 'subFolder'
        public async Task<string> UploadAsync(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("File is null or empty");
            }

            // **1. إنشاء اسم ملف فريد**
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // **2. تحديد مسار المجلد (wwwroot/[subFolder])**
            // سيصبح المسار: D:\...\wwwroot\images أو D:\...\wwwroot\videos
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", subFolder);

            // **3. التأكد من وجود المجلد وإنشاءه إذا لم يكن موجوداً**
            // هذا يضمن إنشاء مجلد 'videos' إذا تم تمرير "videos"
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // **4. تحديد المسار الكامل للملف**
            var filePath = Path.Combine(folderPath, fileName);

            // **5. حفظ الملف**
            using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            // **6. إرجاع اسم الملف**
            return fileName;
        }
    
       



    //public class FileService : IFileService
    //{
    //    public async Task<string> UploadAsync(IFormFile file)
    //    {
    //        if (file != null && file.Length > 0)
    //        {
    //            // 1. إنشاء اسم ملف فريد
    //            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

    //            // 2. تحديد مسار المجلد (wwwroot/images)
    //            // **مهم:** يجب أن يشير المسار إلى المجلد فقط، وليس المجلد واسم الملف.
    //            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

    //            // 3. التأكد من وجود المجلد وإنشاءه إذا لم يكن موجوداً
    //            if (!Directory.Exists(folderPath))
    //            {
    //                Directory.CreateDirectory(folderPath);
    //            }

    //            // 4. تحديد المسار الكامل للملف (المجلد + اسم الملف)
    //            var filePath = Path.Combine(folderPath, fileName);

    //            // 5. حفظ الملف باستخدام المسار الكامل
    //            using (var stream = File.Create(filePath)) // هنا يتم استخدام المسار الكامل filePath
    //            {
    //                await file.CopyToAsync(stream);
    //            }

    //            // 6. إرجاع اسم الملف فقط لاستخدامه في قاعدة البيانات
    //            return fileName;
    //        }

    //        throw new Exception("File is null or empty");
    //    }

    //}

     public void Delete(string fileName)
        {
            // **الخطوة 1: تحديد مسار المجلد**
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            // **الخطوة 2: تحديد مسار الملف الكامل**
            var filePath = Path.Combine(folderPath, fileName);

            // **الخطوة 3: التحقق من وجود الملف وحذفه**
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public async Task<List<string>> UploadManyAsync(List<IFormFile> files)
        {
            var fileNames = new List<string>();
            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // **الخطوة 1: تحديد مسار المجلد**
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);



                    using (var stream = File.Create(folderPath))
                    {
                        await file.CopyToAsync(stream);
                    }

                    fileNames.Add(fileName);
                }
            }
            return fileNames;
        }

        public Task<List<string>> UploadManyAsync(List<string> filePaths)
        {
            throw new NotImplementedException();
        }
    }
}