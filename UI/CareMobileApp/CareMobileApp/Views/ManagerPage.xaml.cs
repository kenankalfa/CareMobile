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
using System.Threading;
using System.Runtime.CompilerServices;

namespace CareMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManagerPage : ContentPage
    {
        public ManagerPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            await UITaskFactory.StartNew(GetManagerViewList);
            base.OnAppearing();
        }
        

        private async void GetManagerViewList()
        {
            var result = await HttpServices.JobApplicationService.GetApproveList();
            
            Device.BeginInvokeOnMainThread(() =>
            {
                SeparatorListView.ItemsSource = new ObservableCollection<JobApplication>(result.OrderByDescending(q => q.CreateDate));
            });
        }
    }
}
