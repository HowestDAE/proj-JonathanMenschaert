using Project.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace Project.Repository
{
    public class CardLocalRepository : CardRepository
    {
        private List<BaseCard> cardList;

        public int TotalCards
        {
            get
            {
                if (cardList != null)
                {
                    return cardList.Count;
                }
                return 0;
            }
        }
        
        protected override async Task<List<CardType>> LoadCardTypesAsync()
        {
            CardTypes = new List<CardType>();

            await Task.Run(() =>
            {
                CardTypes.Add(new CardType()
                {
                    Class = "Energy"
                });

                CardTypes.Add(new CardType()
                {
                    Class = "Pokémon"
                });

                CardTypes.Add(new CardType()
                {
                    Class = "Trainer"
                });
            });
            return CardTypes;
        }

        private bool ContainsProperty(BaseCard item, string queryName, string queryValue)
        {
            if (queryName.Equals("subtypes") && item.SubTypes != null)
            {
                if (item.SubTypes == null)
                {
                    return false;
                }
                return item.SubTypes.Contains(queryValue, StringComparer.OrdinalIgnoreCase);
            }
            else if (queryName.Equals("types"))
            {

            }
            else if (queryName.Equals("name"))
            {

            }
            return false;
        }

        public async Task<List<BaseCard>> LoadCardsAsync(string query)
        {
            if (cardList == null)
            {
                await LoadCardsAsync();
            }
            List<BaseCard> filteredCards = null;
            await Task.Run(() =>
            {
                List<string> queries = query.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                
                filteredCards = cardList.Where(item =>
                {
                    bool containsValue = true;
                    foreach (string searchQuery in queries)
                    {
                        int separatorIndex = searchQuery.IndexOf("=");
                        string queryName = searchQuery.Substring(0, separatorIndex);
                        string queryValue = searchQuery.Substring(separatorIndex + 1);

                        containsValue = containsValue && ContainsProperty(item, queryName, queryValue);
                    }
                    return containsValue;
                }).ToList();
            });
            return filteredCards;
        }

        public async Task<List<string>> LoadPropertyAsync(string property)
        {
            List<string> propertyValues = null;
            await Task.Run(() =>
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = $"Project.Resources.Data.card{property}.json";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        JArray propArr = JArray.Parse(json);

                        propertyValues = propArr.ToObject<List<string>>();
                    }
                }
            });
            return propertyValues;
        }

        public async Task LoadCardsAsync()
        {            
            cardList = new List<BaseCard>();
            
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Project.Resources.Data.cards.json";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    JArray cardsArr = JArray.Parse(json);
                    foreach (var card in cardsArr)
                    {
                        BaseCard currentCard = await PopulateCard(card);
                        cardList.Add(currentCard);

                    }
                }
            }           
        }
    }
}
