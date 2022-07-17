using AppApi.Services;
using AppApi.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppApi
{
    public partial class App : Application
    {
        public static API GetAPI { get; set; }
        public App()
        {
            InitializeComponent();
            GetAPI = new API();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
