namespace SMSService.Dtos
{
    public class SmsDto
    {
        public string Text { get; set; }
        public string Vendor { get; set; }
        public string Receiver { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
