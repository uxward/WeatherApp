﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Logic.Models;
using WeatherApp.Logic.Services;
using System.Net.Http;

namespace WeatherApp.Logic.ViewModels
{
    public class ViewModelLocator
    {
        public static ViewModelLocator Instance { get; private set; }

        public static void Initialize(string mashapeKey)
        {
            if (Instance == null)
                Instance = new ViewModelLocator(mashapeKey);
        }

        private readonly Document _document;
        private readonly WeatherServiceAgent _weatherServiceAgent;
        private readonly CitySelection _citySelection;

        private ViewModelLocator(string mashapeKey)
        {
            _document = new Document();
            _citySelection = new CitySelection();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-Mashape-Key", mashapeKey);
            httpClient.BaseAddress = new Uri("https://george-vustrey-weather.p.mashape.com/", UriKind.Absolute);
            _weatherServiceAgent = new WeatherServiceAgent(_document, httpClient);

            // For now, initialize the document to one city.
            var city = _document.NewCity();
            city.Name = "Dallas";
        }

        public MainViewModel Main
        {
            get
            {
                return new MainViewModel(_document, _citySelection);
            }
        }

        public CityViewModel City
        {
            get
            {
                if (_citySelection.SelectedCity == null)
                    return null;

                return new CityViewModel(_citySelection.SelectedCity, _weatherServiceAgent);
            }
        }
    }
}
