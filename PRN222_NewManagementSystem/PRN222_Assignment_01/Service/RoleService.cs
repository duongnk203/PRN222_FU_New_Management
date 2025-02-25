using Microsoft.AspNetCore.Mvc.Rendering;

namespace PRN222_Assignment_01.Service
{
    public class RoleService
    {
        private readonly Dictionary<int, string> _roleDictionary;
        private readonly Dictionary<string, int> _roleMapping;
        public RoleService(IConfiguration configuration)
        {
            _roleMapping = configuration.GetSection("AccountRole").Get<Dictionary<string, int>>();
            _roleDictionary = _roleMapping.ToDictionary(kv => kv.Value, kv => kv.Key);
        }

        public string GetRoleName(int roleID)
        {
            return _roleDictionary.ContainsKey(roleID) ? _roleDictionary[roleID] : "Unknown";
        }

        public List<SelectListItem> GetRoles()
        {
            return _roleMapping.Where(r => !r.Key.Equals("Admin")).Select(r => new SelectListItem
            {
                Value = r.Value.ToString(),
                Text = r.Key
            }).ToList();
            
        }
    }
}
