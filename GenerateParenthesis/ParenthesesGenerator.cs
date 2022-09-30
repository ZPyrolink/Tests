namespace GenerateParentheses;

public class ParenthesesGenerator
{
	public static readonly Dictionary<char, char[]> Chars = new()
	{
		['\0'] = new[]
		{
			'x', '(', '[', '{', // 0..^2
		},
		['('] = new[]
		{
			'x', '(', '[', '{', // 0..^2
			')', 'x'			// ^2..^0
		},
		['['] = new[]
		{
			'x', '(', '[', '{',
			']', 'x'
		},
		['{'] = new[]
		{
			'x', '(', '[', '{',
			'}', 'x'
		}
	};

	public static readonly char[] SimpleParentheses =
	{
		'x',
		'(', ')',
		'x'
	};

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
	/// <param name="anyParentheses">Generate full ('()', '[]', '{}') parentheses or only simple ('()')</param>
	/// <param name="correct">Generate good (<see langword="true"/>) or bad (<see langword="false"/>) parentheses</param>
	/// <returns>The parentheses generated</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="mode"/> is not a correct enum value</exception>
	public string Generate(GeneratorMode mode, bool anyParentheses, bool correct)
	{
		ResetRandom();
		return (mode, anyParentheses, correct) switch
		{
			(GeneratorMode.Length, false, true) => GenerateMinLength(),
			(GeneratorMode.Imbrication, false, true) => GenerateMinImb(),

			(GeneratorMode.Length, false, false) => GenerateBadMinLength(),
			(GeneratorMode.Imbrication, false, false) => GenerateBadMinImb(),

			
			(GeneratorMode.Length, true, true) => GenerateAnyMinLength(),
			(GeneratorMode.Imbrication, true, true) => GenerateAnyMinImb(),

			(GeneratorMode.Length, true, false) => GenerateAnyBadMinLength(),
			(GeneratorMode.Imbrication, true, false) => GenerateAnyBadMinImb(),

			_ => throw new ArgumentOutOfRangeException(nameof(mode))
		};
	}

	private string GenerateAnyMinLength()
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
			{
				randChar = Chars['\0'][_random.Next(Chars['\0'].Length)];
			}
			else
			{
				char[] charArray = Chars[stack.Peek()];

				if (result.Length < MinLength)
					randChar = charArray[_random.Next(charArray.Length - 1)];
				else
					randChar = charArray[_random.Next(charArray.Length - 2, charArray.Length)];
			}

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

	private string GenerateAnyMinImb()
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
			{
				randChar = Chars['\0'][_random.Next(Chars['\0'].Length)];
			}
			else
			{
				char[] charArray = Chars[stack.Peek()];

				if (!canExit)
					randChar = charArray[_random.Next(charArray.Length - 1)];
				else
					randChar = charArray[_random.Next(charArray.Length - 2, charArray.Length)];
			}

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

	private string GenerateAnyBadMinLength()
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
			{
				randChar = Chars['\0'][_random.Next(Chars['\0'].Length)];
			}
			else
			{
				char[] charArray = Chars[stack.Peek()];

				if (result.Length < MinLength)
					randChar = charArray[_random.Next(charArray.Length - 1)];
				else
					randChar = charArray[_random.Next(charArray.Length - 2, charArray.Length)];
			}

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

	private string GenerateAnyBadMinImb()
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
			{
				randChar = Chars['\0'][_random.Next(Chars['\0'].Length)];
			}
			else
			{
				char[] charArray = Chars[stack.Peek()];

				if (!canExit)
					randChar = charArray[_random.Next(charArray.Length - 1)];
				else
					randChar = charArray[_random.Next(charArray.Length - 2, charArray.Length)];
			}

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
				randChar = SimpleParentheses[_random.Next(2)];
			else if (result.Length < MinLength)
				randChar = SimpleParentheses[_random.Next(3)];
			else
				randChar = SimpleParentheses[_random.Next(2, 4)];

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
				randChar = SimpleParentheses[_random.Next(2)];
			else if (!canExit)
				randChar = SimpleParentheses[_random.Next(3)];
			else
				randChar = SimpleParentheses[_random.Next(2, 4)];

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
				randChar = SimpleParentheses[_random.Next(2)];
			else if (result.Length < MinLength)
				randChar = SimpleParentheses[_random.Next(3)];
			else
				randChar = SimpleParentheses[_random.Next(2, 4)];

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
				randChar = SimpleParentheses[_random.Next(2)];
			else if (!canExit)
				randChar = SimpleParentheses[_random.Next(3)];
			else
				randChar = SimpleParentheses[_random.Next(2, 4)];

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