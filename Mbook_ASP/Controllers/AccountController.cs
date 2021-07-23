using Mbook_ASP.Models;
using Mbook_ASP.Service;
using MbookDatabaseAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
namespace Mbook_ASP.Controllers
{
    public class AccountController : ApiController
    {
        //Call ContextDB 
        private MbookEntities context = new MbookEntities();


        // Authorization Function
        [HttpGet]
        public string GetToken(string username)
        {
            string key = "my_secret_key_anhdaden";
            var issuer = "https://github.com/hoainho";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("username", username));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token ;
        }

        [HttpPost]
        [Authorize]
        public Boolean Check()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        [Authorize]
        [HttpPost]
        public string GetUsername()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var username = claims.Where(p => p.Type == "username").FirstOrDefault()?.Value;
                return username;

            }   
            return "null";
        }

        // Account Function
        [Authorize]
        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return context.Accounts.ToList();
        }
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetId(Guid id)
        {
            return Ok(context.Accounts.FirstOrDefault(item => item.idAccount == id));
        }

        [HttpPost]
        [Obsolete]
        public IHttpActionResult signup(Account acc)
        {
            if (acc != null)
            {
                Account isExist = context.Accounts.FirstOrDefault(a => a.username == acc.username);
                if(isExist == null)
                {
                    acc.password = FormsAuthentication.HashPasswordForStoringInConfigFile(acc.password, "SHA1");
                    acc.idAccount = Guid.NewGuid();
                    context.Accounts.Add(acc);
                    context.SaveChanges();
                    return Ok(acc);
                }
                else
                {
                    return Conflict();
                }
                
            }
            else
            {
                return BadRequest();
            }
            
        }
        
        [HttpPost]
        [Obsolete]
        public IHttpActionResult signin(Account acc)
        {
            string parsePassword = FormsAuthentication.HashPasswordForStoringInConfigFile(acc.password, "SHA1");
            var check = context.Accounts.FirstOrDefault(a => a.username == acc.username 
            && a.password == parsePassword);
            if(check != null)
            {
                if (check.status)
                {
                    return BadRequest("Tài Khoản Bị Khóa");
                }
                else
                {
                    check.token = GetToken(acc.username).ToString();
                    return Ok(check);
                }
            }
            else
            {
                return NotFound();
            }
        }
        
        [HttpPost]
        [Obsolete]
        [Route("account/dashboard/signin")]
        public IHttpActionResult signinDashboard(Account acc)
        {
            string parsePassword = FormsAuthentication.HashPasswordForStoringInConfigFile(acc.password, "SHA1");
            var check = context.Accounts.FirstOrDefault(a => a.username == acc.username
            && a.password == parsePassword);
            if (check != null)
            {
                if (check.roleid)
                {
                    if (check.status)
                    {
                        return BadRequest("Tài Khoản Bị Khóa");
                    }
                    else
                    {
                        return Ok(check);
                    }
                }
                else
                {
                    return StatusCode(HttpStatusCode.NotAcceptable);
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Obsolete]
        [Authorize]
        public IHttpActionResult changepassword(Password pass)
        {
            if (Check())
            {
                string username = GetUsername();
                Account acc = context.Accounts.FirstOrDefault(a => a.username == username);
                if (acc.password == pass.passwordOld)
                {
                    acc.password = FormsAuthentication.HashPasswordForStoringInConfigFile(pass.passwordNew, "SHA1");
                    context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
           
        }
        [Authorize]
        [HttpPost]
        [Obsolete]
        public IHttpActionResult delete(Guid id)
        {
            
            Account acc = context.Accounts.FirstOrDefault(item => item.idAccount == id);
            if(acc != null)
            {
                context.Accounts.Remove(acc);
                context.SaveChanges();
                return Ok("Xóa Thành Công");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
