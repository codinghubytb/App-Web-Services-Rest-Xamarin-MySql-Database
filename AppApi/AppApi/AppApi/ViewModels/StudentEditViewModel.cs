using AppApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace AppApi.ViewModels
{
    [QueryProperty(nameof(Ids), nameof(Ids))]
    public class StudentEditViewModel : BaseViewModel
    {

        #region Variable

        private Student GetStudent;

        public Command CancelCommand { get; }
        public Command SaveCommand { get; }

        private string _Ids;
        private string _studentName;
        private DateTime _studentModified;
        private int _studentAge;
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
        public bool StudentChatty
        {
            get => _studentChatty;
            set => SetProperty(ref _studentChatty, value);
        }
        public int StudentAge
        {
            get => _studentAge;
            set => SetProperty(ref _studentAge, value);
        }


        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public StudentEditViewModel()
        {
            CancelCommand = new Command(OnCancel);
            SaveCommand = new Command(OnSave, ValidateSave); 
            this.PropertyChanged +=
                 (_, __) => SaveCommand.ChangeCanExecute();
        }


        #region Function

        private async void LoadStudentId(string ids)
        {
            try
            {
                var result = ids.Split('/');
                GetStudent = await App.GetAPI.GetIdAsync(int.Parse(result[0]), int.Parse(result[1]));
                StudentName = GetStudent.StudentName;
                StudentModified = GetStudent.StudentModified;
                StudentAge = GetStudent.StudentAge;
                StudentChatty = GetStudent.StudentChatty;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }


        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(StudentName);
        }


        private async void OnSave()
        {
            GetStudent.StudentName = _studentName;
            GetStudent.StudentModified = DateTime.Now;
            GetStudent.StudentChatty = _studentChatty;
            GetStudent.StudentAge = _studentAge;
            await App.GetAPI.PutAsync(GetStudent);
            await Shell.Current.GoToAsync("..");
        }


        #endregion
    }
}
