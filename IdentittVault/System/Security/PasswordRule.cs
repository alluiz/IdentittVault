using System;
using System.Text.RegularExpressions;

namespace IdentittVault.System.Security
{
    internal class PasswordRule
    {
        public PasswordRule(string name, string message, Func<string, bool> validateFunction)
        {
            regexMode = false;
            this.validateFunction = validateFunction;
            Message = message;
            Name = name;
        }

        public PasswordRule(string name, string message, string pattern)
        {
            regexMode = true;
            regex = new Regex(pattern);
            Message = message;
            Name = name;
        }

        private readonly bool regexMode;
        private readonly Func<string, bool> validateFunction;
        private readonly Regex regex;

        public string Message { get; internal set; }
        public string Name { get; internal set; }

        public bool Match(string password)
        {
            if (regexMode)
                return this.regex.IsMatch(password);
            else
                return this.validateFunction(password);
        }
    }
}