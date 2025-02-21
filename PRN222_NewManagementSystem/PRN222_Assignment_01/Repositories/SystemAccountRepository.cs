using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.ViewModel;

namespace PRN222_Assignment_01.Repositories
{

    public interface ISystemAccountRepository
    {
        List<SystemAccount> GetAccounts(out string message);
        void CreateAccount(SystemAccount newAccount, out string message);
        void UpdateAccount(int id, SystemAccount updateAccount, out string message);
        void DeleteAccount(int id, out string message);
        List<SystemAccount> SearchAccount(string searchNameAccount);
        int Login(AccountLogin accountLogin, out string message);
        SystemAccount GetAccount(int id, out string message);
        List<int> GetAccountIds(out string message);
    }

    public class SystemAccountRepository : ISystemAccountRepository
    {
        private string EMAIL_ADMIN;
        private string PASSWORD_ADMIN;
        private readonly FUNewsManagementContext _context;
        private readonly IConfiguration _configuration;

        public SystemAccountRepository(FUNewsManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            EMAIL_ADMIN = _configuration.GetSection("AdminAccount")["Email"];
            PASSWORD_ADMIN = _configuration.GetSection("AdminAccount")["Password"];
        }

        public void CreateAccount(SystemAccount newAccount, out string message)
        {
            message = "";
            if (newAccount == null)
            {
                message = "Account is invalid!";
                return;
            }
            else
            {
                if (IsEmailExit(newAccount.AccountEmail) || newAccount.AccountEmail.Equals(EMAIL_ADMIN))
                {
                    message = "Email is exist!";
                    return;
                }
                _context.Add<SystemAccount>(newAccount);
                _context.SaveChanges();
            }
        }

        public void DeleteAccount(int id, out string message)
        {
            message = "";
            if (id == 0)
            {
                message = "AccountId is invalid!";
                return;
            }
            SystemAccount account = _context.SystemAccounts.FirstOrDefault(x => x.AccountID == id);
            if (account == null)
            {
                message = "Account is not exist!";
                return;
            }
            _context.Remove<SystemAccount>(account);
            _context.SaveChanges();
        }

        public List<SystemAccount> GetAccounts(out string message)
        {
            message = "";
            List<SystemAccount> accounts = _context.SystemAccounts.ToList();
            if (accounts.Count == 0)
            {
                message = "Danh sách tài khoản trống!";
                return accounts;
            }
            return accounts;
        }

        private bool IsEmailExit(string email)
        {
            if (email.IsNullOrEmpty()) return false;
            return _context.SystemAccounts.FirstOrDefault(x => x.AccountEmail.Equals(email)) != null;
        }

        public List<SystemAccount> SearchAccount(string searchNameAccount)
        {
            throw new NotImplementedException();
        }

        public void UpdateAccount(int id, SystemAccount updateAccount, out string message)
        {
            message = "";
            var account = _context.SystemAccounts.FirstOrDefault(y => y.AccountID == id);
            if (account == null)
            {
                message = "Account is not exist!";
                return;
            }
            if (!account.AccountEmail.Equals(updateAccount.AccountEmail))
            {
                if (IsEmailExit(updateAccount.AccountEmail))
                {
                    message = "Email is exist!";
                    return;
                }
                account.AccountEmail = updateAccount.AccountEmail;
            }
            account.AccountName = updateAccount.AccountName;
            account.AccountRole = updateAccount.AccountRole;
            _context.SystemAccounts.Update(account);
            _context.SaveChanges();
        }

        public int Login(AccountLogin accountLogin, out string message)
        {
            message = "";
            if (accountLogin == null)
            {
                message = "Tài khoản không đúng!";
                return 0;
            }

            bool checkEmailAdmin = accountLogin.AccountEmail.Equals(EMAIL_ADMIN);
            if (!IsEmailExit(accountLogin.AccountEmail) && !checkEmailAdmin)
            {
                message = "Email không tồn tại!";
                return 0;
            }

            bool checkPassword = accountLogin.Password.Equals(PASSWORD_ADMIN);
            if (checkEmailAdmin && checkPassword)
            {
                message = "";
                return 3;
            }
            else
            {
                var account = _context.SystemAccounts.FirstOrDefault(x => x.AccountEmail.Equals(accountLogin.AccountEmail) && x.AccountPassword.Equals(accountLogin.Password));
                if (account == null)
                {
                    message = "Mật khẩu không đúng!";
                    return 0;
                }
                else
                {
                    message = "";
                    return account.AccountRole;
                }
            }
        }

        public SystemAccount GetAccount(int id, out string message)
        {
            var account = new SystemAccount();
            message = "";
            if (id == 0)
            {
                message = "AccountID is invalid!";
                return account;
            }
            else
            {
                account = _context.SystemAccounts.FirstOrDefault(x => x.AccountID == id);
                if (account == null)
                {
                    message = "Account is not exits!";
                }
                return account;
            }
        }

        public List<int> GetAccountIds(out string message)
        {
            message = "";
            List<int> getAccountIds = _context.SystemAccounts.Select(x => x.AccountID).ToList().ConvertAll(x => (int)x);
            if (getAccountIds.Count == 0)
                message = "The list account is empty";
            return getAccountIds;
        }
    }
}
