using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProvaPub.Domain.Helpers
{
    public class BrazilianDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => reader.GetDateTime();

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var brasilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            var brasilTime = TimeZoneInfo.ConvertTimeFromUtc(value, brasilTimeZone);
            writer.WriteStringValue(brasilTime);
        }
    }
}
