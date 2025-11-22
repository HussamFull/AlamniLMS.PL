using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Interfacese
{
    public interface IEnrollmentService
    {
        Task<bool> AddToEnrollmentAsync(EnrollmentRequest request, string UserId);

        Task<EnrollmentSummaryResponse> EnrollmentSummaryResponseAsync(string UserId);

        Task<bool> ClearEnrollmentAsync(string UserId);
    }
}
