using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegSystemSolver
{
    /// <summary>
    /// Объединение (+)
    /// </summary>
    class Union
    {
        public object Left { get; private set; }
        public object Right { get; private set; }

        private Union(object left, object right)
        {
            if (left is null || right is null) throw new Exception();
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Обёртка над конструктором с целью оптимизации.
        /// Если объединяются e и A*, возвращает A*
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static object Make(object l, object r)
        {
            if (l is string s && s == "e" && r is Star)
                return r;
            if (r is string ss && ss == "e" && l is Star)
                return l;
            return new Union(l, r);
        }

        public override string ToString()
        {
            return Left.ToString() + "+" + Right.ToString();
        }
    }
}
