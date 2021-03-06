using System;
using System.Collections.Generic;
using System.Reflection;
using Auth0.Core;
using Auth0.Core.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Auth0.ManagementApi.Serialization
{
    internal class PagedListConverter<T> : JsonConverter
    {
        private readonly string _collectionFieldName;

        public PagedListConverter(string collectionFieldName)
        {
            _collectionFieldName = collectionFieldName;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (IPagedList<T>).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject item = JObject.Load(reader);

                if (item[_collectionFieldName] != null)
                {
                    var collection = item[_collectionFieldName].ToObject<IList<T>>(serializer);

                    int length = item["length"].Value<int>();
                    int limit = item["limit"].Value<int>();
                    int start = item["start"].Value<int>();
                    int total = item["total"].Value<int>();

                    return new PagedList<T>(collection, new PagingInformation(start, limit, length, total));
                }
            }
            else
            {
                JArray array = JArray.Load(reader);

                var collection = array.ToObject<IList<T>>();

                return new PagedList<T>(collection);
            }

            // This should not happen. Perhaps better to throw exception at this point?
            return null;
        }
    }
}