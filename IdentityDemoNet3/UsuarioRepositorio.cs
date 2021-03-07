using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityDemoNet3
{
    public class UsuarioRepositorio : IUserStore<Usuario>, IUserPasswordStore<Usuario>
    {

        public static DbConnection GetOpenConnection()
        {
            var connection = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;" +
                "database=IdentityDemoNet3;" +
                "trusted_connection=yes;");
            connection.Open();

            return connection;

        }
        public async Task<IdentityResult> CreateAsync(Usuario user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "insert into Usuarios(id,Descripcion,DescripcionNormalizada,ContraseñaHash) " +
                    "Values(@id,@descripcion,@descripcionNormalizada,@contraseñaHash)",
                    new
                    {
                        id = user.Id,
                        descripcion = user.Descripcion,
                        descripcionNormalizada = user.DescripcionNormalizada,
                        contraseñaHash = user.ContraseñaHash
                    }
                    );
            }

            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(Usuario user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
      
        }

        public async Task<Usuario> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connetion = GetOpenConnection())
            {
                return await connetion.QueryFirstOrDefaultAsync<Usuario>(
                    "select * from Usuarios where Id = @id", new { id = userId });
            }
        }

        public async Task<Usuario> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connetion = GetOpenConnection())
            {
                return await connetion.QueryFirstOrDefaultAsync<Usuario>(
                    "select * from Usuarios where DescripcionNormalizada = @descripcion", new { descripcion = normalizedUserName });
            }
        }

        public Task<string> GetNormalizedUserNameAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.DescripcionNormalizada);
        }

        public Task<string> GetPasswordHashAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.ContraseñaHash);
        }

        public Task<string> GetUserIdAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Descripcion);
        }

        public Task<bool> HasPasswordAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.ContraseñaHash != null);
        }

        public Task SetNormalizedUserNameAsync(Usuario user, string normalizedName, CancellationToken cancellationToken)
        {
            user.DescripcionNormalizada = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(Usuario user, string passwordHash, CancellationToken cancellationToken)
        {
            user.ContraseñaHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(Usuario user, string userName, CancellationToken cancellationToken)
        {
            user.Descripcion = user.Descripcion;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(Usuario user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "update PluralsightUsers set " +
                    "id = @id," +
                    "Descripcion = @descripcion," +
                    "DescripcionNormalizada = @descripcionNormalizada," +
                    "ContraseñaHash =@contraseñaHash where Id =  @id ",
                    new
                    {
                        id = user.Id,
                        descripcion = user.Descripcion,
                        descripcionNormalizada = user.DescripcionNormalizada,
                        contraseñaHash = user.ContraseñaHash
                    }
                    );
            }

            return IdentityResult.Success;
        }
    }
}
