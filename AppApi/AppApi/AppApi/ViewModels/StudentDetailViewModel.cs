using AppApi.Models;
using AppApi.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace AppApi.ViewModels
{
    [QueryProperty(nameof(Ids), nameof(Ids))]
    public class StudentDetailViewModel : BaseViewModel
    {

        #region Variable

        public Command OkCommand { get; }
        public Command SetterCommand { get; }
        public Command DeleteCommand { get; }

        private Student GetStudent { get; set; }

        private string _Ids;
        private string _studentName;
        private int _studentAge;
        private string _classroomName;
        private DateTime _studentModified;
        private bool _studentChatty;

        public string Ids
        {
            get
            {
                return _Ids;
            }
            set
            {
                _Ids = value;
                LoadStudentId(value);
            }
        }

        public string StudentName
        {
            get => _studentName;
            set => SetProperty(ref _studentName, value);
        }
        public DateTime StudentModified
        {
            get => _studentModified;
            set => SetProperty(ref _studentModified, value);
        }
        public int StudentAge
        {
            get => _studentAge;
            set => SetProperty(ref _studentAge, value);
        }
        public string StudentClassroom
        {
            get => _classroomName;
            set => SetProperty(ref _classroomName, value);
        }
        public bool StudentChatty
        {
            get => _studentChatty;
            set => SetProperty(ref _studentChatty, value);
        }


        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public StudentDetailViewModel()
        {
            OkCommand = new Command(OnOk);
            SetterCommand = new Command(OnSetter);
            DeleteCommand = new Command(OnDelete);
        }


        #region Function

        public async void LoadStudentId(string ids)
        {
            try
            {
                var result = ids.Split('/');
                GetStudent = await App.GetAPI.GetIdAsync(int.Parse(result[0]) , int.Parse(result[1]));
                StudentName = GetStudent.StudentName;
                StudentAge = GetStudent.StudentAge;
                StudentModified = GetStudent.StudentModified;
                StudentChatty = GetStudent.StudentChatty;
                StudentClassroom = App.GetAPI.GetIdAsync(GetStudent.ClassroomId).ClassroomName;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private async void OnOk()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSetter()
        {
            await Shell.Current.GoToAsync($"{nameof(StudentEditPage)}?{nameof(StudentEditViewModel.Ids)}={$"{GetStudent.ClassroomId}/{GetStudent.StudentId}"}");
        }

        private async void OnDelete()
        {
            await App.GetAPI.DeleteAsync(GetStudent);
            Classroom classroom = App.GetAPI.GetIdAsync(GetStudent.ClassroomId);
            await App.GetAPI.PutAsync(classroom, false);
            await Shell.Current.GoToAsync("..");
        }


        #endregion
    }
}
