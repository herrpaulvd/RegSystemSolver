using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegSystemSolver
{
    /// <summary>
    /// Класс уравнения с регулярными коэффициентами
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Equation<T> where T : IComparable<T>
    {
        /// <summary>
        /// Левая часть уравнения
        /// </summary>
        public T Left { get; private set; }

        /// <summary>
        /// Правая часть уравнения
        /// </summary>
        public SortedDictionary<T, object> Right { get; private set; } = new();
        /// <summary>
        /// Свободный коэффициент правой части уравнения
        /// </summary>
        public object FreeCoeff { get; private set; }

        /// <summary>
        /// Добавление к коэффициенту при переменной
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="expr"></param>
        public void Unite(T variable, object expr)
        {
            if (Right.ContainsKey(variable))
                Right[variable] = Union.Make(Right[variable], expr);
            else
                Right.Add(variable, expr);
        }

        /// <summary>
        /// Добавление к свободному коэффициенту
        /// </summary>
        /// <param name="expr"></param>
        public void UniteFree(object expr)
        {
            if (expr is null) return;
            if (FreeCoeff is null)
                FreeCoeff = expr;
            else
                FreeCoeff = Union.Make(FreeCoeff, expr);
        }

        public Equation(T left, object freeCoeff, params (T, object)[] right)
        {
            Left = left;
            FreeCoeff = freeCoeff;
            foreach (var (k, v) in right)
                Right.Add(k, v);
        }

        public Equation<T> Copy()
        {
            return new(Left, FreeCoeff, Right.Select(kv => (kv.Key, kv.Value)).ToArray());
        }
    }
}
