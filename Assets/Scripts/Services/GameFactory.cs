using System.Collections.Generic;
using arttmptt.Components;
using arttmptt.Data;
using arttmptt.UnityComponents;
using Leopotam.EcsLite;
using UnityEngine;

namespace arttmptt.Services
{
    public class GameFactory : IGameFactory
    {
        private readonly EcsWorld _world;
        private readonly UiRoot _uiRoot;
        private readonly SaveData _saveData;
        private readonly IStaticDataService _staticDataService;
        private EcsPool<UpdateBalanceViewEvent> _updateBalancePool;

        public GameFactory(EcsWorld world, UiRoot uiRootRoot, SaveData saveData,
            IStaticDataService staticDataService)
        {
            _world = world;
            _uiRoot = uiRootRoot;
            _saveData = saveData;
            _staticDataService = staticDataService;

            _updateBalancePool = _world.GetPool<UpdateBalanceViewEvent>();
        }

        public Currency Balance { get; set; }

        public void CreateBalance()
        {
            Balance = new Currency
            {
                Value = _saveData.Balance.Value
            };

            var balanceView = _uiRoot.Canvas.GetComponentInChildren<BalanceView>();
            balanceView.Label.text = _staticDataService.ForCanvas().BalanceLabel;
            _updateBalancePool.Add(_world.NewEntity());
        }

        public void CreateBusinessCards()
        {
            var businessCards = _saveData.BusinessCards;
            foreach (var businessCardFromSave in businessCards) { CreateBusinessCard(businessCardFromSave); }
        }

        private void CreateBusinessCard(BusinessCardData businessCardFromSave)
        {
            var entity = _world.NewEntity();
            ref var businessCard = ref _world.GetPool<BusinessCard>().Add(entity);

            var businessStaticData = _staticDataService.ForBusiness(businessCardFromSave.Id);

            businessCard.EntityId = entity;
            businessCard.Id = businessCardFromSave.Id;
            businessCard.Level = businessCardFromSave.Level;
            businessCard.Income = businessCardFromSave.Income;
            businessCard.LevelUpPrice = businessCardFromSave.LevelUpPrice;
            businessCard.PowerUps = new List<EcsPackedEntityWithWorld>();

            businessCard.IncomeDelay = businessStaticData.IncomeDelay;

            var businessCardObj = (GameObject)Object.Instantiate(Resources.Load(AssetPath.BusinessCard),
                _uiRoot.BusinessesContent);

            var businessCardView = businessCardObj.GetComponent<BusinessCardView>();

            businessCardView.Name.text = businessStaticData.Name;
            businessCardView.LevelUpPriceLabel.text = businessStaticData.LevelUpPriceLabel;
            businessCardView.IncomeLabel.text = businessStaticData.IncomeLabel;
            businessCardView.LevelLabel.text = businessStaticData.LevelLabel;

            businessCardView.Id = businessCard.Id;
            businessCardView.Level.text = businessCard.Level.ToString();
            businessCardView.Income.text = $"${businessCard.Income:#,#}";
            businessCardView.LevelUpPrice.text = $"${businessCard.LevelUpPrice:#,#}";

            CreatePowerUps(businessCardFromSave, businessCardView, ref businessCard);

            ref var incomeTimer = ref _world.GetPool<IncomeTimer>().Add(entity);
            incomeTimer.Timer = businessCard.IncomeDelay;
        }

        private void CreatePowerUps(BusinessCardData businessCardFromSave, BusinessCardView businessCardView,
            ref BusinessCard businessCard)
        {
            var powerUps = businessCardFromSave.PowerUps;

            foreach (var powerUpFromSave in powerUps)
            {
                CreatePowerUp(businessCardView, powerUpFromSave, ref businessCard);
            }
        }

        private void CreatePowerUp(BusinessCardView businessCardView, PowerUpData powerUpFromSave,
            ref BusinessCard businessCard)
        {
            var entity = _world.NewEntity();
            ref var powerUp = ref _world.GetPool<PowerUp>().Add(entity);

            var powerUpStaticData = _staticDataService.ForPowerUp(powerUpFromSave.Id);

            powerUp.Id = powerUpFromSave.Id;
            powerUp.BusinessId = powerUpFromSave.BusinessId;
            powerUp.Unlocked = powerUpFromSave.Unlocked;

            powerUp.Price = powerUpStaticData.Price;
            powerUp.IncomeMultiplierPercent = powerUpStaticData.IncomeMultiplierPercent;

            var powerUpObj = (GameObject)Object.Instantiate(Resources.Load(AssetPath.PowerUp),
                businessCardView.PowerUpRoot);

            var powerUpView = powerUpObj.GetComponent<PowerUpView>();

            powerUpView.Id = powerUpFromSave.Id;

            powerUpView.Name.text = powerUpStaticData.Name;
            powerUpView.IncomeLabel.text = powerUpStaticData.IncomeLabel;
            powerUpView.PriceLabel.text = powerUpStaticData.PriceLabel;
            powerUpView.BoughtLabel.text = powerUpStaticData.BoughtLabel;

            powerUpView.Income.text = $"+{powerUpStaticData.IncomeMultiplierPercent}%";
            powerUpView.Price.text = $"${powerUpStaticData.Price:#,#}";

            var packed = _world.PackEntityWithWorld(entity);

            businessCard.PowerUps.Add(packed);

            if (powerUp.Unlocked) { _world.GetPool<Unlocked>().Add(entity); }
        }
    }
}