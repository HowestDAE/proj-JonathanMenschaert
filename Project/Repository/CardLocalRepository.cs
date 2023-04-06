using Project.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Project.Repository
{
    public class CardLocalRepository
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
            foreach (var cardType in CardTypes)
            {
                if (cardType.Class == cardClass) return cardType;
            }
            return null;
        }

        public static List<BaseCard> GetCards()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Project.Resources.Data.cards.json";

            List<BaseCard> cards = new List<BaseCard>();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    JArray cardsObj = JArray.Parse(json);
                    //cards = JsonConvert.DeserializeObject<List<BaseCard>>(json);
                    foreach (var card in cardsObj)
                    {
                        string cardTypeClass = card.SelectToken("supertype").ToObject<string>();
                        Type cardType = GetCardType(cardTypeClass.Replace("é", "e")).ActualType;
                        var currentCard = Activator.CreateInstance(cardType) as BaseCard;
                        currentCard.Id = card.SelectToken("id").ToObject<string>();
                        currentCard.Name = card.SelectToken("name").ToObject<string>();
                        currentCard.SuperType = cardTypeClass;

                        var subTypeToken = card.SelectToken("subtypes");
                        if (subTypeToken != null)
                        {
                            currentCard.SubTypes = subTypeToken.ToObject<List<string>>();
                        }

                        var energyCard = currentCard as EnergyCard;
                        if (energyCard != null)
                        {
                            energyCard.Rules = card.SelectToken("rules").ToObject<List<string>>();
                        }

                        var trainerCard = currentCard as TrainerCard;
                        if (trainerCard != null)
                        {
                            trainerCard.Rules = card.SelectToken("rules").ToObject<List<string>>();
                        }

                        var pokemonCard = currentCard as PokemonCard;
                        if (pokemonCard != null)
                        {
                            var attacksToken = card.SelectToken("attacks");
                            if (attacksToken != null)
                            {
                                pokemonCard.Attacks = attacksToken.ToObject<List<Attack>>();
                            }

                            var abilityToken = card.SelectToken("abilities");
                            if (abilityToken != null)
                            {
                                pokemonCard.Abilities = abilityToken.ToObject<List<Ability>>();
                            }
                        }

                        cards.Add(currentCard);
                    }
                }
            }
            return cards;
        }
    }
}
