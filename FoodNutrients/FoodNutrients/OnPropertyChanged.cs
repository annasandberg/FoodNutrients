using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FoodNutrients
{
    public class OnPropertyChanged : INotifyPropertyChanged
    {
        List<Food> foods;

        public event PropertyChangedEventHandler PropertyChanged;

        public OnPropertyChanged(List<Food> list)
        {
            foods = list;
        }

        public List<Food> Foods
        { //Property that will be used to get and set the item
            get { return foods; }

            set
            {
                foods = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("Foods"));// Throw!!
                }
            }
        }

        public void Reinitialize()
        { // mymethod
            Foods = foods;
        }
    }
}

//public class Wrapper : INotifyPropertyChanged
//{
//    List<Filiale> list;
//    JsonManager jM = new JsonManager();//retrieve the list

//    public event PropertyChangedEventHandler PropertyChanged;
//    public NearMeViewModel()
//    {
//        list = (jM.ReadData()).OrderBy(x => x.distanza).ToList();//initialize the list
//    }

   