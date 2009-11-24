using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SL30PropertyGrid
{
	public partial class MainPage : UserControl
	{
		Person person;

		public MainPage()
		{
			InitializeComponent();
			this.Loaded += new RoutedEventHandler(Page_Loaded);
		}

		void Page_Loaded(object sender, RoutedEventArgs e)
		{

			ChildWindow1 cw = new ChildWindow1();
			cw.Show();

			person = new Person
			{
				String = "John Lennon",
				Datetime = new DateTime(1940, 10, 9),
				Int = -42,
				Short = 21,
				Long = -2878987,
				Uint = 324,
				Ushort = 21,
				Ulong = 21422,
				Car = new Car()
			};
			this.propertyGrid.SelectedObject = person;
		}

		private void test_Click(object sender, RoutedEventArgs e)
		{
			if (this.propertyGrid.SelectedObject == this.person)
			{
				this.propertyGrid.SelectedObject = this.test;
			}
			else
			{
				this.propertyGrid.SelectedObject = this.person;
			}
		}
	}
}
