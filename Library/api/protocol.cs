using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel;

namespace BU
{
    [TypeConverter(typeof(_SystemUser.TypeConverter))]
    [JsonConverter(typeof(_SystemUser.JsonConverter))]
    public class _SystemUser
    {
        public UserType UserType { get; private set; }
        public int ID { get; private set; }
        public string ACNT { get; private set; }
        static Dictionary<int, _SystemUser> cache = new Dictionary<int, _SystemUser>();

        public static _SystemUser GetUser(int id)
        {
            lock (cache)
            {
                _SystemUser obj;
                if (!cache.TryGetValue(id, out obj))
                    obj = cache[id] = new _SystemUser() { ID = id };
                return obj;
            }
        }

        public class TypeConverter : System.ComponentModel.TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(Int32);
            }
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return base.CanConvertTo(context, destinationType);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value is Int32)
                    return _SystemUser.GetUser((int)value);
                return base.ConvertFrom(context, culture, value);
            }
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
        public class JsonConverter : Newtonsoft.Json.JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return null;
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is _SystemUser)
                {
                    _SystemUser obj = (_SystemUser)value;
                    lock (obj)
                    {
                        if (obj.ACNT == null)
                        {
                            if (obj.ID == 0)
                            {
                                obj.ACNT = "_sys";
                            }
                            else
                            {
                                web.api.json_writer w = writer as web.api.json_writer;
                                if (w != null)
                                {
                                    obj.ACNT = w.sqlcmd.ExecuteScalar("select ACNT from Admin nolock where ID={0}", obj.ID) as string;
                                    if (obj.ACNT != null) obj.UserType = UserType.Admin;
                                }
                                obj.ACNT = obj.ACNT ?? obj.ID.ToString();
                            }
                        }
                    }
                    serializer.Serialize(writer, obj.ACNT);
                }
                else
                {
                    serializer.Serialize(writer, value);
                }
            }
        }
    }

    //public struct Amount
    //{
    //}
}