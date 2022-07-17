using AppApi.Models;
using AppApi.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace AppApi.ViewModels
{
    [QueryProperty(nameof(ClassroomId), nameof(ClassroomId))]
    public class ClassroomDetailViewModel : BaseViewModel
    {

        #region Variable

        private Classroom GetClassroom;

        public ObservableCollection<Student> DataStudent { get; }

        public Command LoadDataCommand { get; }
        public Command<Student> StudentTapped { get; set; }
        public Command OkCommand { get; }
        public Command SetterCommand { get; }
        public Command DeleteCommand { get; }
        public Command CreateStudentCommand { get; }

        private int _classroomId;

        private string _classroomTitle;

        private int _page = 1;

        private double _nbPage;

        private bool _seeMoreVisible= true;

        private int _nbPersonList;

        public int ClassroomId
        {
            get
            {
                return _classroomId;
            }
            set
            {
                _classroomId = value;
                LoadItemId(value);
            }
        }

        public Command SeeMoreCommand
        {
            get
            {
                return new Command(OnSeeMore);
            }
        }

        public string ClassroomTitle
        {
            get => _classroomTitle;
            set =>  SetProperty(ref _classroomTitle, value);
        }

        public bool SeeMoreVisible
        {
            get => _seeMoreVisible;
            set => SetProperty(ref _seeMoreVisible, value);
        }

        public int NbPersonList
        {
            get => _nbPersonList;
            set => SetProperty(ref _nbPersonList, value);
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
        public ClassroomDetailViewModel()
        {
            DataStudent = new ObservableCollection<Student>();
            LoadDataCommand = new Command(() => ExecuteLoadDataCommand(true));
            StudentTapped = new Command<Student>(OnStudentSelected);
            OkCommand = new Command(OnOk);
            SetterCommand = new Command(OnSetter);
            DeleteCommand = new Command(OnDelete);
            CreateStudentCommand = new Command(OnCreate);
        }

        #region Variable


        /// <summary>
        /// Get data table classroom
        /// </summary>
        /// <param name="classroomId">ClassroomId</param>
        public void LoadItemId(int classroomId)
        {
            try
            {
                App.GetAPI.PaginationStudentAsync(_classroomId);
                GetClassroom = App.GetAPI.GetIdAsync(classroomId);
                ClassroomTitle = GetClassroom.ClassroomName;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Classroom");
            }
        }


        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="isBusy"></param>
        async void ExecuteLoadDataCommand(bool isBusy)
        {
            IsBusy = isBusy;
            try
            {
                if (isBusy)
                {
                    DataStudent.Clear();
                    Page = 1;
                }
                List<Student> content = await App.GetAPI.GetStudentAsync(_page, $"classroom={_classroomId}/student=all");
                foreach (var d in content)
                {
                    DataStudent.Add(d);
                }

                NbPage = App.GetAPI.studentPage.nb_pages;
                NbPersonList = DataStudent.Count;

                if(isBusy)
                    SeeMoreVisible = true;
                if(_page == _nbPage || NbPage == 0)
                {
                    SeeMoreVisible = false;
                    NbPage = _page;
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error celui la", e.Message, "Cancel");
            }
            if (isBusy)
                IsBusy = !isBusy;
        }


        /// <summary>
        /// Selected student
        /// </summary>
        /// <param name="student">Student</param>
        async void OnStudentSelected(Student student)
        {
            if (student == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(StudentDetailPage)}?{nameof(StudentDetailViewModel.Ids)}={$"{student.ClassroomId}/{student.StudentId}"}");
        }

        private async void OnOk()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSetter()
        {
            await Shell.Current.GoToAsync($"{nameof(ClassroomEditPage)}?{nameof(ClassroomEditViewModel.ClassroomId)}={GetClassroom.ClassroomId}");
        }

        private async void OnDelete()
        {
            await App.GetAPI.DeleteAsync(GetClassroom);
            await Shell.Current.GoToAsync("..");
        }

        public async void OnCreate()
        {
            await Shell.Current.GoToAsync($"{nameof(StudentCreatePage)}?{nameof(StudentCreateViewModel.ClassroomId)}={_classroomId}");
        }

        private void OnSeeMore()
        {
            Debug.WriteLine(App.GetAPI.studentPage.nb_pages);
            if (App.GetAPI.studentPage != null)
            {
                if (_page < App.GetAPI.studentPage.nb_pages)
                {
                    Page++;
                    SeeMoreVisible = true;
                    ExecuteLoadDataCommand(false);
                }
                else
                    SeeMoreVisible = false;
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
