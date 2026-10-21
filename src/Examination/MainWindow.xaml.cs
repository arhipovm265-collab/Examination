using Examination.Models;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Examination
{
    public partial class MainWindow : Window
    {
        private CityControl control;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCount.Text, out int count) || count <= 0)
            {
                MessageBox.Show("Введите целое положительное количество городов.");
                return;
            }
            control = new CityControl(count);
            dgInput.ItemsSource = control.Cities;
            dgUnsorted.ItemsSource = null;
            dgSorted.ItemsSource = null;
        }
        private bool CheckData()
        {
            if (control == null)
            {
                MessageBox.Show("Сначала создайте массив.");
                return false;
            }
            dgInput.CommitEdit(DataGridEditingUnit.Row, true);
            for (int i = 0; i < control.Cities.Length; i++)
            {
                City city = control.Cities[i];

                if (string.IsNullOrWhiteSpace(city.Name))
                {
                    MessageBox.Show($"Строка {i + 1}: не указано название.");
                    return false;
                }
                if (!CityControl.TryParseDate(city.FoundationDate, out _))
                {
                    MessageBox.Show($"Строка {i + 1}: неверная дата основания.");
                    return false;
                }

                if (city.Population < 0)
                {
                    MessageBox.Show($"Строка {i + 1}: количество жителей не может быть отрицательным.");
                    return false;
                }
            }
            return true;
        }
        private void btnUnsorted_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            dgUnsorted.ItemsSource = (City[])control.Cities.Clone();
        }
        private void SortAndShow()
        {
            control.Sort();
            dgSorted.ItemsSource = (City[])control.Cities.Clone();
            dgInput.Items.Refresh();
        }
        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            SortAndShow();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckData())
            {
                return;
            }
            SortAndShow();
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Текстовый файл (*.txt)|*.txt";
            dialog.FileName = "cities.txt";
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    control.SaveToFile(dialog.FileName);
                    MessageBox.Show("Файл сохранён.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения: " + ex.Message);
                }
            }
        }
    }
}