using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TO_DO.Models;
using TO_DO.Tools;

namespace ToDo_Project.Controllers
{
    [EnableCors("appCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly TododbContext _context;
        private readonly IConfiguration _configuration;
        public UsersController(TododbContext context, IConfiguration configuration)
        {
            this._context = context;
            this._configuration = configuration;
        }

        //[Authorize]
        [HttpGet]
        [Route("Users")]
        public IActionResult GetUsers()
        {
            try
            {
                List<User> ListUsers = _context.Users.ToList();

                if (ListUsers != null)
                {
                    return Ok(ListUsers);
                }
                return Ok("there are no users");
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> userLogin([FromBody] User user)
        {
            try
            {
                string password = Password.hashPassword(user.Password);
                var dbUser = _context.Users.Where(u => u.Username == user.Username && u.Password == password).Select(u => new
                {
                    u.UserID,
                    u.Username
                }).FirstOrDefault();

                if (dbUser == null)
                {
                    return BadRequest("Username or password is incorrect");
                }

                List<Claim> autClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, dbUser.Username),
                    new Claim("userId",dbUser.UserID.ToString()),
                    new Claim("username",dbUser.Username)

                };

                var token = this.getToken(autClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });

            }
            catch (Exception)
            {
                return BadRequest();
            }

        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> userRegistry([FromBody] User user)
        {

            try
            {
                var dbUser = _context.Users.Where(u => u.Username == user.Username).FirstOrDefault();

                if (dbUser != null)
                {
                    return BadRequest("Username already exists");
                }

                user.Password = Password.hashPassword(user.Password);

                _context.Users.Add(user);
                _context.SaveChanges();

                return Ok("User added successfully");

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private JwtSecurityToken getToken(List<Claim> authClaim)
        {

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
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
