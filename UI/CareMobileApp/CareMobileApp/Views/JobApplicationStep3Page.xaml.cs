using CareMobileApp.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CareMobileApp.Utils;

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

        private async void FinishJobApplicationButton_Clicked(object sender, EventArgs e)
        {
            await UITaskFactory.StartNew(MakeJobApplication);
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
        private async void MakeJobApplication()
        {
            var operationResult = await HttpServices.JobApplicationService.MakeJobApplication(_profilePhotoStreamOnDisappearing);

            Device.BeginInvokeOnMainThread(() =>
            {
                JobApplicationPagesDataManager.Clear();

                if (operationResult.IsSucceed)
                {
                    DisplayAlert("Process Result", "Success", "OK");
                    var masterDetailMainPage = new MasterPageViews.MasterMainPage();
                    Navigation.PushAsync(masterDetailMainPage);
                }
                else
                {
                    DisplayAlert("Save Application Error", operationResult.Messages.FirstOrDefault(), "OK");
                }
            });
        }
    }
}
