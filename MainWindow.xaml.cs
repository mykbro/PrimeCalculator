﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        //private readonly App primeCalculator;
        private readonly PrimeCalc primeCalcControl;

        public MainWindow()
        {            
            InitializeComponent();
            this.primeCalcControl = (PrimeCalc) this.FindResource("defaultControl");
        }

        //<Events>
        public event EventHandler<EventArgs> CalcButtonClick;
        public event EventHandler<EventArgs> StopButtonClick;
        public event EventHandler<EventArgs> SwitchLangButtonClick;
        //</Events>

        public int? From
        {
            get
            {
                return primeCalcControl.From;
            }
        }

        public int? To
        {
            get
            {
                return primeCalcControl.To;
            }
        }

        public bool CalculationEnabled
        {
            set
            {
                primeCalcControl.CalculationEnabled = value;
            }
        }

        public void addPrime(int p)
        {
            primeCalcControl.addPrime(p);
        }

        public void addFactors(int num, List<int> factors)
        {
           primeCalcControl.addFactors(num, factors);
        }

        public void clearPrimes()
        {
            primeCalcControl.clearPrimes();
        }

        public void updateTimeElapsed(long ms)
        {
            primeCalcControl.updateTimeElapsed(ms);
        }



        //////////////////////////////////////////////////

        private void PrimeCalc_CalcButtonClick(object sender, EventArgs e)
        {
            if (CalcButtonClick != null)
                CalcButtonClick(this, EventArgs.Empty);
        }

        private void PrimeCalc_StopButtonClick(object sender, EventArgs e)
        {
            if (StopButtonClick != null)
                StopButtonClick(this, EventArgs.Empty);
        }

        private void PrimeCalc_SwitchLangButtonClick(object sender, EventArgs e)
        {
            if (SwitchLangButtonClick != null)
                SwitchLangButtonClick(this, EventArgs.Empty);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Content = primeCalcControl;
        }
    }
}
