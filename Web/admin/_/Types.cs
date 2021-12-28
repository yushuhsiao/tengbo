using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using web;

namespace BU
{
    class USERACNTConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return new USERACNT(value as string);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if ((value is USERACNT) && (destinationType == typeof(string)))
                return ((USERACNT)value).Value;
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverter(typeof(USERACNTConverter))]
    public struct USERACNT 
    {
        public readonly bool HasValue;
        public readonly string Value;
        public readonly string OriginalString;

        public USERACNT(string acnt)
        {
            Value = ValidAs.ACNT * acnt;
            OriginalString = acnt;
            HasValue = Value != null;
        }

        public override string ToString()
        {
            return this.Value ?? "";
        }

        public override bool Equals(object obj)
        {
            return (this.Value ?? "").Equals(obj);
        }

        public override int GetHashCode()
        {
            return (this.Value ?? "").GetHashCode();
        }

        public string CreatePassword(string password)
        {
            return text.EncodePassword(this.Value, password);
        }

        public static explicit operator string(USERACNT value)
        {
            return value.Value;
        }
        public static implicit operator USERACNT(string value)
        {
            return new USERACNT(value);
        }
    }
}