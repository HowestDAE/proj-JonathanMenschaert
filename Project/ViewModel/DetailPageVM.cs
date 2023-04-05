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
    public class DetailPageVM : ObservableObject
    {
        private BaseCard currentCard = null;

        public BaseCard CurrentCard
        {
            get
            {
                return currentCard;
            }
            set
            {
                currentCard = value;
                OnPropertyChanged(nameof(CurrentCard));
            }
        }
    }
}
