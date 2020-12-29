using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;
using WeatherApp.ViewModel.Commands;
using WeatherApp.ViewModel.Helpers;

namespace WeatherApp.ViewModel
{
    public class WeatherVM : INotifyPropertyChanged
    {
        private string query;

        public string Query
        {
            get { return query; }
            set { 
                query = value;
                NotifyPropertyChanged(propertyName: nameof(Query));
            }
        }

        public SearchCommand SearchCommand { get; set; }

        public ObservableCollection<City> Cities { get; set; }

        private CurrentCondition currentCondition;

        public CurrentCondition CurrentCondition
        {
            get { return currentCondition; }
            set { 
                currentCondition = value;
                NotifyPropertyChanged(nameof(CurrentCondition));
            }
        }

        private City selectedCity;

        public City SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                NotifyPropertyChanged(nameof(SelectedCity));

              //  Cities.Clear();
                GetCurrentCondition();
            }
        }

        private async void GetCurrentCondition()
        {
            Cities.Clear();
            CurrentCondition = await AccuWeatherHelper.GetCurrentConditions(this.SelectedCity.Key);
            
        }

        public async void MakeQuery()
        {
            var cities = await AccuWeatherHelper.GetCities(this.Query);
            Cities.Clear();

            foreach (var city in cities)
                Cities.Add(city);

             

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public WeatherVM()
        {
            SearchCommand = new SearchCommand(this);
            Cities = new ObservableCollection<City>();
        }
    }
}
