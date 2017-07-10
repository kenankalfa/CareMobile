using CareMobileApp.Utils;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;
using System;

namespace CareMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManagerPage : ContentPage
    {
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
            var result = await HttpServices.JobApplicationService.GetApproveList();
            JobApplications = new ObservableCollection<JobApplication>(result);
            SeparatorListView.ItemsSource = JobApplications;

            BindingContext = this;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            BindingContext = null;
            base.OnDisappearing();
        }
    }
}
