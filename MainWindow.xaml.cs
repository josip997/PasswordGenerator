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

namespace PasswordGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            short nLength = Convert.ToInt16(cmbLength.Text);
            var generator = new RandomGenerator();
            txtPassword.Text = generator.GenerateString(nLength, 
                                                        chkUppercase.IsChecked ?? false,
                                                        chkNumbers.IsChecked ?? false,
                                                        chkSymbols.IsChecked ?? false);
            txtPassword.IsEnabled = true;
            txtPassword.Focus();
            txtPassword.SelectAll();
        }
        public class RandomGenerator
        {
            private readonly Random _random = new Random();

            public int GenerateNumber(int min, int max)
            {
                return _random.Next(min, max);
            }

            public string GenerateString(int nLength, bool bUppercase, bool bNumbers, bool bSymbols)
            {
                var builder = new StringBuilder(nLength);

                // Unicode/ASCII Letters are divided into two blocks
                // (Letters 65–90 / 97–122):
                // The first group containing the uppercase letters and
                // the second group containing the lowercase.  

                //initial values for lowercase letters
                char offset = 'a';
                int offsetLength = 26;

                for (var i = 0; i<nLength; i++)
                {

                    bool NextCharIsNumber = false;
                    bool NextCharIsSymbol = false;

                    if (bNumbers)
                    {
                        // 0.8 since we want more letters than numbers
                        NextCharIsNumber = _random.NextDouble() >= 0.8 ? true : false;
                    }
 
                    if (bSymbols & !NextCharIsNumber)
                    {
                        // 0.8 since we want more letters than symbols
                        NextCharIsSymbol = _random.NextDouble() >= 0.8 ? true : false;
                    }

                    if (NextCharIsNumber)
                    {
                        //offset for numbers
                        offset = '0';
                        offsetLength = 10;
                    }
                    else if (NextCharIsSymbol)
                    {
                        //offset for symbols
                        offset = '#';
                        offsetLength = 4;
                    }   
                    else
                    {
                        //offset for letters
                        offset = 'a';
                        offsetLength = 26;

                        //If uppercase is checked, next character could be uppercase letter
                        if (bUppercase)
                        {
                            offset = _random.NextDouble() >= 0.7 ? 'A' : 'a';
                        }
                    }

                    var @char = (char)_random.Next(offset, offset + offsetLength);
                    builder.Append(@char);
                }

                return builder.ToString();
            }
        }
    }
}
