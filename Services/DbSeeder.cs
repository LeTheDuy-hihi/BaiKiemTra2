using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Data;
using PickleballClubManagement.Models;

namespace PickleballClubManagement.Services
{
    public class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var memberService = serviceProvider.GetRequiredService<IMemberService>();

            // Create roles if they don't exist
            string[] roleNames = { "Admin", "Member" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create default admin user
            var adminEmail = "admin@pickleballclub.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    await memberService.CreateMemberAsync(adminUser.Id, "Admin - Trọng Tài");
                }
            }

            // Check if we already have sample data
            if (await context.Members.CountAsync() > 1)
            {
                return; // Data already seeded
            }

            // Create sample members
            var sampleMembers = new[]
            {
                new { Email = "nguyenvana@example.com", Name = "Nguyễn Văn A", Rank = 1.5 },
                new { Email = "tranthib@example.com", Name = "Trần Thị B", Rank = 2.1 },
                new { Email = "levanc@example.com", Name = "Lê Văn C", Rank = 1.6 },
                new { Email = "phamvand@example.com", Name = "Phạm Văn D", Rank = 2.0 },
                new { Email = "hoangthie@example.com", Name = "Hoàng Thị E", Rank = 1.9 },
                new { Email = "vuvanf@example.com", Name = "Vũ Văn F", Rank = 1.8 },
                new { Email = "dothig@example.com", Name = "Đỗ Thị G", Rank = 2.2 },
                new { Email = "buivanh@example.com", Name = "Bùi Văn H", Rank = 1.7 }
            };

            var createdMembers = new List<Member>();

            foreach (var sample in sampleMembers)
            {
                var user = await userManager.FindByEmailAsync(sample.Email);
                if (user == null)
                {
                    user = new IdentityUser
                    {
                        UserName = sample.Email,
                        Email = sample.Email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "Member@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Member");
                        var member = await memberService.CreateMemberAsync(user.Id, sample.Name);
                        member.RankLevel = sample.Rank;
                        await context.SaveChangesAsync();
                        createdMembers.Add(member);
                    }
                }
            }

            // Create sample challenges
            if (createdMembers.Count >= 4)
            {
                var challenges = new[]
                {
                    new Challenge
                    {
                        CreatorId = createdMembers[0].Id,
                        Title = "Thách đấu Đơn - Ai dám nhận?",
                        Description = "Tôi chấp nửa trái, ai dám không? Đánh sân 2, chiều nay 5h.",
                        Status = ChallengeStatus.Open,
                        CreatedAt = DateTime.Now.AddHours(-2)
                    },
                    new Challenge
                    {
                        CreatorId = createdMembers[1].Id,
                        Title = "Đôi Nam - Quyết chiến cuối tuần",
                        Description = "Tìm cặp đôi mạnh để thi đấu vào thứ 7 này. Ai tự tin hãy nhận kèo!",
                        Status = ChallengeStatus.Accepted,
                        CreatedAt = DateTime.Now.AddDays(-1)
                    },
                    new Challenge
                    {
                        CreatorId = createdMembers[2].Id,
                        Title = "Giao lưu Đơn - Rank 2.0+",
                        Description = "Chỉ nhận thách đấu từ những người có Rank từ 2.0 trở lên.",
                        Status = ChallengeStatus.Completed,
                        CreatedAt = DateTime.Now.AddDays(-3)
                    },
                    new Challenge
                    {
                        CreatorId = createdMembers[3].Id,
                        Title = "Đôi Nữ - Friendly Match",
                        Description = "Tìm cặp đôi nữ để giao lưu, không tính điểm Rank.",
                        Status = ChallengeStatus.Open,
                        CreatedAt = DateTime.Now.AddHours(-5)
                    }
                };

                context.Challenges.AddRange(challenges);
                await context.SaveChangesAsync();

                // Create sample matches
                var matches = new[]
                {
                    new Match
                    {
                        MatchDate = DateTime.Now.AddDays(-2),
                        Format = MatchFormat.Singles,
                        IsRanked = true,
                        ChallengeId = challenges[2].Id,
                        Winner1Id = createdMembers[2].Id,
                        Loser1Id = createdMembers[0].Id
                    },
                    new Match
                    {
                        MatchDate = DateTime.Now.AddDays(-1),
                        Format = MatchFormat.Doubles,
                        IsRanked = true,
                        ChallengeId = null, // Free play
                        Winner1Id = createdMembers[1].Id,
                        Winner2Id = createdMembers[3].Id,
                        Loser1Id = createdMembers[4].Id,
                        Loser2Id = createdMembers[5].Id
                    },
                    new Match
                    {
                        MatchDate = DateTime.Now.AddHours(-3),
                        Format = MatchFormat.Singles,
                        IsRanked = false,
                        ChallengeId = null,
                        Winner1Id = createdMembers[6].Id,
                        Loser1Id = createdMembers[7].Id
                    }
                };

                context.Matches.AddRange(matches);
                await context.SaveChangesAsync();

                // Create sample bookings
                var today = DateTime.Now.Date;
                var bookings = new[]
                {
                    new Booking
                    {
                        MemberId = createdMembers[0].Id,
                        StartTime = today.AddHours(8),
                        EndTime = today.AddHours(10),
                        CreatedDate = DateTime.Now.AddDays(-1)
                    },
                    new Booking
                    {
                        MemberId = createdMembers[1].Id,
                        StartTime = today.AddHours(14),
                        EndTime = today.AddHours(16),
                        CreatedDate = DateTime.Now.AddHours(-2)
                    },
                    new Booking
                    {
                        MemberId = createdMembers[2].Id,
                        StartTime = today.AddHours(16),
                        EndTime = today.AddHours(18),
                        CreatedDate = DateTime.Now.AddHours(-1)
                    },
                    new Booking
                    {
                        MemberId = createdMembers[3].Id,
                        StartTime = today.AddDays(1).AddHours(10),
                        EndTime = today.AddDays(1).AddHours(12),
                        CreatedDate = DateTime.Now
                    }
                };

                context.Bookings.AddRange(bookings);
                await context.SaveChangesAsync();
            }

            // Seed Advanced Data
            if (!await context.Courts.AnyAsync())
            {
                // 1. Seed Courts
                var courts = new[]
                {
                    new Court { Name = "Sân 1 - Trong Nhà", Description = "Sân tiêu chuẩn thi đấu, sàn gỗ cao cấp.", IsActive = true },
                    new Court { Name = "Sân 2 - Ngoài Trời", Description = "Sân thoáng mát, có đèn chiếu sáng ban đêm.", IsActive = true },
                    new Court { Name = "Sân 3 - VIP", Description = "Sân riêng tư, có phục vụ nước uống.", IsActive = true }
                };
                context.Courts.AddRange(courts);

                // 2. Seed Transaction Categories
                var categories = new[]
                {
                    new TransactionCategory { Name = "Tiền Sân", Type = TransactionType.Income },
                    new TransactionCategory { Name = "Quỹ Tháng", Type = TransactionType.Income },
                    new TransactionCategory { Name = "Tiền Nước & Bóng", Type = TransactionType.Expense },
                    new TransactionCategory { Name = "Bảo Trì Sân", Type = TransactionType.Expense },
                    new TransactionCategory { Name = "Giải Thưởng", Type = TransactionType.Expense }
                };
                context.TransactionCategories.AddRange(categories);
                await context.SaveChangesAsync();

                // 3. Seed Sample Transactions
                var financialAdmin = await userManager.FindByEmailAsync("admin@pickleballclub.com");
                context.Transactions.AddRange(
                    new Transaction
                    {
                        Date = DateTime.Now.AddDays(-5),
                        Amount = 2000000,
                        Description = "Thu quỹ tháng 10/2026",
                        CategoryId = categories[1].Id,
                        CreatedById = financialAdmin?.Id
                    },
                    new Transaction
                    {
                        Date = DateTime.Now.AddDays(-3),
                        Amount = 500000,
                        Description = "Mua bóng thi đấu",
                        CategoryId = categories[2].Id,
                        CreatedById = financialAdmin?.Id
                    },
                    new Transaction
                    {
                        Date = DateTime.Now.AddDays(-1),
                        Amount = 150000,
                        Description = "Thu tiền khách vãng lai",
                        CategoryId = categories[0].Id,
                        CreatedById = financialAdmin?.Id
                    }
                );
                await context.SaveChangesAsync();

                // 4. Seed Mini-Game Challenge (Team Battle)
                var members = await context.Members.ToListAsync();
                if (members.Count >= 6)
                {
                    var miniGame = new Challenge
                    {
                        CreatorId = members[0].Id,
                        Title = "Giải Mini Tứ Hùng - Team Battle",
                        Description = "Giải đấu giao hữu giữa 2 Team A và Team B. Đội nào thắng 3 trận trước sẽ giành chiến thắng chung cuộc.",
                        Status = ChallengeStatus.Ongoing,
                        Type = ChallengeType.MiniGame,
                        ResultMode = GameMode.TeamBattle,
                        EntryFee = 50000,
                        PrizePool = 300000, // 6 người * 50k
                        Config_TargetWins = 3,
                        CurrentScore_TeamA = 1,
                        CurrentScore_TeamB = 0,
                        CreatedAt = DateTime.Now.AddDays(-2),
                        StartDate = DateTime.Now.AddDays(-2)
                    };

                    context.Challenges.Add(miniGame);
                    await context.SaveChangesAsync();

                    // Add Participants
                    var participants = new List<Participant>();

                    // Team A (Rank cao)
                    participants.Add(new Participant { ChallengeId = miniGame.Id, MemberId = members[1].Id, Team = TeamName.TeamA, Status = ParticipantStatus.Confirmed, EntryFeePaid = true, EntryFeeAmount = 50000 });
                    participants.Add(new Participant { ChallengeId = miniGame.Id, MemberId = members[2].Id, Team = TeamName.TeamA, Status = ParticipantStatus.Confirmed, EntryFeePaid = true, EntryFeeAmount = 50000 });
                    participants.Add(new Participant { ChallengeId = miniGame.Id, MemberId = members[3].Id, Team = TeamName.TeamA, Status = ParticipantStatus.Confirmed, EntryFeePaid = true, EntryFeeAmount = 50000 });

                    // Team B (Rank thấp hơn)
                    participants.Add(new Participant { ChallengeId = miniGame.Id, MemberId = members[4].Id, Team = TeamName.TeamB, Status = ParticipantStatus.Confirmed, EntryFeePaid = true, EntryFeeAmount = 50000 });
                    participants.Add(new Participant { ChallengeId = miniGame.Id, MemberId = members[5].Id, Team = TeamName.TeamB, Status = ParticipantStatus.Confirmed, EntryFeePaid = true, EntryFeeAmount = 50000 });
                    participants.Add(new Participant { ChallengeId = miniGame.Id, MemberId = members[6].Id, Team = TeamName.TeamB, Status = ParticipantStatus.Confirmed, EntryFeePaid = true, EntryFeeAmount = 50000 });

                    context.Participants.AddRange(participants);
                    await context.SaveChangesAsync();

                    // Add 1 Match for this MiniGame
                    var match = new Match
                    {
                        MatchDate = DateTime.Now.AddHours(-2),
                        Format = MatchFormat.Doubles,
                        IsRanked = true,
                        ChallengeId = miniGame.Id,
                        WinningSide = WinningSide.Team1, // Team A thắng
                        Winner1Id = members[1].Id,
                        Winner2Id = members[2].Id,
                        Loser1Id = members[4].Id,
                        Loser2Id = members[5].Id
                    };
                    context.Matches.Add(match);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
