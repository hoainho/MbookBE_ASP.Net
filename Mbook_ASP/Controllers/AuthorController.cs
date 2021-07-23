using MbookDatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
namespace Mbook_ASP.Controllers
{
    public class AuthorController : ApiController
    {
        private MbookEntities context = new MbookEntities();

        [Authorize]
        [HttpGet]
        public IEnumerable<Author> Get()
        {
            return context.Authors.ToList();
        }
        [HttpGet]
        [Authorize]
        public IHttpActionResult details(Guid id)
        {
            return Ok(context.Authors.FirstOrDefault(item => item.idAuthor == id));
        }
        [HttpPost]
        [Authorize]
        public IHttpActionResult upload(Author author)
        {
            //((System.Data.Entity.Validation.DbEntityValidationException)$exception).EntityValidationErrors   //Check Details Error
            Author check = context.Authors.FirstOrDefault(item => item.name == author.name);
            if (check == null)
            {
                author.idAuthor = Guid.NewGuid();
                author.createdby = GetUsername();    
                context.Authors.Add(author);
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return Conflict();
            }
        }


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

    }
}
