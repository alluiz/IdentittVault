using System.Collections.Generic;

namespace IdentittVault.System.Security
{
    public class PasswordResult
    {
        public PasswordResult()
        {
            Errors = new();
        }

        public Dictionary<string, string> Errors { get; set; }
        public bool Success
        {
            get
            {
                return this.Errors.Count == 0;
            }
        }

        public void AddError(string key, string error)
        {
            this.Errors.Add(key, error);
        }
    }
}
