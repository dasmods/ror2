## How to add a Command

To add a command:

1. Open `Commands.cs`
2. Create a new class that inherits from the`ICommand` interface
3. Add a command instance by name to `COMMANDS` - the name cannot have any spaces
4. The command will be callable in game by typing `/command arg1 arg2 arg3` in chat
