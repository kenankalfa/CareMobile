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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            LoginButton.Clicked += LoginButton_Clicked;
        }
        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            var userName = UserNameEntry.Text;
            var password = UserPasswordEntry.Text;
            // for testing purposes
            if (userName == "a" && password == "s")
            {
                var masterDetailMainPage = new MasterPageViews.MasterMainPage();
                Navigation.PushAsync(masterDetailMainPage);
            }
        }
    }
}
