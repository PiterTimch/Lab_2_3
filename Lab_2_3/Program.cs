using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2_3
{
	public enum Point { 
		Female, 
		Male 
	}

	public enum Animals
	{
		Pisces,
		Beasts,
		Birds
    }

	public class Vertebrates
	{
		public Point Gender { get; set; }
		public string Name { get; set; }
		public double PopulationRating { get; set; }

		public Vertebrates(Point gender, string name, double populationRating)
		{
			Gender = gender;
			Name = name;
			PopulationRating = populationRating;
		}

		public Vertebrates()
		{
			Gender = Point.Female;
			Name = "Unknown";
			PopulationRating = 0.0;
		}

		public override string ToString()
		{
			return $"Name: {Name}, Point: {Gender}, PopulationRating: {PopulationRating:F2}";
		}
	}

	public class Classification
	{
		private string _groupName;
		private Animals _subgroup;
		private string _habitat;
		private double _researchedSpeciesCount;
		private Vertebrates[] _animals;

		public Classification(string groupName, Animals subgroup, string habitat, double researchedSpeciesCount)
		{
			_groupName = groupName;
			_subgroup = subgroup;
			_habitat = habitat;
			_researchedSpeciesCount = researchedSpeciesCount;
			_animals = Array.Empty<Vertebrates>();
		}

		public Classification()
		{
			_groupName = "Unknown Group";
			_subgroup = Animals.Beasts;
			_habitat = "Unknown";
			_researchedSpeciesCount = 0.0;
			_animals = Array.Empty<Vertebrates>();
		}

		public string GroupName
		{
			get => _groupName;
			set => _groupName = value;
		}

		public Animals Subgroup
		{
			get => _subgroup;
			set => _subgroup = value;
		}

		public string Habitat
		{
			get => _habitat;
			set => _habitat = value;
		}

		public double ResearchedSpeciesCount
		{
			get => _researchedSpeciesCount;
			set => _researchedSpeciesCount = value;
		}

		public Vertebrates[] AnimalsList
		{
			get => _animals;
			set => _animals = value ?? Array.Empty<Vertebrates>();
		}

		public double AveragePopulationRating
		{
			get
			{
				if (_animals == null || _animals.Length == 0) return 0.0;
				double sum = 0.0;
				for (int i = 0; i < _animals.Length; i++) sum += _animals[i].PopulationRating;
				return sum / _animals.Length;
			}
		}

		public bool this[Animals subgroup]
		{
			get => _subgroup == subgroup;
		}

		public void AddVertebrates(params Vertebrates[] items)
		{
			if (items == null || items.Length == 0) return;
			if (_animals == null || _animals.Length == 0)
			{
				_animals = items.ToArray();
				return;
			}
			var result = new Vertebrates[_animals.Length + items.Length];
			Array.Copy(_animals, result, _animals.Length);
			Array.Copy(items, 0, result, _animals.Length, items.Length);
			_animals = result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"Group: {_groupName}");
			sb.AppendLine($"Subgroup: {_subgroup}");
			sb.AppendLine($"Habitat: {_habitat}");
			sb.AppendLine($"Researched species: {_researchedSpeciesCount}");
			sb.AppendLine($"Animals ({_animals?.Length ?? 0}):");
			if (_animals != null)
			{
				for (int i = 0; i < _animals.Length; i++)
				{
					sb.AppendLine("  - " + _animals[i].ToString());
				}
			}
			return sb.ToString();
		}

		public string ToShortString()
		{
			return $"Group: {_groupName}, Subgroup: {_subgroup}, Habitat: {_habitat}, Researched species: {_researchedSpeciesCount}, Avg rating: {AveragePopulationRating:F2}";
		}
	}

    internal class Program
    {
        static void Main(string[] args)
        {
			var classification = new Classification();
			Console.WriteLine(classification.ToShortString());
			Console.WriteLine();

			Console.WriteLine($"Idx Pisces: {classification[Animals.Pisces]}");
			Console.WriteLine($"Idx Beasts: {classification[Animals.Beasts]}");
			Console.WriteLine($"Idx Birds: {classification[Animals.Birds]}");
			Console.WriteLine();

			classification.GroupName = "Vertebrate Study";
			classification.Subgroup = Animals.Birds;
			classification.Habitat = "Forests";
			classification.ResearchedSpeciesCount = 123.0;
			classification.AnimalsList = new[]
			{
				new Vertebrates(Point.Male, "Eagle", 82.5),
				new Vertebrates(Point.Female, "Sparrow", 63.2)
			};
			Console.WriteLine(classification.ToString());

			classification.AddVertebrates(
				new Vertebrates(Point.Male, "Hawk", 74.1),
				new Vertebrates(Point.Female, "Owl", 69.7)
			);
			Console.WriteLine(classification.ToString());

			const int dimension = 600;
			const int total = dimension * dimension;

			var testAnimal = new Vertebrates(Point.Male, "testAnimal", 50.0);

			var arr1D = new Vertebrates[total];
			var sw = Stopwatch.StartNew();
			for (int i = 0; i < arr1D.Length; i++) arr1D[i] = testAnimal;
			double sum1 = 0;
			for (int i = 0; i < arr1D.Length; i++) sum1 += arr1D[i].PopulationRating;
			sw.Stop();
			Console.WriteLine($"1D array: elements={arr1D.Length}, time={sw.ElapsedMilliseconds} ms, sum={sum1}");

			var arr2D = new Vertebrates[dimension, dimension];
			sw.Restart();
			for (int i = 0; i < dimension; i++)
				for (int j = 0; j < dimension; j++)
					arr2D[i, j] = testAnimal;
			double sum2 = 0;
			for (int i = 0; i < dimension; i++)
				for (int j = 0; j < dimension; j++)
					sum2 += arr2D[i, j].PopulationRating;
			sw.Stop();
			Console.WriteLine($"2D rectangular: elements={total}, time={sw.ElapsedMilliseconds} ms, sum={sum2}");

			var arrJagged = new Vertebrates[dimension][];
			sw.Restart();
			for (int i = 0; i < dimension; i++) arrJagged[i] = new Vertebrates[dimension];
			for (int i = 0; i < dimension; i++)
				for (int j = 0; j < dimension; j++)
					arrJagged[i][j] = testAnimal;
			double sum3 = 0;
			for (int i = 0; i < dimension; i++)
				for (int j = 0; j < dimension; j++)
					sum3 += arrJagged[i][j].PopulationRating;
			sw.Stop();
			
			Console.WriteLine($"2D jagged: elements={total}, time={sw.ElapsedMilliseconds} ms, sum={sum3}");

			Console.WriteLine("\nPress any key to exit...");
			Console.ReadKey();
		}
    }
}
