using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace N_Tier.Core.DTOs.CardDtos;

public class CardExpiryDateConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();
        if (string.IsNullOrEmpty(value))
            throw new JsonException("Expiry date cannot be null or empty");

        // "MM/yy" formatidan parse qilish
        if (!DateTime.TryParseExact(value, "MM/yy", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out DateTime date))
        {
            throw new JsonException("Invalid expiry date format. Use MM/yy format (e.g. 10/25)");
        }

        // 20xx yilga o'tkazish
        return new DateTime(2000 + date.Year, date.Month, 1);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // "MM/yy" formatida yozish
        writer.WriteStringValue(value.ToString("MM/yy"));
    }
}
