using System;
using System.Collections.Generic;
using System.Linq;

namespace RegSystemSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            // составляем систему уравнений
            // грамматика имеет вид
            // A(i, j) -> aA((i + 1) mod 3, j) | bA(i, (j + 1) mod 5)
            // для всех (i, j) \in {0, 1, 2} \times {0, 1, 2, 3, 4}
            // плюс правило
            // A(0, 0) -> e
            List<Equation<(int, int)>> syseqs = new();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 5; j++)
                {
                    List<((int, int), object)> comps = new()
                    {
                        (((i + 1) % 3, j), "a"),
                        ((i, (j + 1) % 5), "b"),
                    };
                    object free = null;
                    if (i == 0 && j == 0)
                        free = "e";
                    syseqs.Add(new Equation<(int, int)>((i, j), free, comps.ToArray()));
                }
            // загружаем систему в класс
            var sys = new EquationSystem<(int, int)>(syseqs.Select(e => (e.Left, e)).ToArray());
            // решаем относительно (0, 0) подробнее см. метод EquationSystem::Solve
            Console.WriteLine(sys.Solve((0, 0)));
            Console.ReadKey();
        }
    }
}
