using AutoMapper;
using SMSService.DbContexts;
using SMSService.Dtos;
using SMSService.Entities;
using SMSService.Interfaces;

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

        public async Task<SmsDto> Create(SmsDto smsModel)
        {
            var message = new Sms()
            {
                Receiver = smsModel.Receiver,
                Text = smsModel.Text,
                Vendor = smsModel.Vendor,
                TimeStamp = DateTime.Now
            };
            await _context.Sms.AddAsync(message);
            await _context.SaveChangesAsync();

            var recordDto = _mapper.Map<SmsDto>(message);
            return recordDto;
        }
        public async Task<SmsDto> SendSmsService(SmsDto smsModel)
        {
            try
            {
                var ventor = await SelectVentor(smsModel.Text);
                var message = new Sms()
                {
                    Receiver = smsModel.Receiver,
                    Text = smsModel.Text,
                    Vendor = ventor,
                    TimeStamp = DateTime.Now
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

        public async Task<String> SelectVentor(string ven)
        {

            var ventor = ven;

            if (ventor == null)
            {
                return null;
            }
            else
            {


            }

            return ventor;
        }
    }
}
