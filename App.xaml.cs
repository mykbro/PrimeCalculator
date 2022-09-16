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
using System.IO;
using System.Collections.ObjectModel;

namespace PrimeCalculator
{    

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {       
        private MainWindow window;           
        private readonly Stopwatch timer = new Stopwatch();       
        private volatile bool _calculating;       
        private string currentCulture;


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _calculating = false;
           
            StreamReader tx = null;
            try
            {  
                tx = new StreamReader("config.txt");
                String temp = tx.ReadLine();
                
                if (temp == null)
                    currentCulture = "en-us";
                else
                    currentCulture = temp;
            }
            catch
            {
                currentCulture = "en-us";   
            }
            finally
            {
                if(tx != null)
                    tx.Close();
            }

            if(currentCulture != "en-us") 
                this.tryToSetLanguage(currentCulture);
               
            

            this.window = new MainWindow();
            this.window.CalcButtonClick += onCalcButtonClick;
            this.window.StopButtonClick += onStopButtonClick;
            this.window.Closed += onWindowClose;
            this.window.SwitchLangButtonClick += onSwitchLangButtonClick;
            this.window.Show();
        }
       

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }

        private async void onCalcButtonClick(object sender, EventArgs e)
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
                _calculating = true;             
                
                await calcAndPrintFactorsAsync((int)fromNr, (int)toNr);

                _calculating = false;
                this.window.CalculationEnabled = true;
                timer.Stop();                
            }
        }

        private void onStopButtonClick(object sender, EventArgs e)
        {
            _calculating = false;
        }

        private void onWindowClose(object sender, EventArgs e)
        {

        }

        private void onSwitchLangButtonClick(object sender, EventArgs e)
        {            
            if (currentCulture == "en-us")
                    this.tryToSetLanguage("it-it");
                else
                    this.tryToSetLanguage("en-us");
        }        

        private void tryToSetLanguage(string cultureToSet)
        {
            try
            {
                String uriString = "/PrimeCalculator;component/Localizations/" + cultureToSet + ".xaml";
                ResourceDictionary dictToAdd = new ResourceDictionary() { Source = new Uri(uriString, UriKind.Relative) };
                window.Resources.MergedDictionaries.Clear();
                window.Resources.MergedDictionaries.Add(dictToAdd);

                this.currentCulture = cultureToSet;
            }
            catch { }
        }

        private async Task calcAndPrintFactorsAsync(int fromNr, int toNr)
        {            
            Task<List<int>>[] factorizationTasks = new Task<List<int>>[toNr - fromNr + 1];     
            SemaphoreSlim sem = new SemaphoreSlim(0);       //used to ensure that printFactorsTask does not overspeed calcFactorsTask reading 'null' cells

            //We need to spawn a new Task for calculating the factorization because creating a huge nr of Factorization Task would hang the UI
            //We also need to parallelize the printing process because if we spawn a lot of Task we would wait a lot before the first number appears
            Task calcFactorsTask = Task.Run(() => calcFactors(factorizationTasks, sem, fromNr));
            Task printFactorsTask = Task.Run(() => printFactors(factorizationTasks, sem, fromNr));

            await Task.WhenAll(calcFactorsTask, printFactorsTask);
        }

        private void calcFactors(Task<List<int>>[] factorizationTasks, SemaphoreSlim sem, int fromNr)
        {
            for (int i = 0, numToFactorize = fromNr; i < factorizationTasks.Length && _calculating; i++, numToFactorize++)
            {
                factorizationTasks[i] = calculateFactorsAsync(numToFactorize);
                sem.Release(); 
            }
        }

        private void printFactors(Task<List<int>>[] factorizationTasks, SemaphoreSlim sem, int fromNr)
        {            
            for (int i = 0, numToFactorize = fromNr; i < factorizationTasks.Length && _calculating; i++, numToFactorize++)
            {
                //SpinWait.SpinUntil(() => factorizationTasks[i] != null);        //not needed with the Sem anymore
                sem.Wait();
                List<int> factors = factorizationTasks[i].Result;
                Dispatcher.Invoke(
                    () =>
                    {
                        this.window.addFactors(numToFactorize, factors);
                        this.window.updateTimeElapsed(timer.ElapsedMilliseconds);
                    }, DispatcherPriority.Background);          //Invoke e non BeginInvoke è quello che mi limita e mi permette di stoppare right on time_
                                                                //_altrimenti tutte le chiamate vengono fatte e incodate al Dispatcher.
                factorizationTasks[i] = null;                   //we let the Task be Garbage Collected in order to eagerly free resources
            }
           
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

            for (int div = 2; div <= number && _calculating; div++)
                while (number % div == 0)
                {
                    primes.Add(div);
                    number = number / div;
                }

            return primes;
        }

        private Task<List<int>> calculateFactorsAsync(int number)
        {
            return Task.Run(() => calculateFactors(number));
        }

        
    }

  
}
