using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrescriptionAPI.Services
{
    public class MedicamentModel
    {
        [Required] 
        public int IdMedicament { get; set; }

        [Required] 
        public string Name { get; set; }

        [Required] 
        public string Description { get; set; }
        
        [Required] 
        public string Type { get; set; }

        public List<PrescriptionModel> Prescriptions { get; set; }

        public override string ToString()
        {
            return $"{IdMedicament}, {Name}, {Description}, {Type}";
        }
    }
}