using Microsoft.EntityFrameworkCore;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Data;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Infrastructure.Repositories.Implements
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Users.AnyAsync(u => u.Name == name);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByRutAsync(string rut)
        {
            return await _context.Users.AnyAsync(u => u.Rut == rut);
        }

        public async Task<bool> ExistsByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<bool> SaveVerificationCodeAsync(int userId, string verificationCode, DateTime verificationCodeExpiry)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.VerificationCode = verificationCode;
            user.VerificationCodeExpiry = verificationCodeExpiry;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email) ?? null!;
        }

        public async Task<bool> MarkEmailAsVerifiedAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }

            user.EmailConfirmed = true;
            user.VerificationCode = null;
            user.VerificationCodeExpiry = null;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}