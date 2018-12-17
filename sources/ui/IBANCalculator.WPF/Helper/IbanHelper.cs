using System;
using System.Linq;
using IBANCalculator.WPF.Definitions;

namespace IBANCalculator.WPF.Helper
{
	internal class IbanHelper
	{
		// Berechnung entnommen von http://www.pruefziffernberechnung.de/I/IBAN.shtml
		// Längen der jeweiligen Länder (https://www.naspa.de/firmenkunden/internationale-aktivitaeten/auslandszahlungsverkehr/ihre-internationale-bankadresse/iban-rechner/iban-uebersicht/)
		public static string GenerateIban(Countries country, string ktoNr, string blz)
		{
			// Parameter prüfen
			if (String.IsNullOrEmpty(ktoNr) || String.IsNullOrEmpty(blz))
				return String.Empty;

			// BBAN ermitteln und falls nötig mit Nullen auffüllen
			string bban = string.Empty;
			switch (country)
			{
				case Countries.AT:
					bban = blz.PadLeft(5, '0') + ktoNr.PadLeft(11, '0');
					break;

				case Countries.CH:
					bban = blz.PadLeft(5, '0') + ktoNr.PadLeft(12, '0');
					break;

				case Countries.DE:
					bban = blz.PadLeft(8, '0') + ktoNr.PadLeft(10, '0');
					break;

				case Countries.NL:
					bban = blz.PadLeft(4, '0') + ktoNr.PadLeft(10, '0');
					break;

				default:
					return String.Empty;
			}

			// Konvertierung des Chars in UTF-8 Integer Wert
			// Subtraktion dieses Wertes um 55 damit wir auf ASCII Wert gelangen
			// Anhängen von "00" da keine Prüfsumme vorhanden
			string cId = country.ToString().Aggregate("", (current, c) => current + (c - 55).ToString()) + "00";

			// Da Schweizer Kontonummern Buchstaben enthalten können müssen wir
			// zur Prüfsummenberechnung diese erst in ihr ASCII Equivalent umwandeln
			string bbanNumeric = bban.Aggregate("", (current, c) =>
			{
				if (!Char.IsDigit(c))
					current += (c - 55).ToString();
				else
					current += c;

				return current;
			});

			// Konvertieren in decimal (meines Wissens nach der einzige Datentyp der die nötige Größe hat)
			// um Modulooperation und die laut ISO auszuführende Subtraktion von 98 durchzuführen
			decimal bbanDouble = decimal.Parse(bbanNumeric + cId);
			int checksum = 98 - (int)(bbanDouble % 97);

			// Gegenprüfung ausführen falls erfolgreich dann zusammensetzen und zurückgeben :)
			string ccStr = bbanNumeric + cId.Substring(0, 4) + checksum.ToString().PadLeft(2, '0');
			decimal cc = decimal.Parse(ccStr) % 97;

			if (cc == 1)
				return country.ToString() + checksum.ToString().PadLeft(2, '0') + bban;

			return String.Empty;
		}
	}
}