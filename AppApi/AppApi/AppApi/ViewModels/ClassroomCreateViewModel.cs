using AppApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppApi.ViewModels
{
    public class ClassroomCreateViewModel : BaseViewModel
    {

        #region Variable

        public Command CancelCommand { get; }
        public Command SaveCommand { get; }

        private string _classroomName;
        private DateTime _classroomCreated = DateTime.Now;

        public DateTime ClassroomCreated
        {
            get => _classroomCreated;
        }

        public string ClassroomName
        {
            get => _classroomName;
            set => SetProperty(ref _classroomName, value);
        }


        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public ClassroomCreateViewModel()
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
            return !String.IsNullOrWhiteSpace(ClassroomName);
        }

        private async void OnSave()
        {
            await App.GetAPI.PostAsync(new Classroom
            {
                ClassroomName = _classroomName,
                ClassroomCreated = _classroomCreated,
                ClassroomModified = _classroomCreated,
                ClassroomNbPerson = 0
            });
            await Shell.Current.GoToAsync("..");
        }

        #endregion

    }
}
