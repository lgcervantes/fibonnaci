using System;
using System.Diagnostics;
using System.Numerics;

namespace FibonacciBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Precalentamiento del JIT Compiler de .NET
            // Esto evita que el tiempo de compilación inicial contamine el benchmark
            ObtenerFibonacciLineal(5);
            ObtenerFibonacciRapido(5);

            int[] casosDePrueba = { 10, 500, 1000 };

            Console.WriteLine("=== Benchmark de Multiplicación Matricial ===");
            Console.WriteLine(String.Format("{0,-10} | {1,-20} | {2,-20}", "N", "Tiempo Lineal O(N)", "Tiempo Rápido O(log N)"));
            Console.WriteLine(new String('-', 55));

            foreach (int n in casosDePrueba)
            {
                // Benchmark Método Lineal
                Stopwatch swLineal = Stopwatch.StartNew();
                BigInteger resultadoLineal = ObtenerFibonacciLineal(n);
                swLineal.Stop();

                // Benchmark Método Exponenciación Rápida
                Stopwatch swRapido = Stopwatch.StartNew();
                BigInteger resultadoRapido = ObtenerFibonacciRapido(n);
                swRapido.Stop();

                Console.WriteLine(String.Format("{0,-10} | {1,-20} | {2,-20}", 
                    n, 
                    $"{swLineal.ElapsedTicks} ticks", 
                    $"{swRapido.ElapsedTicks} ticks"));
            }
        }

        // ==========================================
        // ALGORITMO 1: LINEAL O(N)
        // ==========================================
        public static BigInteger ObtenerFibonacciLineal(int n)
        {
            if (n == 0) return 0;
            if (n == 1) return 1;

            BigInteger[,] matrizBase = {
                {0, 1},
                {1, 1}
            };

            BigInteger[,] resultado = RecursivoLineal(matrizBase, n - 1);
            return resultado[1, 1];
        }

        private static BigInteger[,] RecursivoLineal(BigInteger[,] LM, int n)
        {
            if (n <= 1) return LM;

            BigInteger[,] M = {
                {0, 1},
                {1, 1}
            };
            BigInteger[,] NM = new BigInteger[2, 2];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        NM[i, j] += M[i, k] * LM[k, j];
                    }
                }
            }
            return RecursivoLineal(NM, n - 1);
        }

        // ==========================================
        // ALGORITMO 2: EXPONENCIACIÓN RÁPIDA O(log N)
        // ==========================================
        public static BigInteger ObtenerFibonacciRapido(int n)
        {
            if (n == 0) return 0;

            BigInteger[,] matrizBase = {
                {1, 1},
                {1, 0} 
            };

            BigInteger[,] resultado = PotenciaMatriz(matrizBase, n - 1);
            return resultado[0, 0];
        }

        private static BigInteger[,] PotenciaMatriz(BigInteger[,] M, int n)
        {
            // Matriz identidad para n=0
            if (n == 0) return new BigInteger[,] { { 1, 0 }, { 0, 1 } };
            if (n == 1) return M;

            // Divide y Vencerás
            BigInteger[,] mitad = PotenciaMatriz(M, n / 2);
            BigInteger[,] mitadCuadrada = MultiplicarMatrices(mitad, mitad);

            if (n % 2 == 0)
                return mitadCuadrada;
            else
                return MultiplicarMatrices(M, mitadCuadrada);
        }

        private static BigInteger[,] MultiplicarMatrices(BigInteger[,] A, BigInteger[,] B)
        {
            BigInteger[,] resultado = new BigInteger[2, 2];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        resultado[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            return resultado;
        }
    }
}