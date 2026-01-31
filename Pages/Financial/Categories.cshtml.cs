using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PickleballClubManagement.Models;
using PickleballClubManagement.Services;

namespace PickleballClubManagement.Pages.Financial
{
    [Authorize(Roles = "Admin,Treasurer")]
    public class CategoriesModel : PageModel
    {
        private readonly ITreasuryService _treasuryService;
        private readonly ILogger<CategoriesModel> _logger;

        public CategoriesModel(ITreasuryService treasuryService, ILogger<CategoriesModel> logger)
        {
            _treasuryService = treasuryService;
            _logger = logger;
        }

        public List<TransactionCategory> Categories { get; set; } = new();
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        [BindProperty]
        public TransactionCategory InputCategory { get; set; } = new();

        [BindProperty]
        public int? EditingCategoryId { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                Categories = await _treasuryService.GetCategoriesAsync();
                if (TempData["SuccessMessage"] != null)
                    SuccessMessage = TempData["SuccessMessage"]?.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi tải danh mục: {ex.Message}");
                ErrorMessage = "Lỗi tải dữ liệu";
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(InputCategory.Name))
                {
                    ModelState.AddModelError("InputCategory.Name", "Tên danh mục không được trống");
                    Categories = await _treasuryService.GetCategoriesAsync();
                    return Page();
                }

                await _treasuryService.CreateCategoryAsync(InputCategory.Name, InputCategory.Type, InputCategory.Description);
                TempData["SuccessMessage"] = "Thêm danh mục thành công";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi thêm danh mục: {ex.Message}");
                ModelState.AddModelError("", "Lỗi thêm danh mục");
                Categories = await _treasuryService.GetCategoriesAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            try
            {
                if (!EditingCategoryId.HasValue)
                {
                    ModelState.AddModelError("", "Danh mục không tồn tại");
                    Categories = await _treasuryService.GetCategoriesAsync();
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(InputCategory.Name))
                {
                    ModelState.AddModelError("InputCategory.Name", "Tên danh mục không được trống");
                    Categories = await _treasuryService.GetCategoriesAsync();
                    return Page();
                }

                await _treasuryService.UpdateCategoryAsync(EditingCategoryId.Value, InputCategory.Name, InputCategory.Description);
                TempData["SuccessMessage"] = "Cập nhật danh mục thành công";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi cập nhật danh mục: {ex.Message}");
                ModelState.AddModelError("", "Lỗi cập nhật danh mục");
                Categories = await _treasuryService.GetCategoriesAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _treasuryService.DeleteCategoryAsync(id);
                TempData["SuccessMessage"] = "Xóa danh mục thành công";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi xóa danh mục: {ex.Message}");
                TempData["ErrorMessage"] = "Lỗi xóa danh mục";
                return RedirectToPage();
            }
        }
    }
}
