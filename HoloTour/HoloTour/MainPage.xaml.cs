﻿using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace HoloTour
{
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();

            var mapTypes = Enum.GetValues(typeof(MapType)).OfType<MapType>().ToList();
            foreach (var mType in mapTypes)
            {
                this.pckMapType.Items.Add(mType.ToString());

            }
            this.pckMapType.SelectedIndex = this.pckMapType.Items.IndexOf(this.MapView.MapType.ToString());
            
            this.DelayedZoomIn();


           
        }
        
        private async void BtnGoLocation_Clicked(object sender, EventArgs e)
        {
            var mapPos = await GetCurrentPosition();
            if (mapPos.HasValue)
            {
                var region = MapSpan.FromCenterAndRadius(mapPos.Value, Distance.FromMiles(0.3));
                this.MapView.MoveToRegion(region);
            }
            
        }

        private static async Task<Position?> GetCurrentPosition()
        {
            Position? mapPos;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

                Debug.WriteLine("Position Status: {0}", position.Timestamp);
                Debug.WriteLine("Position Latitude: {0}", position.Latitude);
                Debug.WriteLine("Position Longitude: {0}", position.Longitude);

                mapPos = new Position(position.Latitude, position.Longitude);


            }
            catch (Exception ex)
            {
                mapPos = null;
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
            }
            return mapPos;
        }

        private void PckMapType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mapTypeStr = this.pckMapType.Items[this.pckMapType.SelectedIndex];
            MapType mType;
            if (Enum.TryParse(mapTypeStr, out mType))
            {
                this.MapView.MapType = mType;
            }
        }
        

        private async void DelayedZoomIn()
        {
            
            await Task.Run(async () =>
            {
                await Task.Delay(5000);
                
                
            }).ConfigureAwait(true);

            var position = await GetCurrentPosition();
            if (position.HasValue)
            {
                var region = MapSpan.FromCenterAndRadius(position.Value, Distance.FromMiles(0.3));
                //var region = MapSpan.FromCenterAndRadius(new Position(37, -122), Distance.FromMiles(0.3));
                this.MapView.MoveToRegion(region);
            }
        }
    }
}
