using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        public MainViewModel()
        {
            CurrentPage = MainPage;
        }
    }
}
