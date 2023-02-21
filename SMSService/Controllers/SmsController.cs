using Microsoft.AspNetCore.Mvc;
using SMSService.DbContexts;
using SMSService.Dtos;
using SMSService.Interfaces;

namespace SMSService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        #region Members
        private ISmsService _smsService;
        private ILogger<SmsController> _logger;
        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor
        public SmsController(ISmsService smsService,
            ILogger<SmsController> logger,
            ApplicationDbContext context)
        {
            _smsService = smsService;
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

        [HttpPost]
        public async Task<IActionResult> SendSms(SmsDto model)
        {
            try
            {
                var request = await _smsService.SendSmsService(model);
                return Ok(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
