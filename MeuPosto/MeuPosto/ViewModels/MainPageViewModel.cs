using MeuPosto.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MeuPosto.Models;
using Newtonsoft.Json;
using Plugin.ExternalMaps;
using Plugin.ExternalMaps.Abstractions;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Services;
using Xamarin.Forms;

namespace MeuPosto.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {

        private readonly INavigationService _navigationService;

        private readonly IPageDialogService _dialogService;

        private readonly String _servidorLocal = "http://192.168.0.15:8080/meuposto";

        private readonly String _servidorProducao= "http://104.131.104.191:8080";

        private ObservableCollection<Posto> _listaPostos;

        public ObservableCollection<Posto> ListaPostos
        {
            get { return _listaPostos; }
            set { SetProperty(ref _listaPostos, value); }
        }

        public DelegateCommand<Posto> PostoCommand => new DelegateCommand<Posto>(DetalhePosto);

        public DelegateCommand RefreshCommand => new DelegateCommand(BuscarEstabelecimentos);

        public string Cabecalho { get; set; }

        private bool _listaVazia ;

        public bool ListaVazia
        {
            get { return _listaVazia; }
            set { SetProperty(ref _listaVazia, value); }

        }

        private ObservableCollection<View> _stackLista;

        public ObservableCollection<View> StackLista
        {
            get { return _stackLista; }
            set { SetProperty(ref _stackLista, value); }

        }

        private List<string> _pesquisas = new List<string>
        {
            "Distância", //0
            "Gasolina Comum", //1
            "Gasolina Aditivada", //2
            "Etanol", //3
            "Diesel", //4
            "GNV", //5
             
        };

        public List<string> Pesquisas => _pesquisas;

        private int _indicePesquisa;

        public int IndicePesquisa
        {
            get{return _indicePesquisa; }
            set
            {
                if (value > -1)
                {
                    SetProperty(ref _indicePesquisa, value);
                      //  BuscarEstabelecimentos();

                }
               
            }

        }
        private String _textoVazio;

        public String TextoVazio
        {
            get { return _textoVazio; }
            set { SetProperty(ref _textoVazio, value); }

        }

