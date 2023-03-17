using CommunityToolkit.Mvvm.ComponentModel;
using Project.Model;
using Project.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel
{
    public class OverviewPageVM : ObservableObject
    {
        public List<BaseCard> Cards { get; private set; }

        public OverviewPageVM()
        {
            Cards = CardRepository.GetCards();             
        }
    }
}
