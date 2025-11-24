using AlamniLMS.DAL.DTO.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Interfacese
{
    public interface ICertificateService
    {
        Task<CertificateResponse> GenerateCertificateAsync(string userId, int courseId);
        Task<CertificateResponse> GetCertificateAsync(int certificateId, HttpRequest request);
    }
}
