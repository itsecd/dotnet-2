using System.Collections.Generic;

namespace Laba2Client.ViewModels
{
    public class MonthlyReportViewModel
    {
        public List<KeyValuePair<string, int>> PopularProducts { get; set; }
        public double TotalCost { get; set; }
        public MonthlyReportViewModel()
        {
            PopularProducts = new List<KeyValuePair<string, int>>();
        }
        public void Initialize(IDictionary<string, int> dict, double totalCost)
        {
            foreach (KeyValuePair<string, int> pair in dict)
            {
                PopularProducts.Add(pair);
            }
            TotalCost = totalCost;
        }
    }
}