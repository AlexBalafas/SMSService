using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SMSService.DbContexts;
using SMSService.Dtos;
using SMSService.Entities;
using SMSService.Interfaces;
using SMSService.Services;

namespace SMSService.Context
{
    public class SmsVendorContext
    {
        private readonly Dictionary<string, ISmsVendorStrategy> _strategies;
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;

        public SmsVendorContext(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public SmsVendorContext()
        {
            _strategies = new Dictionary<string, ISmsVendorStrategy>
            {
                { "30", new SmsVendorGRStrategy() },
                { "35", new SmsVendorCYStrategy() }               
            };
        }

        public void SendSmsWithStrategy(SmsDto model)
        {
            try
            {
                var messageLang = GetVendorCode(model.Receiver);

                if (_strategies.TryGetValue(messageLang, out var strategy))
                {
                  
                    var mod = strategy.SendSms(model.Receiver, model.Text);


                    Console.WriteLine(mod);

                }              

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

        //private static string GetVendorCode(string number,string message)
        //{
        //    if (!Regex.IsMatch(message, @"[^\p{IsGreek}]") && message.Length < 160)
        //    {
        //        return "GR";
        //    }
        //    else if(!Regex.IsMatch(message, @"[^a-zA-Zα-ωΑ-Ω]"))
        //    {
        //        return "CY";
        //    }
        //    else
        //    {
        //        return "Rest";
        //    }
        //}
    }
}
