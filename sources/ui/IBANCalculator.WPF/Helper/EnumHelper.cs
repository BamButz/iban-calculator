using System;
using System.ComponentModel;
using System.Linq;

namespace IBANCalculator.WPF.Helper
{
	internal static class EnumHelper
	{
		public static string GetDescription(this Enum enumValue)
		{
			// Per Reflection alle Attribute vom Type DescriptionAttribute für den Wert des Enums ermitteln
			Type type = enumValue.GetType();
			var attributes = type.GetField(enumValue.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

			// Ist ein Description Attribut vorhanden?
			// Wenn ja die Beschreibung des ersten Attributes verwenden
			// Wenn nicht geben wir den Wert als String zurück
			if (attributes.Any())
				return (attributes.First() as DescriptionAttribute).Description;

			return enumValue.ToString();
		}
	}
}