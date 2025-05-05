using System;
using System.Collections.Generic;

namespace arttmptt.Data
{
    [Serializable]
    public class SaveData
    {
        public Currency Balance;
        public List<BusinessCardData> BusinessCards;

        public SaveData()
        {
            Balance = new Currency();
            BusinessCards = new List<BusinessCardData>();
        }
    }
}