using GenerateParentheses;

Console.WriteLine("Without seed ".PadRight(200, '='));
Console.WriteLine();

for (int i = 0; i < 10; i++)
{
	ParenthesesGenerator generator = new(i, i);

	Console.WriteLine($"Min {i}");
	Console.WriteLine();

	Console.WriteLine("Good generation:");
	Console.WriteLine("Good length = " + generator.Generate(GeneratorMode.Length, true));
	Console.WriteLine("Good imbrication = " + generator.Generate(GeneratorMode.Imbrication, true));

	Console.WriteLine();
	Console.WriteLine("Bad generation");
	Console.WriteLine("Bad length = " + generator.Generate(GeneratorMode.Length, false));
	Console.WriteLine("Bad imbrication = " + generator.Generate(GeneratorMode.Imbrication, false));

	Console.WriteLine($"\n{"".PadLeft(200, '-')}\n");
}

int seed = new Random().Next();

Console.WriteLine($"With seed = {seed} ".PadRight(200, '='));
Console.WriteLine();

for (int i = 0; i < 10; i++)
{
	ParenthesesGenerator generator = new(i, i, seed);

	Console.WriteLine($"Min {i}");
	Console.WriteLine();

	Console.WriteLine("Good generation:");
	Console.WriteLine("Good length = " + generator.Generate(GeneratorMode.Length, true));
	Console.WriteLine("Good imbrication = " + generator.Generate(GeneratorMode.Imbrication, true));

	Console.WriteLine();
	Console.WriteLine("Bad generation");
	Console.WriteLine("Bad length = " + generator.Generate(GeneratorMode.Length, false));
	Console.WriteLine("Bad imbrication = " + generator.Generate(GeneratorMode.Imbrication, false));

	Console.WriteLine($"\n{"".PadLeft(200, '-')}\n");
}