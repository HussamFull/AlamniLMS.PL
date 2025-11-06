using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Classes
{
    public class AuthenticationSerive : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IConfiguration _configuration;
        //private readonly IEmailSender _emailSender;
        //private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticationSerive(UserManager<ApplicationUser> userManager
      // IConfiguration configuration,
        //IEmailSender emailSender,
        //SignInManager<ApplicationUser> signInManager
       )
        {
            _userManager = userManager;
           // _configuration = configuration;
            //_emailSender = emailSender;
            //_signInManager = signInManager;
        }

        public async Task<UserResponse> LoginAsync(LoginRequest loginRequest)
        {
            // البحث عن المستخدم باستخدام البريد الإلكتروني
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
            {
                throw new Exception("Invalid Email or password");
            }

            var isPassValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!isPassValid)
            {
                throw new Exception("Invalid Email or password");
            }
            return new UserResponse()
            {
                Email = loginRequest.Email,
            };

            // التحقق من صحة كلمة المرور
            // var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);

            // التحقق من نتيجة تسجيل الدخول
            //if (result.Succeeded)
            //{
            //    // إذا نجحت العملية، قم بإنشاء وإرجاع التوكن
            //    return new UserResponse()
            //    {
            //        Token = await CreateTokenAsync(user),
            //    };
            //}
            //else if (result.IsLockedOut)
            //{
            //    // إذا كان المستخدم مقفولاً
            //    throw new Exception("User is locked out");
            //}
            //else if (result.IsNotAllowed)
            //{
            //    // إذا كان المستخدم غير مسموح له بالدخول (مثل عدم تأكيد البريد الإلكتروني)
            //    throw new Exception("Please Confirm your Email");
            //}
            //else
            //{
            //    // إذا فشلت العملية لأي سبب آخر (مثل كلمة مرور خاطئة)
            //    throw new Exception("Invalid Email or password");
            //}
        }


        public async Task<UserResponse> RegisterAsync(RegisterRequest registerRequest
           // , HttpRequest request
            )
        {
            var user = new ApplicationUser()
            {
                FullName = registerRequest.FullName,
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber,
                UserName = registerRequest.UserName,
            };

            var Result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (Result.Succeeded)
            {
                // Send Email Confirmation
                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var escapeToken = Uri.EscapeDataString(token);
                //var emailUrl = $"{request.Scheme}://{request.Host}/api/identity/account/confirmEmail?token={escapeToken}&userId={user.Id}";

                //await _userManager.AddToRoleAsync(user, "Customer");
                //await _emailSender.SendEmailAsync(
                //   user.Email,
                //    "Confirm your Email",
                //    $"<h1>Welcome {user.FullName}</h1>" +
                //    $"<p>Please confirm your email by clicking the link below:</p>" +
                //    $"<a href='{emailUrl}'> Confirm your email  </a>"
                //);
                return new UserResponse()
                {
                    Email = registerRequest.Email,
                   // Token = registerRequest.Email,
                };
            }
            else
            {
                throw new Exception($"{Result.Errors}");
            }
        }
    }
}
