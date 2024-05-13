using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PrescriptionAPI.Services
{
    public class MedicamentService : IMedicamentService
    {
        private readonly IConfiguration _configuration;

        public MedicamentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<MedicamentModel> GetMedicament(int medicamentId)
        {
            string connectionString = _configuration.GetConnectionString("Database");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT m.IdMedicament, m.Name, m.Description, m.Type,
                           p.IdPrescription, p.Date, p.DueDate
                    FROM Medicament m
                    JOIN Prescription_Medicament pm ON m.IdMedicament = pm.IdMedicament
                    JOIN Prescription p ON pm.IdPrescription = p.IdPrescription
                    WHERE m.IdMedicament = @MedicamentId
                    ORDER BY p.Date DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@MedicamentId", medicamentId);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows)
                        {
                            return null; 
                        }

                        MedicamentModel medicament = null;

                        while (await reader.ReadAsync())
                        {
                            if (medicament == null)
                            {
                                medicament = new MedicamentModel
                                {
                                    IdMedicament = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    Type = reader.GetString(3),
                                    Prescriptions = new List<PrescriptionModel>()
                                };
                            }
                            var prescription = new PrescriptionModel
                            {
                                IdPrescription = reader.GetInt32(4),
                                Date = reader.GetDateTime(5),
                                DueDate = reader.GetDateTime(6)
                            };
                            medicament.Prescriptions.Add(prescription);
                        }

                        return medicament;
                    }
                }
            }
        }
    }
}
