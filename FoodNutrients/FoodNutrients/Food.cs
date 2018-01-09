using System;
using System.Collections.Generic;
using System.Text;

namespace FoodNutrients
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double EnergyInKcal { get; set; }
        public double Carbohydrates { get; set; }
        public double Fat { get; set; }
        public double Protein { get; set; }
        public double Fibers { get; set; }
        public double Sugars { get; set; }
        public double SaturatedFat { get; set; }
        public double Salt { get; set; }
        public double VitaminD { get; set; }
        public double Iron { get; set; }
    }
}
