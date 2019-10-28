using System;
using Newtonsoft.Json;

namespace TestHarness
{
    [JsonObject(MemberSerialization.Fields)]
    public class Wibbler
    {
        public Guid Id { get; }
        public DateTime TimeStamp { get; }
        public string Comment { get; }

        public Wibbler(string comment)
        {
            Id = Guid.NewGuid();
            TimeStamp = DateTime.Now;
            Comment = comment;
        }
    }
}
