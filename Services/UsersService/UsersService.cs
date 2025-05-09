using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Services.Auth;

namespace WebApplication1.Services.UsersService
{
    public class UsersService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly PasswordHasher<UserModel> _hasher;

        public UsersService(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
            _hasher = new PasswordHasher<UserModel>();  
        }

        public string HashPassword(string plainPassword)
        {
            return _hasher.HashPassword(null, plainPassword);  
        }

        public bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            var result = _hasher.VerifyHashedPassword(null, hashedPassword, inputPassword);
            return result == PasswordVerificationResult.Success;
        }

        public async Task<ActionResult> CreateNewUser(UserModel user)
        {
            var existingUserByEmail = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUserByEmail != null)
            {
                return new BadRequestObjectResult(new { message = "Email já está em uso." });
            }

            var existingUserByName = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == user.Name);

            if (existingUserByName != null)
            {
                return new BadRequestObjectResult(new { message = "Nome já está em uso." });
            }

            var newUser = new UserModel
            {
                Name = user.Name,
                Email = user.Email,
                Password = HashPassword(user.Password)
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return new OkObjectResult(newUser);
        }

        public async Task<ActionResult> LoginUser(UserModel user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser == null)
            {
                return new BadRequestObjectResult(new { message = "Usuário não encontrado." });
            }

            var passwordVerified = VerifyPassword(existingUser.Password, user.Password);

            if (!passwordVerified)
            {
                return new BadRequestObjectResult(new { message = "Senha inválida." });
            }

            var token = _jwtService.GenerateToken(existingUser);
            return new OkObjectResult(new { token });
        }
    }
}
