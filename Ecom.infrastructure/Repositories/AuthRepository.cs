

using Ecom.core.DTO;
using Ecom.core.Entites;
using Ecom.core.Interfaces;
using Ecom.core.services;
using Ecom.core.sharing;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailServices emailServices;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken generateToken;

        public AuthRepository(UserManager<AppUser> userManager, IEmailServices emailServices, SignInManager<AppUser> signInManager, IGenerateToken generateToken)
        {
            _userManager = userManager;
            this.emailServices = emailServices;
            this.signInManager = signInManager;
            this.generateToken = generateToken;
        }

        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                return null;
            }
            if (await _userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return "this UserName is already Registerd";
            }
            if (await _userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return "this Email is already Registerd";
            }

            AppUser user = new AppUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                return result.Errors.ToList()[0].Description;
            }

            try
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await SendEmailAsync(user.Email, token, "active", "ActiveEmail", "please activeyour email, click on button to active");
            }
            catch (Exception ex)
            {
                Console.WriteLine("REGISTER EMAIL FAILED: " + ex.Message);
            }

            return "Done";
        }

        public async Task SendEmailAsync(string email, string code, string component, string subject, string message)
        {
            var result = new EmailDTO(email,
                "saifaldenmohamed400@gmail.com",
                subject, EmailStringBody.send(email, code, component, message));
            await emailServices.SendEmail(result);
        }

        public async Task<string> LoginAsync(LoginDto login)
        {
            if (login == null)
            {
                return null;
            }
            var finduser = await _userManager.FindByEmailAsync(login.Email);
            if (finduser == null)
            {
                return "Please check your email and password, something went wrong";
            }
            if (!finduser.EmailConfirmed)
            {
                // CHANGED: لفّينا إرسال الإيميل في try/catch عشان فشل الـ SMTP ميبوظش اللوجن (500)
                try
                {
                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(finduser);
                    await SendEmailAsync(finduser.Email, token, "active", "ActiveEmail", "please activeyour email, click on button to active");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("LOGIN ACTIVATION EMAIL FAILED: " + ex.Message);
                }
                return "Please confirem your email first, we have send activat to your E-mail";
            }
            var result = await signInManager.CheckPasswordSignInAsync(finduser, login.Password, true);
            if (result.Succeeded)
            {
                return generateToken.GetAndCreateToken(finduser);
            }

            Console.WriteLine($"LOGIN FAILED: IsLockedOut={result.IsLockedOut}, IsNotAllowed={result.IsNotAllowed}");

            return "Please check your email and password, something went wrong";
        }

        public async Task<bool> SendEmailForForgetPasword(string email)
        {
            var findUser = await _userManager.FindByEmailAsync(email);
            if (findUser is null)
            {
                return false;
            }

            try
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(findUser);
                // CHANGED: الاسم بقى "reset-password" (حروف صغيرة وشَرطة) عشان يطابق روت Angular بالظبط
                await SendEmailAsync(findUser.Email, token, "reset-password", "Reset password", "click on button to Reset your password");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FORGET PASSWORD EMAIL FAILED: " + ex.Message);
                return false;
            }
        }

        public async Task<string> ResetPassword(ResetPasswordDTO resetPassword)
        {
            if (resetPassword == null ||
                string.IsNullOrWhiteSpace(resetPassword.Email) ||
                string.IsNullOrWhiteSpace(resetPassword.Token))
            {
                return "Invalid request";
            }

            var findUser = await _userManager.FindByEmailAsync(resetPassword.Email.Trim());
            if (findUser is null)
            {
                return "Invalid request";
            }

            var result = await _userManager.ResetPasswordAsync(findUser, resetPassword.Token, resetPassword.Password);
            if (result.Succeeded)
            {
                // CHANGED: كانت " Password Change Success" والكنترولر بيقارن بـ "done" فكان دايمًا 400
                return "Done";
            }

            Console.WriteLine("RESET FAILED: " +
                string.Join(" | ", result.Errors.Select(e => e.Code + ": " + e.Description)));
            return result.Errors.ToList()[0].Description;
        }

        public async Task<bool> ActiveAccount(ActiveAccountDTO accountDTO)
        {
            if (accountDTO == null ||
                string.IsNullOrWhiteSpace(accountDTO.Email) ||
                string.IsNullOrWhiteSpace(accountDTO.Token))
            {
                Console.WriteLine("ACTIVATE FAILED: email or token is empty");
                return false;
            }

            var findUser = await _userManager.FindByEmailAsync(accountDTO.Email.Trim());
            if (findUser is null)
            {
                Console.WriteLine("ACTIVATE FAILED: user not found");
                return false;
            }

            if (findUser.EmailConfirmed) return true;

            var result = await _userManager.ConfirmEmailAsync(findUser, accountDTO.Token);
            if (result.Succeeded) return true;

            Console.WriteLine("ACTIVATE FAILED: " +
                string.Join(" | ", result.Errors.Select(e => e.Code + ": " + e.Description)));
            return false;
        }
    }
}