using AppApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppApi.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassroomDetailPage : ContentPage
    {
        ClassroomDetailViewModel _classroomDetailViewModel;
        public ClassroomDetailPage()
        {
            InitializeComponent();
            BindingContext = _classroomDetailViewModel = new ClassroomDetailViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _classroomDetailViewModel.OnAppearing();
        }
    }
}