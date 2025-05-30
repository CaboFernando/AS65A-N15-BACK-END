﻿using BolsaFamilia.Domain.Entities;

namespace BolsaFamilia.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> ListarTodos();
        Task<Usuario> BuscarById(int id);
        Task<Usuario> BuscarByCpf(string cpf);
        Task<Usuario> BuscarByEmail(string email);
        Task AdicionarAsync(Usuario user);
        Task AtualizarAsync(Usuario user);
        Task RemoverAsync(Usuario user);
    }
}
