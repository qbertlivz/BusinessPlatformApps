using System;
using System.Dynamic;

using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Helpers
{
    public class ResponseObjectConverter : Newtonsoft.Json.JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Type valueType = value.GetType();

             if (value is string || valueType.IsArray)
            {
                dynamic obj = new ExpandoObject();
                obj.Value = value;
                serializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                serializer.Serialize(writer, obj);
            }
            else
            {
                serializer.Serialize(writer, value);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}