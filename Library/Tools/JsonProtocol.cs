﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace Tools.Protocol
{
    [_DebuggerStepThrough]
    public class JsonProtocol
    {
        Dictionary<string, Type> types1 = new Dictionary<string, Type>();
        Dictionary<Type, string> types2 = new Dictionary<Type, string>();
        JsonProtocol(Assembly asm)
        {
            Dictionary<string, Type> tmp = new Dictionary<string, Type>();
            foreach (Type t in asm.GetTypes())
            {
                foreach (JsonObjectAttribute a in t.GetCustomAttributes(typeof(JsonObjectAttribute), false))
                {
                    string n = a.Id ?? t.Name;
                    if (tmp.ContainsKey(n))
                        tmp[n] = null;
                    else
                        tmp[n] = t;
                    break;
                }
            }
            foreach (var p in tmp)
            {
                if (p.Value == null) continue;
                types1[p.Key] = p.Value;
                types2[p.Value] = p.Key;
            }
        }

        static Dictionary<Assembly, JsonProtocol> defines = new Dictionary<Assembly, JsonProtocol>();
        public static JsonProtocol GetDefines(Assembly asm)
        {
            asm = asm ?? typeof(object).Assembly;
            lock (defines)
                if (defines.ContainsKey(asm))
                    return defines[asm];
                else
                    return defines[asm] = new JsonProtocol(asm);
        }
        //public static JsonProtocol GetDefines()
        //{
        //    return GetDefines(Assembly.GetCallingAssembly());
        //}

        public static bool MapName
        {
            get { return serializer.ProtocolContractResolver.MapName; }
            set { serializer.ProtocolContractResolver.MapName = value; }
        }
        static readonly _JsonSerializer serializer = new _JsonSerializer();

        [_DebuggerStepThrough]
        class _JsonSerializer : JsonSerializer
        {
            public static readonly _JsonSerializer Instance = new _JsonSerializer();

            public _JsonSerializer()
            {
                base.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                base.ContractResolver = new _ContractResolver();
            }

            public _ContractResolver ProtocolContractResolver
            {
                get { return (_ContractResolver)base.ContractResolver; }
                set { base.ContractResolver = value; }
            }
        }

        [_DebuggerStepThrough]
        class _ContractResolver : DefaultContractResolver
        {
            public bool MapName = true;

            protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
            {
                JsonDictionaryContract contract = base.CreateDictionaryContract(objectType);
                UnderlyingValueInDictionaryKeyAttribute.Apply(contract);
                return contract;
            }

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                JsonProperty p = base.CreateProperty(member, memberSerialization);
                if (!MapName)
                    p.PropertyName = member.Name;
                if (p.MemberConverter == null)
                {
                    Type t;
                    if (p.PropertyType.IsNullable(out t))
                    {
                        if (t == typeof(Guid))
                            p.MemberConverter = new GuidJsonConverter();
                        else if (t == typeof(Boolean))
                            p.MemberConverter = new BooleanJsonConverter();
                    }
                    else if (p.PropertyType == typeof(string))
                    {
                        p.MemberConverter = new StringJsonConverter(member, p);
                    }
                }
                return p;
            }
        }

        [_DebuggerStepThrough]
        public class BooleanJsonConverter : JsonConverter
        {
            public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.Value is Boolean)
                    return reader.Value;
                string input = reader.Value as string;
                if (string.IsNullOrEmpty(input))
                    return null;
                input = input.ToLower();
                if (Regex.IsMatch(input, "(false|f|0|no|n|off|undefined)", RegexOptions.IgnoreCase))
                    return false;
                if (Regex.IsMatch(input, "(true|t|1|yes|y|on)", RegexOptions.IgnoreCase))
                    return true;
                return null;
            }

            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }
        }

        [_DebuggerStepThrough, AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
        public class StringAttribute : Attribute
        {
            public bool Trim = false;
            public bool Empty = false;
        }

        [_DebuggerStepThrough]
        public class StringJsonConverter : JsonConverter
        {
            static int _id = 0;
            readonly int ID = Interlocked.Increment(ref _id);
            JsonProperty p;
            StringAttribute a;
            bool trim = true;
            bool empty = false;

            public StringJsonConverter() { }
            public StringJsonConverter(MemberInfo member, JsonProperty p)
            {
                this.p = p;
                foreach (StringAttribute a in member.GetCustomAttributes(typeof(StringAttribute), true))
                {
                    this.a = a;
                    this.trim = a.Trim;
                    this.empty = a.Empty;
                }
            }

            public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                string s = reader.Value as string;
                if (s != null)
                {
                    if (this.trim)
                        s = s.Trim();
                    if ((s == string.Empty) && (this.empty == false))
                        s = null;
                }
                return s;
            }

            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }
        }

        [_DebuggerStepThrough]
        public class Int32JsonConverter : JsonConverter
        {
            public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }
        }

        [_DebuggerStepThrough]
        public class GuidJsonConverter : JsonConverter
        {
            public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                try { return new Guid(reader.Value as string); }
                catch { if (objectType.IsNullable()) return null; else return Guid.Empty; }
            }

            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// 序列化的時候, 如果作為 Dictionary 的 Key, 將會使用數值而非字串
        /// </summary>
        [_DebuggerStepThrough, AttributeUsage(AttributeTargets.Enum)]
        public class UnderlyingValueInDictionaryKeyAttribute : Attribute
        {
            static List<UnderlyingValueInDictionaryKeyAttribute> cache = new List<UnderlyingValueInDictionaryKeyAttribute>();

            public static void Apply(JsonDictionaryContract contract)
            {
                if (contract == null) return;
                if (!contract.DictionaryKeyType.IsEnum) return;
                //if (contract.PropertyNameResolver != null) return;
                lock (cache)
                {
                    foreach (UnderlyingValueInDictionaryKeyAttribute a in cache)
                    {
                        if (a.type != contract.DictionaryKeyType) continue;
                        contract.PropertyNameResolver = a.ResolvePropertyName;
                        return;
                    }
                    foreach (UnderlyingValueInDictionaryKeyAttribute a in contract.DictionaryKeyType.GetCustomAttributes(typeof(UnderlyingValueInDictionaryKeyAttribute), true))
                    {
                        a.type = contract.DictionaryKeyType;
                        contract.PropertyNameResolver = a.ResolvePropertyName;
                        cache.Add(a);
                        return;
                    }
                }
            }

            Type type;
            string ResolvePropertyName(string propertyName)
            {
                if (Enum.IsDefined(type, propertyName))
                {
                    object o1 = Enum.Parse(type, propertyName);
                    object o2 = Convert.ChangeType(o1, Enum.GetUnderlyingType(type));
                    return Convert.ToString(o2);
                }
                return propertyName;
            }
        }

        //public class SerializerSettings
        //{
        //    public static SerializerSettings Default = new SerializerSettings();
        //    public bool MapName = true;
        //    public char QuoteChar = '\"';
        //    public bool QuoteName = false;
        //    public Formatting Formatting = Formatting.None;
        //}


        //public static string SerializeObject(object value, SerializerSettings setting)
        //{
        //    string result;
        //    JsonProtocol.Serialize<JsonWriter>(false, value, null, out result, setting);
        //    return result;
        //}
        //public static string Serialize(object value, Type propertyName, SerializerSettings setting)
        //{
        //    string result;
        //    JsonProtocol.Serialize<JsonWriter>(true, value, propertyName, out result, setting);
        //    return result;
        //}
        //public static bool Serialize(object value, Type propertyName, out string json, SerializerSettings setting)
        //{
        //    json = null;
        //    if (propertyName == null)
        //        if (value == null)
        //            return false;
        //        else
        //            propertyName = value.GetType();
        //    JsonProtocol define = JsonProtocol.GetDefines(propertyName.Assembly);
        //    if (!define.types2.ContainsKey(propertyName))
        //        return false;
        //    setting = setting ?? SerializerSettings.Default;
        //    using (StringWriter s = new StringWriter())
        //    using (JsonTextWriter j = new JsonTextWriter(s)
        //    {
        //        QuoteChar = setting.QuoteChar,
        //        QuoteName = setting.QuoteName,
        //        Formatting = setting.Formatting,
        //    })
        //    {
        //        j.WriteStartObject();
        //        j.WritePropertyName(define.types2[propertyName]);
        //        if (value != null)
        //            (setting.MapName ? serializer1 : serializer2).Serialize(j, value);
        //        j.WriteEndObject();
        //        j.Flush();
        //        s.Flush();
        //        json = s.ToString();
        //        return true;
        //    }
        //}

        //public static string SerializeObject(object value)
        //{
        //    return JsonProtocol.Serialize<JsonWriter>(value);
        //}
        //public static string SerializeObject<TJsonWriter>(object value) where TJsonWriter : JsonTextWriter
        //{
        //    string result;
        //    JsonProtocol.Serialize<TJsonWriter>(false, value, null, out result);
        //    return result;
        //}
        //public static string Serialize(object value)
        //{
        //    return JsonProtocol.Serialize<_Writer>(value);
        //}
        //public static string Serialize<TJsonWriter>(object value) where TJsonWriter : JsonTextWriter
        //{
        //    string result;
        //    JsonProtocol.Serialize<TJsonWriter>(true, value, null, out result);
        //    return result;
        //}
        //public static bool Serialize(object value, out string json)
        //{
        //    return JsonProtocol.Serialize<_Writer>(value, out json);
        //}
        //public static bool Serialize<TJsonWriter>(object value, out string json) where TJsonWriter : JsonTextWriter
        //{
        //    return Serialize<TJsonWriter>(true, value, null, out json);
        //}

        //public static bool Serialize<TJsonWriter>(bool protocol, object value, Type propertyName, out string json/*, SerializerSettings setting*/) where TJsonWriter : JsonTextWriter
        //{
        //    JsonProtocol define;
        //    json = null;
        //    if (protocol)
        //    {
        //        if (propertyName == null)
        //            if (value == null)
        //                return false;
        //            else
        //                propertyName = value.GetType();
        //        define = JsonProtocol.GetDefines(propertyName.Assembly);
        //        if (!define.types2.ContainsKey(propertyName))
        //            return false;
        //    }
        //    else
        //        define = null;
        //    //setting = setting ?? SerializerSettings.Default;
        //    using (StringWriter s = new StringWriter())
        //    using (TJsonWriter j = (TJsonWriter)Activator.CreateInstance(typeof(TJsonWriter), s))
        //    {
        //        if (protocol)
        //        {
        //            j.WriteStartObject();
        //            j.WritePropertyName(define.types2[propertyName]);
        //        }
        //        if (value != null)
        //            serializer.Serialize(j, value);
        //        if (protocol)
        //            j.WriteEndObject();
        //        j.Flush();
        //        s.Flush();
        //        json = s.ToString();
        //        return true;
        //    }
        //}

        [_DebuggerStepThrough]
        public class JsonWriter : JsonTextWriter
        {
            public JsonWriter(TextWriter textWriter)
                : base(textWriter)
            {
                base.QuoteChar = '\"';
                base.QuoteName = false;
                base.Formatting = Formatting.None;
            }
        }

        public static bool Serialize(object value, out string json)
        {
            return JsonProtocol.Serialize<JsonWriter>(null, value, out json);
        }
        public static bool Serialize(Type protocolType, object value, out string json)
        {
            return JsonProtocol.Serialize<JsonWriter>(protocolType, value, out json);
        }
        public static bool Serialize<TJsonWriter>(object value, out string json) where TJsonWriter : JsonWriter
        {
            return JsonProtocol.Serialize<TJsonWriter>(null, value, out json);
        }
        public static bool Serialize<TJsonWriter>(Type protocolType, object value, out string json) where TJsonWriter : JsonWriter
        {
            json = null;
            if (protocolType == null)
            {
                if (value == null)
                    return false;
                protocolType = value.GetType();
            }
            JsonProtocol define = GetDefines(protocolType.Assembly);
            if (define.types2.ContainsKey(protocolType))
            {
                json = JsonProtocol.SerializeObject<TJsonWriter>(define.types2[protocolType], value);
                return true;
            }
            return false;
        }

        public static string SerializeObject(string propertyName, object value)
        {
            return JsonProtocol.SerializeObject<JsonWriter>(propertyName, value);
        }
        public static string SerializeObject(object value)
        {
            return JsonProtocol.SerializeObject<JsonWriter>(null, value);
        }
        public static string SerializeObject<TJsonWriter>(object value) where TJsonWriter : JsonWriter
        {
            return JsonProtocol.SerializeObject<TJsonWriter>(null, value);
        }
        public static string SerializeObject<TJsonWriter>(string propertyName, object value) where TJsonWriter : JsonWriter
        {
            using (StringWriter s = new StringWriter())
            using (TJsonWriter j = (TJsonWriter)Activator.CreateInstance(typeof(TJsonWriter), s))
            {
                if (propertyName != null)
                {
                    j.WriteStartObject();
                    j.WritePropertyName(propertyName);
                }
                if (value != null)
                    serializer.Serialize(j, value);
                if (propertyName != null)
                    j.WriteEndObject();
                j.Flush();
                s.Flush();
                return s.ToString();
            }
        }




        [_DebuggerStepThrough]
        public class JsonReader : JsonTextReader
        {
            public JsonReader(TextReader reader) : base(reader) { }

            public bool ReadToName(out string name)
            {
                if (this.Read() && (this.TokenType == JsonToken.StartObject))
                {
                    if (this.Read() && (this.TokenType == JsonToken.PropertyName))
                    {
                        name = this.Value as string;
                        return this.Read() && (this.TokenType == JsonToken.StartObject);
                    }
                }
                name = null;
                return false;
            }

            public bool ReadProtocolType(out string name, out Type type, Assembly asm)
            {
                Type t = null;
                if (this.ReadToName(out name))
                {
                    JsonProtocol define = GetDefines(asm);
                    if (!define.types1.TryGetValue(name, out t))
                    {
                        Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
                        for (int i = asms.Length - 1; i >= 0; i--)
                        {
                            define = GetDefines(asms[i]);
                            if (define.types1.TryGetValue(name, out t))
                            {
                                type = t;
                                return true;
                            }
                        }
                    }
                }
                type = null;
                return false;
            }
        }

        public static bool Deserialize(string json, out object value)
        {
            return JsonProtocol.Deserialize<JsonReader>(json, out value, null, Assembly.GetCallingAssembly());
        }
        public static bool Deserialize(string json, out object value, Assembly asm)
        {
            return JsonProtocol.Deserialize<JsonReader>(json, out value, null, asm);
        }

        public static bool Deserialize<TJsonReader>(string json, out object value) where TJsonReader : JsonReader
        {
            return JsonProtocol.Deserialize<TJsonReader>(json, out value, null, Assembly.GetCallingAssembly());
        }
        public static bool Deserialize<TJsonReader>(string json, out object value, Assembly asm) where TJsonReader : JsonReader
        {
            return JsonProtocol.Deserialize<TJsonReader>(json, out value, null, asm);
        }

        public static T DeserializeObject<T>(string json)
        {
            return JsonProtocol.DeserializeObject<JsonReader, T>(json);
        }
        public static T DeserializeObject<TJsonReader, T>(string json) where TJsonReader : JsonReader
        {
            object value;
            if (JsonProtocol.Deserialize<TJsonReader>(json, out value, typeof(T), null) && (value is T))
                return (T)value;
            return default(T);
        }

        // valueType != null, 不使用 Protocol Define
        public static bool Deserialize<TJsonReader>(string json, out object value, Type valueType, Assembly asm) where TJsonReader : JsonReader
        {
            if (json != null)
            {
                using (StringReader r1 = new StringReader(json))
                using (TJsonReader r2 = (TJsonReader)Activator.CreateInstance(typeof(TJsonReader), r1))
                {
                    string name;
                    if (r2.ReadToName(out name))
                    {
                        if (valueType == null)
                        {
                            Assembly a1;
                            Assembly[] a2 = null;
                            for (int i = -1; ; i++)
                            {
                                if (i == -1)
                                    a1 = asm;
                                else
                                {
                                    if (a2 == null)
                                        a2 = AppDomain.CurrentDomain.GetAssemblies();
                                    if (i >= a2.Length)
                                        break;
                                    a1 = a2[i];
                                }
                                if (a1 == null) continue;
                                JsonProtocol define = GetDefines(a1);
                                if (define.types1.TryGetValue(name, out valueType))
                                    break;
                            }
                        }
                        if (valueType != null)
                        {
                            value = serializer.Deserialize(r2, valueType);
                            return true;
                        }
                    }
                }
            }
            value = null;
            return false;
        }

        //public static bool Deserialize(string json, out object value, Assembly asm)
        //{
        //    value = null;
        //    if (json != null)
        //    {
        //        using (StringReader r1 = new StringReader(json))
        //        using (JsonReader r2 = new JsonReader(r1))
        //        {
        //            string name;
        //            Type type;
        //            if (r2.ReadProtocolType(out name, out type, asm))
        //            {
        //                value = serializer.Deserialize(r2, type);
        //                return true;
        //            }
        //            //if (r2.ReadToName(out name))
        //            //{
        //            //    Type t = null;
        //            //    JsonProtocol define = GetDefines(asm);
        //            //    if (!define.types1.TryGetValue(name, out t))
        //            //    {
        //            //        Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
        //            //        for (int i = asms.Length - 1; i >= 0; i--)
        //            //        {
        //            //            define = GetDefines(asms[i]);
        //            //            if (define.types1.TryGetValue(name, out t))
        //            //                break;
        //            //        }
        //            //    }
        //            //    if (t != null)
        //            //    {
        //            //        value = (JsonProtocol.MapName ? serializer1 : serializer2).Deserialize(r2, t);
        //            //        return true;
        //            //    }
        //            //}
        //        }
        //    }
        //    return false;
        //}

        //public static bool Deserialize(string json, out object value)
        //{
        //    return Deserialize(json, out value, Assembly.GetCallingAssembly());
        //}
        //public static bool Deserialize(string json, out object value, Assembly asm)
        //{
        //    value = null;
        //    if (json != null)
        //    {
        //        JsonProtocol define = GetDefines(asm);
        //        using (StringReader r1 = new StringReader(json))
        //        using (JsonTextReader r2 = new JsonTextReader(r1))
        //        {
        //            string name;
        //            if (ReadToName(r2, out name, define))
        //            {
        //                value = (SerializerSettings.Default.MapName ? serializer1 : serializer2).Deserialize(r2, define.types1[name]);
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        public static bool Populate<TJsonReader>(string json, object value) where TJsonReader : JsonReader
        {
            if ((json != null) && (value != null))
            {
                using (StringReader r1 = new StringReader(json))
                using (TJsonReader r2 = (TJsonReader)Activator.CreateInstance(typeof(TJsonReader), r1))
                {
                    serializer.Populate(r2, value);
                    return true;
                }
            }
            return false;
        }

        public static bool Populate(string json, object value)
        {
            if ((json != null) && (value != null))
            {
                using (StringReader s = new StringReader(json))
                using (JsonReader j = new JsonReader(s))
                {
                    string name;
                    if (j.ReadToName(out name))
                    {
                        serializer.Populate(j, value);
                        return true;
                    }
                }
            }
            return false;
        }

        #region //

        //public static bool Serialize(object value, out string json)
        //{
        //    return Serialize(null, value, out json, null, null, null);
        //}

        //public static bool Serialize(Type type, object value, out string json)
        //{
        //    return Serialize(type, value, out json, null, null, null);
        //}

        //public static bool Serialize<T>(object value, out string json)
        //{
        //    return Serialize(typeof(T), value, out json, null, null, null);
        //}

        //public static bool Serialize(Type type, object value, out string json, bool? mapName, bool? quoteName, Formatting? formatting)
        //{
        //    if (type == null)
        //    {
        //        if (value == null)
        //        {
        //            json = null;
        //            return false;
        //        }
        //        type = value.GetType();
        //    }
        //    if (type.IsPrimitive || (type == typeof(string)))
        //    {
        //        json = JsonConvert.SerializeObject(value);
        //        return true;
        //    }
        //    JsonProtocol define = GetDefines(type.Assembly);
        //    if (define.types2.ContainsKey(type))
        //    {
        //        using (StringWriter sw = new StringWriter())
        //        {
        //            using (JsonTextWriter jw = new JsonTextWriter(sw)
        //            {
        //                QuoteName = quoteName ?? SerializerSettings.QuoteName,
        //                Formatting = formatting ?? SerializerSettings.Formatting,
        //            })
        //            {
        //                jw.WriteStartObject();
        //                jw.WritePropertyName(define.types2[type]);
        //                ((mapName ?? SerializerSettings.MapName) ? serializer1 : serializer2).Serialize(jw, value);
        //                jw.WriteEndObject();
        //            }
        //            sw.Flush();
        //            json = sw.ToString();
        //        }
        //    }
        //    else
        //        json = null;
        //    return json != null;
        //}

        //public string Serialize(object value)
        //{
        //    string json; Serialize(value, out json); return json;
        //}

        //public static bool Populate(string json, object result)
        //{
        //    if ((json != null) && (result != null))
        //        using (StringReader r1 = new StringReader(json))
        //        using (JsonTextReader r2 = new JsonTextReader(r1))
        //            if (r2.Read() && (r2.TokenType == JsonToken.StartObject))
        //                if (r2.Read() && (r2.TokenType == JsonToken.PropertyName))
        //                    if (r2.TokenType == JsonToken.StartObject)
        //                        (SerializerSettings.MapName ? serializer1 : serializer2).Populate(r2, result);
        //    return false;
        //}

        //public static bool Deserialize(string json, out object value, Assembly asm)
        //{
        //    value = null;
        //    if (json == null)
        //        return false;
        //    JsonProtocol define = GetDefines(asm ?? Assembly.GetCallingAssembly());
        //    using (StringReader r1 = new StringReader(json))
        //    using (JsonTextReader r2 = new JsonTextReader(r1))
        //    {
        //        if (r2.Read())
        //        {
        //            if (r2.TokenType == JsonToken.StartObject)
        //            {
        //                if (r2.Read() && (r2.TokenType == JsonToken.PropertyName))
        //                {
        //                    string name = r2.Value as string;
        //                    if ((name != null) && define.types1.ContainsKey(name))
        //                    {
        //                        if (r2.Read() && (r2.TokenType == JsonToken.StartObject))
        //                        {
        //                            value = (SerializerSettings.MapName ? serializer1 : serializer2).Deserialize(r2, define.types1[name]);
        //                        }
        //                    }
        //                }
        //            }
        //            else if (
        //                (r2.TokenType == JsonToken.Integer) ||
        //                (r2.TokenType == JsonToken.Float) ||
        //                (r2.TokenType == JsonToken.String) ||
        //                (r2.TokenType == JsonToken.Boolean) ||
        //                (r2.TokenType == JsonToken.Null))
        //                value = JsonConvert.DeserializeObject(json);
        //        }
        //    }
        //    return value != null;
        //}

        //public static bool Deserialize(string json, out object value)
        //{
        //    return Deserialize(json, out value, Assembly.GetCallingAssembly());
        //}

        //public static object Deserialize(string json, Assembly asm)
        //{
        //    object value; Deserialize(json, out value, asm); return value;
        //}

        //public static object Deserialize(string json)
        //{
        //    object value; Deserialize(json, out value, Assembly.GetCallingAssembly()); return value;
        //}

        #endregion

    }


    #region //
    //[_DebuggerStepThrough]
    //public static class JsonProtocol2
    //{
    //    static Dictionary<Assembly, Dictionary<string, Type>> defines = new Dictionary<Assembly, Dictionary<string, Type>>();
    //    static Dictionary<string, Type> define_null = new Dictionary<string, Type>();

    //    static Dictionary<string, Type> GetCache(Assembly asm)
    //    {
    //        if (asm == null)
    //            return define_null;
    //        lock (defines)
    //        {
    //            if (defines.ContainsKey(asm))
    //                return defines[asm];
    //            Dictionary<string, Type> dict = new Dictionary<string, Type>();
    //            foreach (Type t in asm.GetTypes())
    //            {
    //                foreach (JsonObjectAttribute a in t.GetCustomAttributes(typeof(JsonObjectAttribute), false))
    //                {
    //                    string id = a.Id ?? t.Name;
    //                    if (dict.ContainsKey(id))
    //                        dict[id] = null;
    //                    else
    //                        dict[id] = t;
    //                }
    //            }
    //            while (dict.ContainsValue(null))
    //            {
    //                foreach (string s in dict.Keys)
    //                {
    //                    if (dict[s] == null)
    //                    {
    //                        dict.Remove(s);
    //                        break;
    //                    }
    //                }
    //            }
    //            return defines[asm] = dict;
    //        }
    //    }

    //    public static string GetDefine(Type t)
    //    {
    //        if (t != null)
    //            foreach (KeyValuePair<string, Type> p in GetCache(t.Assembly))
    //                if (p.Value == t)
    //                    return p.Key;
    //        return null;
    //    }
    //    public static string GetDefine(object value)
    //    {
    //        if (value != null)
    //            return GetDefine(value.GetType());
    //        return null;
    //    }

    //    static Type GetDefine(Assembly asm, string commandID)
    //    {
    //        if (commandID != null)
    //        {
    //            Dictionary<string, Type> dict = GetCache(asm);
    //            if (dict.ContainsKey(commandID))
    //                return dict[commandID];
    //        }
    //        return null;
    //    }

    //    public static T Deserialize<T>(string jsonString) where T : new()
    //    {
    //        T value = new T();
    //        Populate(value, jsonString);
    //        return value;
    //    }
    //    public static object Deserialize(Type type, string jsonString)
    //    {
    //        if (type == null)
    //            return null;
    //        object value = Activator.CreateInstance(type);
    //        Populate(value, jsonString);
    //        return value;
    //    }
    //    public static object Deserialize(Assembly asm, string commandID, string jsonString)
    //    {
    //        return Deserialize(GetDefine(asm, commandID), jsonString);
    //    }
    //    public static bool Deserialize(Assembly asm, string commandID, string jsonString, out object value)
    //    {
    //        return (value = Deserialize(GetDefine(asm, commandID), jsonString)) != null;
    //    }
    //    public static bool Deserialize(Assembly asm, string commandID, string jsonString, out object value, out Type type)
    //    {
    //        return (value = Deserialize(type = GetDefine(asm, commandID), jsonString)) != null;
    //    }

    //    public static void Populate(object value, string jsonString)
    //    {
    //        Populate(value, jsonString, false);
    //    }
    //    public static void Populate(object value, string jsonString, bool? mapName)
    //    {
    //        using (StringReader sr = new StringReader(jsonString))
    //        using (JsonTextReader jr = new JsonTextReader(sr))
    //            ((mapName ?? Serialize_MapName) ? serializer1 : serializer2).Populate(jr, value);
    //    }

    //    public static bool Serialize(object value, out string commandID, out string jsonString)
    //    {
    //        return Serialize(value, out commandID, out jsonString, null, null, null);
    //    }
    //    public static bool Serialize(object value, out string commandID, out string jsonString, bool? mapName, Formatting? formatting, bool? quoteName)
    //    {
    //        commandID = GetDefine(value);
    //        if (commandID != null)
    //        {
    //            jsonString = Serialize(value, mapName, formatting, quoteName);
    //            return true;
    //        }
    //        jsonString = null;
    //        return false;
    //    }
    //    public static string Serialize(object value)
    //    {
    //        return Serialize(value, null, null, null);
    //    }
    //    public static string Serialize(object value, bool? mapName, Formatting? formatting, bool? quoteName)
    //    {
    //        using (StringWriter sw = new StringWriter())
    //        {
    //            using (JsonTextWriter jw = new JsonTextWriter(sw) { Formatting = formatting ?? Serialize_Formatting, QuoteName = quoteName ?? Serialize_QuoteName, })
    //                ((mapName ?? Serialize_MapName) ? serializer1 : serializer2).Serialize(jw, value);
    //            sw.Flush();
    //            return sw.ToString();
    //        }
    //    }
    //    public static bool Serialize_QuoteName = false;
    //    public static Formatting Serialize_Formatting = Formatting.None;
    //    public static bool Serialize_MapName = true;

    //    static JsonSerializer serializer1 = new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore, };
    //    static JsonSerializer serializer2 = new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new ContractResolver(), };

    //    class ContractResolver : DefaultContractResolver
    //    {
    //        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    //        {
    //            JsonProperty p = base.CreateProperty(member, memberSerialization);
    //            p.PropertyName = member.Name;
    //            return p;
    //        }
    //    }
    //}
    #endregion

    [_DebuggerStepThrough]
    public class IPEndPointArrayJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IPEndPoint[]);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IPEndPoint[] ip = value as IPEndPoint[];
            if (ip != null)
            {
                writer.WriteStartArray();
                for (int i = 0; i < ip.Length; i++)
                    writer.WriteValue(ip[i].ToString());
                writer.WriteEndArray();
            }
            else
                //throw new NotImplementedException();
                serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IPEndPoint[] ips = null;
            if (reader.TokenType == JsonToken.StartArray)
            {
                int depth = reader.Depth;
                while (reader.Read())
                {
                    if ((reader.Depth <= depth) && (reader.TokenType == JsonToken.EndArray))
                        break;
                    if (reader.ValueType == typeof(string))
                    {
                        string[] s = ((string)reader.Value).Split(':');
                        if (s.Length >= 2)
                        {
                            IPAddress ip;
                            int port;
                            if (IPAddress.TryParse(s[0], out ip) && int.TryParse(s[1], out port))
                            {
                                if (ips == null)
                                    ips = new IPEndPoint[1];
                                else
                                    Array.Resize(ref ips, ips.Length + 1);
                                ips[ips.Length - 1] = new IPEndPoint(ip, port);
                            }
                        }
                    }
                }
            }
            return ips;
        }
    }

    // 為了避免 decimal 在序列化的時候, 小數點位數出現太多0, 所以如果值不包含小數點, 就將值轉換成 long 之後再進行序列化
    [_DebuggerStepThrough]
    public class PointsJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal?)) || (objectType == typeof(decimal));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if ((value is decimal) || (value is decimal?))
            {
                decimal v1;
                if (value is decimal)
                    v1 = (decimal)value;
                else
                {
                    decimal? v_ = (decimal?)value;
                    if (v_ == null) return;
                    v1 = v_.Value;
                }

                long v2 = (long)v1;
                decimal v3 = (decimal)v2;
                if (v1 == v3)
                {
                    serializer.Serialize(writer, v2);
                    return;
                }
            }
            serializer.Serialize(writer, value);
        }
    }

    //[_DebuggerStepThrough]
    //public static partial class JsonConvert<T>
    //{
    //    static Dictionary<Assembly, Dictionary<T, Type>> cache = new Dictionary<Assembly, Dictionary<T, Type>>();

    //    static Dictionary<T, Type> GetCache(Assembly assembly)
    //    {
    //        if (assembly == null)
    //            return null;
    //        lock (cache)
    //        {
    //            if (!cache.ContainsKey(assembly))
    //            {
    //                cache[assembly] = new Dictionary<T, Type>();
    //                foreach (Type t in assembly.GetTypes())
    //                {
    //                    foreach (object a in t.GetCustomAttributes(true))
    //                    {
    //                        JsonObjectAttribute p = a as JsonObjectAttribute;
    //                        if (p == null) continue;
    //                        T id;
    //                        if (typeof(T) == typeof(int))
    //                        {
    //                            if (string.IsNullOrEmpty(p.Id)) continue;
    //                            int commandID;
    //                            if (!int.TryParse(p.Id.Substring(1), out commandID))
    //                                continue;
    //                            id = (T)(object)commandID;
    //                        }
    //                        else if (typeof(T) == typeof(string))
    //                            id = (T)(object)(string.IsNullOrEmpty(p.Id) ? t.Name : p.Id);
    //                        else continue;

    //                        if (cache[assembly].ContainsKey(id))
    //                            cache[assembly][id] = null;
    //                        else
    //                            cache[assembly][id] = t;
    //                    }
    //                }
    //                while (cache[assembly].ContainsValue(null))
    //                {
    //                    foreach (T key in cache[assembly].Keys)
    //                    {
    //                        if (cache[assembly][key] == null)
    //                        {
    //                            cache[assembly].Remove(key);
    //                            break;
    //                        }
    //                    }
    //                }
    //            }
    //            return cache[assembly];
    //        }
    //    }

    //    public static bool GetDefine(T id, out Type type)
    //    {
    //        return GetDefineFrom(Assembly.GetEntryAssembly(), id, out type);
    //    }
    //    public static bool GetDefineFrom<assembly>(T id, out Type type)
    //    {
    //        return GetDefineFrom(Assembly.GetAssembly(typeof(assembly)), id, out type);
    //    }
    //    public static bool GetDefineFrom(Assembly assembly, T id, out Type type)
    //    {
    //        Dictionary<T, Type> table = GetCache(assembly);
    //        if (table != null)
    //        {
    //            if (table.ContainsKey(id))
    //            {
    //                type = table[id];
    //                return true;
    //            }
    //        }
    //        type = null;
    //        return false;
    //    }

    //    public static bool GetDefine(Type type, out T commandID)
    //    {
    //        return GetDefineFrom(Assembly.GetEntryAssembly(), type, out commandID);
    //    }
    //    public static bool GetDefineFrom<assembly>(Type type, out T commandID)
    //    {
    //        return GetDefineFrom(Assembly.GetAssembly(typeof(assembly)), type, out commandID);
    //    }
    //    public static bool GetDefineFrom(Assembly assembly, Type type, out T commandID)
    //    {
    //        Dictionary<T, Type> table = GetCache(assembly);
    //        if (table != null)
    //        {
    //            if (table.ContainsValue(type))
    //            {
    //                foreach (KeyValuePair<T, Type> k in table)
    //                {
    //                    if (k.Value == type)
    //                    {
    //                        commandID = k.Key;
    //                        return true;
    //                    }
    //                }
    //            }
    //        }
    //        commandID = default(T);
    //        return false;
    //    }

    //    public static bool Serialize(object value, out T commandID, out string jsonString)
    //    {
    //        return SerializeFrom(Assembly.GetEntryAssembly(), value, out commandID, out jsonString);
    //    }
    //    public static bool SerializeFrom<assembly>(object value, out T commandID, out string jsonString)
    //    {
    //        return SerializeFrom(Assembly.GetAssembly(typeof(assembly)), value, out commandID, out jsonString);
    //    }
    //    public static bool SerializeFrom(Assembly assembly, object value, out T commandID, out string jsonString)
    //    {
    //        if (value != null)
    //        {
    //            if (GetDefineFrom(assembly, value.GetType(), out commandID))
    //            {
    //                jsonString = JsonConvert.Serialize(value);
    //                return true;
    //            }
    //        }
    //        commandID = default(T);
    //        jsonString = null;
    //        return false;
    //    }

    //    public static object Deserialize(T commandID, string jsonString)
    //    {
    //        return DeserializeFrom(Assembly.GetEntryAssembly(), commandID, jsonString);
    //    }
    //    public static object DeserializeFrom<assembly>(T commandID, string jsonString)
    //    {
    //        return DeserializeFrom(Assembly.GetAssembly(typeof(assembly)), commandID, jsonString);
    //    }
    //    public static object DeserializeFrom(Assembly assembly, T commandID, string jsonString)
    //    {
    //        Type t;
    //        if (GetDefineFrom(assembly, commandID, out t))
    //        {
    //            object value = Activator.CreateInstance(t);
    //            JsonConvert.Populate(value, jsonString);
    //            return value;
    //        }
    //        return null;
    //    }
    //}


    //[_DebuggerStepThrough]
    //public static partial class JsonConvert
    //{
    //    public static bool GetDefine(int commandID, out Type type)
    //    {
    //        return JsonConvert<int>.GetDefine(commandID, out type);
    //    }
    //    public static bool GetDefineFrom<assembly>(int commandID, out Type type)
    //    {
    //        return JsonConvert<int>.GetDefineFrom<assembly>(commandID, out type);
    //    }
    //    public static bool GetDefineFrom(Assembly assembly, int commandID, out Type type)
    //    {
    //        return JsonConvert<int>.GetDefineFrom(assembly, commandID, out type);
    //    }

    //    public static bool GetDefine(Type type, out int commandID)
    //    {
    //        return JsonConvert<int>.GetDefine(type, out commandID);
    //    }
    //    public static bool GetDefineFrom<assembly>(Type type, out int commandID)
    //    {
    //        return JsonConvert<int>.GetDefineFrom<assembly>(type, out commandID);
    //    }
    //    public static bool GetDefineFrom(Assembly assembly, Type type, out int commandID)
    //    {
    //        return JsonConvert<int>.GetDefineFrom(assembly, type, out commandID);
    //    }

    //    public static bool Serialize(object value, out int commandID, out string jsonString)
    //    {
    //        return JsonConvert<int>.Serialize(value, out commandID, out jsonString);
    //    }
    //    public static bool SerializeFrom<assembly>(object value, out int commandID, out string jsonString)
    //    {
    //        return JsonConvert<int>.SerializeFrom<assembly>(value, out commandID, out jsonString);
    //    }
    //    public static bool SerializeFrom(Assembly assembly, object value, out int commandID, out string jsonString)
    //    {
    //        return JsonConvert<int>.SerializeFrom(assembly, value, out commandID, out jsonString);
    //    }

    //    public static object Deserialize(int commandID, string jsonString)
    //    {
    //        return JsonConvert<int>.Deserialize(commandID, jsonString);
    //    }
    //    public static object DeserializeFrom<assembly>(int commandID, string jsonString)
    //    {
    //        return JsonConvert<int>.DeserializeFrom<assembly>(commandID, jsonString);
    //    }
    //    public static object DeserializeFrom(Assembly assembly, int commandID, string jsonString)
    //    {
    //        return JsonConvert<int>.DeserializeFrom(assembly, commandID, jsonString);
    //    }


    //    public static bool Serialize_QuoteName = false;
    //    public static string Serialize(object value)
    //    {
    //        return Serialize(value, false, Formatting.None, Serialize_QuoteName);
    //    }
    //    public static string Serialize(object value, bool originalPropertyName, Formatting formatting, bool quoteName)
    //    {
    //        using (StringWriter sw = new StringWriter())
    //        {
    //            using (JsonTextWriter jw = new JsonTextWriter(sw) { Formatting = formatting, QuoteName = quoteName, })
    //                (originalPropertyName ? serializer1 : serializer2).Serialize(jw, value);
    //            sw.Flush();
    //            return sw.ToString();
    //        }
    //    }

    //    public static void Populate(object value, string jsonString)
    //    {
    //        Populate(value, jsonString, false);
    //    }
    //    public static void Populate(object value, string jsonString, bool originalPropertyName)
    //    {
    //        using (StringReader sr = new StringReader(jsonString))
    //        using (JsonTextReader jr = new JsonTextReader(sr))
    //            (originalPropertyName ? serializer1 : serializer2).Populate(jr, value);
    //    }

    //    public static object Deserialize(Type type, string jsonString, params object[] args)
    //    {
    //        return Deserialize(type, jsonString, false, args);
    //    }
    //    public static object Deserialize(Type type, string jsonString, bool originalPropertyName, params object[] args)
    //    {
    //        object obj = Activator.CreateInstance(type, args);
    //        Populate(obj, jsonString, originalPropertyName);
    //        return obj;
    //    }

    //    public static T Deserialize<T>(string jsonString, bool originalPropertyName, params object[] args)
    //    {
    //        return (T)Deserialize(typeof(T), jsonString, originalPropertyName, args);
    //    }
    //    public static T Deserialize<T>(string jsonString, params object[] args)
    //    {
    //        return (T)Deserialize(typeof(T), jsonString, false, args);
    //    }





    //    static JsonSerializer serializer1 = new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new ContractResolver(), };
    //    static JsonSerializer serializer2 = new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore, };

    //    class ContractResolver : DefaultContractResolver
    //    {
    //        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    //        {
    //            JsonProperty p = base.CreateProperty(member, memberSerialization);
    //            p.PropertyName = member.Name;
    //            return p;
    //        }
    //    }

    //}
}