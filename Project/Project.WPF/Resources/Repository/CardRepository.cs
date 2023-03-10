using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.LIB;
using Project.LIB.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.WPF.Resources.Repository
{
    public class CardRepository
    {

        public static List<CardType> CardTypes { get; private set; }
        public static List<CardType> GetCardTypes()
        {
            CardTypes = new List<CardType>();
            CardTypes.Add(new CardType()
            {
                Class = "Energy"
            });

            CardTypes.Add(new CardType()
            {
                Class = "Pokemon"
            });

            CardTypes.Add(new CardType()
            {
                Class = "Trainer"
            });

            return CardTypes;
        }

        public static CardType GetCardType(string cardClass)
        {
            if (CardTypes == null) GetCardTypes();
            foreach(var cardType in CardTypes)
            {
                if (cardType.Class == cardClass) return cardType;
            }
            return null;
        }

        public static List<BaseCard> GetCard()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Project.WPF.Resources.DataFiles.cards.json";

            List<BaseCard> cards = new List<BaseCard>();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    JArray cardsObj = JArray.Parse(json);
                    //cards = JsonConvert.DeserializeObject<List<BaseCard>>(json);
                    foreach(var card in cardsObj)
                    {
                        string cardTypeClass = card.SelectToken("supertype").ToObject<string>();
                        Type cardType = GetCardType(cardTypeClass.Replace("é", "e")).ActualType;
                        var currentCard = Activator.CreateInstance(cardType) as BaseCard;

                        currentCard.Id = card.SelectToken("id").ToObject<string>();
                        currentCard.Name = card.SelectToken("name").ToObject<string>();
                        currentCard.SuperType = cardTypeClass;
                        //currentCard.SubTypes = card.SelectToken("subtypes").ToObject<string[]>();

                        cards.Add(currentCard);
                    }
                }
            }
            return cards;
        }
    }
}
