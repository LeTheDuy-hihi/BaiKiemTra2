using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PickleballClubManagement.Models;
using PickleballClubManagement.Services;
using System.Security.Claims;

namespace PickleballClubManagement.Pages.Financial
{
    [Authorize(Roles = "Admin,Treasurer")]
    public class TransactionsModel : PageModel
    {
        private readonly ITreasuryService _treasuryService;
        private readonly ILogger<TransactionsModel> _logger;

        public TransactionsModel(ITreasuryService treasuryService, ILogger<TransactionsModel> logger)
        {
            _treasuryService = treasuryService;
            _logger = logger;
        }

        public List<Transaction> Transactions { get; set; } = new();
        public List<TransactionCategory> Categories { get; set; } = new();
        public decimal TreasuryBalance { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public bool IsTreasuryDeficit { get; set; }
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        [BindProperty]
        public TransactionInput Input { get; set; } = new();

        public class TransactionInput
        {
            public int CategoryId { get; set; }
            public decimal Amount { get; set; }
            public string Description { get; set; } = string.Empty;
            public DateTime Date { get; set; } = DateTime.Now;
        }

        public async Task OnGetAsync(DateTime? startDate, DateTime? endDate, int? categoryId)
        {
            try
            {
                Categories = await _treasuryService.GetCategoriesAsync();
                Transactions = await _treasuryService.GetTransactionsAsync(startDate, endDate, categoryId);
                TreasuryBalance = await _treasuryService.GetTreasuryBalanceAsync();
                TotalIncome = await _treasuryService.GetTotalIncomeAsync();
                TotalExpense = await _treasuryService.GetTotalExpenseAsync();
                IsTreasuryDeficit = await _treasuryService.IsTreasuryDeficitAsync();

                if (TempData["SuccessMessage"] != null)
                    SuccessMessage = TempData["SuccessMessage"]?.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi tải giao dịch: {ex.Message}");
                ErrorMessage = "Lỗi tải dữ liệu";
            }
        }

        public async Task<IActionResult> OnPostAddTransactionAsync()
        {
            try
            {
                if (Input.CategoryId <= 0 || Input.Amount <= 0 || string.IsNullOrWhiteSpace(Input.Description))
                {
                    ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin");
                    return await OnGetAsyncPrivate();
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _treasuryService.CreateTransactionAsync(Input.CategoryId, Input.Amount, Input.Description, userId);
                TempData["SuccessMessage"] = "Thêm giao dịch thành công";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi thêm giao dịch: {ex.Message}");
                ModelState.AddModelError("", "Lỗi thêm giao dịch");
                return await OnGetAsyncPrivate();
            }
        }

        private async Task<PageResult> OnGetAsyncPrivate()
        {
            await OnGetAsync(null, null, null);
            return Page();
        }
    }
}
