using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace PrimeCalculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow window;
        private bool calculating;       

       
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.window = new MainWindow(this);
            this.calculating = false;
            window.Show();
        }      

        public void calcButtonClicked()
        {
            int? fromNr = window.From;
            int? toNr = window.To;

            window.clearPrimes();

            if (fromNr != null && toNr != null && toNr >= fromNr)
            {
                this.window.CalculationEnabled = false;
                this.calculating = true;
                //this.setCalculating(true);


                /*
                for (int i = (int)fromNr; i <= toNr && this.calculating; i++)
                //if (await Task.Run(() => isPrime(i)))
                {
                    await Task.Delay(1000);
                    window.addPrime(i);
                }

                window.CalculationEnabled = true;
                this.calculating = false;
                */

                /*
                List<int> factors;
                for (int i = (int)fromNr; i <= toNr && this.calculating; i++)
                {
                    int toPass = i;
                    factors = await Task.Run(() => calculateFactors(toPass));                    
                    window.addFactors(i, factors);
                }
                
                window.CalculationEnabled = true;
                this.calculating = false;
                */

                /*
                Thread t = new Thread(() => this.calcAndPrintPrimes((int)fromNr,(int)toNr,canceller.Token));
                t.Start(); 
                */
                Task.Run(() => this.calcAndPrintFactors((int)fromNr, (int)toNr));



            }
        }

        public void calcAndPrintFactors(int fromNr, int toNr)
        {

            
            int i;
            for (i = fromNr; i <= toNr && this.calculating; i++)
            {
                List<int> factors = calculateFactors(i);
                int toPass = i;
                Dispatcher.Invoke(()=>window.addFactors(toPass, factors), DispatcherPriority.Background);             
            }
           
                
            Dispatcher.Invoke(() => window.CalculationEnabled = true, DispatcherPriority.Background);
            this.calculating = false;
        }
       

        public void stopButtonClicked()
        {
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
