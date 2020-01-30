var target = Argument("target", "Build");

Task("Cleanup")
	.Does(() =>
{
	Information("Cleanup");
});

Task("Build")
	.IsDependentOn("Cleanup")
	.DoesForEach(GetFiles("Mods/**/*.sln"), (file) =>
{
	Information("Building: {0}", file);
});

RunTarget(target);