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
    public class CardApiRepository : CardRepository
    {

        public int TotalCards { get; private set; }

        protected override async Task<List<CardType>> LoadCardTypesAsync()
        {
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
                    TotalCards = cardsObj.SelectToken("totalCount").ToObject<int>();
                    List<BaseCard> cardList = new List<BaseCard>();
                    foreach(var card in cardArray)
                    {
                        BaseCard currentCard = await PopulateCard(card);
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
