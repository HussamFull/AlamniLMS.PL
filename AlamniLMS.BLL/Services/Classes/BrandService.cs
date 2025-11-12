using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Models;
using AlamniLMS.DAL.Repository.Interfaces;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Classes
{
    public class BrandService : GenericService<BrandRequest, BrandResponses, Brand>,IBrandService
    {
        private readonly IBrandRepository _repository;
        private readonly IFileService _fileService;

        public BrandService(IBrandRepository repository,
         IFileService fileService) : base(repository)
        {
            _repository = repository;
            _fileService = fileService;
        }


        public async Task<int> CreateFile(BrandRequest request)
        {

            var entity = request.Adapt<Brand>();
            entity.CreatedAt = DateTime.Now;

            if (request.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage, "images");
                entity.MainImage = imagePath;
            }

            return _repository.Add(entity);
        }

        public async Task<int> UpdateFile(int id, BrandRequest request)
        {
            // 1. البحث عن البراند الموجود في قاعدة البيانات باستخدام الـ ID
            var existingBrand = await _repository.GetAsync(id);

            if (existingBrand == null)
            {
                return 0; // إرجاع 0 إذا لم يتم العثور على البراند
            }

            // 2. تحديث خصائص البراند بالبيانات الجديدة من الـ request
            request.Adapt(existingBrand);
            existingBrand.UpdatedAt = DateTime.Now;

            // 3. التحقق مما إذا كان هناك ملف صورة جديد تم رفعه
            if (request.MainImage != null)
            {
                // 4. حذف الصورة القديمة إذا كانت موجودة
                if (!string.IsNullOrEmpty(existingBrand.MainImage))
                {
                    _fileService.Delete(existingBrand.MainImage);
                }

                // 5. رفع الصورة الجديدة وتحديث مسارها في البراند
                var newImagePath = await _fileService.UploadAsync(request.MainImage, "images");
                existingBrand.MainImage = newImagePath;
            }

            // 6. حفظ التغييرات في قاعدة البيانات
            return _repository.Update(existingBrand);
        }


    }
}
