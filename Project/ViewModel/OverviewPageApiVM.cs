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

        private List<string> subTypes;
        public List<string> SubTypes 
        { 
            get
            {
                return subTypes;
            }
            private set
            {
                subTypes = value;
                OnPropertyChanged(nameof(SubTypes));
            }
        }

        private string selectedSubType;
        public string SelectedSubType 
        { 
            get
            {
                return selectedSubType;
            }
            set
            {
                selectedSubType = value;
                OnPropertyChanged(nameof(SelectedSubType));
            }
        }

        private List<string> types;
        public List<string> Types
        {
            get
            {
                return types;
            }
            private set
            {
                types = value;
                OnPropertyChanged(nameof(Types));
            }
        }

        private string selectedType;
        public string SelectedType
        {
            get
            {
                return selectedType;
            }
            set
            {
                selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
            }
        }

        private string cardMessage = "Select query to search cards";
        public string CardMessage 
        { 
            get
            {
                return cardMessage;
            }
            private set
            {
                cardMessage = value;
                OnPropertyChanged(nameof(CardMessage));
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

        private readonly string anyWildCard = "Any";

        public RelayCommand SearchCommand { get; private set; }

        public OverviewPageApiVM()
        {
            SearchCommand = new RelayCommand(SearchCardsAsync);
            LoadComboBoxesAsync();         
        }

        private async void LoadComboBoxesAsync()
        {
            SubTypes = await apiRepository.LoadPropertyAsync("subtypes");
            SubTypes.Add(anyWildCard);
            SelectedSubType = anyWildCard;
            Types = await apiRepository.LoadPropertyAsync("types");
            Types.Add(anyWildCard);
            SelectedType = anyWildCard;
        }

        private async void SearchCardsAsync()
        {
            List<string> searchQueries = new List<string>();            

            if (SelectedSubType != null && SelectedSubType != anyWildCard)
            {
                searchQueries.Add($"subtypes:\"{SelectedSubType.ToLower()}\"");
            }

            if (SelectedType != null && SelectedType != anyWildCard)
            {
                searchQueries.Add($"types:\"{SelectedType}\"");
            }


            string query = "q=";
            foreach(string searchQuery in searchQueries)
            {
                if (query.Length > 3) query += " ";
                query += searchQuery;
            }

            CardMessage = "Loading cards ...";
            Cards = null;
            Cards = await apiRepository.LoadCardsAsync(query);
            if (Cards.Count > 0)
            {
                CardMessage = "";
            }
            else
            {
                CardMessage = "No cards found! Try another search query.";
            }
        }
    }
}
