using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        private BaseCard selectedCard = null;

        public BaseCard SelectedCard
        {
            get { return selectedCard; }
            set
            {
                selectedCard = value;
                OnPropertyChanged(nameof(SelectedCard));
            }
        }

        public OverviewPageVM()
        {
            Cards = CardRepository.GetCards();             
        }
    }
}
