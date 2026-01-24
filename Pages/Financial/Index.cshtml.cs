using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Data;
using PickleballClubManagement.Models;

namespace PickleballClubManagement.Pages.Financial
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Transaction> Transactions { get; set; } = new();
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal CurrentBalance { get; set; }

        public async Task OnGetAsync()
        {
            Transactions = await _context.Transactions
                .Include(t => t.Category)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            TotalIncome = Transactions
                .Where(t => t.Category?.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            TotalExpense = Transactions
                .Where(t => t.Category?.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            CurrentBalance = TotalIncome - TotalExpense;
        }
    }
}
