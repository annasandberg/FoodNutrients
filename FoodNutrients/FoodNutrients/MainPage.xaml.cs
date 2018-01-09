using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodNutrients
{
	public partial class MainPage : ContentPage
	{
        private string Uri = "http://172.20.201.168:58279/api/food";
        List<Food> foods;
        ObservableCollection<FoodSearch> foodCollection;
        Label infoLabel = new Label();
        //ListView list;
       // OnPropertyChanged onchange = new OnPropertyChanged(foods);

        public MainPage()
		{
			InitializeComponent();
            foods = new List<Food>();
            foodCollection = new ObservableCollection<FoodSearch>();

            var layout = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children = {
                    new Label {
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        Text = "Sök näringsinnehåll",
                        FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
                        FontAttributes= FontAttributes.Bold,
                        TextColor = Color.OrangeRed,
                        Margin = new Thickness(10)
                    }
                }
            };

            Content = layout;

            var search = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            
            var searchEntry = new Entry
            {
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            Button searchButton = new Button
            {
                Text = "Sök",
                HorizontalOptions = LayoutOptions.End
            };
            search.Children.Add(searchEntry);
            search.Children.Add(searchButton);
            layout.Children.Add(search);
            list.ItemsSource = foodCollection;

            searchButton.Clicked += async (s, e) => {
                layout.Children.Add(list);
                layout.Children.Remove(table);
                layout.Children.Remove(infoLabel);
                list.ItemsSource = null;
                //infoLabel.Text = "";
                foods = await GetMatchingFoods(searchEntry.Text);
                if (foods.Count == 0)
                {
                    infoLabel.Text = "Din sökning gav inga träffar";
                    layout.Children.Add(infoLabel);
                }
                foodCollection = FoodSearch.Result(foods);
                list.ItemsSource = foodCollection;
                  
            };


            infoLabel.Text = "";
            infoLabel.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            infoLabel.FontAttributes = FontAttributes.Bold;
            infoLabel.TextColor = Color.OrangeRed;

            
            //om klickar på item ilistan, visa näringsinnehåll
            list.ItemSelected += async (s, e) =>
            {
                list.ItemsSource = null;
                layout.Children.Remove(list);
                searchEntry.Text = "";
                var selectedFood = (FoodSearch)e.SelectedItem;
                var food = await GetFoodById(selectedFood.Id);
                infoLabel.Text = food.Name;
                Energy.Text = food.EnergyInKcal.ToString();
                Carbohydrates.Text = food.Carbohydrates.ToString();
                Fat.Text = food.Fat.ToString();
                Protein.Text = food.Protein.ToString();
                Fiber.Text = food.Fibers.ToString();
                Sugars.Text = food.Sugars.ToString();
                SaturatedFat.Text = food.SaturatedFat.ToString();
                Salt.Text = food.Salt.ToString();
                VitD.Text = food.VitaminD.ToString();
                Iron.Text = food.Iron.ToString();
                layout.Children.Add(infoLabel);
                layout.Children.Add(table);
            };
        }

        private async Task<List<Food>> GetMatchingFoods(string searchString)
        {
            var uri = new Uri(string.Format(Uri, string.Empty));
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(1000);
                try
                {
                    HttpResponseMessage response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content;
                        var result = await content.ReadAsStringAsync();
                        foods = JsonConvert.DeserializeObject<List<Food>>(result);
                        foods = foods.Where(x => x.Name.ToLower().Contains(searchString.ToLower())).ToList();

                        return foods;
                    }
                }
                catch (Exception)
                {
                    client.CancelPendingRequests();
                    await DisplayAlert("Error", "Det gick inte att nå API:et för tillfället.", "Stäng");
                    return foods;
                }
                
            }
            return foods;   
            //using (HttpResponseMessage response = await client.GetAsync(uri))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var content = response.Content;
            //        var result = await content.ReadAsStringAsync();
            //        foods = JsonConvert.DeserializeObject<List<Food>>(result);
            //        foods = foods.Where(x => x.Name.ToLower().Contains(searchString.ToLower())).ToList();

            //        return foods;
            //    }
            //    else
            //    {
            //        await DisplayAlert("Error", "Det gick inte att nå API:et för tillfället.", "Stäng");
            //        return null;
            //    }
            //}
            
        }

        private async Task<Food> GetFoodById(int id)
        {
            var uri = new Uri(string.Format(Uri + "/" + id, string.Empty));
            Food food = null;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                food = JsonConvert.DeserializeObject<Food>(content);
            }
            
            return food;
        }
    }

    public class FoodSearch : ObservableCollection<Food>
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public static ObservableCollection<FoodSearch> Result(List<Food> foods)
        {
            var result = new FoodSearch();
            var collection = new ObservableCollection<FoodSearch>();
            foreach (Food f in foods)
            {
                result = new FoodSearch() { Name = f.Name, Id = f.Id };
             
                collection.Add(result);
            }

            return collection;
        }
    }
}
