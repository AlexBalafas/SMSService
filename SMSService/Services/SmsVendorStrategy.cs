using AutoMapper;
using SMSService.DbContexts;
using SMSService.Dtos;
using SMSService.Entities;
using SMSService.Interfaces;
using System.Text;

namespace SMSService.Services
{
    public class SmsVendorGRStrategy : ISmsVendorStrategy
    {

        public async Task<SmsDto> SendSms(string number, string message)
        {
            if (message.Length < 160)
            {

                var m = new SmsDto()
                {
                    Text = message,
                    Vendor = "Greek "
                };
                return m;
            }
            else
            {
                throw new ArgumentException($"Message Can not Be Gratter Than 160");
            }
        }
    }
}

public class SmsVendorCYStrategy : ISmsVendorStrategy
{
    private const int MaxSmsLength = 160;

    public async Task<SmsDto> SendSms(string number, string message)
    {
        if (message.Length <= MaxSmsLength)
        {
            var m = new SmsDto()
            {
                Text = message,
                Vendor = "Cyprus "
            };
            return m;
        }
        else
        {
            var parts = SplitMessage(message);
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < parts.Count; i++)
            {
                // Append each part of the message to the previous one
                stringBuilder.Append($"{i + 1}/{parts.Count} {parts[i]} ");
            }

            var combinedMessage = stringBuilder.ToString().TrimEnd();
            var m = new SmsDto()
            {
                Text = combinedMessage,
                Vendor = "Cyprus "
            };

            return m;
        }
       
        return null;

    }
    private static List<string> SplitMessage(string message)
    {
        var parts = new List<string>();

        // Split the message into chunks of MaxSmsLength characters
        for (var i = 0; i < message.Length; i += MaxSmsLength)
        {
            var chunkLength = Math.Min(MaxSmsLength, message.Length - i);
            var chunk = message.Substring(i, chunkLength);
            parts.Add(chunk);
        }

        return parts;
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


