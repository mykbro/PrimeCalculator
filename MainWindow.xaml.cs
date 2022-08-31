﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrimeCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private App primeCalculator;

        public MainWindow(App app)
        {            
            InitializeComponent();
            this.primeCalculator = app;
        }

        public int? From
        {
            get
            {
                int toReturn;
                Int32.TryParse(this.fromTextBox.Text, out toReturn);
                return toReturn;
            }
        }

        public int? To
        {
            get
            {
                int toReturn;
                Int32.TryParse(this.toTextBox.Text, out toReturn);
                return toReturn;
            }
        }

        public bool CalculationEnabled
        {
            set
            {
                this.calcButton.IsEnabled = value;
                this.stopButton.IsEnabled = !value;
            }
        }

        public void addPrime(int p)
        {
            this.primeListBox.Items.Add(p);
            this.primeListBox.Items.MoveCurrentToLast();
            this.primeListBox.ScrollIntoView(this.primeListBox.Items.CurrentItem);
        }

        public void addFactors(int num, List<int> factors)
        {
            StringBuilder toAdd = new StringBuilder(num+" => ");
            foreach(int factor in factors)
                toAdd.Append(factor+" ");
            this.primeListBox.Items.Add(toAdd);
            this.primeListBox.Items.MoveCurrentToLast();
            this.primeListBox.ScrollIntoView(this.primeListBox.Items.CurrentItem);
        }

        public void clearPrimes()
        {
            this.primeListBox.Items.Clear();
        }


        //////////////////////////////////////////////////

       

        private void calcButton_Click(object sender, RoutedEventArgs e)
        {
            primeCalculator.calcButtonClicked();
        }       

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            primeCalculator.stopButtonClicked();
        }
    }
}