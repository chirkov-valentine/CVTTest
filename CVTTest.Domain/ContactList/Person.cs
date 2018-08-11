using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CVTTest.Domain.ContactList
{
    public class Person
    {
        public int PersonId { get; set; }
        [DisplayName("Фамилия")]
        public string LastName { get; set; }
        [DisplayName("Имя")]
        public string FirstName { get; set; }
        [DisplayName("Отчество")]
        public string MiddleName { get; set; }
        [DisplayName("Организация")]
        public string Organization { get; set; }
        [DisplayName("Должность")]
        public string Position { get; set; }
        [DisplayName("Дата рождения")]
        public DateTime DateOfBirth { get; set; }
        public virtual ICollection<ContactInfo> ContactInfos { get; set; }
    }
}
