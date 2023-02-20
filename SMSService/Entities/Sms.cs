using System.ComponentModel.DataAnnotations;

namespace SMSService.Entities
{
    public class Sms
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Vendor { get; set; }
        public string Receiver { get; set; }
    }
}
