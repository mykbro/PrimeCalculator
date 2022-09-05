using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;

namespace PrimeCalculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly MainWindow window = new MainWindow();           
        private readonly Stopwatch timer = new Stopwatch();
        private bool calculating;


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.calculating = false;
           
            this.window.CalcButtonClick += onCalcButtonClick;
            this.window.StopButtonClick += onStopButtonClick;
            this.window.Closed += onWindowClose;
            this.window.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }

        private void onCalcButtonClick(object sender, EventArgs e)
        {
            int? fromNr = window.From;
            int? toNr = window.To;

            window.clearPrimes();
            timer.Reset();
            window.updateTimeElapsed(timer.ElapsedMilliseconds);

            if (fromNr != null && toNr != null && toNr >= fromNr && fromNr >= 0 && toNr >= 0)
            {                
                timer.Start();                
                this.window.CalculationEnabled = false;
                this.calculating = true;
               
                Task.Run(() => this.calcAndPrintFactors((int)fromNr, (int)toNr));

            }
        }

        private void onStopButtonClick(object sender, EventArgs e)
        {
            this.calculating = false;
        }

        private void onWindowClose(object sender, EventArgs e)
        {

        }

        private void calcAndPrintFactors(int fromNr, int toNr)
        {

            
            int i;
            for (i = fromNr; i <= toNr && this.calculating; i++)
            {
                List<int> factors = calculateFactors(i);
                int toPass = i;
                Dispatcher.Invoke(() => this.window.addFactors(toPass, factors), DispatcherPriority.Background);      //Invoke e non BeginInvoke è quello che mi limita e mi permette di stoppare right on time_
                                                                                                                      //_altrimenti tutte le chiamate vengono fatte e incodate al Dispatcher.     
                Dispatcher.Invoke(() => this.window.updateTimeElapsed(timer.ElapsedMilliseconds), DispatcherPriority.Background);
            }
           
                
            Dispatcher.Invoke(() => this.window.CalculationEnabled = true, DispatcherPriority.Background);
            Dispatcher.Invoke(() => this.window.updateTimeElapsed(timer.ElapsedMilliseconds), DispatcherPriority.Background);
            this.calculating = false;
        }     

       
        
        private bool isPrime(int n)
        {
            if (n == 2 || n == 3)
                return true;

            if (n <= 1 || n % 2 == 0 || n % 3 == 0)
                return false;

            for (int i = 5; i * i <= n; i += 6)
            {
                if (n % i == 0 || n % (i + 2) == 0)
                    return false;
            }

            return true;          
        }

        private List<int> calculateFactors(int number)
        {
            List<int> primes = new List<int>();

            for (int div = 2; div <= number; div++)
                while (number % div == 0)
                {
                    primes.Add(div);
                    number = number / div;
                }

            return primes;
        }

        
    }

  
}
