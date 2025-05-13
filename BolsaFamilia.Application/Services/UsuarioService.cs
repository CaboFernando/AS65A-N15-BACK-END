using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Domain.Entities;
using BolsaFamilia.Domain.Interfaces;

namespace BolsaFamilia.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task AdicionarAsync(UsuarioDto dto)
        {
            var exist = await _usuarioRepository.BuscarByCpf(dto.Cpf);

            if (exist == null) 
            {
                var user = new Usuario
                {
                    Nome = dto.Nome,
                    Cpf = dto.Cpf,
                };

                await _usuarioRepository.AdicionarAsync(user);
            }
        }

        public async Task AtualizarAsync(UsuarioDto dto)
        {
            var exist = await _usuarioRepository.BuscarByCpf(dto.Cpf);

            if (exist != null)
            {
                exist.Nome = dto.Nome;
                exist.Cpf = dto.Cpf;

                await _usuarioRepository.AtualizarAsync(exist);
            }
        }

        public async Task<UsuarioDto> BuscarByCpf(string cpf)
        {
            var exist = await _usuarioRepository.BuscarByCpf(cpf);

            if (exist != null)
            {
                return new UsuarioDto { Nome = exist.Nome, Cpf = exist.Cpf };
            }

            return null;
        }

        public async Task<UsuarioDto> BuscarById(int id)
        {
            var exist = await _usuarioRepository.BuscarById(id);

            if (exist != null)
            {
                return new UsuarioDto { Nome = exist.Nome, Cpf = exist.Cpf };
            }

            return null;
        }

        public async Task<IEnumerable<UsuarioDto>> ListarTodos()
        {
            var list = await _usuarioRepository.ListarTodos();

            return list.Select(x => new UsuarioDto { Nome = x.Nome, Cpf = x.Cpf });
        }

        public async Task RemoverAsync(string cpf)
        {
            var exist = await _usuarioRepository.BuscarByCpf(cpf);

            if (exist != null)
            {
                await _usuarioRepository.RemoverAsync(exist);
            }
        }
    }
}
