using System.Collections.Generic;
using arttmptt.Components;
using arttmptt.Services;
using Leopotam.EcsLite;

public static class Extentions
{
    public static float CalculateBusinessIncome(this ref BusinessCard businessCard,
        IStaticDataService staticDataService)
    {
        var businessStaticData = staticDataService.ForBusiness(businessCard.Id);

        return businessCard.Level
               * businessStaticData.DefaultIncome
               * (1 + businessCard.GetUnlockedPowerUpsMultiplier());
    }

    private static float GetUnlockedPowerUpsMultiplier(this ref BusinessCard businessCard)
    {
        float powerUpsMultiplier = 0;

        var businessCardPowerUps = UnpackPowerUps(ref businessCard);

        foreach (var powerUp in businessCardPowerUps)
        {
            var pwrUp = powerUp;
            if (pwrUp.Unlocked) powerUpsMultiplier += pwrUp.GetIncomeMultiplier();
        }

        return powerUpsMultiplier;
    }

    private static List<PowerUp> UnpackPowerUps(ref BusinessCard businessCard)
    {
        var businessCardPowerUps = new List<PowerUp>();
        foreach (var packedEntity in businessCard.PowerUps)
        {
            packedEntity.Unpack(out var world, out var entity);
            ref var powerUp = ref world.GetPool<PowerUp>().Get(entity);
            businessCardPowerUps.Add(powerUp);
        }

        return businessCardPowerUps;
    }

    private static float GetIncomeMultiplier(this PowerUp powerUp)
        => 1f + powerUp.IncomeMultiplierPercent / 100f;
}