using CareMobileApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CareMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobApplicationStep2Page : ContentPage
    {
        public JobApplicationPagesData FormData { get; set; }

        public JobApplicationStep2Page()
        {
            InitializeComponent();
            NextStepButton.Clicked += NextStepButton_Clicked;
            PreviousStepButton.Clicked += PreviousStepButton_Clicked;
        }

        private async void PreviousStepButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync().ConfigureAwait(false);
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (FormData != null)
            {
                FullNameEntry.Text = FormData.FullName;
                EmailEntry.Text = FormData.EmailAddress;
                BirthDatePicker.Date = FormData.BirthDate;
                PositionPicker.SelectedItem = FormData.SelectedPosition;
            }

            BindPositionPicker();
        }

        private async void NextStepButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var formData = new JobApplicationPagesData();

                formData.FullName = FullNameEntry.Text;
                formData.EmailAddress = EmailEntry.Text;
                formData.BirthDate = BirthDatePicker.Date;
                formData.SelectedPosition = PositionPicker.SelectedItem.ToString();

                FormData = formData;

                JobApplicationPagesDataManager.SetFormData(formData);

                var nextPage = new JobApplicationStep3Page();
                await Navigation.PushAsync(nextPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("step 2 next hata", ex.ToString(), "iptal");
            }
        }

        private async void BindPositionPicker()
        {
            var result = await HttpServices.PositionService.GetPositions();
            foreach (var position in result)
            {
                PositionPicker.Items.Add(position.PositionName);
            }
        }
    }
}
