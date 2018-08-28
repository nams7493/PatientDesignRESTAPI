using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace PatientDesigns
{
    public static class Utility
    {
        static string conn = "Server=tcp:nams7493.database.windows.net,1433;Initial Catalog=nams7493;Persist Security Info=False;User ID=nams7493;Password=Abcdabc2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public static void CreateRequestforDoctortoPatient(int doctorid, int patientid)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("insert into patient_doctor_approval_status(patientid,doctorid,status)values(@patientid,@doctorid,0)", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@patientid", patientid);
                    command.Parameters.AddWithValue("@doctorid", doctorid);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }

        public static void UpdateStausDoctorPatient(int doctorid, int patientid, int status)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("update patient_doctor_approval_status set status=@status where patientid=@patientid and doctorid=@doctorid", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@patientid", patientid);
                    command.Parameters.AddWithValue("@doctorid", doctorid);
                    command.Parameters.AddWithValue("@status", status);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }

        public static void CreateRequestforPharmacisttoPatient(int patientid, int pharmacistid)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("insert into patient_pharmacist_approval_status(pharmacistid,patientid,status)values(@pharmacistid,@patientid,0)", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@pharmacistid", pharmacistid);
                    command.Parameters.AddWithValue("@patientid", patientid);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }

        public static void UpdateStausDoctorPharmacist(int pharmacistid, int patientid, int status)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("update patient_pharmacist_approval_status set status=@status where patientid=@patientid and pharmacistid=@pharmacistid", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@patientid", patientid);
                    command.Parameters.AddWithValue("@pharmacistid", pharmacistid);
                    command.Parameters.AddWithValue("@status", status);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }
    }        
    }