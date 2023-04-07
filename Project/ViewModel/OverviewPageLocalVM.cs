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
    public class OverviewPageLocalVM : ObservableObject
    {
        private int[] pageSizes = new int[] { 5, 10, 25, 50, 100, 150, 200, 250 };
        public int[] PageSizes
        {
            get
            {
                return pageSizes;
            }
            private set
            {
                pageSizes = value;
                OnPropertyChanged(nameof(PageSizes));
            }
        }

        private int selectedPageSize = 250;
        public int SelectedPageSize
        {
            get
            {
                return selectedPageSize;
            }
            set
            {
                selectedPageSize = value;
                OnPropertyChanged(nameof(SelectedPageSize));
            }
        }

        private int pageNr = 1;
        public int PageNr
        {
            get
            {
                return pageNr;
            }
            set
            {
                if (value <= TotalPages)
                {
                    pageNr = value;
                }
                OnPropertyChanged(nameof(PageNr));
                DecreasePageCommand.NotifyCanExecuteChanged();
                IncreasePageCommand.NotifyCanExecuteChanged();
            }
        }

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

        private string cardName;
        public string CardName
        {
            get
            {
                return cardName;
            }
            set
            {
                cardName = value;
                OnPropertyChanged(nameof(CardName));
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

        private int totalPages = 1;
        public int TotalPages
        {
            get
            {
                return totalPages;
            }
            private set
            {
                totalPages = value;
                OnPropertyChanged(nameof(TotalPages));
                IncreasePageCommand.NotifyCanExecuteChanged();
                DecreasePageCommand.NotifyCanExecuteChanged();
            }
        }

        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand IncreasePageCommand { get; private set; }
        public RelayCommand DecreasePageCommand { get; private set; }

        private CardLocalRepository localRepository = new CardLocalRepository();

        public OverviewPageLocalVM()
        {
            Cards = new List<BaseCard>();
            SearchCommand = new RelayCommand(SearchCardsAsync, CanSearchCards);
            IncreasePageCommand = new RelayCommand(IncreasePage, CanIncreasePage);
            DecreasePageCommand = new RelayCommand(DecreasePage, CanDecreasePage);
            LoadComboBoxesAsync();
        }



        private bool CanIncreasePage()
        {
            return pageNr < TotalPages;
        }

        private void IncreasePage()
        {
            ++PageNr;
        }

        private bool CanDecreasePage()
        {
            return PageNr > 1;
        }

        private void DecreasePage()
        {
            --PageNr;
        }

        private async void LoadComboBoxesAsync()
        {
            SubTypes = await localRepository.LoadPropertyAsync("Subtypes");
            SubTypes.Add(anyWildCard);
            SelectedSubType = anyWildCard;
            Types = await localRepository.LoadPropertyAsync("Types");
            Types.Add(anyWildCard);
            SelectedType = anyWildCard;
        }

        private bool CanSearchCards()
        {
            if (Cards == null)
            {
                return false;
            }
            return true;
        }

        private async void SearchCardsAsync()
        {
            List<string> searchQueries = new List<string>();

            if (SelectedSubType != null && SelectedSubType != anyWildCard)
            {
                searchQueries.Add($"subtypes={SelectedSubType.ToLower()}");
            }

            if (SelectedType != null && SelectedType != anyWildCard)
            {
                searchQueries.Add($"types={SelectedType}");
            }

            if (!string.IsNullOrEmpty(CardName))
            {
                searchQueries.Add($"name={CardName}");
            }

            string query = ""; // $"pageSize={SelectedPageSize}&page={PageNr}&";


            if (searchQueries.Count > 0)
            {
                foreach (string searchQuery in searchQueries)
                {
                    if (query.Length > 0) query += " ";
                    query += searchQuery;
                }
            }

            CardMessage = "Loading cards ...";
            Cards = null;
            SearchCommand.NotifyCanExecuteChanged();
            Cards = await localRepository.LoadCardsAsync(query);
            OnPropertyChanged(nameof(PageNr));
            TotalPages = (int)Math.Ceiling((float)localRepository.TotalCards / SelectedPageSize);
            if (Cards != null && Cards.Count > 0)
            {
                CardMessage = "";
            }
            else
            {
                CardMessage = "No cards found! Try another search query.";
            }
            SearchCommand.NotifyCanExecuteChanged();
        }
    }
}
