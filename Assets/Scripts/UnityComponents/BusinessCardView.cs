using arttmptt.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace arttmptt.UnityComponents
{
    public class BusinessCardView : MonoBehaviour
    {
        public BusinessTypeId Id;

        public TextMeshProUGUI Name;
        public TextMeshProUGUI LevelLabel;
        public TextMeshProUGUI Level;
        public TextMeshProUGUI IncomeLabel;
        public TextMeshProUGUI Income;
        public TextMeshProUGUI LevelUpPriceLabel;
        public TextMeshProUGUI LevelUpPrice;

        public Transform PowerUpRoot;
        public Transform LevelUpButton;
        public Slider Slider;
    }
}