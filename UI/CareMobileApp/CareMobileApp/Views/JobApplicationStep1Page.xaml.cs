using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CareMobileApp.Utils;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;

namespace CareMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobApplicationStep1Page : ContentPage
    {
        public JobApplicationStep1Page()
        {
            InitializeComponent();

            profilePhoto.GestureRecognizers.Clear();
            var profilePhotoClickGesture = new TapGestureRecognizer();
            profilePhotoClickGesture.Tapped += ProfilePhotoClickGesture_Tapped;

            profilePhoto.GestureRecognizers.Add(profilePhotoClickGesture);
            NextStepButton.Clicked += NextStepButton_Clicked;

            JobApplicationPagesDataManager.Clear();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (profilePhoto.Source == null)
            {
                profilePhoto.Source = ProfileImage;
            }

            NextStepButton.IsVisible = IsPhotoPicked;

            if (IsPhotoPicked)
            {
                LetsTakePhotoLabel.IsVisible = IsPhotoPicked;
                LetsTakePhotoLabel.Text = "You could re-take your photo.";
            }
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

        private ImageSource _profileImage;
        private ImageSource _defaultProfileImage = ImageSource.FromFile("icon_camera");
        protected ImageSource ProfileImage
        {
            get
            {
                if (_profileImage == null)
                {
                    _profileImage = _defaultProfileImage;
                }
                return _profileImage;
            }
            set
            {
                _profileImage = value;
            }
        }
        public bool IsPhotoPicked { get; set; }
        private async void ProfilePhotoClickGesture_Tapped(object sender, EventArgs e)
        {
            NextStepButton.IsVisible = false;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var fileNameGuid = Guid.NewGuid().ToString();
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = $"{fileNameGuid}.jpg",
                DefaultCamera = CameraDevice.Front
            });

            if (file == null)
            {
                profilePhoto.Source = _defaultProfileImage;
                LetsTakePhotoLabel.IsVisible = true;
                return;
            }

            IsPhotoPicked = true;

            ProfileImage = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                var ms = new MemoryStream();

                stream.CopyTo(ms);
                stream.Position = 0;

                JobApplicationPagesDataManager.SetPhotoStream(ms);

                file.Dispose();
                return stream;
            });

            profilePhoto.Source = ProfileImage;

            NextStepButton.IsVisible = true;
            LetsTakePhotoLabel.IsVisible = false;
        }
        private async void NextStepButton_Clicked(object sender, EventArgs e)
        {
            var nextPage = new JobApplicationStep2Page();
            await Navigation.PushAsync(nextPage, true);
        }
    }
}
