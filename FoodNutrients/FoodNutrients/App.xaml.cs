using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace FoodNutrients
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            MainPage = new MainPage();

            
        }

        

        protected override void OnStart ()
		{
            AppCenter.Start("android=97e2e47f-6524-403c-acde-276b21bbf256;",
                   typeof(Analytics), typeof(Crashes));
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
