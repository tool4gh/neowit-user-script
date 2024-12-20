// See https://aka.ms/new-console-template for more information
using CreateUsersNeowit;
using Spectre.Console;

AnsiConsole.MarkupLine("[red]Neowit Bulk Import of Users[/]");
var confirmation = AnsiConsole.Prompt(
    new TextPrompt<bool>("Are you ready to proceed?")
        .AddChoice(true)
        .AddChoice(false)
        .DefaultValue(true)
        .WithConverter(choice => choice ? "y" : "n"));

if (!confirmation)
{
    return;
}

var orgId = AnsiConsole.Prompt(new TextPrompt<string>("Please provide the [green]org[/] you want to add users to?"));

var fileConfirm = AnsiConsole.Prompt(
    new TextPrompt<bool>("Have you placed the [red bold underline]importUsers.csv[/] file in the same folder as this program?")
        .AddChoice(true)
        .AddChoice(false)
        .DefaultValue(true)
        .WithConverter(choice => choice ? "y" : "n"));
var test = new User();

await test.ReadUsersFromFileAsync(orgId);