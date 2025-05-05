using arttmptt.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace arttmptt.UnityComponents
{
    public class PowerUpView : MonoBehaviour
    {
        public PowerUpId Id;
        public TextMeshProUGUI Name;

        public Transform IncomeTransform;
        public TextMeshProUGUI IncomeLabel;
        public TextMeshProUGUI Income;

        public Transform PriceTransform;
        public TextMeshProUGUI PriceLabel;
        public TextMeshProUGUI Price;

        public TextMeshProUGUI BoughtLabel;
        public Button Button;
    }
}