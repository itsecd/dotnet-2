using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatServer.Converters
{
    public class UsersBagJsonConverter : JsonConverter<ConcurrentBag<User>>
    {
        public override ConcurrentBag<User> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var users = JsonSerializer.Deserialize<User[]>(ref reader, options);
            return users != null ? new ConcurrentBag<User>(users) : null;
        }

        public override void Write(Utf8JsonWriter writer, ConcurrentBag<User> value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(JsonSerializer.Serialize(value, options));
        }
    }
}
