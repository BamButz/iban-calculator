using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using IBANCalculator.WPF.Helper;
using IBANCalculator.WPF.ViewModels;

namespace IBANCalculator.WPF
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private IbanVM VM;

		public MainWindow()
		{
			InitializeComponent();

			// Wir benutzen das ViewModel zum Datenaustausch zwischen UI und Logik
			VM = new IbanVM();

			// Normalerweise gehört das Commandrouting auch in die VM
			// aber aus Gründen der Einfachheit binden wir hier direkt den Button
			btnCalculate.Click += (sender, e) => VM.IBAN = IbanHelper.GenerateIban(VM.Country, VM.KtoNr, VM.BLZ);

			// DataContext für Bindings setzen
			DataContext = VM;
		}

		private void PreviewTextInput_AlphaNumeric(object sender, TextCompositionEventArgs e)
		{
			// Wenn Regex nicht zutrifft dann lassen wir die Eingabe nicht zu
			// Zugelassen sind nur (große) Alphanumerische Werte wenn Schweiz ansonsten nur Zahlen

			if (VM.Country == Definitions.Countries.CH)
				e.Handled = !(new Regex("[A-Z0-9]").IsMatch(e.Text));
			else
				e.Handled = !(new Regex("[0-9]").IsMatch(e.Text));
		}
	}
}