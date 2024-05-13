using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace PrescriptionAPI.Services
{
    public class PatientService : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string _connectionString;

        public PatientService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Database");
        }

        public IActionResult RemovePatient(int IdPatient)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    DeletePrescriptions(conn, transaction, IdPatient);
                    DeletePatient(conn, transaction, IdPatient);
                    transaction.Commit();

                    return Ok("Patient removed");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, "Error occurred while removing the patient");
                }
            }
        }
        private void DeletePrescriptions(SqlConnection connection, SqlTransaction transaction, int IdPatient)
        {
            string deletePrescriptionQuery = "DELETE FROM Prescription WHERE IdPatient = @IdPatient";
            string deletePrescriptionMedicamentQuery = "DELETE FROM Prescription_Medicament" +
                                                       "WHERE IdPrescription IN (SELECT IdPrescription FROM Prescription WHERE IdPatient = @IdPatient)";

            using (SqlCommand cmd = new SqlCommand(deletePrescriptionQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@IdPatient", IdPatient);
                cmd.ExecuteNonQuery();
            }

            using (SqlCommand cmd = new SqlCommand(deletePrescriptionMedicamentQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@IdPatient", IdPatient);
                cmd.ExecuteNonQuery();
            }
        }

        private void DeletePatient(SqlConnection connection, SqlTransaction transaction, int IdPatient)
        {
            string deletePatientQuery = "DELETE FROM Patient WHERE IdPatient = @IdPatient";

            using (SqlCommand cmd = new SqlCommand(deletePatientQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@IdPatient", IdPatient);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
