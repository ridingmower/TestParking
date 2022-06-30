using System.Runtime.Serialization;

namespace Kztek.Web.Models
{
    [DataContract]
    public class RecaptchaResult
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "error-codes")]
        public string[] ErrorCodes { get; set; }
    }
}