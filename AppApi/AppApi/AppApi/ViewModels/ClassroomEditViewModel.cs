using AppApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace AppApi.ViewModels
{
    [QueryProperty(nameof(ClassroomId), nameof(ClassroomId))]
    public class ClassroomEditViewModel : BaseViewModel
    {

        #region Variable

        private Classroom GetClassroom;

        public Command CancelCommand { get; }
        public Command SaveCommand { get; }

        private int _classroomId;
        private string _classroomName;
        private DateTime _classroomModified;
        private int _classroomNbPerson;

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

        public string ClassroomName
        {
            get => _classroomName;
            set => SetProperty(ref _classroomName, value);
        }
        public DateTime ClassroomModified
        {
            get => _classroomModified;
            set => SetProperty(ref _classroomModified, value);
        }
        public int ClassroomNbPerson
        {
            get => _classroomNbPerson;
            set
            {
                _classroomNbPerson = value;
            }
        }


        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public ClassroomEditViewModel()
        {
            CancelCommand = new Command(OnCancel);
            SaveCommand = new Command(OnSave, ValidateSave);
            this.PropertyChanged +=
                 (_, __) => SaveCommand.ChangeCanExecute();
        }


        #region Function


        /// <summary>
        /// Get Data classroom Table with classroomId
        /// </summary>
        /// <param name="classroomId"></param>
        private void LoadItemId(int classroomId)
        {
            try
            {
                GetClassroom = App.GetAPI.GetIdAsync(classroomId);
                ClassroomName = GetClassroom.ClassroomName;
                ClassroomModified = DateTime.Now;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }


        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("../..");
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(ClassroomName);
        }
        private async void OnSave()
        {
            GetClassroom.ClassroomName = _classroomName;
            GetClassroom.ClassroomModified = _classroomModified;
            await App.GetAPI.PutAsync(GetClassroom);
            await Shell.Current.GoToAsync("../..");
        }

        #endregion
    }
}
