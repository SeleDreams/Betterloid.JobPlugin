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
using System.Windows.Shapes;

namespace JobPlugin
{
    /// <summary>
    /// Logique d'interaction pour JobPluginUI.xaml
    /// </summary>
    public partial class JobPluginUI : Window
    {
        private JobPluginUIViewModel model;
        public JobPluginUI()
        {
            model = new JobPluginUIViewModel();
            DataContext = model;
            InitializeComponent();
        }


    }
}
