using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegSystemSolver
{
    /// <summary>
    /// Замыкание Клини
    /// </summary>
    class Star
    {
        public object Operand { get; private set; }

        private Star(object operand)
        {
            if (operand is null) throw new Exception();
            Operand = operand;
        }

        /// <summary>
        /// Обёртка над конструктором. При итерации e возвращает саму e
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static object Make(object operand)
        {
            if (operand is string s && s == "e")
                return "e";
            return new Star(operand);
        }

        public override string ToString()
        {
            var opres = Operand.ToString();
            if (Operand is Concatenation || Operand is Union)
                return $"({opres})*";
            else
                return $"{opres}*";
        }
    }
}
