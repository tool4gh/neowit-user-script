using System;
using System.Linq;
using Spectre.Console;
using static System.Net.Mime.MediaTypeNames;

namespace CreateUsersNeowit
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string IdpId { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string Device { get; internal set; }
        public string Location { get; internal set; }
        public bool Remember { get; internal set; }

        public User()

        {
        }
        public async Task CreateUsers(string orgId, List<User> importedUsers, List<User> existingUsers, ApiClient apiClient, string token)
        {
            await AnsiConsole.Progress()
                .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn(),
                    new PercentageColumn(),
                    new ProgressBarColumn(),
                    new SpinnerColumn(),
                    new ElapsedTimeColumn()
                })
                .StartAsync(async ctx =>
            {
                var createUsers = ctx.AddTask("Creating users");

                var tasks = importedUsers.Select(async user =>
                {
                    if (existingUsers.Any(u => u.Email == user.Email))
                          await apiClient.PostDeviceRequest($"user/v1/user?orgId={orgId}", user, token);
                    
                    createUsers.Increment(100.0 / importedUsers.Count());
                });
                await Task.WhenAll(tasks);
            });
        }

        private void CreateUserTableCli(List<User> users)
        {
            var table = new Table();
            table.Title("List of Users");

            table.AddColumn("Name");
            table.AddColumn("E-mail");
            table.AddColumn("Role");

            foreach (var user in users)
            {
                table.AddRow(user.Name, user.Email, user.Role);
            }
            
            AnsiConsole.Write(table);
        }

        public async Task<List<User>> ReadUsersFromFileAsync(string orgId)
        {
            var path = AppContext.BaseDirectory;
            string filePath = path + "/importUsers.csv";
            
            var sr = new StreamReader(filePath);
            var importedUsers = new List<User>();
            string line;
            string[] row = new string[2];

            while ((line = sr.ReadLine()) != null)
            {
                row = line.Split(",");

                importedUsers.Add(new User
                {
                    Name = row[0],
                    Email = row[1].ToLower(),
                    //IdpId = "epgvYfC8K9BSD9Rukfehgz",
                    Role = "ROLE_MEMBER"
                });
            }
            importedUsers.RemoveAt(0);
            
            CreateUserTableCli(importedUsers);
            
            AnsiConsole.MarkupLine("I found [green]{0}[/] users in the file: ", importedUsers.Count);
            var confirmation = AnsiConsole.Prompt(
                new TextPrompt<bool>("Are you ready to proceed?")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(true)
                    .WithConverter(choice => choice ? "y" : "n"));

            if (!confirmation)
            {
                return [];
            }
                var apiClient = new ApiClient();
                int x = 1;
                var token = apiClient.GetBearerToken(new User
                {
                    Email = "hugo@neowit.io",
                    Password = "tbp*nbq.uaj8KRK@xfz",
                    Device = "Hugo",
                    Location = "https://app.neowit.io/locations",
                    Remember = true
                }).Result;

                //var userListInPlatform = apiClient.GetUsers("user/v1/user?orgId=tCjvD73dSUeLyC7JGzxnmj", token);
                var userListInPlatform = apiClient.GetUsers($"user/v1/user?orgId={orgId}", token);

                await CreateUsers(orgId, importedUsers, userListInPlatform, apiClient, token);
            
                return null;
        }

        
    }
}

