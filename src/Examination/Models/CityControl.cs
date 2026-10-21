using Examination.Models;
using System;
using System.Globalization;
using System.IO;

namespace Examination.Models
{
    public class CityControl
    {
        public City[] Cities { get; private set; }
        public CityControl(int size)
        {
            Cities = new City[size];
            for (int i = 0; i < size; i++)
            {
                Cities[i] = new City();
            }
        }
        public static bool TryParseDate(string text, out DateTime date)
        {
            return DateTime.TryParse(text, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out date);
        }
        private static bool IsLess(City a, City b)
        {
            if (a.Population != b.Population)
            {
                return a.Population < b.Population;
            }

            TryParseDate(a.FoundationDate, out DateTime dateA);
            TryParseDate(b.FoundationDate, out DateTime dateB);
            return dateA < dateB;
        }
        public void Sort()
        {
            for (int i = 0; i < Cities.Length - 1; i++)
            {
                for (int j = 0; j < Cities.Length - i - 1; j++)
                {
                    if (IsLess(Cities[j], Cities[j + 1]))
                    {
                        City temp = Cities[j];
                        Cities[j] = Cities[j + 1];
                        Cities[j + 1] = temp;
                    }
                }
            }
        }
        public void SaveToFile(string path)
        {
            using StreamWriter writer = new StreamWriter(path);
            foreach (City city in Cities)
            {
                writer.WriteLine($"{city.Name};{city.FoundationDate};{city.Population}");
            }
        }
    }
}