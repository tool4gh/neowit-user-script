// See https://aka.ms/new-console-template for more information
using CreateUsersNeowit;

Console.WriteLine("Neowit Bulk Import of Users.");
Console.WriteLine("Click to Continue");
var hugo = Console.ReadLine();

var test = new User();

test.ReadUsersFromFileAsync();