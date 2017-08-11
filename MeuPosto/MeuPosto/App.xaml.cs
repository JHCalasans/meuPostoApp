using Acr.UserDialogs;
using Prism.Unity;
using MeuPosto.Views;
using Xamarin.Forms;
using Microsoft.Practices.Unity;

namespace MeuPosto
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            Container.RegisterInstance<IUserDialogs>(UserDialogs.Instance);
            
            InitializeComponent();

            NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<Detalhes>();
        }

        public App()
        {
        
            InitializeComponent();
        }
    }
}
