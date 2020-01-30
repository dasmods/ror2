var target = Argument("target", "Build");

Task("Cleanup")
	.Does(() =>
{
	Information("Removing old binaries");
	CreateDirectory("./bin");
	CleanDirectory("./bin");

	Information("Cleaning up old build objects");
	CleanDirectories(GetDirectories("Mods/**/bin/"));
	CleanDirectories(GetDirectories("Mods/**/obj/"));
});

Task("Build")
	.IsDependentOn("Cleanup")
	.DoesForEach(GetFiles("Mods/**/*.sln"), (file) =>
{
	Information("Building: {0}", file);
});

Task("Release")
	.IsDependentOn("Build")
	.Does(() =>
{
	Information("Release");
});

RunTarget(target);