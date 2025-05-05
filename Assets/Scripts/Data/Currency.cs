using System;

namespace arttmptt.Data
{
    [Serializable]
    public class Currency
    {
        public float Value;

        public bool HasEnoughCurrency(float price)
        {
            return price <= Value;
        }

        public void SpendCurrency(float price)
        {
            Value -= price;
        }
    }
}