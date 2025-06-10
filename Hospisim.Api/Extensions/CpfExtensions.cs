using System;
using System.Linq;

namespace Hospisim.Api.Extensions
{
    public static class CpfExtensions
    {
        public static string ToCPFFormat(this string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return cpf;

            var onlyNumbers = new string(cpf.Where(char.IsDigit).ToArray());

            if (onlyNumbers.Length != 11)
                return cpf; 
           
            return Convert.ToUInt64(onlyNumbers).ToString(@"000\.000\.000\-00");
        }
    }
}
