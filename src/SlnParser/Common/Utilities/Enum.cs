using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SlnParser.Common.Utilities
{
    public class Enum<TEnum> where TEnum : System.Enum
    {
        public IDictionary<TEnum, string> DescriptionsByEnumValue { get; set; }
        public IDictionary<string, TEnum> EnumValuesByDescription { get; set; }

        public Enum()
        {
            DescriptionsByEnumValue = System.Enum.GetNames(typeof(TEnum))
                .Select(enumValueName => { (string name, TEnum value, DescriptionAttribute descriptionAttr) e = (enumValueName, (TEnum)System.Enum.Parse(typeof(TEnum), enumValueName), typeof(TEnum).GetField(enumValueName).GetCustomAttribute<DescriptionAttribute>()); return e; })
                .Where(e => e.descriptionAttr != null)
                .ToDictionary(e => e.value, e => e.descriptionAttr.Description);
            EnumValuesByDescription = DescriptionsByEnumValue.AsReverseLookup();
        }

        public static string GetDescription<TEnumType>(TEnumType enumValue) where TEnumType : System.Enum => Enum.OfType<TEnumType>().DescriptionsByEnumValue[enumValue];
    }

    public static class Enum
    {
        private static readonly IDictionary<Type, object> _lookups = new Dictionary<Type, object>();

        public static Enum<TEnumType> OfType<TEnumType>() where TEnumType : System.Enum
        {
            Enum<TEnumType> enumType = null;
            if (_lookups.TryGetValue(typeof(TEnumType), out var enumTypeLookup) && enumTypeLookup is Enum<TEnumType> et)
                enumType = et;
            else
            {
                enumType = new Enum<TEnumType>();
                _lookups[typeof(TEnumType)] = enumType;
            }
            return enumType;
        }

        public static bool TryParse<TEnumType>(string s, out TEnumType result) where TEnumType : System.Enum
        {
            if (OfType<TEnumType>().EnumValuesByDescription.TryGetValue(s, out result))
                return true;
            var parsed = System.Enum.Parse(typeof(TEnumType), s);
            if (parsed != null && parsed is TEnumType parsedEnumType)
            {
                result = parsedEnumType;
                return true;
            }

            return false;
        }

        public static TEnumType Parse<TEnumType>(string s) where TEnumType : System.Enum => TryParse<TEnumType>(s, out var result) ? result : throw new Exception($"{s} is not recognized as a possible value for {nameof(TEnumType)}.");

    }
}
