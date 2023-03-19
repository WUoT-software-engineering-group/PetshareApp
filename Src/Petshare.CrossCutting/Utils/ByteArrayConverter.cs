using System.Text.Json;
using System.Text.Json.Serialization;

namespace Petshare.CrossCutting.Utils
{
    public class StringToByteArrayConverter : JsonConverter<byte[]>
    {
        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stream = JsonSerializer.Deserialize<string>(ref reader);
            if (stream is null)
            {
                return new byte[0];
            }

            var value = new byte[stream.Length];
            for (int i = 0; i < stream.Length; i++)
            {
                value[i] = (byte)stream[i];
            }

            return value;
        }

        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var val in value)
            {
                writer.WriteNumberValue(val);
            }

            writer.WriteEndArray();
        }
    }
}
