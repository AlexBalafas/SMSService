using Microsoft.AspNetCore.Mvc;
using SMSService.Dtos;

namespace SMSService.Interfaces
{
    public interface ISmsService
    {
        Task<SmsDto> SendSmsService(SmsDto smsModel);

    }
}
