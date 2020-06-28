using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProAgil.Domain.Identity;
using ProAgil.WebAPI.Dtos;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public UserController(IConfiguration config, 
                              UserManager<User> userManager, 
                              SignInManager<User> signInManager,
                              IMapper mapper)
        {
            this._config = config;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._mapper = mapper;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            return Ok(new UserDto());
        }

        [HttpPost("Register")]
        [AllowAnonymous]   
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);
                var userToReturn = _mapper.Map<UserDto>(user);

                if(result.Succeeded)
                {
                    return Created("GetUser", userToReturn);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception exception)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de Dados Falhou {exception.Message}");
            }           
        }
        

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userLogin.UserName);
                var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);
                if(result.Succeeded){
                    var appUser = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.NormalizedUserName == userLogin.UserName.ToUpper());
                    var userToReturn = _mapper.Map<UserLoginDto>(appUser);
                    
                    return Ok(new {
                        token = GenerateJWToken(appUser).Result,
                        user = userToReturn
                    });  
                }

                return Unauthorized();
            }
            catch (System.Exception exception)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de Dados Falhou {exception.Message}");
            }
        }

        private async Task<string> GenerateJWToken(User appUser)
        {
           var claims = new List<Claim>{
               new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
               new Claim(ClaimTypes.Name, appUser.UserName)
           };

            var roles = await _userManager.GetRolesAsync(appUser);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII
                    .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
            
        }
    }
}