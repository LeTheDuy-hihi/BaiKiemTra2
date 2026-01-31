using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Data;
using PickleballClubManagement.Models;

namespace PickleballClubManagement.Services
{
    public interface ITreasuryService
    {
        Task<decimal> GetTotalIncomeAsync();
        Task<decimal> GetTotalExpenseAsync();
        Task<decimal> GetTreasuryBalanceAsync();
        Task<List<Transaction>> GetTransactionsAsync(DateTime? startDate = null, DateTime? endDate = null, int? categoryId = null);
        Task<Transaction> CreateTransactionAsync(int categoryId, decimal amount, string description, string? createdById);
        Task<List<TransactionCategory>> GetCategoriesAsync();
        Task<TransactionCategory> CreateCategoryAsync(string name, TransactionType type, string? description);
        Task UpdateCategoryAsync(int categoryId, string name, string? description);
        Task DeleteCategoryAsync(int categoryId);
        Task<bool> IsTreasuryDeficitAsync();
    }

    public class TreasuryService : ITreasuryService
    {
        private readonly ApplicationDbContext _context;

        public TreasuryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetTotalIncomeAsync()
        {
            var incomeCategories = _context.TransactionCategories
                .Where(c => c.Type == TransactionType.Income)
                .Select(c => c.Id)
                .ToList();

            return await _context.Transactions
                .Where(t => incomeCategories.Contains(t.CategoryId))
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetTotalExpenseAsync()
        {
            var expenseCategories = _context.TransactionCategories
                .Where(c => c.Type == TransactionType.Expense)
                .Select(c => c.Id)
                .ToList();

            return await _context.Transactions
                .Where(t => expenseCategories.Contains(t.CategoryId))
                .SumAsync(t => Math.Abs(t.Amount));
        }

        public async Task<decimal> GetTreasuryBalanceAsync()
        {
            var income = await GetTotalIncomeAsync();
            var expense = await GetTotalExpenseAsync();
            return income - expense;
        }

        public async Task<List<Transaction>> GetTransactionsAsync(DateTime? startDate = null, DateTime? endDate = null, int? categoryId = null)
        {
            var query = _context.Transactions.Include(t => t.Category).AsQueryable();

            if (startDate.HasValue)
                query = query.Where(t => t.Date >= startDate);

            if (endDate.HasValue)
                query = query.Where(t => t.Date <= endDate);

            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId);

            return await query.OrderByDescending(t => t.Date).ToListAsync();
        }

        public async Task<Transaction> CreateTransactionAsync(int categoryId, decimal amount, string description, string? createdById)
        {
            var transaction = new Transaction
            {
                CategoryId = categoryId,
                Amount = amount,
                Description = description,
                CreatedById = createdById,
                Date = DateTime.Now,
                CreatedDate = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<List<TransactionCategory>> GetCategoriesAsync()
        {
            return await _context.TransactionCategories.ToListAsync();
        }

        public async Task<TransactionCategory> CreateCategoryAsync(string name, TransactionType type, string? description)
        {
            var category = new TransactionCategory
            {
                Name = name,
                Type = type,
                Description = description,
                CreatedDate = DateTime.Now
            };

            _context.TransactionCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task UpdateCategoryAsync(int categoryId, string name, string? description)
        {
            var category = await _context.TransactionCategories.FindAsync(categoryId);
            if (category != null)
            {
                category.Name = name;
                category.Description = description;
                category.ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.TransactionCategories.FindAsync(categoryId);
            if (category != null)
            {
                _context.TransactionCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsTreasuryDeficitAsync()
        {
            var balance = await GetTreasuryBalanceAsync();
            return balance < 0;
        }
    }
}
