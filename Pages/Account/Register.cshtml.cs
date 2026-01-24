using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PickleballClubManagement.Services;
using System.ComponentModel.DataAnnotations;

namespace PickleballClubManagement.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMemberService _memberService;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IMemberService memberService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _memberService = memberService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Vui lòng nhập họ tên")]
            [StringLength(100, ErrorMessage = "Họ tên không được quá 100 ký tự")]
            [Display(Name = "Họ và Tên")]
            public string FullName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Vui lòng nhập email")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ")]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
            [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Xác nhận mật khẩu")]
            [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);

                try
                {
                    if (result.Succeeded)
                    {
                        // Add to Member role
                        var roleResult = await _userManager.AddToRoleAsync(user, "Member");
                        if (!roleResult.Succeeded)
                        {
                            foreach (var error in roleResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, $"Lỗi Role: {error.Description}");
                            }
                            // Delete user if role assignment fails to keep clean state
                            await _userManager.DeleteAsync(user);
                            return Page();
                        }

                        // Create member record with Rank 1.0
                        await _memberService.CreateMemberAsync(user.Id, Input.FullName);

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Lỗi hệ thống: {ex.Message}");
                    // Clean up if user was created but member creation failed
                    var createdUser = await _userManager.FindByEmailAsync(Input.Email);
                    if (createdUser != null)
                    {
                         await _userManager.DeleteAsync(createdUser);
                    }
                }
            }

            return Page();
        }
    }
}
