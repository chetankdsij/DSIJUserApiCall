using System;
namespace DSIJUserApiCall
{
    public class PaymentFailure
    {
        public Int64 triggerId { get; set; }
        public string eventState { get; set; }
        public string MOBILE { get; set; }
        public string EMAIL { get; set; }
        public string NAME { get; set; }
    }
}
