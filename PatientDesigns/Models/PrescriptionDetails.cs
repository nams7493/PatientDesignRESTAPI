using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientDesigns.Models
{
    public class PrescriptionDetails
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public string Details { get; set; }
    }
}