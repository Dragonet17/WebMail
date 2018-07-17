using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EGrower.Core.Domains;
using EGrower.Infrastructure.DTO.EmailAccount;
using EGrower.Infrastructure.Repositories.Interfaces;
using EGrower.Infrastructure.Services.Interfaces;

namespace EGrower.Infrastructure.Services {
    public class IMapService : IIMapService {
        private readonly IEmailAccountProtocolRepository<Imap> _settingsRepository;
        private readonly IMapper _mapper;

        public IMapService (IEmailAccountProtocolRepository<Imap> settingsRepository, IMapper mapper) {
            _settingsRepository = settingsRepository;
            _mapper = mapper;
        }

        public async Task<IMapDTO> GetAsyncByEmailProvider (string emailProvider) {
            var iMap = await _settingsRepository.GetAsyncByEmailProvider (emailProvider);

            return _mapper.Map<IMapDTO> (iMap);
        }

        public async Task<IMapDTO> GetAsyncByHost (string host) {
            var iMap = await _settingsRepository.GetAsyncByHost (host);

            return _mapper.Map<IMapDTO> (iMap);
        }

        public async Task<IMapDTO> GetAsyncById (int id) {
            var iMap = await _settingsRepository.GetAsyncById (id);

            return _mapper.Map<IMapDTO> (iMap);
        }

        public async Task<IMapDTO> GetAsyncByPort (int port) {
            var iMap = await _settingsRepository.GetAsyncByPort (port);

            return _mapper.Map<IMapDTO> (iMap);
        }

        public async Task<IEnumerable<IMapDTO>> BrowseAsync (string host = null) {
            var iMap = await _settingsRepository.BrowseAsync (host);

            return _mapper.Map<IEnumerable<IMapDTO>> (iMap);
        }

        public async Task UpdateAsync (string host, int port, string emailProvider) {
            var iMap = await _settingsRepository.GetAsyncByHost (host);
            iMap.Update (port, host, emailProvider);
            await _settingsRepository.UpdateAsync (iMap);
        }

        public async Task DeleteAsync (int id) {
            var iMap = await _settingsRepository.GetAsyncById (id);
            await _settingsRepository.DeleteAsync (iMap);
        }
    }
}