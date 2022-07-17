using AppApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppApi.ViewModels
{
    [QueryProperty(nameof(ClassroomId), nameof(ClassroomId))]
    public class StudentCreateViewModel : BaseViewModel
    {

        #region Variable

        public Command CancelCommand { get; }
        public Command SaveCommand { get; }

        private int _classroomId;

        private string _studentName;
        private int _studentAge;
        private string _classroomName;
        private DateTime _studentCreated = DateTime.Now;
        private DateTime _studentModified = DateTime.Now;
        private bool _studentChatty;

        public int ClassroomId
        {
            get
            {
                return _classroomId;
            }
            set
            {
                _classroomId = value;
            }
        }

        public string StudentName
        {
            get => _studentName;
            set => SetProperty(ref _studentName, value);
        }
        public DateTime StudentCreated
        {
            get => _studentCreated;
            set => SetProperty(ref _studentCreated, value);
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
        public StudentCreateViewModel()
        {
            CancelCommand = new Command(OnCancel);
            SaveCommand = new Command(OnSave, ValidateSave);
            this.PropertyChanged +=
                 (_, __) => SaveCommand.ChangeCanExecute();
        }


        #region Function


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
            await App.GetAPI.PostAsync(new Student
            {
                StudentName = _studentName,
                StudentAge = _studentAge,
                StudentCreated = _studentCreated,
                StudentModified = _studentModified,
                StudentChatty = _studentChatty,
                ClassroomId = _classroomId
            });
            Classroom classroom = App.GetAPI.GetIdAsync(_classroomId);
            await App.GetAPI.PutAsync(classroom, false);
            await Shell.Current.GoToAsync("..");
        }


        #endregion
    }
}
