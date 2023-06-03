using System;
using System.ComponentModel;
using System.Reflection;

namespace Unidork.Extensions
{
	public static class EnumExtensions
	{
		public static string GetDescription<T>(this T enumValue) where T : Enum
		{
			FieldInfo enumField = enumValue.GetType().GetField(enumValue.ToString());
			DescriptionAttribute descriptionAttribute = Attribute.GetCustomAttribute(enumField, typeof(DescriptionAttribute)) as DescriptionAttribute;
			return descriptionAttribute == null ? enumValue.ToString() : descriptionAttribute.Description;
		}
	}
}