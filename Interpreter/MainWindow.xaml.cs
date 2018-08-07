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

using LexerModule;
using ParseModule;

namespace Interpreter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Run_Button_Click(object sender, RoutedEventArgs e)
        {
            string text = new TextRange(Input.Document.ContentStart, Input.Document.ContentEnd).Text;
            Lexer lexer = new Lexer(text);
            List<Token> tokens = lexer.Tokenize();
            Parser parser = null;
            string result = "";
            try
            {
                parser = new Parser(tokens);
                result = parser.Run();
            }
            catch(Exception ex)
            {
                result = ex.Message;
            }
            parser.Reset();
            Output.Text = result;
        }
    }
}
