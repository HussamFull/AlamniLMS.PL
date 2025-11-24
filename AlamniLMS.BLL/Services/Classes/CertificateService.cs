using AlamniLMS.BLL.Services.Documents;
using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Models;
using AlamniLMS.DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AlamniLMS.BLL.Services.Classes
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _certificateRepo;
        private readonly ICourseRepository _courseRepo;
        private readonly IUserRepository _userRepo;
        private readonly IWebHostEnvironment _env;

        public CertificateService(
            ICertificateRepository certificateRepo,
            ICourseRepository courseRepo,
            IUserRepository userRepo,
            IWebHostEnvironment env)
        {
            _certificateRepo = certificateRepo;
            _courseRepo = courseRepo;
            _userRepo = userRepo;
            _env = env;

            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<CertificateResponse> GenerateCertificateAsync(string userId, int courseId)
        {
            var course = _courseRepo.GetById(courseId);
            var user = await _userRepo.GetByIdAsync(userId);

            string certificateNumber = $"CRT-{courseId}-{Guid.NewGuid().ToString().Substring(0, 8)}";

            string folder = Path.Combine(_env.WebRootPath, "certificates");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string fileName = $"{certificateNumber}.pdf";
            string filePath = Path.Combine(folder, fileName);

            var doc = new CertificateDocument(user.FullName, course.Title, certificateNumber);
            doc.GeneratePdf(filePath);

            var certificate = new Certificate
            {
                CertificateNumber = certificateNumber,
                CourseId = courseId,
                UserId = userId,
                IssuedAt = DateTime.UtcNow,
                FilePath = $"/certificates/{fileName}"
            };

            _certificateRepo.Add(certificate);

            return new CertificateResponse
            {
                Id = certificate.Id,
                CertificateNumber = certificate.CertificateNumber,
                UserId = userId,
                CourseId = courseId,
                CourseTitle = course.Title,
                FilePath = certificate.FilePath,
                IssuedAt = certificate.IssuedAt.ToString("yyyy-MM-dd")
            };
        }

        public async Task<CertificateResponse> GetCertificateAsync(int certificateId, HttpRequest request)
        {
            var cert =  _certificateRepo.GetById(certificateId);
            if (cert == null) return null;

            // **الخطوة الرئيسية: بناء الرابط الكامل**
            // (مثال: https://localhost:7122/certificates/CRT-123-ABCD.pdf)
            string fullUrl = $"{request.Scheme}://{request.Host}{cert.FilePath}";

            return new CertificateResponse
            {
                Id = cert.Id,
                CertificateNumber = cert.CertificateNumber,
                UserId = cert.UserId,
                CourseId = cert.CourseId,
                FilePath = fullUrl,
                IssuedAt = cert.IssuedAt.ToString("yyyy-MM-dd")
            };
        }

        public byte[] GenerateCertificate(string studentName, string courseTitle, string certificateNumber)
        {
            var document = new CertificateDocument(studentName, courseTitle, certificateNumber);
            byte[] pdfBytes = document.GeneratePdf();
            return pdfBytes;
        }


        public string IssueCertificate(string studentName, string courseTitle, string certificateNumber)
        {
            // توليد PDF
            var pdfBytes = GenerateCertificate(studentName, courseTitle, certificateNumber);

            // مسار حفظ الشهادة على السيرفر
            string certificatesFolder = Path.Combine("wwwroot", "certificates");
            if (!Directory.Exists(certificatesFolder))
            {
                Directory.CreateDirectory(certificatesFolder); // إنشاء المجلد إذا لم يكن موجوداً
            }

            string filePath = Path.Combine(certificatesFolder, $"{certificateNumber}.pdf");
            File.WriteAllBytes(filePath, pdfBytes);

            // إعادة رابط الشهادة للوصول إليها عبر المتصفح
            string certificateUrl = $"/certificates/{certificateNumber}.pdf";
            return certificateUrl;
        }




    }
}
