using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Models;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ResetPasswordRequest = AlamniLMS.DAL.DTO.Requests.ResetPasswordRequest;

namespace AlamniLMS.BLL.Services.Classes
{
    public class AuthenticationSerive : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticationSerive(UserManager<ApplicationUser> userManager,
         IConfiguration configuration,
        IEmailSender emailSender,
        SignInManager<ApplicationUser> signInManager
       )
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }

        public async Task<UserResponse> LoginAsync(DAL.DTO.Requests.LoginRequest loginRequest)
        {
            // البحث عن المستخدم باستخدام البريد الإلكتروني
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
            {
                throw new Exception("Invalid Email or password");
            }
           

            //التحقق من صحة كلمة المرور
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);

           // التحقق من نتيجة تسجيل الدخول
            if (result.Succeeded)
            {
                // إذا نجحت العملية، قم بإنشاء وإرجاع التوكن
                return new UserResponse()
                {
                    Token = await CreateTokenAsync(user),
                };
            }
            else if (result.IsLockedOut)
            {
                // إذا كان المستخدم مقفولاً
                throw new Exception("User is locked out");
            }
            else if (result.IsNotAllowed)
            {
                // إذا كان المستخدم غير مسموح له بالدخول (مثل عدم تأكيد البريد الإلكتروني)
                throw new Exception("Please Confirm your Email");
            }
            else
            {
                // إذا فشلت العملية لأي سبب آخر (مثل كلمة مرور خاطئة)
                throw new Exception("Invalid Email or password");
            }
        }


        public async Task<UserResponse> RegisterAsync(DAL.DTO.Requests.RegisterRequest registerRequest  , HttpRequest request)
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
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var escapeToken = Uri.EscapeDataString(token);
                //var emailUrl = $"https://localhost:7227/api/identity/account/ConfirmEmail?token={escapeToken}&userId={user.Id}";

                var emailUrl = $"{request.Scheme}://{request.Host}/api/identity/account/confirmEmail?token={escapeToken}&userId={user.Id}";

                await _userManager.AddToRoleAsync(user, "Customer");
                await _emailSender.SendEmailAsync(
                   user.Email,
                    "Confirm your Email",
                    $"<h1>Welcome {user.FullName}</h1>" +
                    $"<p>Please confirm your email by clicking the link below:</p>" +
                    $"<a href='{emailUrl}'> Confirm your email  </a>"
                );
                return new UserResponse()
                {
                    Token = registerRequest.Email,
                };
            }
            else
            {
                throw new Exception($"{Result.Errors}");
            }



        }


        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName)
            };

            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("jwtOptions")["SecretKey"]));
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: Claims,
                expires: DateTime.Now.AddDays(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("Invalid User Id");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return ("Email Confirmation Succesfully");
            }
            return "Email Confirmation Failed";
        }

        public async Task<bool> ForgotPasswordAsync(DAL.DTO.Requests.ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("user not found  Email");
            }

            var random = new Random();
            var code = random.Next(1000, 9999).ToString();

            user.CodeRestPassword = code;
            user.PasswordRestCodeExpiry = DateTime.Now.AddMinutes(15);

            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(
                user.Email,
                "Reset Password Code",
                $"<h1>Password Reset Code</h1>" +
                $"<p>Your password reset code is: <strong>{code}</strong></p>" +
                $"<p>This code will expire in 15 minutes.</p>"
            );
            return true;

        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (user.CodeRestPassword != request.Code) return false;
            if (user.PasswordRestCodeExpiry < DateTime.UtcNow) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (result.Succeeded)
            {
                
                await _emailSender.SendEmailAsync(
                    request.Email,
                    "Password Reset Successful",
                    $"<h1>Password Reset Successful</h1>" +
                    $"<p>Your password has been successfully reset.</p>"
                );
                return true;
            }
            else
            {
                throw new Exception("Failed to reset password");
            }
        }

   
    }
}
