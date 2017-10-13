using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IdStuff
{
    public class MyUser : IUser<string>
    {
        public string Id { get; set; }

        public string UserName { get; set; }
    }
    
    public class MyUserStore : IUserStore<IUser, string>
    {
        public Task CreateAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IUser user)
        {
            throw new NotImplementedException();
        }
    }


    public class MyRole : IRole<string>
    {
        public string Id => throw new NotImplementedException();

        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public class MyRole2 : IRole
    {
        public string Id => throw new NotImplementedException();

        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public class MyRoleStore : IRoleStore<IRole, string>
    {
        public Task CreateAsync(IRole role)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IRole role)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IRole> FindByIdAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public Task<IRole> FindByNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IRole role)
        {
            throw new NotImplementedException();
        }
    }

    public class MyUserPasswordStore : IUserPasswordStore<IUser, string>
    {
        public Task CreateAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(IUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MyUserPasswordStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }

    public class MyUserManager : UserManager<IUser, string>
    {
        public MyUserManager(IUserStore<IUser, string> store) : base(store)
        {
        }
    }


}
