using GeoAppAtmClient.ViewModels;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.Utilities;
using System.Linq;
using System.Windows;
using IFeature = Mapsui.IFeature;

namespace GeoAppAtmClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (MapViewModel)DataContext;
            var atms = await viewModel.AtmRepository.GetAtmsAsync();

            if (MapControl.Map == null) return;

            MapControl.Map?.Layers.Add(OpenStreetMap.CreateTileLayer());
            MapControl.Navigator.NavigateTo(SphericalMercator.FromLonLat(new MPoint(50.2, 53.22)), 80);
            MapControl.Info += MapControl_Info;

            var atmLayer = new MemoryLayer("Atm")
            {
                IsMapInfoLayer = true,
                DataSource = new MemoryProvider<IFeature>(atms.Select(atm =>
                {
                    var (x, y) = SphericalMercator.FromLonLat(atm.Latitude, atm.Longitude);

                    var feature = new PointFeature(x, y);

                    feature["id"] = atm.Id;
                    feature["name"] = atm.Name;
                    return feature;
                })),

                Style = CreateSvgStyle("atm_icon.svg", 0.05)
            };

            MapControl.Map.Layers.Add(atmLayer);
        }

        private async void MapControl_Info(object sender, Mapsui.UI.MapInfoEventArgs e)
        {
            if (e.MapInfo?.Feature == null) return;
            var viewModel = (MapViewModel)DataContext;
            await viewModel.ShowAtmInfo((string)e.MapInfo.Feature["id"]);
        }

        private static SymbolStyle CreateSvgStyle(string embeddedResourcePath, double scale)
        {
            var bitmapId = typeof(MainWindow).LoadSvgId(embeddedResourcePath);
            return new SymbolStyle { BitmapId = bitmapId, SymbolScale = scale, SymbolOffset = new Offset(0.0, 0.5, true) };
        }
    }
}