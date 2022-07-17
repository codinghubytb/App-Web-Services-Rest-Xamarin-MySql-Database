using AppApi.ViewModels;
using AppApi.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AppApi
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ClassroomDetailPage), typeof(ClassroomDetailPage));
            Routing.RegisterRoute(nameof(ClassroomEditPage), typeof(ClassroomEditPage));
            Routing.RegisterRoute(nameof(StudentDetailPage), typeof(StudentDetailPage));
            Routing.RegisterRoute(nameof(StudentEditPage), typeof(StudentEditPage));
            Routing.RegisterRoute(nameof(ClassroomCreatePage), typeof(ClassroomCreatePage));
            Routing.RegisterRoute(nameof(StudentCreatePage), typeof(StudentCreatePage));
        }

    }
}
