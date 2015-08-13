using System;
using System.Threading.Tasks;

using bgle.Contracts.DateTimeHandling;
using bgle.Contracts.ObjectMapper;
using bgle.Core.Identity;
using bgle.CQRS.Command.Identity;
using bgle.CQRS.CommandDispatcher;
using bgle.CQRS.Query.Identity;
using bgle.CQRS.QueryDispatcher;
using bgle.CQRS.QueryResult.Identity;

using Microsoft.AspNet.Identity;

namespace bgle.AspNet.Identity.CQRS
{
    public class BgleUserStore<TUser> : IUserStore<TUser, string>,
                                 IUserPasswordStore<TUser, string>,
                                 IUserEmailStore<TUser, string>,
                                 IUserSecurityStampStore<TUser, string>,
                                 IUserLockoutStore<TUser, string>
        where TUser : class, IUser<string>, IIdentityUser<string>

    {
        private readonly ICommandDispatcher commandDispatcher;

        private readonly IQueryDispatcher queryDispatcher;

        private readonly IObjectMapper objectMapper;

        private readonly IDateTimeProvider dateTimeProvider;

        protected bool IsDisposed { get; private set; }

        public BgleUserStore(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IObjectMapper objectMapper, IDateTimeProvider dateTimeProvider)
        {
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
            this.objectMapper = objectMapper;
            this.dateTimeProvider = dateTimeProvider;
            this.IsDisposed = false;
        }

        #region IUserStore

        public Task CreateAsync(TUser user)
        {
            this.ThrowIfDisposed();
            var iidentiy = (IIdentityUser<string>)user;
            var createUserCommand = new CreateUserCommand(iidentiy.UserName, iidentiy.PasswordHash, iidentiy.Email)
                                        {
                                            EmailConfirmed = false,
                                            AccessFailedCount = 0,
                                            LockoutEnabled = true,
                                        };
            var result = this.commandDispatcher.Dispatch(createUserCommand);
            return Task.FromResult(result.IsValid);
        }

        public Task UpdateAsync(TUser user)
        {
            this.ThrowIfDisposed();
            var iidentiy = (IIdentityUser<string>)user;
            var createUserCommand = new UpdateUserCommand(iidentiy.Id)
            {
                PasswordHash = user.PasswordHash,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                AccessFailedCount = 0,
                LockoutEnabled = true,
                LockoutEndDate = user.LockoutEndDate,
                SecurityStamp = user.SecurityStamp,
            };
            var result = this.commandDispatcher.Dispatch(createUserCommand);
            return Task.FromResult(result.IsValid);
        }

        public Task DeleteAsync(TUser user)
        {
            this.ThrowIfDisposed();
            var iidentiy = (IIdentityUser<string>)user;
            var result = this.commandDispatcher.Dispatch(new DeleteUserCommand(iidentiy.Id));
            return Task.FromResult(result.IsValid);
        }

        public async Task<TUser> FindByIdAsync(string userId)
        {
            this.ThrowIfDisposed();
            var query = new UserByIdQuery(userId);
            var queryResult = await this.queryDispatcher.DispatchAsync<UserByIdQuery, UserQueryResult>(query);
            var user = this.objectMapper.Map<UserQueryResult, TUser>(queryResult);
            return user;
        }

        public async Task<TUser> FindByNameAsync(string userName)
        {
            this.ThrowIfDisposed();
            this.ThrowIfDisposed();
            var query = new UserByNameQuery(userName);
            var queryResult = await this.queryDispatcher.DispatchAsync<UserByNameQuery, UserQueryResult>(query);
            var user = this.objectMapper.Map<UserQueryResult, TUser>(queryResult);
            return user;
        }
        
        #endregion

        #region IUserPasswordStore

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            this.ThrowIfDisposed();
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            this.ThrowIfDisposed();
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            this.ThrowIfDisposed();
            return Task.FromResult(user.PasswordHash != null);
        }

        #endregion

        #region IUserEmailStore

        public Task SetEmailAsync(TUser user, string email)
        {
            this.ThrowIfDisposed();
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            this.ThrowIfDisposed();
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            this.ThrowIfDisposed();
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            this.ThrowIfDisposed();
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public async Task<TUser> FindByEmailAsync(string email)
        {
            this.ThrowIfDisposed();
            this.ThrowIfDisposed();
            var query = new UserByEmailQuery(email);
            var queryResult = await this.queryDispatcher.DispatchAsync<UserByEmailQuery, UserQueryResult>(query);
            var user = this.objectMapper.Map<UserQueryResult, TUser>(queryResult);
            return user;
        }

        #endregion

        #region IUserSecurityStampStore

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            this.ThrowIfDisposed();
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            this.ThrowIfDisposed();
            return Task.FromResult(user.SecurityStamp);
        }

        #endregion

        #region IUserLockoutStore

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            this.ThrowIfDisposed();

            var lockoutEndDate = user.LockoutEndDate ?? new DateTimeOffset(this.dateTimeProvider.Epoch);
            return Task.FromResult(lockoutEndDate);
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            this.ThrowIfDisposed();
            user.LockoutEndDate = lockoutEnd;
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            this.ThrowIfDisposed();
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            this.ThrowIfDisposed();
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            this.ThrowIfDisposed();
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            this.ThrowIfDisposed();
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            this.ThrowIfDisposed();
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            this.IsDisposed = true;
            if (disposing)
            {
            }
        }

        #endregion

        private void ThrowIfDisposed()
        {
            if (this.IsDisposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

    }

    
}
