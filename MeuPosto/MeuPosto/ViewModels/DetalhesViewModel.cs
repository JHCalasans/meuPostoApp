using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MeuPosto.Models;
using MeuPosto.Renderers;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Prism.Navigation;
using Xamarin.Forms;

namespace MeuPosto.ViewModels
{
    public class DetalhesViewModel : BindableBase, INavigationAware
    {

        private Distribuidora _distribuidoraVar;

        public Distribuidora DistribuidoraVar
        {
            get { return _distribuidoraVar; }
            set { SetProperty(ref _distribuidoraVar, value); }
        }

        private String _uriString;

        public String UriString
        {
            get { return _uriString; }
            set { SetProperty(ref _uriString, value); }
        }

        public DetalhesViewModel()
        {
             Task.Run(async () => await BuscarEstabelecimentosInicial());
        }

         private async Task BuscarEstabelecimentosInicial()
        {
          

            try
            {

                UserDialogs.Instance.ShowLoading("Carregando", MaskType.Gradient);



                var client = new HttpClient();
                // client.BaseAddress = new Uri(Settings.Servidor);
                client.Timeout = TimeSpan.FromMilliseconds(10000);

                var response = new HttpResponseMessage();

                    response = await client.GetAsync("http://192.168.0.15:8080/meuPosto/ws/posto/testePdf");



                if (response.IsSuccessStatusCode)
                {
                    var respStr = await response.Content.ReadAsStringAsync();
                    DistribuidoraVar = JsonConvert.DeserializeObject<Distribuidora>(respStr);
                    //string base64BinaryStr = await vm.DownloadAttFile();
                    //byte[] bytes = Convert.FromBase64String(base64BinaryStr);

                    UriString = await DependencyService.Get<ISaveFile>().SaveFiles("TesteNovo.pdf", DistribuidoraVar.logo);
                    
                    
                   // return filepath;



                }
                else
                    throw new Exception();


            }
            catch (Exception ex)
            {
               
            }
            UserDialogs.Instance.HideLoading();
          
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }
    }
}
