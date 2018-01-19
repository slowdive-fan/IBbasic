using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IBbasic
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NumericDialog : ContentPage
	{
		public NumericDialog ()
		{
			InitializeComponent ();
		}
	}
}