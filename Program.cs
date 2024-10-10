// See https://aka.ms/new-console-template for more information
using CreateUsersNeowit;

Console.WriteLine("Neowit Bulk Import of Users.");
Console.WriteLine("Click to Continue");
Console.ReadLine();
System.Console.WriteLine("Please input the org you want to add users to:");
var orgId = Console.ReadLine();

System.Console.WriteLine(orgId);

var test = new User();

test.ReadUsersFromFileAsync(orgId);