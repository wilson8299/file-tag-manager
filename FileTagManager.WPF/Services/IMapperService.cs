using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileTagManager.WPF.Services
{
    public interface IMapperService
    {
        List<Tnew> Map<Told, Tnew>(IEnumerable<Told> oldData);
        Tnew MapSingle<Told, Tnew>(Told oldData);
        Task<List<Tnew>> MapAsync<Told, Tnew>(IEnumerable<Told> oldData);
        Task<Tnew> MapSingleAsync<Told, Tnew>(Told oldData);
    }
}
