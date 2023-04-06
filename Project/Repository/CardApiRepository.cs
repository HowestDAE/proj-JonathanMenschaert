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

        public List<CardType> CardTypes { get; private set; }
        public async Task<List<CardType>> LoadCardTypesAsync()
        {
            if (CardTypes != null) return CardTypes;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var endPoint = $"https://api.pokemontcg.io/v2/supertypes";
                    var response = await client.GetAsync(endPoint);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(response.ReasonPhrase);
                    }
                    string json = await response.Content.ReadAsStringAsync();
                    JObject superTypeObj = JObject.Parse(json);
                    JToken superTypeArray = superTypeObj.SelectToken("data");

                    CardTypes = new List<CardType>();
                    foreach (var superType in superTypeArray)
                    {
                        CardType cardType = new CardType();
                        cardType.Class = superType.ToObject<string>();
                        CardTypes.Add(cardType);                        
                    }
                    return CardTypes;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return null;
        }

        public async Task<List<string>> LoadPropertyAsync(string property)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var endPoint = $"https://api.pokemontcg.io/v2/{property}";
                    var response = await client.GetAsync(endPoint);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(response.ReasonPhrase);
                    }
                    string json = await response.Content.ReadAsStringAsync();
                    JObject typeObj = JObject.Parse(json);
                    JToken typeArray = typeObj.SelectToken("data");

                    List<string> types = new List<string>();
                    foreach (var type in typeArray)
                    {
                        string cardType = type.ToObject<string>();
                        types.Add(cardType);
                    }
                    return types;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return null;
        }

        private async Task<CardType> GetCardType(string cardClass)
        {
            await LoadCardTypesAsync();
            foreach (var cardType in CardTypes)
            {
                if (cardType.Class == cardClass) return cardType;
            }
            return null;
        }

        public async Task<List<BaseCard>> LoadCardsAsync(string query)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var endPoint = $"https://api.pokemontcg.io/v2/cards?{query}";
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
                        CardType cardType = await GetCardType(cardTypeClass);
                        var currentCard = Activator.CreateInstance(cardType.ActualType) as BaseCard;
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
                            var rules = card.SelectToken("rules");
                            if (rules != null)
                            {
                                energyCard.Rules = rules.ToObject<List<string>>();
                            }
                        }

                        var trainerCard = currentCard as TrainerCard;
                        if (trainerCard != null)
                        {
                            var rules = card.SelectToken("rules");
                            if (rules != null)
                            {
                                trainerCard.Rules = rules.ToObject<List<string>>();
                            }
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
