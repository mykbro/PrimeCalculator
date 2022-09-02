using System;
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

        public MainWindow()
        {            
            InitializeComponent();            
        }

        //<Events>
        public event EventHandler<EventArgs> CalcButtonClick;
        public event EventHandler<EventArgs> StopButtonClick;
        //</Events>

        public int? From
        {
            get
            {
                int toReturn;
                if (Int32.TryParse(this.fromTextBox.Text, out toReturn))
                    return toReturn;
                else
                    return null;
            }
        }

        public int? To
        {
            get
            {
                int toReturn;
                if (Int32.TryParse(this.toTextBox.Text, out toReturn))
                    return toReturn;
                else
                    return null;
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
            if (CalcButtonClick != null)
                CalcButtonClick(this, EventArgs.Empty);
        }       

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            if (StopButtonClick != null)
                StopButtonClick(this, EventArgs.Empty);
        }       



    }
}
