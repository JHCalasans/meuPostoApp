using MeuPosto.ViewModels;
using Xamarin.Forms;

namespace MeuPosto.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ListaBusca_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListaBusca.SelectedItem = null;
        }

        private void DesfocarPicker(object sender, FocusEventArgs e)
        {
             (BindingContext as MainPageViewModel).RefreshCommand.Execute();
        }
    }
}
