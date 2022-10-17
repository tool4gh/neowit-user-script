using System;
using Newtonsoft.Json;

namespace CreateUsersNeowit
{
    public class Root
    {
        [JsonProperty("users")]
        public List<User> Users { get; set; }

        public Root()
        {
        }
    }
}

