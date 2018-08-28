using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientDesigns.Models
{
    public class Doctor
    {
        public string EmailId { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Id { get; set; }
    }
}