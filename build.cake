#addin nuget:?package=Cake.FileHelpers&version=3.2.1
#addin nuget:?package=SharpZipLib&version=1.2.0
#addin nuget:?package=Cake.Compression&version=0.2.4
#addin nuget:?package=Cake.Json&version=4.0.0
#addin nuget:?package=Cake.Http&version=0.7.0

// args
var target = Argument("target", "Pack");

// config
var ror2ConfigFile = File("ror2.config.json");

// misc
var buildVersion = FindRegexMatchInFile(File("AssemblyInfo.cs"), "[0-9]+\\.[0-9]+\\.[0-9]+", System.Text.RegularExpressions.RegexOptions.None);
var binDir = Directory("bin");
var distDir = binDir + Directory("dist");
var enabledModsDir = Directory("dasmods");
var currentCommit = RunGit("rev-parse HEAD");

string RunGit(string command, string separator = "") 
{
    using(var process = StartAndReturnProcess("git", new ProcessSettings { Arguments = command, RedirectStandardOutput = true })) 
    {
        process.WaitForExit();
        return string.Join(separator, process.GetStandardOutput());
    }
}

Task("Cleanup")
	.Does(() =>
{
	Information("Removing old binaries");
	CreateDirectory(binDir);
	CleanDirectory(binDir);
	CreateDirectory(distDir);
	CleanDirectory(distDir);

	Information("Cleaning up old build objects");
	CleanDirectories(GetDirectories("Mods/**/bin/"));
	CleanDirectories(GetDirectories("Mods/**/obj/"));
});

Task("Build")
	.IsDependentOn("Cleanup")
	.DoesForEach(GetFiles("Mods/**/*.sln"), (file) =>
{
	var modName = file.GetFilenameWithoutExtension();
	Information("Building: {0}", modName);
	var buildSettings = new MSBuildSettings {
        Configuration = "Release",
        Restore = true
    };
	MSBuild(file, buildSettings);

	var binaryFile = File($"./Mods/{modName}/bin/Release/netstandard2.0/{modName}.dll");
	Information("Copying from {0} to {1}", binaryFile, distDir);
	CopyFile(binaryFile, $"{distDir}/{modName}.dll");
})
	.Does(() =>
{
	Information("Writing info.json");
	FileWriteText(distDir + File("info.json"),
		SerializeJsonPretty(new Dictionary<string, object> {
			["date"] = DateTime.Now.ToString("o"),
			["hash"] = currentCommit,
			["artifacts"] = new Dictionary<string, object>[] {
				new Dictionary<string, object> {
					["file"] = $"dist-ror2-{buildVersion}.zip",
					["description"] = "Mods for Risk of Rain 2"
				}
			}
		})
	);
});

Task("Pack")
	.IsDependentOn("Build")
	.Does(() =>
{
	Information("Packing ror2");
	ZipCompress(distDir, File($"{binDir}/dist-ror2-{buildVersion}.zip"));
});

Task("InitConfig")
	.Does(() =>
{
	if (!FileExists(ror2ConfigFile))
	{
		Information($"Creating: {ror2ConfigFile}");
		FileWriteText(File(ror2ConfigFile),
			SerializeJsonPretty(new Dictionary<string, object> {
				["ror2Dir"] = "",
				["enabledMods"] = new Dictionary<string, string>[] {}
			})
		);
	} else
	{
		Information($"Already exists: {ror2ConfigFile}");
	}
});

Task("InstallEnabledMods")
	.IsDependentOn("InitConfig")
	.IsDependentOn("Pack")
	.Does(() =>
{
	Information($"Parsing {ror2ConfigFile}\n");
	var config = ParseJsonFromFile(ror2ConfigFile);
	var ror2Dir = (string)config["ror2Dir"];
	var enabledMods = ((JArray)config["enabledMods"]).Select(enabledMod => (string)enabledMod).ToList();
	Information($"ror2Dir: {ror2Dir}");
	Information($"enabledMods ({enabledMods.Count}): {string.Join(",", enabledMods.ToArray())}\n");

	var src = distDir; // where the mods are built
	var dst = Directory($"{ror2Dir}/BepInEx/plugins/") + enabledModsDir;
	CreateDirectory(dst);
	CleanDirectory(dst);
	enabledMods.ForEach((modName) =>
	{
		var modFile = File($"{modName}.dll");
		Information("Installing {0}", modName);
		CopyFile(src + modFile, dst + modFile);
	});
});

Task("Play")
	.IsDependentOn("InstallEnabledMods")
	.Does(() =>
{
	var config = ParseJsonFromFile(ror2ConfigFile);
	var ror2Dir = (string)config["ror2Dir"];
	StartProcess($"{ror2Dir}/Risk of Rain 2");
});

RunTarget(target);