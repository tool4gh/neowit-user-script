using System;
using System.Linq;
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

        public List<User> ReadUsersFromFileAsync()
        {
            string filePath = "/Users/hugofuhlendorff/Documents/text.csv";
           // / Users / hugofuhlendorff / Documents / text.csv
            var sr = new StreamReader(filePath);
            var importedUsers = new List<User>();
            string line;
            string[] row = new string[2];

            while ((line = sr.ReadLine()) != null)
            {
                row = line.Split(";");

                importedUsers.Add(new User
                {
                    Name = row[0],
                    Email = row[1].ToLower(),
                    IdpId = "epgvYfC8K9BSD9Rukfehgz",
                    Role = "ROLE_MEMBER"
                });

               
            }
            importedUsers.RemoveAt(0);
            Console.WriteLine("I found {0} users in the file: ", importedUsers.Count);
            Console.WriteLine("Are you ready to proceed?");
            Console.ReadLine();
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

            var userListInPlatform = apiClient.GetUsers("user/v1/user?orgId=tCjvD73dSUeLyC7JGzxnmj", token);



            foreach (var user in importedUsers)
            {
                if (!userListInPlatform.Any(u => u.Email == user.Email))
                {
                    Console.WriteLine("importing user {0} of {1}", x, importedUsers.Count);
                    var ret = apiClient.PostDeviceRequest("user/v1/user?orgId=tCjvD73dSUeLyC7JGzxnmj", user, token);
                    Console.WriteLine("Success: {0} added:" , ret.Result.Email);
                }
                else
                {
                    Console.WriteLine("User: {0} already exits in platform", user.Name);
                }

                x = x + 1;
                Task.Delay(2000);
            }

            return null;
        }
    }
}

