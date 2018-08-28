using PatientDesigns.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace PatientDesigns.Controllers
{
    public class ValuesController : ApiController
    {

        string conn = "Server=tcp:nams7493.database.windows.net,1433;Initial Catalog=nams7493;Persist Security Info=False;User ID=nams7493;Password=Abcdabc2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        [Route("api/PatientDetails/{id}")]
        [HttpGet]
        public Patient GetPatientDetails(int id)
        {
            Patient patient = new Patient();
            Stopwatch stopwatch = new Stopwatch();
            //To measure function execcution time
            stopwatch.Start();
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("select * from patient where id=@id", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            patient = new Patient { Id=reader.GetInt32(0),FirstName = reader.GetString(1), LastName = reader.GetString(2), EmailId = reader.GetString(3), PhoneNumber = reader.GetString(4) };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

            return patient;
        }

        [Route("api/DoctorDetails/{id}")]
        [HttpGet]
        public Doctor GetDoctortDetails(int id)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Doctor doctor = new Doctor();
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("select * from doctor where id=@id", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            doctor = new Doctor { Id = reader.GetInt32(0), FirstName = reader.GetString(1), LastName = reader.GetString(2), EmailId = reader.GetString(3), PhoneNumber = reader.GetString(4) };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            return doctor;
        }

        [HttpGet]
        [Route("api/PharmacistDetails/{id}")]
        public Pharmacist GetPharmacistDetails(int id)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Pharmacist pharmacist = new Pharmacist();
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("select * from pharmacist where id=@id", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pharmacist = new Pharmacist { Id = reader.GetInt32(0), FirstName = reader.GetString(1), LastName = reader.GetString(2), EmailId = reader.GetString(3), PhoneNumber = reader.GetString(4) };
                    }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            return pharmacist;
        }

        [HttpGet]
        [Route("api/patientdetails/{patientid}")]
        public List<PrescriptionDetails> GetPrescriptions(int patientid)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<PrescriptionDetails> details = new List<PrescriptionDetails>();
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("select patientid,prescriptiondetails from Prescriptions where patientid=@patientid", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@patientid", patientid);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            details.Add(new PrescriptionDetails { PatientId = reader.GetInt32(0), Details = reader.GetString(1) });
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            return details;
        }
        
        // POST api/values
        [HttpPost]
        [Route("api/CreateRequestDoctor")]
        public void DoctorRequestViewData([FromBody] int PatientId, [FromBody] int DoctorId)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                Utility.CreateRequestforDoctortoPatient(DoctorId, PatientId);
                Patient patient = GetPatientDetails(PatientId);
                DooctortoPatientMail(patient.EmailId, DoctorId,patient.Id);
             }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }

        [HttpPost]
        [Route("api/CreateRequestPharmacist")]
        public void PharmacistRequestViewData([FromBody] int PatientId, [FromBody] int PharmacistId)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                Utility.CreateRequestforPharmacisttoPatient(PharmacistId, PatientId);
                Patient patient = GetPatientDetails(PatientId);
                PharmacisttoPatientMail(patient.EmailId, PharmacistId,patient.Id);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }

        [HttpGet]
        [Route("api/PostRequestStatusDoctor/{patientid}/{doctorid}/{success}")]
        public void PostStatusDoctor(int PatientId,int DoctorId, string success)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                if (success == "S")
                {
                    Utility.UpdateStausDoctorPatient(DoctorId, PatientId, 2);
                }
                else
                {
                    Utility.UpdateStausDoctorPatient(DoctorId, PatientId, 1);
                }
                Doctor doctor = GetDoctortDetails(DoctorId);
                SendDataofPatient(doctor.EmailId, success, PatientId);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }

        [HttpPost]
        [Route("api/PostRequestStatusPharmacist/{patientid}/{doctorid}/{success}")]
        public void PostStatusPharmacist(int PatientId, int PharmacistId,string success)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                if (success == "S")
                {
                    Utility.UpdateStausDoctorPatient(PharmacistId, PatientId, 2);
                }
                else
                {
                    Utility.UpdateStausDoctorPatient(PharmacistId, PatientId, 1);
                }
                Pharmacist pharmacist = GetPharmacistDetails(PharmacistId);
                SendDataofPatient(pharmacist.EmailId, success, PatientId);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }

        public void DooctortoPatientMail(string toemailid, int doctorid,int patientid)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                SmtpClient client = new SmtpClient();
                MailMessage mail = new MailMessage("namitshah7493@gmail.com", toemailid);
                mail.Subject = "Request for viewing Patient's details";
                string ApproveApi = "https://webapplication120180825100027.azurewebsites.net/" +patientid.ToString() + doctorid.ToString() +"/S";
                string RejectApi = "https://webapplication120180825100027.azurewebsites.net/" + patientid.ToString() + doctorid.ToString() + "/F";
                string doctordetails = GetDoctortDetails(doctorid).ToString();
                mail.Body = "Please press the given link if you Doctor/Pharmacist to allow him to view your prescription details " + "\r\n"
                + ApproveApi + "\r\n" + "Please press the given link if you want the Doctor/Pharmacist to disallow  him to view your prescription details " + "\r\n" + RejectApi
                + "\r\n" + "Doctor Details :" + doctordetails;
                client.Send(mail);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }

        public void PharmacisttoPatientMail(string toemailid, int pharmacistid,int patientid)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                SmtpClient client = new SmtpClient();
                MailMessage mail = new MailMessage("namitshah7493@gmail.com", toemailid);
                mail.Subject = "Request for viewing Patient's details";
                string ApproveApi = "https://webapplication120180825100027.azurewebsites.net/" + patientid.ToString() + pharmacistid.ToString() + "/S";
                string RejectApi = "https://webapplication120180825100027.azurewebsites.net/" + patientid.ToString() + pharmacistid.ToString() + "/F";
                string pharmacistdetils = GetPharmacistDetails(pharmacistid).ToString();
                mail.Body = "Please press the given link if you Doctor/Pharmacist to allow him to view your prescription details " + "\r\n"
                + ApproveApi + "\r\n" + "Please press the given link if you want the Doctor/Pharmacist to disallow  him to view your prescription details " + "\r\n" + RejectApi + "\r\n" +
                "PharmacistDetails :" + pharmacistdetils.ToString();
                client.Send(mail);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }

        public void SendDataofPatient(string toemailid, string success, int patientid)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                SmtpClient client = new SmtpClient();
                MailMessage mail = new MailMessage("namitshah7493@gmail.com", toemailid);
                mail.Subject = "Request for viewing Patient's details";
                string patientobj = GetPrescriptions(patientid).ToString();
                if (success == "S")
                    mail.Body = "Here are the patient's details:" + patientobj;
                else
                    mail.Body = "Your Request to view PatientId :" + patientid.ToString() + "is rejected";
                client.Send(mail);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }
    }
}