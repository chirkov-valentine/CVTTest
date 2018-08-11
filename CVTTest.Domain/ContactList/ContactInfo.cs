using System.ComponentModel;

namespace CVTTest.Domain.ContactList
{
    public class ContactInfo
    {
        public int ContactInfoId { get; set; }
        [DisplayName("Номер телефона")]
        public string PhoneNumber { get; set; }
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [DisplayName("Skype")]
        public string Skype { get; set; }
        [DisplayName("Другое")]
        public string AdditinalInfo { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
