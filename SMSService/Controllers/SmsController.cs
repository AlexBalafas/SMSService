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
                    Text = model.Text,
                    Receiver = model.Receiver,
                    Vendor = v,
                    TimeStamp = DateTime.Now,
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
        public string SendSmsWithStrategy(SmsDto model)
        {
            //try
            //{
            //    var messageLang = GetVendorCode(model.Receiver);

            //    if (_strategies.TryGetValue(messageLang, out var strategy))
            //    {

            //        var mod = strategy.SendSms(model.Receiver, model.Text);

            //        return mod.Result.Vendor;
            //        Console.WriteLine(mod.Result.);

            //    }

            //}
            try
            {
                // Determine the appropriate strategy based on the destination country code
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

                // Send the SMS using the selected strategy
                var response = strategy.SendSms(model.Receiver, model.Text);

                // Save the SMS to the database (replace with your own implementation)
                //SaveSmsToDatabase(smsDto.To, smsDto.Message, response);

                // Return the success response
                return response.Result.Vendor;
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
