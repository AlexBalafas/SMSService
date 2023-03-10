using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SMSService.Context;
using SMSService.DbContexts;
using SMSService.Dtos;
using SMSService.Entities;
using SMSService.Interfaces;
using SMSService.Services;

namespace SMSService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        #region Members
        private ISmsService _smsService;
        private ISmsVendorStrategy _vendorStrategy;
        private ILogger<SmsController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly Dictionary<string, ISmsVendorStrategy> _strategies;

        #endregion

        #region Constructor
        public SmsController(ISmsService smsService, ISmsVendorStrategy vendorStrategy,
            ILogger<SmsController> logger,
            ApplicationDbContext context, IMapper mapper)
        {
            _smsService = smsService;
            _vendorStrategy = vendorStrategy;
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _strategies = new Dictionary<string, ISmsVendorStrategy>
            {
                { "30", new SmsVendorGRStrategy() },
                { "35", new SmsVendorCYStrategy() }
            };
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
                //_context.
                return Ok(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("strategy")]
        public async Task<SmsDto> SendSmsStrategy(SmsDto model)
        {
            try
            {
                var request = await _vendorStrategy.SendSms(model.Receiver, model.Text);

                var v = SendSmsWithStrategy(model);

                var mod = new Sms()
                {
                    Text = v.Result.Text,
                    Receiver = model.Receiver,
                    Vendor = v.Result.Vendor,
                    TimeStamp = model.TimeStamp,
                };
                await _context.Sms.AddAsync(mod);
                await _context.SaveChangesAsync();

                var recordDto = _mapper.Map<SmsDto>(mod);
                return recordDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Task<SmsDto> SendSmsWithStrategy(SmsDto model)
        {

            try
            {
                ISmsVendorStrategy strategy;
                var messageLang = GetVendorCode(model.Receiver);
                if (messageLang == "30")
                {
                    strategy = new SmsVendorGRStrategy();
                }
                else if (messageLang == "35")
                {
                    strategy = new SmsVendorCYStrategy();
                }
                else
                {
                    strategy = new SmsVendorRestStrategy();
                }

                var response = strategy.SendSms(model.Receiver, model.Text);


                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetVendorCode(string number)
        {
            return number.Substring(1, 2);
        }
    }
}
