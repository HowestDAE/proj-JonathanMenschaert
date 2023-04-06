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
    public class OverviewPageApiVM : ObservableObject
    {
        private List<BaseCard> cards;
        public List<BaseCard> Cards 
        { 
            get
            {
                return cards;
            }
            private set
            {
                cards = value;
                OnPropertyChanged(nameof(Cards));
            } 
        }
        private BaseCard selectedCard = null;
        private CardApiRepository apiRepository = new CardApiRepository();

        public BaseCard SelectedCard
        {
            get { return selectedCard; }
            set
            {
                selectedCard = value;
                OnPropertyChanged(nameof(SelectedCard));
            }
        }

        public OverviewPageApiVM()
        {
            LoadCards();          
        }

        private async void LoadCards()
        {
            Cards = await apiRepository.LoadCardsAsync();
        }
    }
}
