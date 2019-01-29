using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;
namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        RestService _restService;
        string uri = "https://p48-calendars.icloud.com/published/2/G_HUbAYckYLwD04btDaS_e1RMbDBCc8CTxGF4LVCIowXvW4mp8BxmnNoccTIxgr7C7HUxrCb2yFKMSNT_p3BKlGiRShOE0XjEK6uUkQp0AM";

        public MainPage()
        {
            InitializeComponent();
            _restService = new RestService();
            BackgroundColor = Color.LightCyan;
            Button1.Clicked += ButtonClickEvent;

        }
        private void ButtonClickEvent(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new NavigationPage(new MainPage()));
            Navigation.PopModalAsync();
        }
        protected override async void OnAppearing()
        {
            string[,] alldate = new string[32, 4]
             {
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
                {"","","",""},
             };           //６日のデータ格納する型（０行目は並び替え用）
            var result = await VEvent.GetStringAsync(uri);
            int x = 1;                                                  //ループカウント
            foreach (VEvent evset in result)
            {               //６日のデータを格納
                WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestUri(Constants.OpenWeatherMapEndpoint, evset.CODE));
                BindingContext = weatherData;
                alldate[x, 0] = evset.DTSTART;
                alldate[x, 1] = evset.SUMMARY;
                alldate[x, 2] = evset.ADDRESS;
                alldate[x, 3] = weatherData.Weather[0].Description;
                x++;
            }
            for (int i = 1; i <= 31; i++)     //日付の昇順に並び替え
            {
                if (alldate[i, 0] == "")
                {
                    break;
                }
                for (int j = i + 1; j <= 31; j++)
                {
                    if (alldate[j, 0] == "")
                    {
                        break;
                    }
                    if (int.Parse(alldate[i, 0]) > int.Parse(alldate[j, 0]))
                    {
                        alldate[0, 0] = alldate[i, 0];
                        alldate[0, 1] = alldate[i, 1];
                        alldate[0, 2] = alldate[i, 2];
                        alldate[0, 3] = alldate[i, 3];
                        alldate[i, 0] = alldate[j, 0];
                        alldate[i, 1] = alldate[j, 1];
                        alldate[i, 2] = alldate[j, 2];
                        alldate[i, 3] = alldate[j, 3];
                        alldate[j, 0] = alldate[0, 0];
                        alldate[j, 1] = alldate[0, 1];
                        alldate[j, 2] = alldate[0, 2];
                        alldate[j, 3] = alldate[0, 3];
                    }
                }
            }
            int y = 1; 　　　//年月日を日付に変更
            string month;
            string day;
            while (alldate[y, 0] != "")
            {
                if (alldate[y, 0].Substring(4, 1) == "0")
                {
                    month = alldate[y, 0].Substring(5, 1) + "月";
                }
                else
                {
                    month = alldate[y, 0].Substring(4, 2) + "月";
                }
                day = alldate[y, 0].Substring(6, 2) + "日";
                alldate[y, 0] = month + day;
                y++;
            }

            int cnt = 1;                                               //ループカウント
            foreach (VEvent ev in result)               //ラベルに表示
            {
                if (cnt == 1)
                {
                    day1.Text = alldate[cnt, 0];
                    today.Text = alldate[cnt, 0];
                    summary1.Text = alldate[cnt, 1];
                    summarytoday.Text = alldate[cnt, 1];
                    venue1.Text = alldate[cnt, 2];
                    venuetoday.Text = alldate[cnt, 2];
                    weather1.Text = alldate[cnt, 3];
                    weathertoday.Text = alldate[cnt, 3];
                }
                if (cnt == 2)
                {
                    day2.Text = alldate[cnt, 0];
                    summary2.Text = alldate[cnt, 1];
                    venue2.Text = alldate[cnt, 2];
                    weather2.Text = alldate[cnt, 3];
                }
                if (cnt == 3)
                {
                    day3.Text = alldate[cnt, 0];
                    summary3.Text = alldate[cnt, 1];
                    venue3.Text = alldate[cnt, 2];
                    weather3.Text = alldate[cnt, 3];
                }
                if (cnt == 4)
                {
                    day4.Text = alldate[cnt, 0];
                    summary4.Text = alldate[cnt, 1];
                    venue4.Text = alldate[cnt, 2];
                    weather4.Text = alldate[cnt, 3];
                }
                if (cnt == 5)
                {
                    day5.Text = alldate[cnt, 0];
                    summary5.Text = alldate[cnt, 1];
                    venue5.Text = alldate[cnt, 2];
                    weather5.Text = alldate[cnt, 3];
                }
                if (cnt == 6)
                {
                    day6.Text = alldate[cnt, 0];
                    summary6.Text = alldate[cnt, 1];
                    venue6.Text = alldate[cnt, 2];
                    weather6.Text = alldate[cnt, 3];
                }
                cnt++;
            }
        }


        //async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        //{
        //         WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestUri(Constants.OpenWeatherMapEndpoint,));
        //         BindingContext = weatherData;
        // }
        string GenerateRequestUri(string endpoint, string code)
        {
            string requestUri = endpoint;
            string Postalcode = code;
            requestUri += $"?q=" + Postalcode;
            requestUri += "&units=metric&lang=ja"; // or units=metric
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;

        }
    }
}





