using UnityEngine;

namespace arttmptt.StaticData
{
    [CreateAssetMenu(fileName = "BusinessStaticData", menuName = "Static Data/Business")]
    public class BusinessStaticData : ScriptableObject
    {
        public BusinessTypeId Id;
        public string Name;

        public string LevelLabel = "Level";
        public string IncomeLabel = "Income";
        public string LevelUpPriceLabel = "Price";

        public int DefaultPrice;
        public int DefaultLevel;
        public int DefaultIncome;
        public int IncomeDelay;
    }

    public enum BusinessTypeId
    {
        Business1,
        Business2,
        Business3,
        Business4,
        Business5
    }
}