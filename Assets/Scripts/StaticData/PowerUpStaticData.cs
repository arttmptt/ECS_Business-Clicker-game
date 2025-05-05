using arttmptt.Data;
using UnityEngine;

namespace arttmptt.StaticData
{
    [CreateAssetMenu(fileName = "PowerUpStaticData", menuName = "Static Data/Power Up")]
    public class PowerUpStaticData : ScriptableObject
    {
        public PowerUpId Id;
        public BusinessTypeId BusinessId;
        public string Name;

        public string IncomeLabel = "Income";
        public string PriceLabel = "Price";
        public string BoughtLabel = "Bought";

        public int IncomeMultiplierPercent;
        public int Price;
    }
}