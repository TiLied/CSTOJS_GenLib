using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Linq;

namespace CSTOJS_GenLib;

	public partial class Welcome
	{
		public List<TType> TType { get; set; }

		public Welcome() { }
	}

	public class WebIDLType
	{
		[JsonPropertyName("type")]
		public string? Type { get; set; }
		[JsonPropertyName("generic")]
		public string? Generic { get; set; }
		[JsonIgnore]
		public List<WebIDLType>? IDLType { get; set; }
		[JsonPropertyName("idlType")]
		public string? IDLTypeStr { get; set; }
		[JsonPropertyName("nullable")]
		public bool? Nullable { get; set; }
		[JsonPropertyName("union")]
		public bool? Union { get; set; }
		[JsonPropertyName("extAttrs")]
		public List<ExtAttr?> ExtAttrs { get; set; }

		//[JsonExtensionData]
		//public Dictionary<string, JsonElement>? ExtensionData { get; set; } = new();

		public WebIDLType() { }

		public static bool EqualsTest(WebIDLType obj)
		{
			//Check for null and compare run-time types.
			if (obj.Nullable == null &&
				obj.Type == null &&
				obj.Generic == null &&
				obj.ExtAttrs == null &&
				obj.Union == null &&
				obj.IDLTypeStr == null &&
				obj.IDLType == null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public class TType
	{
		[JsonIgnore]
		public List<TType> ListAdditionalInheritance { get; set; } = new();
		[JsonPropertyName("type")]
		public string? Type { get; set; }
		[JsonPropertyName("name")]
		public string? Name { get; set; }
		[JsonPropertyName("inheritance")]
		public string? Inheritance { get; set; }
		[JsonPropertyName("partial")]
		public bool? Partial { get; set; }
		[JsonPropertyName("members")]
		public List<Member> Members { get; set; }
		[JsonPropertyName("extAttrs")]
		public List<ExtAttr?> ExtAttrs { get; set; }

		[JsonConverter(typeof(IDLTypeJsonConverter))]
		[JsonPropertyName("idlType")]
		public List<WebIDLType>? IDLType { get; set; }
		[JsonPropertyName("arguments")]
		public List<Argument>? Arguments { get; set; }

		[JsonPropertyName("values")]
		public List<Value>? Values { get; set; }

		[JsonPropertyName("target")]
		public string? Target { get; set; }
		[JsonPropertyName("includes")]
		public string? Includes { get; set; }

		[JsonExtensionData]
		public Dictionary<string, JsonElement>? ExtensionData { get; set; }

		public TType() { }
	}

	public class Member 
	{
		[JsonPropertyName("type")]
		public string? Type { get; set; }
		[JsonPropertyName("special")]
		public string? Special { get; set; }
		[JsonConverter(typeof(IDLTypeJsonConverter))]
		[JsonPropertyName("idlType")]
		public List<WebIDLType>? IDLType { get; set; }
		[JsonPropertyName("name")]
		public string? Name { get; set; }
		[JsonPropertyName("arguments")]
		public List<Argument>? Arguments { get; set; }
		[JsonPropertyName("extAttrs")]
		public List<ExtAttr?> ExtAttrs { get; set; }
		[JsonPropertyName("parent")]
		public object Parent { get; set; }

		[JsonPropertyName("readonly")]
		public bool? Readonly { get; set; }

		[JsonPropertyName("value")]
		public Value? Value { get; set; }

		[JsonPropertyName("async")]
		public bool? Async { get; set; }

		[JsonPropertyName("default")]
		public Default? Default { get; set; }

		[JsonPropertyName("required")]
		public bool? Required { get; set; }
		[JsonExtensionData]
		public Dictionary<string, JsonElement>? ExtensionData { get; set; }

		public Member() { }
	}

	public class Argument
	{
		[JsonPropertyName("type")]
		public string? Type { get; set; }
		[JsonPropertyName("default")]
		public Default? Default { get; set; }
		[JsonPropertyName("optional")]
		public bool? Optional { get; set; }
		[JsonPropertyName("variadic")]
		public bool? Variadic { get; set; }
		[JsonPropertyName("extAttrs")]
		public List<ExtAttr?> ExtAttrs { get; set; }
		[JsonConverter(typeof(IDLTypeJsonConverter))]
		[JsonPropertyName("idlType")]
		public List<WebIDLType>? IDLType { get; set; }
		[JsonPropertyName("name")]
		public string? Name { get; set; }
		[JsonPropertyName("parent")]
		public object Parent { get; set; }

		[JsonExtensionData]
		public Dictionary<string, JsonElement>? ExtensionData { get; set; }

		public Argument() { }
	}

	public class ExtAttr
	{
		[JsonPropertyName("name")]
		public string? Name { get; set; }
		[JsonPropertyName("arguments")]
		public List<Argument>? Arguments { get; set; }
		[JsonPropertyName("type")]
		public string? Type { get; set; }
		[JsonPropertyName("rhs")]
		public Rhs? Rhs { get; set; }
		[JsonPropertyName("parent")]
		public object Parent { get; set; }

		[JsonExtensionData]
		public Dictionary<string, JsonElement>? ExtensionData { get; set; }

		public ExtAttr() { }
	}

	public class Value
	{
		[JsonPropertyName("type")]
		public string? Type { get; set; }
		[JsonPropertyName("value")]
		public object? ValueObj { get; set; }

		[JsonExtensionData]
		public Dictionary<string, JsonElement>? ExtensionData { get; set; }

		public Value() { }
	}
	public class Rhs
	{
		[JsonPropertyName("type")]
		public string? Type { get; set; }
		[JsonPropertyName("value")]
		public object? Value { get; set; }

		[JsonExtensionData]
		public Dictionary<string, JsonElement>? ExtensionData { get; set; }

		public Rhs() { }
	}
	public class Default
	{
		[JsonPropertyName("type")]
		public string? Type { get; set; }
		[JsonPropertyName("value")]
		public object? Value { get; set; }

		[JsonExtensionData]
		public Dictionary<string, JsonElement>? ExtensionData { get; set; }

		public Default() { }

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
	public class IDLTypeJsonConverter : JsonConverter<List<WebIDLType>>
	{
		public override List<WebIDLType> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			List<WebIDLType> list = new();
			bool array = false;

			if (reader.TokenType == JsonTokenType.StartArray)
			{
				list.Add(new());
				array = true;
			}
			
			if (reader.TokenType == JsonTokenType.StartObject)
			{
				list.Add(new());
			}

			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndArray)
				{
					if (array == true)
					{
						//TODO?
						if (WebIDLType.EqualsTest(list.Last()))
							list.RemoveAt(list.Count - 1);

						return list;
					}
				}

				if (reader.TokenType == JsonTokenType.EndObject)
				{
					if (array == false)
						return list;
					else
						list.Add(new());
				}

				if (reader.TokenType == JsonTokenType.PropertyName)
				{
					string? propertyName = reader.GetString();
					switch (propertyName)
					{
						case "type":
							{
								reader.Read();
								list.Last().Type = reader.GetString();
								break;
							}
						case "generic":
							{
								reader.Read();
								list.Last().Generic = reader.GetString();
								break;
							}
						case "idlType":
							{
								reader.Read();
								if (reader.TokenType == JsonTokenType.StartArray)
									list.Last().IDLType = Read(ref reader, typeToConvert, options);
								else
									list.Last().IDLTypeStr = reader.GetString();
								break;
							}
						case "nullable":
							{
								reader.Read();
								list.Last().Nullable = reader.GetBoolean();
								break;
							}
						case "union":
							{
								reader.Read();
								list.Last().Union = reader.GetBoolean();
								break;
							}
						case "extAttrs":
							{
								//TODO?
								//
								//
								reader.Read();
								list.Last().ExtAttrs = null;
								reader.Skip();
								//reader.Read();
								break;
							}
						default:
							//reader.Read();
							//list.Last().ExtensionData.Add(propertyName, JsonDocument.ParseValue(ref reader).RootElement);
							break;
					}
				}
			}

			return null;
		}

		public override void Write(Utf8JsonWriter writer, List<WebIDLType> value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();

			Write2(ref writer, value, options);

			writer.WriteEndArray();
		}

		public void Write2(ref Utf8JsonWriter writer, List<WebIDLType> value, JsonSerializerOptions options)
		{
			foreach (WebIDLType webIDLType in value)
			{
				writer.WriteStartObject();

				writer.WriteString("type", webIDLType.Type);
				writer.WriteString("generic", webIDLType.Generic);

				writer.WritePropertyName("idlType");
				if (webIDLType.IDLType != null)
				{
					writer.WriteStartArray();
					//JsonSerializer.Serialize(writer, webIDLType.IDLType, options);
					Write2(ref writer, webIDLType.IDLType, options);
					writer.WriteEndArray();
				}
				else
					writer.WriteStringValue(webIDLType.IDLTypeStr);

				writer.WriteBoolean("nullable", (bool)webIDLType.Nullable);
				writer.WriteBoolean("union", (bool)webIDLType.Union);
				//
				//TODO!
				writer.WriteNull("extAttrs");

				writer.WriteEndObject();
			}
		}
	}




