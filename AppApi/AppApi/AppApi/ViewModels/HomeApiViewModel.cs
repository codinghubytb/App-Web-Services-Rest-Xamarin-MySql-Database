using AppApi.Models;
using AppApi.Services;
using AppApi.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace AppApi.ViewModels
{
    public class HomeApiViewModel : BaseViewModel
    {
        #region Variable

        public ObservableCollection<Classroom> DataClassroom { get; }

        public Command LoadDataCommand { get; }
        public Command<Classroom> ClassroomTapped { get; set; }
        public Command CreateClassCommand { get; set; }
        public Command SeeMoreCommand 
        {
            get
            {
                return new Command(OnSeeMore);
            }
        }

        private bool _seeMoreVisible = false;

        private int _page = 1;
        private double _nbPage;

        private int _nbClassList;

        public bool SeeMoreVisible
        {
            get => _seeMoreVisible;
            set => SetProperty(ref _seeMoreVisible, value);
        }

        public int NbClassList
        {
            get => _nbClassList;
            set => SetProperty(ref _nbClassList, value);
        }

        public int Page
        {
            get => _page;
            set => SetProperty(ref _page, value);
        }
        public double NbPage
        {
            get => _nbPage;
            set => SetProperty(ref _nbPage, value);
        }

        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public HomeApiViewModel()
        {
            DataClassroom = new ObservableCollection<Classroom>();
            LoadDataCommand = new Command(() => ExecuteLoadDataCommand(true));
            CreateClassCommand = new Command(OnCreate);
            ClassroomTapped = new Command<Classroom>(OnClassroomSelected);
        }


        #region Function


        /// <summary>
        /// Execute load data
        /// </summary>
        /// <param name="isBusy">bool On Appearing</param>
        async void ExecuteLoadDataCommand(bool isBusy)
        {
            IsBusy = isBusy;
            try
            {
                if(isBusy)
                {
                    DataClassroom.Clear();
                    Page = 1;
                }
                List<Classroom> content = await App.GetAPI.GetClassroomAsync(_page, "classroom");
                foreach (var d in content)
                {
                    DataClassroom.Add(d);
                }

                NbPage = App.GetAPI.GetSchool.nb_pages;
                NbClassList = DataClassroom.Count;

                if (isBusy)
                    SeeMoreVisible = true;
                if (_page == _nbPage || NbPage == 0)
                {
                    SeeMoreVisible = false;
                    NbPage = _page;
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error celui la", e.Message, "Cancel");
            }
            if(isBusy)
                IsBusy = !isBusy;
        }


        /// <summary>
        /// Selected class Classroom
        /// </summary>
        /// <param name="classroom"></param>
        private async void OnClassroomSelected(Classroom classroom)
        {
            if (classroom == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(ClassroomDetailPage)}?{nameof(ClassroomDetailViewModel.ClassroomId)}={classroom.ClassroomId}");
        }


        /// <summary>
        /// Command Create Classroom Page
        /// </summary>
        private async void OnCreate()
        {
            await Shell.Current.GoToAsync(nameof(ClassroomCreatePage));
        }


        /// <summary>
        /// Button Command See More Data
        /// </summary>
        private void OnSeeMore()
        {
            if(App.GetAPI.GetSchool != null)
            {   
                if(_page < App.GetAPI.GetSchool.nb_pages)
                {
                    Page++;
                    SeeMoreVisible = true;
                    ExecuteLoadDataCommand(false);
                }
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            Page = 1;
        }


        #endregion
    }
}
