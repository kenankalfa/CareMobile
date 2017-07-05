using CareMobileApp.Utils;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace CareMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManagerPage : ContentPage
    {
        private ObservableCollection<SamplePersonType> _persons;
        public ObservableCollection<SamplePersonType> Persons
        {
            get { return _persons; }
            set
            {
                _persons = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<JobApplication> _jobApplications;
        public ObservableCollection<JobApplication> JobApplications
        {
            get { return _jobApplications; }
            set
            {
                _jobApplications = value;
                OnPropertyChanged();
            }
        }


        public ManagerPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            BindingContext = this;
            base.OnAppearing();
            var result = await HttpServices.JobApplicationService.GetApproveList();
            JobApplications = new ObservableCollection<JobApplication>(result);
        }

        protected override void OnDisappearing()
        {
            BindingContext = null;
            base.OnDisappearing();
        }
    }
}
