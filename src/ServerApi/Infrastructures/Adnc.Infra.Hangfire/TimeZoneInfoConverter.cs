using Newtonsoft.Json;
using System;

namespace Hangfire
{
    /// <summary>
    /// Converts <see cref="TimeZoneInfo"/> to and from JSON
    /// </summary>
    public class TimeZoneInfoConverter : JsonConverter
	{
		/// <summary>
		/// Determines whether this instance can convert the specified <see cref="TimeZoneInfo"/>
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns>true if this instance can convert the specified <see cref="TimeZoneInfo"/>; otherwise, false.</returns>
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(TimeZoneInfo);
		}
		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		/// <param name="reader">The <see cref="JsonReader"/> to read from</param>
		/// <param name="objectType">Type of the object</param>
		/// <param name="existingValue">The existing value of object being read</param>
		/// <param name="serializer">The calling serializer</param>
		/// <returns>The object value</returns>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null) return null;

			if (reader.Value == null) return null;

			return TimeZoneInfo.FindSystemTimeZoneById(reader.Value.ToString());
		}
		/// <summary>
		/// Writes the JSON representation of the <see cref="TimeZoneInfo"/>.
		/// </summary>
		/// <param name="writer">The <see cref="JsonWriter"/> to write to</param>
		/// <param name="value">The value</param>
		/// <param name="serializer">The calling serializer</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var o = value as TimeZoneInfo;

			writer.WriteValue(o.StandardName);
		}
	}
}
