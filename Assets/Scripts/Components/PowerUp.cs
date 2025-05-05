using arttmptt.Data;
using arttmptt.StaticData;

namespace arttmptt.Components
{
    public struct PowerUp
    {
        public PowerUpId Id;
        public BusinessTypeId BusinessId;
        public bool Unlocked;
        public int Price;
        public float IncomeMultiplierPercent;
    }
}