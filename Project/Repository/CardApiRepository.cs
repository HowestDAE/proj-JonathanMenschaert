using Newtonsoft.Json.Linq;
using Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repository
{
    public class CardApiRepository : ICardRepository
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

        public async Task<List<BaseCard>> LoadCardsAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var endPoint = $"https://api.pokemontcg.io/v2/cards?pageSize=250&q=supertype:trainer";
                    var response = await client.GetAsync(endPoint);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(response.ReasonPhrase);
                    }
                    string json = await response.Content.ReadAsStringAsync();
                    JObject cardsObj = JObject.Parse(json);
                    JToken cardArray = cardsObj.SelectToken("data");

                    List<BaseCard> cardList = new List<BaseCard>();
                    foreach(var card in cardArray)
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

                        cardList.Add(currentCard);
                    }
                    return cardList;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return null;
        }
    }
}
