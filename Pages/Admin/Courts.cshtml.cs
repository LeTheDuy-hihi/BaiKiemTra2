using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PickleballClubManagement.Models;
using PickleballClubManagement.Services;

namespace PickleballClubManagement.Pages.Admin
{
    [Authorize(Roles = "Admin,Treasurer")]
    public class CourtsModel : PageModel
    {
        private readonly ICourtService _courtService;
        private readonly ILogger<CourtsModel> _logger;

        public CourtsModel(ICourtService courtService, ILogger<CourtsModel> logger)
        {
            _courtService = courtService;
            _logger = logger;
        }

        public List<Court> Courts { get; set; } = new();
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        [BindProperty]
        public Court InputCourt { get; set; } = new();

        [BindProperty]
        public int? EditingCourtId { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                Courts = await _courtService.GetAllCourtsAsync();
                if (TempData["SuccessMessage"] != null)
                    SuccessMessage = TempData["SuccessMessage"]?.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi tải danh sách sân: {ex.Message}");
                ErrorMessage = "Lỗi tải danh sách sân";
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(InputCourt.Name))
                {
                    ModelState.AddModelError("InputCourt.Name", "Tên sân không được trống");
                    Courts = await _courtService.GetAllCourtsAsync();
                    return Page();
                }

                await _courtService.CreateCourtAsync(InputCourt.Name, InputCourt.Description);
                TempData["SuccessMessage"] = "Thêm sân mới thành công";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi thêm sân: {ex.Message}");
                ModelState.AddModelError("", "Lỗi thêm sân");
                Courts = await _courtService.GetAllCourtsAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            try
            {
                if (!EditingCourtId.HasValue)
                {
                    ModelState.AddModelError("", "Sân không tồn tại");
                    Courts = await _courtService.GetAllCourtsAsync();
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(InputCourt.Name))
                {
                    ModelState.AddModelError("InputCourt.Name", "Tên sân không được trống");
                    Courts = await _courtService.GetAllCourtsAsync();
                    return Page();
                }

                await _courtService.UpdateCourtAsync(EditingCourtId.Value, InputCourt.Name, InputCourt.IsActive, InputCourt.Description);
                TempData["SuccessMessage"] = "Cập nhật sân thành công";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi cập nhật sân: {ex.Message}");
                ModelState.AddModelError("", "Lỗi cập nhật sân");
                Courts = await _courtService.GetAllCourtsAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _courtService.DeleteCourtAsync(id);
                TempData["SuccessMessage"] = "Xóa sân thành công";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi xóa sân: {ex.Message}");
                TempData["ErrorMessage"] = "Lỗi xóa sân";
                return RedirectToPage();
            }
        }
    }
}
