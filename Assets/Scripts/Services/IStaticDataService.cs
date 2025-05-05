using System.Collections.Generic;
using arttmptt.Data;
using arttmptt.StaticData;

namespace arttmptt.Services
{
    public interface IStaticDataService
    {
        void Load();
        BusinessStaticData ForBusiness(BusinessTypeId businessCardId);
        CanvasStaticData ForCanvas();
        PowerUpStaticData ForPowerUp(PowerUpId powerUpId);
        List<PowerUpStaticData> ForPowerUps(BusinessTypeId businessTypeId);
    }
}