        private bool _isRefreshing = false;

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set{ SetProperty(ref _isRefreshing, value); }
        }

        public double SizeFont => Plugin.XamJam.Screen.CrossScreen.Current.Size.Height / 15;

        public double CabecalhoSize => Plugin.XamJam.Screen.CrossScreen.Current.Size.Height / 6;

        public double MsgErroSize => Plugin.XamJam.Screen.CrossScreen.Current.Size.Height / 35;

        public double NomePostoSize => Plugin.XamJam.Screen.CrossScreen.Current.Size.Height / 40;

        public double MsgDestacadaSize => Plugin.XamJam.Screen.CrossScreen.Current.Size.Height / 42;

        public double MsgPrecosSize => Plugin.XamJam.Screen.CrossScreen.Current.Size.Height / 60;

        public double MsgOrdenarSize => Plugin.XamJam.Screen.CrossScreen.Current.Size.Height / 30;

        public double LinhaSize => Plugin.XamJam.Screen.CrossScreen.Current.Size.Height / 2;

        public double LogoAltura => Plugin.XamJam.Screen.CrossScreen.Current.Size.Height / 7;

        public double LogoLargura => Plugin.XamJam.Screen.CrossScreen.Current.Size.Width / 5;

        public double LarguraTela => Plugin.XamJam.Screen.CrossScreen.Current.Size.Width;

        public double LarguraLayoutPrecos => Plugin.XamJam.Screen.CrossScreen.Current.Size.Width / 2.5;

        public Thickness MargemMetadeDaTela => new Thickness(Plugin.XamJam.Screen.CrossScreen.Current.Size.Width / 3, 140, 0, 0);

        public double PickerSize => Plugin.XamJam.Screen.CrossScreen.Current.Size.Width * 0.65;

        public Thickness MargemImg => new Thickness(Plugin.XamJam.Screen.CrossScreen.Current.Size.Width / 10,0,0,0);

        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            Cabecalho = "<html><body><font> <u><b>Meu Posto</b></u></font></body></html>";
            _navigationService = navigationService;
            _dialogService = dialogService;
            ObterPermissao();
            //BuscarEstabelecimentos();
           

        }

        private async void ObterPermissao()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await _dialogService.DisplayAlertAsync("Localização", "Preciso obter a localização.", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                   await Task.Run(async () => await BuscarEstabelecimentosInicial());
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await _dialogService.DisplayAlertAsync("Localização Negada", "Não foi possível continuar, tente novamente.!", "OK");
                }
            }
            catch (Exception ex)
            {
               Debug.WriteLine(ex.Message);
            }

        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
           
            //if (parameters.ContainsKey("title"))
            //    Title = (string)parameters["title"] + " and Prism";
        }


        private async Task BuscarEstabelecimentosInicial()
        {
            Position position = null;
            IsRefreshing = true;

            try
            {

                UserDialogs.Instance.ShowLoading("Carregando", MaskType.Gradient);

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(10000));

                var client = new HttpClient();
                // client.BaseAddress = new Uri(Settings.Servidor);
                client.Timeout = TimeSpan.FromMilliseconds(10000);

                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";
                var response = new HttpResponseMessage();
                if (_indicePesquisa == 0)
                    response = await client.GetAsync(_servidorProducao + "/ws/posto/porDistancia?lat=" + position.Latitude.ToString(nfi) + "&long=" + position.Longitude.ToString(nfi));
                else
                    response = await client.GetAsync(_servidorProducao + "/ws/posto/porDistancia?lat=" + position.Latitude.ToString(nfi) + "&long=" + position.Longitude.ToString(nfi) + "&tpCombustivel=" + _indicePesquisa);


                if (response.IsSuccessStatusCode)
                {
                    var respStr = await response.Content.ReadAsStringAsync();
                    ListaPostos = JsonConvert.DeserializeObject<ObservableCollection<Posto>>(respStr);
                    if (ListaPostos.Count < 1)
                    {
                        ListaVazia = true;
                        TextoVazio = "Nenhum Posto Próximo.!";
                    }
                    else
                    {
                        ListaVazia = false;
                        TextoVazio = "";
                    }

                }
                else
                    throw new Exception();


            }
            catch (Exception ex)
            {
                ListaPostos = null;
                ListaVazia = true;
                if (position == null)
                    // await Application.Current.MainPage.DisplayAlert("Aviso", "Favor ativar o gps.", "Ok");
                    TextoVazio = "Favor ativar o gps.!";

                else
                    TextoVazio = "Falha ao buscar estabelecimentos.!";
                //await _dialogService.DisplayAlertAsync("Aviso", "Falha ao buscar estabelecimentos", "Ok");
            }
            UserDialogs.Instance.HideLoading();
            IsRefreshing = false;
        }

        private async void BuscarEstabelecimentos()
        {
            Position position = null;
            IsRefreshing = true;

            try
            {

                UserDialogs.Instance.ShowLoading("Carregando", MaskType.Gradient);

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(10000));

                var client = new HttpClient();
               // client.BaseAddress = new Uri(Settings.Servidor);
                client.Timeout = TimeSpan.FromMilliseconds(10000);


                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";
                var response = new HttpResponseMessage();
                if (_indicePesquisa == 0)
                    response = await client.GetAsync(_servidorProducao + "/ws/posto/porDistancia?lat=" + position.Latitude.ToString(nfi) + "&long=" + position.Longitude.ToString(nfi));
                else
                    response = await client.GetAsync(_servidorProducao + "/ws/posto/porDistancia?lat=" + position.Latitude.ToString(nfi) + "&long=" + position.Longitude.ToString(nfi) + "&tpCombustivel=" + _indicePesquisa);


                //response = await client.GetAsync(_servidorLocal + "/ws/posto/porDistancia?lat=-10.92534852&long=-37.16838608");
                if (response.IsSuccessStatusCode)
                {
                    var respStr = await response.Content.ReadAsStringAsync();
                    ListaPostos = JsonConvert.DeserializeObject<ObservableCollection<Posto>>(respStr);
                    if (ListaPostos.Count < 1)
                    {
                        ListaVazia = true;
                        TextoVazio = "Nenhum Posto Próximo.!";
                    }
                    else
                    {
                        ListaVazia = false;
                        TextoVazio = "";
                    }

                }
                else
                    throw new  Exception();
                

            }
            catch (Exception ex)
            {
                ListaPostos = null;
                ListaVazia = true;
                if (position == null)
                    // await Application.Current.MainPage.DisplayAlert("Aviso", "Favor ativar o gps.", "Ok");
                    TextoVazio = "Favor ativar o gps.!";

                else
                    TextoVazio = "Falha ao buscar estabelecimentos.!";
                //await _dialogService.DisplayAlertAsync("Aviso", "Falha ao buscar estabelecimentos", "Ok");
            }
            UserDialogs.Instance.HideLoading();
            IsRefreshing = false;
        }

        private async void DetalhePosto(Posto posto)
        {
              if( await _dialogService.DisplayAlertAsync("Mostrar no Mapa?", "Bairro: " + posto.bairro +"\nEndereço: " + posto.logradouro + "\n\nMostrar no mapa?" , "Sim", "Não"))
                  await CrossExternalMaps.Current.NavigateTo(posto.nome, posto.latitude, posto.longitude, NavigationType.Driving);

        }



    }
}
