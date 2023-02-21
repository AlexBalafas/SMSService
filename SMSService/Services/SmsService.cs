using AutoMapper;
using SMSService.DbContexts;
using SMSService.Dtos;
using SMSService.Entities;
using SMSService.Interfaces;
using System.Text.RegularExpressions;

namespace SMSService.Services
{
    public class SmsService : ISmsService
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;

        public SmsService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SmsDto> SendSmsService(SmsDto smsModel)
        {
            try
            {
                if (smsModel.Text.Length > 480)
                {
                    Console.WriteLine("Message is too long.");
                }
                var ventor = await SelectVentor(smsModel);
                var message = new Sms()
                {
                    Text = smsModel.Text,
                    Receiver = smsModel.Receiver,
                    TimeStamp = DateTime.Now,
                    Vendor = ventor
                };

                _context.Sms.Add(message);
                _context.SaveChanges();

                var recordDto = _mapper.Map<SmsDto>(message);
                return recordDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<String> SelectVentor(SmsDto model)
        {
            Send(model.Text);
            return null;
        }
        public void Send(string text)
        {
            if (!IsGreekText(text))
            {
                Console.WriteLine("Message text must be in Greek characters.");
            }
        }
        private bool IsGreekText(string text)
        {
            return !Regex.IsMatch(text, @"[^\p{IsGreek}]");
        }
        private bool IsGreekOrEnglishText(string text)
        {
            return !Regex.IsMatch(text, @"[^a-zA-Zα-ωΑ-Ω]");
        }
    }
}
