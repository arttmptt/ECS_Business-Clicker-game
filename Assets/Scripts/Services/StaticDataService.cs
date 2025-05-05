using System.Collections.Generic;
using System.Linq;
using arttmptt.Data;
using arttmptt.StaticData;
using UnityEngine;

namespace arttmptt.Services
{
    public class StaticDataService : IStaticDataService
    {
        private const string BusinessesPath = "Static Data/Businesses";
        private const string CanvasPath = "Static Data/Canvas/CanvasStaticData";
        private const string PowerUpsPath = "Static Data/PowerUps";

        private Dictionary<BusinessTypeId, BusinessStaticData> _businesses;
        private CanvasStaticData _canvasStaticData;
        private Dictionary<PowerUpId, PowerUpStaticData> _powerUps;


        public void Load()
        {
            _businesses = Resources
                .LoadAll<BusinessStaticData>(BusinessesPath)
                .ToDictionary(x => x.Id, x => x);

            _powerUps = Resources
                .LoadAll<PowerUpStaticData>(PowerUpsPath)
                .ToDictionary(x => x.Id, x => x);

            _canvasStaticData = Resources.Load<CanvasStaticData>(CanvasPath);
        }

        public BusinessStaticData ForBusiness(BusinessTypeId businessCardId)
        {
            return _businesses.TryGetValue(businessCardId, out var staticData)
                ? staticData
                : null;
        }

        public CanvasStaticData ForCanvas()
        {
            return _canvasStaticData;
        }

        public PowerUpStaticData ForPowerUp(PowerUpId powerUpId)
        {
            return _powerUps.TryGetValue(powerUpId, out var staticData)
                ? staticData
                : null;
        }

        public List<PowerUpStaticData> ForPowerUps(BusinessTypeId businessTypeId)
        {
            var powerUpStaticDataList = new List<PowerUpStaticData>();
            foreach (var powerUpStaticData in _powerUps.Values)
            {
                if (powerUpStaticData.BusinessId == businessTypeId)
                {
                    powerUpStaticDataList.Add(powerUpStaticData);
                }
            }

            powerUpStaticDataList.Reverse();
            return powerUpStaticDataList;
        }
    }
}