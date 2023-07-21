using SlnParser;

var slnFilename = args[0];
var solutionParser = new SolutionParser();
var sln = solutionParser.Parse(slnFilename);
Console.Out.WriteLine(sln);
var generatedSlnFilename = slnFilename.Replace(".sln", ".sln.generated");
File.WriteAllText(generatedSlnFilename, sln.ToString());
Console.Out.WriteLine($"Wrote {generatedSlnFilename}.");
