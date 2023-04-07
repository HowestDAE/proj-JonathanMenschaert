using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Project.Model;
using Project.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Project.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private Page currentPage = null;
        public RelayCommand SwitchToDetailPageCommand { get; private set; }
        public RelayCommand SwitchToSearchPageCommand { get; private set; }
        public RelayCommand SwitchToCollectionPageCommand { get; private set; }

        private SolidColorBrush NormalColor { get; set; }
        private SolidColorBrush SelectedColor { get; set; }

        private SolidColorBrush searchColor;
        public SolidColorBrush SearchColor
        {
            get 
            { 
                return searchColor; 
            }
            set 
            { 
                searchColor = value;
                OnPropertyChanged(nameof(SearchColor));
            }
        }

        private SolidColorBrush collectionColor;
        public SolidColorBrush CollectionColor
        {
            get
            {
                return collectionColor;
            }
            set
            {
                collectionColor = value;
                OnPropertyChanged(nameof(CollectionColor));
            }
        }

        private SolidColorBrush infoColor;
        public SolidColorBrush InfoColor
        {
            get
            {
                return infoColor;
            }
            set
            {
                infoColor = value;
                OnPropertyChanged(nameof(InfoColor));
            }
        }


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

        public OverviewApiPage SearchPage { get; private set; } = new OverviewApiPage();
        public OverviewLocalPage CollectionPage { get; private set; } = new OverviewLocalPage();
        public DetailPage InfoPage { get; private set; } = new DetailPage();

        public MainViewModel()
        {
            CurrentPage = SearchPage;
            SwitchToDetailPageCommand = new RelayCommand(SwitchToDetailPage);
            SwitchToSearchPageCommand = new RelayCommand(SwitchToSearchPage);
            SwitchToCollectionPageCommand = new RelayCommand(SwitchToCollectionPage);
            NormalColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626"));
            SelectedColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2B2D53"));
            SearchColor = SelectedColor;
            CollectionColor = NormalColor;
            InfoColor = NormalColor;
        }

        private void SwitchToDetailPage()
        {
            BaseCard selectedCard = null;
            if (CurrentPage is OverviewApiPage)
            {
                selectedCard = (SearchPage.DataContext as OverviewPageApiVM).SelectedCard;               
            }
            else if (CurrentPage is OverviewLocalPage)
            {
                selectedCard = (CollectionPage.DataContext as OverviewPageLocalVM).SelectedCard;
            }

            if (selectedCard == null)
            {
                return;
            }
            (InfoPage.DataContext as DetailPageVM).CurrentCard = selectedCard;
            CurrentPage = InfoPage;
            SearchColor = NormalColor;
            CollectionColor = NormalColor;
            InfoColor = SelectedColor;
        }

        private void SwitchToSearchPage()
        {
            if (!(CurrentPage is OverviewApiPage))
            {
                CurrentPage = SearchPage;
                SearchColor = SelectedColor;
                CollectionColor = NormalColor;
                InfoColor = NormalColor;
            }
        }

        private void SwitchToCollectionPage()
        {
            if (!(CurrentPage is OverviewLocalPage))
            {
                CurrentPage = CollectionPage;
                SearchColor = NormalColor;
                CollectionColor = SelectedColor;
                InfoColor = NormalColor;
            }
        }
    }
}
