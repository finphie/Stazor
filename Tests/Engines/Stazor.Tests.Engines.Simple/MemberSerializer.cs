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

        public MemberSerializer(T obj) => Object = obj;

        public T Object { get; private set; }

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
            => Object = JsonSerializer.Deserialize<T>(info.GetValue<string>(nameof(Object)))!;

        public void Serialize(IXunitSerializationInfo info)
            => info.AddValue(nameof(Object), JsonSerializer.Serialize(Object, SerializerOptions));

        public override string ToString() => JsonSerializer.Serialize(Object, SerializerOptions);
    }
}