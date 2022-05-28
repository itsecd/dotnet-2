using System.Windows;
using Laba2Client.ViewModels;

namespace Laba2Client.Views
{
    public partial class MonthlyReportView : Window
    {
        public MonthlyReportView()
        {
            InitializeComponent();
        }
        public MonthlyReportView(MonthlyReportViewModel monthlyReportViewModel)
        {
            InitializeComponent();
            DataContext = monthlyReportViewModel;
        }
    }
}