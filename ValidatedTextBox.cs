using System;
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
    /// Per utilizzare questo controllo personalizzato in un file XAML, eseguire i passaggi 1a o 1b e 2.
    ///
    /// Passaggio 1a) Utilizzo di questo controllo personalizzato in un file XAML esistente nel progetto corrente.
    /// Aggiungere questo attributo XmlNamespace all'elemento radice del file di markup dove 
    /// deve essere utilizzato:
    ///
    ///     xmlns:MyNamespace="clr-namespace:PrimeCalculator"
    ///
    ///
    /// Passaggio 1b) Utilizzo di questo controllo personalizzato in un file XAML esistente in un progetto diverso.
    /// Aggiungere questo attributo XmlNamespace all'elemento radice del file di markup dove 
    /// deve essere utilizzato:
    ///
    ///     xmlns:MyNamespace="clr-namespace:PrimeCalculator;assembly=PrimeCalculator"
    ///
    /// Sarà inoltre necessario aggiungere nel progetto in cui si trova il file XAML
    /// un riferimento a questo progetto, quindi ricompilare per evitare errori di compilazione:
    ///
    ///     In Esplora soluzioni, fare clic con il pulsante destro del mouse sul progetto di destinazione, quindi scegliere
    ///     "Aggiungi riferimento"->"Progetti"->[Individuare e selezionare questo progetto]
    ///
    ///
    /// Passaggio 2)
    /// Utilizzare il controllo nel file XAML.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class ValidatedTextBox : TextBox
    {

        private Image errorImg;
        private Border border;
        private TextBox inputTBox;
        private TextBlock errorMsgTextBlock;

        private bool isValid = false;
        private string errorMsg = "Ciao";

        static ValidatedTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ValidatedTextBox), new FrameworkPropertyMetadata(typeof(ValidatedTextBox)));
        }       
                
        public readonly static DependencyProperty IsValidProperty = DependencyProperty.Register("IsValid",typeof(bool),typeof(ValidatedTextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public readonly static DependencyProperty ErrorMsgProperty = DependencyProperty.Register("ErrorMsg", typeof(string), typeof(ValidatedTextBox), new FrameworkPropertyMetadata("Error", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsValid
        {
            get { return (bool) this.GetValue(IsValidProperty); }
            set { this.SetValue(IsValidProperty, value); }
        }

        public string ErrorMsg
        {
            get { return (string) this.GetValue(ErrorMsgProperty); }
            set { this.SetValue(ErrorMsgProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            errorImg = this.GetTemplateChild("errorImg") as Image;
            inputTBox = this.GetTemplateChild("inputTextBox") as TextBox;
            errorMsgTextBlock = this.GetTemplateChild("errorMsgTextBlock") as TextBlock;
            border = this.GetTemplateChild("border") as Border;

            inputTBox.TextChanged += InputTBox_TextChanged;
            //base.OnApplyTemplate();
        }

        private void InputTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //this.RaiseEvent(new RoutedEventArgs(TextChangedEvent));
            this.IsValid = inputTBox != null && inputTBox.Text.StartsWith('C');
        }
    }
}
