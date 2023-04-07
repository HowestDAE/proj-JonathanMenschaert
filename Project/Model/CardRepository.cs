using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public abstract class CardRepository
    {
        public List<CardType> CardTypes { get; protected set; }

        protected abstract Task<List<CardType>> LoadCardTypesAsync();

        protected async Task<CardType> GetCardTypeAsync(string cardClass)
        {
            if (CardTypes == null)
            {
                await LoadCardTypesAsync();
            }
            foreach (var cardType in CardTypes)
            {
                if (cardType.Class == cardClass) return cardType;
            }
            return null;
        }

        protected async Task<BaseCard> PopulateCard(JToken card)
        {
            string cardTypeClass = card.SelectToken("supertype").ToObject<string>();
            CardType cardType = await GetCardTypeAsync(cardTypeClass);
            var currentCard = Activator.CreateInstance(cardType.ActualType) as BaseCard;
            currentCard.Id = card.SelectToken("id").ToObject<string>();
            currentCard.Name = card.SelectToken("name").ToObject<string>();
            currentCard.SuperType = cardTypeClass;

            var subTypeToken = card.SelectToken("subtypes");
            if (subTypeToken != null)
            {
                currentCard.SubTypes = subTypeToken.ToObject<List<string>>();
            }

            if (currentCard is IHasRules ruleCard)
            {
                var rules = card.SelectToken("rules");
                if (rules != null)
                {
                    ruleCard.Rules = rules.ToObject<List<string>>();
                }

            }

            if (currentCard is IHasType typeCard)
            {
                var typeToken = card.SelectToken("types");
                if (typeToken != null)
                {
                    typeCard.Types = typeToken.ToObject<List<string>>();
                }
            }

            if (currentCard is IHasAttacks attackCard)
            {
                var attacksToken = card.SelectToken("attacks");
                if (attacksToken != null)
                {
                    attackCard.Attacks = attacksToken.ToObject<List<Attack>>();
                }

                var abilityToken = card.SelectToken("abilities");
                if (abilityToken != null)
                {
                    attackCard.Abilities = abilityToken.ToObject<List<Ability>>();
                }
            }
            return currentCard;
        }
    }
}
