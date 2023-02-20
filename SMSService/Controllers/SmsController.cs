using Microsoft.AspNetCore.Mvc;
using SMSService.DbContexts;

namespace SMSService.Controllers
{
    public class SmsController : ControllerBase
    {
        #region Members
        private ILogger<SmsController> _logger;
        private readonly ApplicationDbContext _context;
        #endregion

        #region Constructor
        public SmsController( ILogger<SmsController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            var sms = _context.Sms.ToList();
            return Ok(sms);
        }
    }
}
