using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using IBANCalculator.WPF.Definitions;
using IBANCalculator.WPF.Helper;

namespace IBANCalculator.WPF.ViewModels
{
	internal class IbanVM : INotifyPropertyChanged
	{
		private Countries _country;

		public Countries Country
		{
			get { return _country; }
			set
			{
				_country = value;

				// Wenn Land geändert wird setzen wir die restlichen Daten zurück
				KtoNr = String.Empty;
				BLZ = String.Empty;

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Country)));
			}
		}

		private string _ktoNr;

		public string KtoNr
		{
			get { return _ktoNr; }
			set
			{
				_ktoNr = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KtoNr)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderBrushKtoNr)));
			}
		}

		private string _blz;

		public string BLZ
		{
			get { return _blz; }
			set
			{
				_blz = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BLZ)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderBrushBLZ)));
			}
		}

		private string _iban;

		public string IBAN
		{
			get { return _iban; }
			set
			{
				_iban = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IBAN)));
			}
		}

		public Brush BorderBrushKtoNr
		{
			get
			{
				if (String.IsNullOrEmpty(KtoNr))
					return Brushes.Red;
				else
					return Brushes.Green;
			}
		}

		public Brush BorderBrushBLZ
		{
			get
			{
				if (String.IsNullOrEmpty(BLZ))
					return Brushes.Red;
				else
					return Brushes.Green;
			}
		}

		public Dictionary<string, Countries> Countries { get; }

		// INotifyPropertyChanged Implementierung
		public event PropertyChangedEventHandler PropertyChanged;

		public IbanVM()
		{
			// Das Dictionary mit den Werten aus dem Countries Enum füllen
			Countries = new Dictionary<string, Countries>();
			foreach (var value in Enum.GetValues(typeof(Countries)))
				Countries.Add(((Countries)value).GetDescription(), (Countries)value);
		}
	}
}