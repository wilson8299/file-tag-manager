using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileTagManager.WPF.Services
{
    public class MapperService : IMapperService
    {
        public List<Tnew> Map<Told, Tnew>(IEnumerable<Told> oldData)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Told, Tnew>());
            var mapper = config.CreateMapper();
            return mapper.Map<List<Tnew>>(oldData);
        }

        public Tnew MapSingle<Told, Tnew>(Told oldData)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Told, Tnew>());
            var mapper = config.CreateMapper();
            return mapper.Map<Tnew>(oldData);
        }

        public async Task<List<Tnew>> MapAsync<Told, Tnew>(IEnumerable<Told> oldData)
        {
            return await Task.Run(() => Map<Told, Tnew>(oldData));
        }

        public async Task<Tnew> MapSingleAsync<Told, Tnew>(Told oldData)
        {
            return await Task.Run(() => MapSingle<Told, Tnew>(oldData));
        }
    }
}
