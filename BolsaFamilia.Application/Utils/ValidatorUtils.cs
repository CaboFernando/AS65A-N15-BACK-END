using System.Text.RegularExpressions;

namespace BolsaFamilia.Application.Utils
{
    public static class ValidadorUtils
    {
        private static readonly Regex cpfRegex = new Regex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$");
        private static readonly Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        public static bool CpfValido(string cpf) => !string.IsNullOrWhiteSpace(cpf) && cpfRegex.IsMatch(cpf);

        public static bool EmailValido(string email) => !string.IsNullOrWhiteSpace(email) && emailRegex.IsMatch(email);
    }
}
