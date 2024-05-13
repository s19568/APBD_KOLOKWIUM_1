using System;
using System.ComponentModel.DataAnnotations;

namespace PrescriptionAPI.Services
{
    public class PrescriptionModel
    {
        [Required]
        public int IdPrescription { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int IdPatient { get; set; }

        [Required]
        public int IdDoctor { get; set; }

        public override string ToString()
        {
            return $"{IdPrescription}, {Date}, {DueDate}, {IdPatient}, {IdDoctor}";
        }
    }
}