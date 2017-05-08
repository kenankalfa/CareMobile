using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CareMobileApp.MasterPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPageMaster : ContentPage
    {
        public ListView ListView { get { return listView; } }
        public MasterDetailPageMaster()
        {
            InitializeComponent();

            var masterPageItem = new List<MasterPageItem>();

            masterPageItem.Add(new MasterPageItem
            {
                Title = "Job Application",
                IconSource = "mi_jobapplication.png",
                TargetType = typeof(Views.JobApplicationStep1Page)
            });
            masterPageItem.Add(new MasterPageItem
            {
                Title = "Manager View",
                IconSource = "mi_managerview.png",
                TargetType = typeof(Views.ManagerPage)
            });

            listView.ItemsSource = masterPageItem;
        }
    }
}
