using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.DTO.Responses
{
    public class EnrollmentResponse
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice => Price * Count;
        [JsonIgnore]
        public string? ThumbnailPath { get; set; }
        public string ThumbnailPathUrl => $"https://localhost:7122/images/{ThumbnailPath}";

        /// </summary>
        public DateTime EnrolledAt { get; set; }
    }
}
