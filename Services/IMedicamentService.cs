using System.Threading.Tasks;

namespace PrescriptionAPI.Services
{
    public interface IMedicamentService
    {
        Task<MedicamentModel> GetMedicament(int medicamentId);
    }
}