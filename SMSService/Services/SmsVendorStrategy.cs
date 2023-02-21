using AutoMapper;
using SMSService.DbContexts;
using SMSService.Dtos;
using SMSService.Entities;
using SMSService.Interfaces;

namespace SMSService.Services
{
    public class SmsVendorGRStrategy : ISmsVendorStrategy
    {

        public async Task<SmsDto> SendSms(string number, string message)
        {
            var m = new SmsDto()
            {
                Text = message,
                Vendor ="Greek "
            };
            return m;
        }
    }
}

public class SmsVendorCYStrategy : ISmsVendorStrategy
{
    public async Task<SmsDto> SendSms(string number, string message)
    {
        var m = new SmsDto()
        {
            Text = message,
            Vendor = "Cyprus "
        };
        return m;
        
    }
}

public class SmsVendorRestStrategy : ISmsVendorStrategy
{
    public async Task<SmsDto> SendSms(string number, string message)
    {
        var m = new SmsDto()
        {
            Text = message,
            Vendor = "Rest "
        };
        return m;
        
    }
}
