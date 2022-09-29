using System.Diagnostics;

using Tests;

// Test.exe [path] [nb=5] [min=5] [max=100] [seed=null]

#region Default args

string path = "/mnt/c/Users/Pyrolink/Desktop/cours/Prog/APNEE/Tests/programs/";
int nb = 5;
int? seed = null;
Range range = 5..100;

#endregion

switch (args.Length)
{
	case 0:
		// Keep default values
		break;
	case 1:
		path = args[0];
		// Keep other default values
		break;
	case 2:
		path = args[0];
		nb = int.Parse(args[1]);
		// Keep other default values
		break;
	case 3:
		path = args[0];
		nb = int.Parse(args[1]);
		seed = int.Parse(args[2]);
		// Keep other default values
		break;
	case 4:
		path = args[0];
		nb = int.Parse(args[1]);
		range = int.Parse(args[2])..int.Parse(args[3]);
		// Keep other default values
		break;
	case 5:
		path = args[0];
		nb = int.Parse(args[1]);
		range = int.Parse(args[2])..int.Parse(args[3]);
		seed = int.Parse(args[4]);
		break;
}

ParenthesesTest.RandomInit(nb, range, seed);

static Process CreateProces(string path) => new()
{
	StartInfo = new(path)
	{
		UseShellExecute = false,        // Don't open shell
		RedirectStandardInput = true,   // Redirect input
		RedirectStandardOutput = true   // Redirect output
	}
};

// Foreach Process created by enumerating the path
foreach (Process process in Directory.EnumerateFiles(path).Select(CreateProces))
{
	// Instantiate the tests
	ParenthesesTest pt = new(Path.GetFileName(process.StartInfo.FileName));

	Console.WriteLine($"{new('-', 10)}{pt.ProgramPath}{new('-', 10)}");

	// Foreach tests
	foreach (string data in ParenthesesTest.TestData.Keys)
	{
		process.Start();

		// Send test on input and dispose (close) the stream to end EOF
		using (StreamWriter sw = process.StandardInput)
			sw.Write(data);

		// Read and save the output
		using (StreamReader sr = process.StandardOutput)
			pt.Results.Add(data, sr.ReadToEnd().Trim('\n'));

		process.WaitForExit();

		bool? sucess = pt.Check(data);
		string res = sucess switch
		{
			null => $"ERROR ({pt.Results[data]})",
			true => "OK",
			false => "NOT OK"
		};

		Console.WriteLine($"{data} ({ParenthesesTest.TestData[data]}) = {res}");
	}

	Console.WriteLine();

	Console.WriteLine($"Le programme {(pt.CheckAll().All(c => c.result == true) ? "semble correct" : "est incorrect")}");
}