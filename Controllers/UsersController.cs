using Median.Intranet.DAL.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Median.Intranet.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IAdminUserRepository adminUserRepository;
        public UsersController(IAdminUserRepository adminUserRepository) 
        { 
            this.adminUserRepository = adminUserRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await this.adminUserRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var userResult = await this.adminUserRepository.GetUser(Guid.Parse(id));
            return FromResult(userResult);
        }
    }
}
