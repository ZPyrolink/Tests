namespace GenerateParentheses;

public class ParenthesesGenerator
{
	/// <summary>
	/// List of characters. 'x' represent any character
	/// </summary>
	public static char[] Chars = { 'x', '(', ')', 'x' };

	/// <summary>
	/// Minimum length on <see cref="GeneratorMode.Length"/> generation
	/// </summary>
	public int MinLength { get; set; }

	/// <summary>
	/// Minimum level of imbrication on <see cref="GeneratorMode.Imbrication"/>
	/// </summary>
	public int MinImbrication { get; set; }

	/// <summary>
	/// <see cref="_random"/> seed
	/// </summary>
	private int? Seed { get; set; }
	/// <summary>
	/// Random generator
	/// </summary>
	private Random _random;

	/// <summary>
	/// Initialise the generator
	/// </summary>
	/// <param name="minLength"><see cref="MinLength"/></param>
	/// <param name="minImbrication"><see cref="MinImbrication"/></param>
	/// <param name="randomSeed"><see cref="Seed"/></param>
	public ParenthesesGenerator(int minLength, int minImbrication, int? randomSeed = null)
	{
		MinLength = minLength;
		MinImbrication = minImbrication;

		Seed = randomSeed;
	}

	/// <summary>
	/// Recreate the random (used before generate)
	/// </summary>
	private void ResetRandom() => _random = new(Seed ?? (int) DateTime.Now.Ticks);

	/// <summary>
	/// Global generate method
	/// </summary>
	/// <param name="mode">Mode of the generation</param>
	/// <param name="correct">Generate good (<see langword="true"/>) or bad (<see langword="false"/>) parentheses</param>
	/// <returns>The parentheses generated</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="mode"/> is not a correct enum value</exception>
	public string Generate(GeneratorMode mode, bool correct)
	{
		ResetRandom();
		return (mode, correct) switch
		{
			(GeneratorMode.Length, true) => GenerateMinLength(),
			(GeneratorMode.Imbrication, true) => GenerateMinImb(),

			(GeneratorMode.Length, false) => GenerateBadMinLength(),
			(GeneratorMode.Imbrication, false) => GenerateBadMinImb(),

			_ => throw new ArgumentOutOfRangeException(nameof(mode))
		};
	}

	private string GenerateMinLength()
	{
		switch (MinLength)
		{
			case < 0:
				throw new ArgumentOutOfRangeException(nameof(MinLength));
			case 0:
				return "X";
		}

		string result = "";
		Stack<char> stack = new();

		do
		{
			char randChar;
			if (stack.Count == 0)
				randChar = Chars[_random.Next(2)];
			else if (result.Length < MinLength)
				randChar = Chars[_random.Next(3)];
			else
				randChar = Chars[_random.Next(2, 4)];

			result += randChar;

			switch (randChar)
			{
				case '(':
					stack.Push(randChar);
					break;
				case ')':
					stack.Pop();
					break;
			}

		} while (stack.Count != 0 || result.Length < MinLength);

		return result;
	}

	private string GenerateMinImb()
	{
		switch (MinImbrication)
		{
			case < 0:
				throw new ArgumentOutOfRangeException(nameof(MinImbrication));
			case 0:
				return "X";
		}

		bool canExit = false;

		string result = "";
		Stack<char> stack = new();

		do
		{
			char randChar;
			if (stack.Count == 0)
				randChar = Chars[_random.Next(2)];
			else if (!canExit)
				randChar = Chars[_random.Next(3)];
			else
				randChar = Chars[_random.Next(2, 4)];

			result += randChar;

			switch (randChar)
			{
				case '(':
					stack.Push(randChar);
					break;
				case ')':
					stack.Pop();
					break;
			}

			if (stack.Count >= MinImbrication)
				canExit = true;

		} while (stack.Count != 0 || !canExit);

		return result;
	}

	private string GenerateBadMinLength()
	{
		switch (MinLength)
		{
			case < 0:
				throw new ArgumentOutOfRangeException(nameof(MinLength));
			case 0:
				return "X";
		}

		string result = "";
		Stack<char> stack = new();

		do
		{
			char randChar;
			if (stack.Count == 0)
				randChar = Chars[_random.Next(2)];
			else if (result.Length < MinLength)
				randChar = Chars[_random.Next(3)];
			else
				randChar = Chars[_random.Next(2, 4)];

			result += randChar;

			switch (randChar)
			{
				case '(':
					stack.Push(randChar);
					break;
				case ')':
					stack.Pop();
					break;
			}

		} while (stack.Count == 0 || result.Length < MinLength);

		return result;
	}

	private string GenerateBadMinImb()
	{
		switch (MinImbrication)
		{
			case < 0:
				throw new ArgumentOutOfRangeException(nameof(MinImbrication));
			case 0:
				return "X";
		}

		bool canExit = false;
		
		string result = "";
		Stack<char> stack = new();

		do
		{
			char randChar;
			if (stack.Count == 0)
				randChar = Chars[_random.Next(2)];
			else if (!canExit)
				randChar = Chars[_random.Next(3)];
			else
				randChar = Chars[_random.Next(2, 4)];

			result += randChar;

			switch (randChar)
			{
				case '(':
					stack.Push(randChar);
					break;
				case ')':
					stack.Pop();
					break;
			}

			if (stack.Count >= MinImbrication)
				canExit = true;

		} while (stack.Count == 0 || !canExit);

		return result;
	}
}