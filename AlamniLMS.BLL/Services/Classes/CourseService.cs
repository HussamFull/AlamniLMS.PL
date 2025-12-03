using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Models;
using AlamniLMS.DAL.Repository.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Http;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Classes
{
    public class CourseService : GenericService<CourseRequest, CourseResponse, Course>, ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly IFileService _fileService;

        public CourseService(ICourseRepository repository, IFileService fileService) : base(repository)
        {
            _repository = repository;
            _fileService = fileService;


        }

        public async Task<int> CreateCourse(CourseRequest request)
        {
            var entity = request.Adapt<Course>();
            entity.CreatedAt = DateTime.UtcNow;

            if (request.ThumbnailPath != null)
            {
                var imagePath = await _fileService.UploadAsync(request.ThumbnailPath, "images");
                entity.ThumbnailPath = imagePath;
            }
            if (request.SubImages != null)
            {
                var subImageFolderName = "subimage";
                var SubImagesPaths = await _fileService.UploadManyAsync(request.SubImages, subImageFolderName);
                entity.SubImages = SubImagesPaths.Select(img => new CourseImage { ImageName = img }).ToList();
            }
            return _repository.Add(entity);
        }

        //public async Task<List<CourseResponse>> GetAllCourses(HttpRequest request, int pageNumber = 1, int pageSize = 1, bool onlayActive = false)


        //{
        //    var courses = _repository.GetAllCoursesWithImage();

        //    if (onlayActive)
        //    {
        //        courses = courses.Where(p => p.Status == Status.Active).ToList();
        //    }
        //    var pagedCourses = courses.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        //    return pagedCourses.Select(p => new CourseResponse
        //    {
        //        Id = p.Id,
        //        Title = p.Title,
        //        FullDescription = p.FullDescription,
        //        //Quantity = p.Quantity,
        //        ThumbnailPathUrl = $"{request.Scheme}://{request.Host}/Images/{p.ThumbnailPath}",
        //        SubImagesUrls = p.SubImages.Select(img => $"{request.Scheme}://{request.Host}/Images/SubImage/{img.ImageName}").ToList(),
        //        Reviews = p.Reviews.Select(r => new ReviewResponse
        //        {
        //            Id = r.Id,
        //            FullName = r.User.FullName,
        //            Comment = r.Comment,
        //            Rate = r.Rate
        //        }).ToList()

        //    }).ToList();
        //}


        public async Task<List<CourseResponse>> GetAllCourses(HttpRequest request, int pageNumber = 1, int pageSize = 5, bool onlayActive = false)
        {
            // 1. جلب الكورسات مع البيانات المرتبطة (نفترض أن GetAllCoursesWithImage هي دالة متزامنة)
            // إذا كنت تستخدم Entity Framework Core، يفضل استخدام نسخ غير متزامنة (مثل .ToListAsync()) لتحسين الأداء
            var courses = _repository.GetAllCoursesWithImage(); // <--- نفترض أنها تُرجع List<Course>

            // 2. تطبيق فلتر الحالة (Status)
            if (onlayActive)
            {
                // استخدام Where و ToList() هنا صحيح
                courses = courses.Where(p => p.Status == Status.Active).ToList();
            }

            // 3. تطبيق التقسيم (Pagination)
            var pagedCourses = courses
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // 4. بناء الروابط وتحويلها إلى DTO
            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}"; // بناء المسار الأساسي (http://localhost:port)

            return pagedCourses.Select(p => new CourseResponse
            {
                Id = p.Id,
                Title = p.Title,
                FullDescription = p.FullDescription,

                // 4.1. رابط الصورة المصغرة (Thumbnail)
                // يجب أن نستخدم المسار الصحيح للمجلد، وهو عادةً /images (بناءً على الكود الذي أرسلته سابقاً)
                ThumbnailPathUrl = !string.IsNullOrEmpty(p.ThumbnailPath) ?
                                  // $"{baseUrl}/images/{p.ThumbnailPath}" :
                                  $"{baseUrl}/images/{p.ThumbnailPath}".Replace("//", "/") :
                                   null, // يُفضل إرجاع null أو رابط افتراضي إذا كانت الصورة غير موجودة

                // 4.2. روابط الصور الفرعية (SubImages)
                SubImagesUrls = p.SubImages.Select(img =>
                {
                    // المسار المتفق عليه هو /images/SubImage
                    return !string.IsNullOrEmpty(img.ImageName) ?
                         //  $"{baseUrl}/images/SubImage/{img.ImageName}" :
                         $"{baseUrl}/images/SubImage/{img.ImageName}".Replace("//", "/") :
                           null;
                }).Where(url => url != null).ToList()!, // تصفية أي روابط فارغة

                // 4.3. المراجعات (Reviews)
                Reviews = p.Reviews.Select(r => new ReviewResponse
                {
                    Id = r.Id,
                    FullName = r.User.FullName, // يجب التأكد من تحميل User في Repository
                    Comment = r.Comment,
                    Rate = r.Rate
                }).ToList()

            }).ToList();
        }






        public async Task<int> UpdateFile(int id, CourseRequest request)
        {
            // 1. البحث عن الكورس الموجود في قاعدة البيانات مع تحميل الصور الفرعية
            // **يجب تحديث GetAsync في Repository ليجلب SubImages أيضًا**
            var existingCourse = await _repository.GetCourseWithSubImagesAsync(id); // <--- سنضيف هذه الدالة في Repository

            if (existingCourse == null)
            {
                return 0; // إرجاع 0 إذا لم يتم العثور على الكورس
            }

            // 2. تحديث الخصائص الأخرى بالبيانات الجديدة من الـ request
            // **مهم:** يجب استبعاد خصائص الملفات التي من نوع IFormFile من عملية النسخ التلقائي (Adapt)
            // لا يمكن استبعادها مباشرة في Mapster بدون إعدادات إضافية، لذا سنقوم بالنسخ يدويًا لأغلب الخصائص لتجنب المشكلة

            // **تحديث الخصائص يدويًا أو باستخدام Mapster مع التجاهل (إذا كان ممكناً لديك):**
            existingCourse.Title = request.Title;
            existingCourse.ShortDescription = request.ShortDescription;
            existingCourse.FullDescription = request.FullDescription;
            existingCourse.Price = request.Price;
            existingCourse.CategoryId = request.CategoryId;
            existingCourse.IsPublished = request.IsPublished;
            existingCourse.Slug = request.Slug;
            existingCourse.UpdatedAt = DateTime.Now;


            // 3. تحديث الصورة المصغرة (ThumbnailPath)
            if (request.ThumbnailPath != null)
            {
                // حذف الصورة القديمة
                if (!string.IsNullOrEmpty(existingCourse.ThumbnailPath))
                {
                    _fileService.Delete(existingCourse.ThumbnailPath);
                }

                // رفع الصورة الجديدة وتحديث مسارها
                var newImagePath = await _fileService.UploadAsync(request.ThumbnailPath, "images");
                existingCourse.ThumbnailPath = newImagePath;
            }

            // 4. معالجة الصور الفرعية (SubImages)
            if (request.SubImages != null && request.SubImages.Count > 0)
            {
                // 4.1. حذف الصور الفرعية القديمة من التخزين (اختياري، لكن مستحسن)
                foreach (var subImage in existingCourse.SubImages)
                {
                    // يجب تحديث دالة Delete في FileService لقبول اسم المجلد الفرعي لتحديد المسار الصحيح
                    // حاليًا دالة Delete لديك تستخدم "images" فقط، وقد تحتاج إلى تعديلها لتقبل مجلد فرعي
                    // أو تعديلها لـ "images/SubImage"
                    // _fileService.DeleteSubImage(subImage.ImageName); // <--- وظيفة جديدة افتراضية

                    // بما أن الكود الخاص بك يضعها في "SubImage" داخل "images"، يجب أن تكون الدالة Delete قادرة على التعامل مع ذلك.
                    // مؤقتًا، لن نحذف الملفات لتجنب تعقيد المنطق إذا كانت دالة Delete غير جاهزة لذلك.
                    // **ملاحظة: للحذف الصحيح يجب تعديل دالة Delete في FileService لحذف من المجلد "SubImage"**
                }

                // 4.2. إزالة كل الصور الفرعية القديمة من الكيان
                existingCourse.SubImages.Clear();

                // 4.3. رفع الصور الجديدة وإضافتها
                var SubImagesPaths = await _fileService.UploadManyAsync(request.SubImages, "SubImage");
                foreach (var imgPath in SubImagesPaths)
                {
                    existingCourse.SubImages.Add(new CourseImage { ImageName = imgPath });
                }
            }
            // ملاحظة: إذا كان request.SubImages هو NULL، هذا يعني أن المستخدم لم يقم بتحميل صور جديدة، ويجب الاحتفاظ بالصور القديمة (وهذا هو الوضع الحالي).

            // 5. حفظ التغييرات في قاعدة البيانات
            return  _repository.Update(existingCourse); // استخدام UpdateAsync (مع ملاحظة أن GenericRepository.cs:line 46 تستخدم SaveChanges، سنبقيها كما هي لديك)
        }


        //public async Task<int> UpdateFile(int id, CourseRequest request)
        //{
        //    // 1. البحث عن البراند الموجود في قاعدة البيانات باستخدام الـ ID
        //    var existingCourse = await _repository.GetAsync(id);

        //    if (existingCourse == null)
        //    {
        //        return 0; // إرجاع 0 إذا لم يتم العثور على البراند
        //    }

        //    // 2. تحديث خصائص البراند بالبيانات الجديدة من الـ request
        //    request.Adapt(existingCourse);
        //    existingCourse.UpdatedAt = DateTime.Now;

        //    // 3. التحقق مما إذا كان هناك ملف صورة جديد تم رفعه
        //    if (request.ThumbnailPath != null)
        //    {
        //        // 4. حذف الصورة القديمة إذا كانت موجودة
        //        if (!string.IsNullOrEmpty(existingCourse.ThumbnailPath))
        //        {
        //            _fileService.Delete(existingCourse.ThumbnailPath);
        //        }

        //        // 5. رفع الصورة الجديدة وتحديث مسارها في البراند
        //        var newImagePath = await _fileService.UploadAsync(request.ThumbnailPath, "images");
        //        existingCourse.ThumbnailPath = newImagePath;
        //    }

        //    // 6. حفظ التغييرات في قاعدة البيانات
        //    return _repository.Update(existingCourse);
        //}


        public async Task<int> DeleteCourse(int id)
        {
            // 1. جلب الكورس المراد حذفه مع مسارات الصور الفرعية
            // سنستخدم نفس الدالة التي اقترحناها للتعديل:
            var courseToDelete = await _repository.GetCourseWithSubImagesAsync(id); // <--- يجب أن تجلب SubImages

            if (courseToDelete == null)
            {
                return 0;
            }

            // 2. حذف الملفات الفعلية من الخادم

            // 2.1. حذف الصورة المصغرة (Thumbnail)
            if (!string.IsNullOrEmpty(courseToDelete.ThumbnailPath))
            {
                // مجلد الصورة المصغرة هو "images"
                // **ملاحظة:** يجب أن تكون دالة الحذف في FileService قادرة على التعامل مع subFolder
                _fileService.Delete(courseToDelete.ThumbnailPath);
            }

            // 2.2. حذف الصور الفرعية (SubImages)
            foreach (var subImage in courseToDelete.SubImages)
            {
                // مجلد الصور الفرعية هو "SubImage" (داخل Images)
                // **ملاحظة:** سنقوم بتعديل دالة الحذف في FileService لدعم هذا.
                _fileService.DeleteSubImage(subImage.ImageName); // <--- سنضيف هذه الدالة لاحقاً
            }

            // 3. حذف الكورس من قاعدة البيانات
            // بما أننا استخدمنا GetCourseWithSubImagesAsync مع Include، فإن Entity Framework Core
            // سيقوم بحذف CourseImages المرتبطة عند حذف الكورس (Cascading Delete).
            return _repository.Remove(courseToDelete);
        }

        public async Task<CourseResponse?> GetCourseByIdWithImages(int id, HttpRequest request)
        {
            // استخدام دالة Repository الجديدة التي تجلب كل شيء (بما في ذلك SubImages)
            var courseEntity = await _repository.GetCourseWithImagesAsync(id); // ✅ استخدام الدالة الجديدة

            if (courseEntity == null)
            {
                return null;
            }

            // التحويل إلى DTO وبناء الروابط (باستخدام المنطق الذي أنشأناه سابقاً)
            var response = courseEntity.Adapt<CourseResponse>();
            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

            // بناء رابط Thumbnail
            if (!string.IsNullOrEmpty(courseEntity.ThumbnailPath))
            {
                response.ThumbnailPathUrl = $"{baseUrl}/images/{courseEntity.ThumbnailPath}".Replace("//", "/");
                //response.ThumbnailPathUrl = $"{baseUrl}/images/{courseEntity.ThumbnailPath}";
            }

            // بناء روابط SubImages (الآن courseEntity.SubImages لن تكون فارغة)
            if (courseEntity.SubImages != null)
            {
                var subImageFolderName = "subimage"; // ✅ توحيد الاسم هنا أيضًا
                response.SubImagesUrls = courseEntity.SubImages.Select(img =>
                {
                    return !string.IsNullOrEmpty(img.ImageName) ?
                    $"{baseUrl}/images/{subImageFolderName}/{img.ImageName}".Replace("//", "/") :
                           // $"{baseUrl}/images/{subImageFolderName}/{img.ImageName}"  :
                           null;
                }).Where(url => url != null).ToList()!;
            }

            // بناء المراجعات (Reviews)
            if (courseEntity.Reviews != null)
            {
                response.Reviews = courseEntity.Reviews.Select(r => new ReviewResponse
                {
                    Id = r.Id,
                    FullName = r.User.FullName,
                    Comment = r.Comment,
                    Rate = r.Rate
                }).ToList();
            }

            return response;
        }
    }
}
