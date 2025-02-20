using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.ViewModel;

namespace PRN222_Assignment_01.Repositories
{


    public interface ISystemAccountRepository
    {
        List<SystemAccount> GetAccounts(out string message);
        void CreateAccount(SystemAccount newAccount, out string message);
        void UpdateAccount(string email, SystemAccount updateAccount);
        void DeleteAccount(string email);
        List<SystemAccount> SearchAccount(string searchNameAccount);
        int Login(AccountLogin accountLogin, out string message);
    }

    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly FunewsManagementContext _context;
        private readonly IConfiguration _configuration;

        public SystemAccountRepository(FunewsManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
                _context.Add<SystemAccount>(newAccount);
                _context.SaveChanges();
            }
        }

        public void DeleteAccount(string email)
        {
            if (email.IsNullOrEmpty())
            {

            }
            else
            {
                SystemAccount account = _context.SystemAccounts.Find(email);
                if (account == null)
                {

                }
                else
                {
                    _context.Remove<SystemAccount>(account);
                    _context.SaveChanges();
                }
            }
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

        public void UpdateAccount(string email, SystemAccount updateAccount)
        {
            throw new NotImplementedException();
        }

        public int Login(AccountLogin accountLogin, out string message)
        {
            message = "";
            string email = _configuration.GetSection("AdminAccount")["Email"];
            string password = _configuration.GetSection("AdminAccount")["Password"];
            if (accountLogin == null)
            {
                message = "Tài khoản không đúng!";
                return 0;
            }

            bool checkEmailAdmin = accountLogin.AccountEmail.Equals(email);
            if (!IsEmailExit(accountLogin.AccountEmail) && !checkEmailAdmin)
            {
                message = "Email không tồn tại!";
                return 0;
            }

            bool checkPassword = accountLogin.Password.Equals(password);
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
    }
}
