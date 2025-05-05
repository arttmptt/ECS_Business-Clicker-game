using System.Collections.Generic;
using arttmptt.Data;
using arttmptt.StaticData;
using UnityEngine;

namespace arttmptt.Services
{
    public class SaveDataService : ISaveDataService
    {
        private readonly IStaticDataService _staticDataService;

        private const string SaveDataKey = "SaveData";

        public SaveDataService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Save(SaveData saveData)
        {
            PlayerPrefs.SetString(SaveDataKey, JsonUtility.ToJson(saveData));
        }

        public SaveData Load()
        {
            return JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(SaveDataKey));
        }

        public SaveData NewSaveData()
        {
            var saveData = new SaveData
            {
                Balance =
                {
                    Value = 0
                },
                BusinessCards = new List<BusinessCardData>
                {
                    CreateDefaultBusinessCardData(BusinessTypeId.Business1),
                    CreateDefaultBusinessCardData(BusinessTypeId.Business2),
                    CreateDefaultBusinessCardData(BusinessTypeId.Business3),
                    CreateDefaultBusinessCardData(BusinessTypeId.Business4),
                    CreateDefaultBusinessCardData(BusinessTypeId.Business5)
                }
            };

            return saveData;
        }

        private BusinessCardData CreateDefaultBusinessCardData(BusinessTypeId id)
        {
            var businessStaticData = _staticDataService.ForBusiness(id);
            var businessId = businessStaticData.Id;
            var defaultLevel = businessStaticData.DefaultLevel;
            var defaultPrice = businessStaticData.DefaultPrice;
            var defaultIncome = businessStaticData.DefaultIncome;

            var powerUpDataList = CreateDefaultPowerUpData(id);

            return new BusinessCardData
            {
                Id = businessId,
                Level = defaultLevel,
                LevelUpPrice = defaultPrice,
                Income = defaultIncome,
                PowerUps = powerUpDataList
            };
        }

        private List<PowerUpData> CreateDefaultPowerUpData(BusinessTypeId id)
        {
            var powerUpStaticDataList = _staticDataService.ForPowerUps(id);
            var powerUpDataList = new List<PowerUpData>();
            foreach (var powerUpStaticData in powerUpStaticDataList)
            {
                var powerUpData = new PowerUpData
                {
                    Id = powerUpStaticData.Id,
                    BusinessId = powerUpStaticData.BusinessId,
                    Unlocked = false
                };
                powerUpDataList.Add(powerUpData);
            }

            powerUpDataList.Reverse();

            return powerUpDataList;
        }
    }
}