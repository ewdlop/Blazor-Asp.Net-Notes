using Gremlin.Net.Process.Traversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorServerApp.Models
{
    public class Demo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("output")]
        public string Output { get; set; }
    }
}
