var target = Argument("target", "Build");

Task("Build")
	.DoesForEach(GetFiles("Mods/**/*.sln"), (file) =>
{
	Information("Building: {0}", file);
});

RunTarget(target);