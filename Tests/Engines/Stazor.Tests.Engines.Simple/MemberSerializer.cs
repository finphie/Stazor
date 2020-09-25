using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit.Abstractions;

namespace Stazor.Tests.Engines.Simple
{
    public sealed class MemberSerializer<T> : IXunitSerializable
    {
        static JsonSerializerOptions? _serializerOptions;

#nullable disable
        public MemberSerializer()
        {
        }
#nullable restore

        public MemberSerializer(T obj) => Value = obj;

        public T Value { get; private set; }

        static JsonSerializerOptions SerializerOptions
        {
            get
            {
                if (_serializerOptions is null)
                {
                    _serializerOptions = new();
                    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
                }

                return _serializerOptions;
            }
        }

        public void Deserialize(IXunitSerializationInfo info)
            => Value = JsonSerializer.Deserialize<T>(info.GetValue<string>(nameof(Value)))!;

        public void Serialize(IXunitSerializationInfo info)
            => info.AddValue(nameof(Value), JsonSerializer.Serialize(Value, SerializerOptions));

        public override string ToString() => JsonSerializer.Serialize(Value, SerializerOptions);
    }
}