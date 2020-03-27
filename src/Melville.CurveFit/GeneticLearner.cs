using System;
using System.Diagnostics;
using System.Linq;

namespace Melville.CurveFit
{
    public class GeneticLearner: Learner
    {
        private readonly Func<int,double> sizes;
        private GeneticOption[] population;
        public int PopulationSize { get; set; } = 10000;
        public int QuarterPop => PopulationSize / 4;
        public int Generations { get; set; } = 100;
        public GeneticLearner(Func<double[], double> goodness, Func<int, double> sizes) : base(goodness)
        {
            this.sizes = sizes;
        }

        public override double[] Run(params double[] seed)
        {
            Debug.Assert(PopulationSize % 4 == 0);
            CreateInitialPopulation(seed);
            DoAllGenerations();
            Array.Sort(population);
            return population[0].Solution;
        }

        private void DoAllGenerations()
        {
            for (int i = 0; i < Generations; i++)
            {
                DoiSingleGeneration();
            }
        }

        private void DoiSingleGeneration()
        {
            Array.Sort(population);
            Debug.WriteLine($"({string.Join(", ", population[0].Solution.Select(k => k.ToString("####0.00")))}): {population[0].Goodness}");
            for (int i = 0; i < QuarterPop; i++)
            {
                InsertOption(QuarterPop + i, Mutate(population[i].Solution, sizes));
                Crossover(population[i].Solution, population[Rnd.Next(QuarterPop)].Solution, out var child1, out var child2);
                InsertOption((2 * QuarterPop) + i, child1);
                InsertOption((3 * QuarterPop) + i, child2);
            }
        }

        private void CreateInitialPopulation(double[] seed)
        {
            population = new GeneticOption[PopulationSize];
            for (int i = 0; i < PopulationSize; i++)
            {
                var sol = Mutate(seed, sizes);
                for (int j = 0; j < seed.Length; j++)
                {
                    sol = Mutate(sol, sizes);
                }
                InsertOption(i, sol);
            }
        }

        private void InsertOption(int index, double[] sol)
        {
            population[index] = new GeneticOption(sol, Goodness(sol));
        }

        private class GeneticOption: IComparable
        {
            public double[] Solution { get; }
            public double Goodness { get; }

            public GeneticOption(double[] solution, double goodness)
            {
                Solution = solution;
                Goodness = goodness;
            }

            public int CompareTo(object obj) => Goodness.CompareTo(((GeneticOption) obj).Goodness);
        }
    }
}