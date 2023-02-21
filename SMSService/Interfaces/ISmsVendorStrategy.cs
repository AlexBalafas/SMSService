using SMSService.Dtos;

namespace SMSService.Interfaces
{
    public interface ISmsVendorStrategy
    {
        Task<SmsDto> SendSms(string number,string message);
    }
}
