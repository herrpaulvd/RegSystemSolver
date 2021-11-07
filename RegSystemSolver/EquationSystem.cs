using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegSystemSolver
{
    /// <summary>
    /// Система уравнений с регулярными коэффициентами
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class EquationSystem<T> where T : IComparable<T>
    {
        /// <summary>
        /// собственно уравнения
        /// </summary>
        private SortedDictionary<T, Equation<T>> equations = new();

        public Equation<T> EquationByLeft(T left)
            => equations.ContainsKey(left) ? equations[left] : null;

        public EquationSystem(params (T, Equation<T>)[] equations)
        {
            foreach (var (k, v) in equations)
                this.equations.Add(k, v);
        }

        /// <summary>
        /// Решение системы. Возвращает регулярное выражение,
        /// являющееся решением относительно переменной start
        /// Здесь реализуется, таким образом, только прямой ход метода Гаусса
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public object Solve(T start)
        {
            // порядок решения: всё, кроме start, затем start
            var order = equations.Keys.Where(e => e.CompareTo(start) != 0).Append(start).ToArray();

            // уравнения, уже обработанные прямым ходом
            SortedDictionary<T, Equation<T>> solved = new();

            // собственно прямой ход
            foreach(var y in order)
            {
                var eq = equations[y].Copy(); // создаём копию исходного уравнения
                // её мы будем изменять
                // пока среди переменных правой части уравнения есть те,
                // которые были левыми частями уравнений уже обработанных прямым ходом
                // будем их заменять в текущем уравнении
                while(eq.Right.Keys.Any(solved.ContainsKey))
                {
                    // собственно заменяемая переменная
                    T toReplace = eq.Right.Keys.First(solved.ContainsKey);
                    // коэффициент при этой переменной
                    var coeff = eq.Right[toReplace];
                    // удаляем переменную из уравнения
                    eq.Right.Remove(toReplace);
                    // добавляем в уравнение новые члены, получаемые из соответствующего уравнения
                    // добавить нужно коэффициенты при них в виде
                    // коэффициент в предыдущей версии текущего уравнения (coeff)
                    // конкатенация
                    // коэффициент в обработанном уравнении
                    foreach (var kv in solved[toReplace].Right)
                        eq.Unite(kv.Key, Concatenation.Make(coeff, kv.Value));
                    // аналогично добавить к свободному коэффициенту
                    if (solved[toReplace].FreeCoeff is object)
                        eq.UniteFree(Concatenation.Make(coeff, solved[toReplace].FreeCoeff));
                }

                // если переменная из левой части уравнения встречается в правой
                if (eq.Right.ContainsKey(y))
                {
                    var a = Star.Make(eq.Right[y]); // берём коэффициент при ней и делаем замыкание Клини
                    eq.Right.Remove(y); // удаляем из уравнения
                    // заменяем все коэффициенты b при других переменных на ab (a --- уже *)
                    foreach (var k in eq.Right.Keys.ToArray())
                        eq.Right[k] = Concatenation.Make(a, eq.Right[k]);
                    // то же самое со свободным коэффициентом
                    if(eq.FreeCoeff is object)
                        eq.UniteFree(Concatenation.Make(a, eq.FreeCoeff));
                }
                // текущее уравнение обработано
                solved.Add(y, eq);
            }

            // поскольку последним обрабатывается уравнение при переменной start
            // правая часть не будет содержать переменных
            // поэтому достаточно вернуть свободный коэффициент обработанного уравнения
            return solved[start].FreeCoeff;
        }
    }
}
