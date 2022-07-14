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
using CsvExcelTransformer.ViewModels;

namespace CsvExcelTransformer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Members

        private readonly MainWindowViewModel m_ViewModel;

        #endregion Private Members

        #region constructor

        public MainWindow()
        {
            InitializeComponent();

            m_ViewModel = new MainWindowViewModel(pageTransitionControl);
            m_ViewModel.TitleText = Title;

            DataContext = m_ViewModel;
        }

        #endregion constructor

        #region Private Methods

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion Private Methods

        #region Public Methods

        internal void SetStatusText(string statusText)
        {
            m_ViewModel.StatusText = statusText;
        }

        #endregion Public Methods

    }
}
