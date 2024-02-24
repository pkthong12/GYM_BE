using API;
using GYM_BE.DTO;
using GYM_BE.ENTITIES;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.TestSystem
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestSystemController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly ITestSystemRepository _TestSystemRepository;
        private readonly AppSettings _appSettings;

        public TestSystemController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _TestSystemRepository = new TestSystemRepository(_dbContext);
            _appSettings = options.Value;
        }
        [HttpPost]
        public async Task<IActionResult> Create(TrCenterDTO dto)
        {
                var x = await _TestSystemRepository.Create(dto,"1222");
                return Ok(x);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response =await _TestSystemRepository.GetById(id);
            return Ok(response);
        }

    }
}
