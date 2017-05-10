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
            try
            {
                var currentInstance = JobApplicationPagesDataManager.Instance;

                var postPhotoStream = new MemoryStream(_profilePhotoStreamOnDisappearing.ToArray());
                postPhotoStream.Position = 0;

                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("http://caremobileaphostv2.azurewebsites.net/");

                var instance = new JobApplication();

                instance.Applicant = new Applicant();
                instance.Applicant.BirthDate = currentInstance.BirthDate;
                instance.Applicant.EmailAddress = currentInstance.EmailAddress;
                instance.Applicant.FullName = currentInstance.FullName;
                instance.Position = new Position();
                instance.Position.PositionName = currentInstance.SelectedPosition;

                var request = new HttpRequestMessage();
                var requestContent = new MultipartFormDataContent();
                var streamContent = new StreamContent(postPhotoStream);
                var stringContent = new StringContent(JsonConvert.SerializeObject(instance), Encoding.UTF8, "application/json");

                request.Method = HttpMethod.Post;

                requestContent.Add(streamContent, "PhotoStreamInstance");
                requestContent.Add(stringContent, "EntityInstance");

                request.Content = requestContent;

                var response = await httpClient.PostAsync("api/JobApplication/Post", requestContent);
                
                await DisplayAlert("Process Result", "Success", "Cancel");
            }
            catch (Exception exception)
            {
                await DisplayAlert("Save Application Error", exception.InnerException.Message, "Cancel");
            }
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
