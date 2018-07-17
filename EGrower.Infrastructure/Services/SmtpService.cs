using AutoMapper;
using EGrower.Core.Domains;
using EGrower.Infrastructure.DTO.EmailAccount;
using EGrower.Infrastructure.Repositories.Interfaces;
using EGrower.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGrower.Infrastructure.Services
{
    public class SmtpService : ISmtpService
    {
        private readonly IEmailAccountProtocolRepository<Smtp> _settingsRepository;
        private readonly IMapper _mapper;

        public SmtpService(IEmailAccountProtocolRepository<Smtp> settingsRepository, IMapper mapper)
        {
            _settingsRepository = settingsRepository;
            _mapper = mapper;
        }

        public async Task<SmtpDTO> GetAsyncByEmailProvider(string emailProvider)
        {
            var smtp = await _settingsRepository.GetAsyncByEmailProvider(emailProvider);

            return _mapper.Map<SmtpDTO>(smtp);
        }

        public async Task<SmtpDTO> GetAsyncByHost(string host)
        {
            var smtp = await _settingsRepository.GetAsyncByHost(host);

            return _mapper.Map<SmtpDTO>(smtp);
        }

        public async Task<SmtpDTO> GetAsyncById(int id)
        {
            var smtp = await _settingsRepository.GetAsyncById(id);

            return _mapper.Map<SmtpDTO>(smtp);
        }

        public async Task<SmtpDTO> GetAsyncByPort(int port)
        {
            var smtp = await _settingsRepository.GetAsyncByPort(port);

            return _mapper.Map<SmtpDTO>(smtp);
        }

        public async Task<IEnumerable<SmtpDTO>> BrowseAsync(string host = null)
        {
            var smtp = await _settingsRepository.BrowseAsync(host);

            return _mapper.Map<IEnumerable<SmtpDTO>>(smtp);
        }

        public async Task UpdateAsync(string host, int port, string emailProvider)
        {
            var smtp = await _settingsRepository.GetAsyncByHost(host);
           smtp.Update (port, host, emailProvider);
            await _settingsRepository.UpdateAsync(smtp);
        }

        public async Task DeleteAsync(int id)
        {
            var smtp = await _settingsRepository.GetAsyncById(id);
            await _settingsRepository.DeleteAsync(smtp);
        }
    }
}
