using Newtonsoft.Json;
using Project.LIB;
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

        public static List<BaseCard> GetCard()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Project.WPF.Resources.DataFiles.cards.json";

            List<BaseCard> cards = null;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    cards = JsonConvert.DeserializeObject<List<BaseCard>>(json);
                }
            }
            return cards;
        }
    }
}
