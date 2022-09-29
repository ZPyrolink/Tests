using System.Collections.ObjectModel;
using GenerateParentheses;

namespace Tests;

internal class ParenthesesTest
{
	/// <summary>
	/// Program output when good parentheses
	/// </summary>
	private const string _OK = "Bon parenthesage";
	/// <summary>
	/// Program output when bad parentheses
	/// </summary>
	private const string _NOT_OK = "Mauvais parenthesage";

	///// <summary>
	///// Manual data
	///// </summary>
	//public static readonly ReadOnlyDictionary<string, bool> TestData = new(new Dictionary<string, bool>
	//{
	//	["()"] = true,
	//	["(())"] = true,
	//	["()()"] = true,
	//	["(())()"] = true,
	//	["(())(())"] = true,
	//	[$"{new('(', 50)}{new(')', 50)}"] = true,

	//	["("] = false,
	//	["())"] = false,
	//	["(()"] = false,
	//	["(()()"] = false,
	//	["(())())"] = false,
	//});

	/// <summary>
	/// Test data. Key = parentheses, Value = good (<see langword="true"/>) or not (<see langword="false"/>)
	/// </summary>
	public static ReadOnlyDictionary<string, bool> TestData { get; private set; }

	/// <summary>
	/// Init with random mins
	/// </summary>
	/// <param name="nb">Count of each generated data (Length, Imbrication, true, false)</param>
	/// <param name="range">Range of the randomized min</param>
	/// <param name="seed">Seed of the randomizer. If <see langword="null"/>, <see langword="DateTime.Now.Ticks"/></param>
	public static void RandomInit(int nb, Range range, int? seed = null)
	{
		Dictionary<string, bool> testData = new(nb * 4);
		Random rand = new(seed ?? (int) DateTime.Now.Ticks);

		int start = range.Start.Value;
		int end = range.End.Value;

		ParenthesesGenerator generator = new(0, 0, seed);

		for (int i = 0; i < nb; i++)
		{
			generator.MinLength = rand.Next(start, end);
			testData.Add(generator.Generate(GeneratorMode.Length, true), true);
			generator.MinImbrication = rand.Next(start, end);
			testData.Add(generator.Generate(GeneratorMode.Imbrication, true), true);
			generator.MinLength = rand.Next(start, end);
			testData.Add(generator.Generate(GeneratorMode.Length, false), false);
			generator.MinImbrication = rand.Next(start, end);
			testData.Add(generator.Generate(GeneratorMode.Length, false), false);
		}

		TestData = new(testData);
	}

	/// <summary>
	/// Init with specific mins
	/// </summary>
	/// <param name="nb">Count of each generated data (Length, Imbrication, true, false)</param>
	/// <param name="minLength">Minimum length</param>
	/// <param name="minImbr">Minimum imbrication</param>
	/// <param name="seed">Seed of the randomizer. If <see langword="null"/>, <see langword="DateTime.Now.Ticks"/></param>
	public static void Init(int nb, int minLength, int minImbr, int? seed = null)
	{
		Dictionary<string, bool> testData = new(nb * 4);

		ParenthesesGenerator generator = new(minLength, minImbr, seed);

		for (int i = 0; i < nb; i++)
		{
			testData.Add(generator.Generate(GeneratorMode.Length, true), true);
			testData.Add(generator.Generate(GeneratorMode.Imbrication, true), true);
			testData.Add(generator.Generate(GeneratorMode.Length, false), false);
			testData.Add(generator.Generate(GeneratorMode.Imbrication, false), false);
		}

		TestData = new(testData);
	}

	/// <summary>
	/// Path of the program
	/// </summary>
	public string ProgramPath { get; }

	/// <summary>
	/// Result of the executions test.
	/// Key = parentheses, Value = good (<see langword="true"/>) or not (<see langword="false"/>)
	/// </summary>
	public Dictionary<string, string> Results { get; }

	public ParenthesesTest(string path)
	{
		ProgramPath = path;
		Results = new();
	}

	/// <summary>
	/// Check if all tests are good or not
	/// </summary>
	public IEnumerable<(string key, bool? result)> CheckAll() => TestData.Keys.Select(key => (key, Check(key)));

	/// <summary>
	/// Check if one test is good or not
	/// </summary>
	/// <param name="key">Parentheses test</param>
	public bool? Check(string key)
	{
		bool real = TestData[key];
		bool? current = Results[key] switch
		{
			_OK => true,
			_NOT_OK => false,

			_ => null
		};

		return current is null ? current : (real && (bool) current) || (!real && !(bool) current);
	}
}
