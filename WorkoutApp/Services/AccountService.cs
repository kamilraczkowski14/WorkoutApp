using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkoutApp.DAL;
using WorkoutApp.Dtos;
using WorkoutApp.Exceptions;
using WorkoutApp.Models;
using WorkoutApp.Tools;

namespace WorkoutApp.Services
{
    public interface IAccountService
    {
        void Register(RegisterDto dto);
        string Login(LoginDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public AccountService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void Register(RegisterDto dto)
        {
            var existingUsername = _context.Users.FirstOrDefault(u => u.Username == dto.Username);
            var existingEmail = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            dto.Password = Password.hashPassword(dto.Password);

            if (existingUsername != null)
            {
                throw new NotFoundException("Uzytkownik o podanej nazwie juz istnieje!");
            }

            if (existingEmail != null)
            {
                throw new BadRequestException("Uzytkownik o podanym adresie email juz istnieje!");
            }


            //walidacja hasla
            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 3)
            {
                throw new BadRequestException("Haslo musi zawierac co najmniej 3 znaki");
            }

            var newUser = new User()
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password
            };


            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
        public string Login(LoginDto dto)
        {
            string password = Password.hashPassword(dto.Password);

            var existingUser = _context.Users.Where(u => u.Username == dto.Username && u.Password == password).Select(u => new
            {
                u.UserId,
                u.Username
            }).FirstOrDefault();

            if (existingUser == null)
            {
                throw new BadRequestException("Podales niepoprawna nazwe lub haslo!");
            }


            List<Claim> autClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, existingUser.UserId.ToString() ),
                    new Claim(ClaimTypes.Name, existingUser.Username),
                    //new Claim("userID", existingUser.UserId.ToString()),
                    new Claim("Username", existingUser.Username)
                };

            var tok = this.getToken(autClaims);

            var token = new JwtSecurityTokenHandler().WriteToken(tok);

            return token;
        }

        private JwtSecurityToken getToken(List<Claim> authClaim)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken
            (
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(24),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;

        }

    }
}
