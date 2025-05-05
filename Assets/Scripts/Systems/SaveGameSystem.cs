using System.Collections.Generic;
using arttmptt.Components;
using arttmptt.Data;
using arttmptt.Services;
using Leopotam.EcsLite;

namespace arttmptt.Systems
{
    public class SaveGameSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly ISaveDataService _saveDataService;
        private readonly IGameFactory _factory;
        private EcsFilter _businessCardFilter;
        private EcsPool<BusinessCard> _businessCardPool;
        private EcsFilter _powerUpFilter;
        private EcsPool<PowerUp> _powerUpPool;
        private EcsFilter _saveEventFilter;

        public SaveGameSystem(EcsWorld world, ISaveDataService saveDataService, IGameFactory factory)
        {
            _world = world;
            _saveDataService = saveDataService;
            _factory = factory;
        }

        public void Init(IEcsSystems systems)
        {
            _saveEventFilter = _world.Filter<SaveEvent>().End();
            _businessCardFilter = _world.Filter<BusinessCard>().End();
            _powerUpFilter = _world.Filter<PowerUp>().End();

            _businessCardPool = _world.GetPool<BusinessCard>();
            _powerUpPool = _world.GetPool<PowerUp>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var saveEventIndex in _saveEventFilter)
            {
                var saveData = new SaveData();
                saveData.BusinessCards = GetBusinessCardDataList();
                saveData.Balance.Value = _factory.Balance.Value;

                _saveDataService.Save(saveData);
            }
        }

        private List<BusinessCardData> GetBusinessCardDataList()
        {
            var businessCardDataList = new List<BusinessCardData>();
            foreach (var businessCardIndex in _businessCardFilter)
            {
                ref var businessCard = ref _businessCardPool.Get(businessCardIndex);

                var businessCardData = new BusinessCardData();

                businessCardData.Id = businessCard.Id;
                businessCardData.Income = businessCard.Income;
                businessCardData.Level = businessCard.Level;
                businessCardData.LevelUpPrice = businessCard.LevelUpPrice;

                businessCardData.PowerUps = GetPowerUpDataList(businessCard);

                businessCardDataList.Add(businessCardData);
            }

            return businessCardDataList;
        }

        private List<PowerUpData> GetPowerUpDataList(BusinessCard businessCard)
        {
            var powerUpDataList = new List<PowerUpData>();
            foreach (var entityWithWorld in businessCard.PowerUps)
            {
                entityWithWorld.Unpack(out var world, out var packedPowerUp);
                ref var powerUp = ref _powerUpPool.Get(packedPowerUp);

                var powerUpData = new PowerUpData();
                powerUpData.Id = powerUp.Id;
                powerUpData.Unlocked = powerUp.Unlocked;
                powerUpData.BusinessId = powerUp.BusinessId;

                powerUpDataList.Add(powerUpData);
            }

            return powerUpDataList;
        }
    }
}