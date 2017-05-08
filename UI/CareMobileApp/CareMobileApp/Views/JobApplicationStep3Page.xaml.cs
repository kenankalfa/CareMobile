using CareMobileApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CareMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobApplicationStep3Page : ContentPage
    {
        private MemoryStream _profilePhotoStreamOnDisappearing;
        public JobApplicationStep3Page()
        {
            InitializeComponent();
            FinishJobApplicationButton.Clicked += FinishJobApplicationButton_Clicked;
            PreviousStepButton.Clicked += PreviousStepButton_Clicked;
        }

        private void FinishJobApplicationButton_Clicked(object sender, EventArgs e)
        {
            // TODO : CALL AZURE
            
        }

        private async void PreviousStepButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync().ConfigureAwait(false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                BindApplicationForm();
            }
            catch (Exception ex)
            {
                DisplayAlert("step3 appear", ex.ToString(), "iptal");
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            JobApplicationPagesDataManager.SetPhotoStream(_profilePhotoStreamOnDisappearing);
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
        private void BindApplicationForm()
        {
            var currentInstance = JobApplicationPagesDataManager.Instance;

            if (!currentInstance.IsValid)
            {
                return;
            }

            if (currentInstance.PhotoStream != null)
            {
                _profilePhotoStreamOnDisappearing = new MemoryStream(currentInstance.PhotoStream.ToArray());
                _profilePhotoStreamOnDisappearing.Position = 0;

                currentInstance.PhotoStream.Position = 0;
                ProfilePhoto.Source = ImageSource.FromStream(() => { return currentInstance.PhotoStream; });
            }

            FullNameLabel.Text = currentInstance.FullName;
            EmailLabel.Text = currentInstance.EmailAddress;
            BirthDateLabel.Text = currentInstance.BirthDate.ToString("dd.MM.yyyy");
            PositionLabel.Text = currentInstance.SelectedPosition;
        }
    }
}
