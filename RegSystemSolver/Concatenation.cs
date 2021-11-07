using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegSystemSolver
{
    /// <summary>
    /// Класс для конкатенации двух выражений
    /// </summary>
    class Concatenation
    {
        public object Left { get; private set; }
        public object Right { get; private set; }

        private Concatenation(object left, object right)
        {
            if (left is null || right is null) throw new Exception();
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Обёртка над конструктором с целью небольшой оптимизации
        /// При конкатенации с e возвращает другой параметр
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static object Make(object l, object r)
        {
            if (l is string s && s == "e") return r;
            if (r is string ss && ss == "e") return l;
            return new Concatenation(l, r);
        }

        public override string ToString()
        {
            var l = Left.ToString();
            var r = Right.ToString();

            if (Left is Union) l = $"({l})";
            if (Right is Union) r = $"({r})";
            return l + r;
        }
    }
}
