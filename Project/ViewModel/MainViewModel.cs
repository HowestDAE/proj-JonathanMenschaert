using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Project.Model;
using Project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Project.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private Page currentPage = null;
        public RelayCommand SwitchToDetailPageCommand { get; private set; }
        public RelayCommand SwitchToSearchPageCommand { get; private set; }

        public Page CurrentPage
        {
            get
            {
                return currentPage;
            }
            private set
            {
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        public OverviewPage MainPage { get; private set; } = new OverviewPage();
        public DetailPage InfoPage { get; private set; } = new DetailPage();

        public MainViewModel()
        {
            CurrentPage = MainPage;
            SwitchToDetailPageCommand = new RelayCommand(SwitchToDetailPage);
            SwitchToSearchPageCommand = new RelayCommand(SwitchToSearchPage);
        }

        private void SwitchToDetailPage()
        {
            if (CurrentPage is OverviewPage)
            {
                BaseCard selectedCard = (MainPage.DataContext as OverviewPageVM).SelectedCard;
                if (selectedCard == null)
                {
                    return;
                }
                (InfoPage.DataContext as DetailPageVM).CurrentCard = selectedCard;
                CurrentPage = InfoPage;
            }
        }

        private void SwitchToSearchPage()
        {
            if (!(CurrentPage is OverviewPage))
            {
                CurrentPage = MainPage;
            }
        }
    }
}
