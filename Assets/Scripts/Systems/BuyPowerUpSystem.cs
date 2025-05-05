using arttmptt.Components;
using arttmptt.Services;
using Leopotam.EcsLite;

namespace arttmptt.Systems
{
    public class BuyPowerUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly IGameFactory _factory;
        private readonly IStaticDataService _staticDataService;
        private EcsFilter _businessFilter;
        private EcsPool<BusinessCard> _businessPool;
        private EcsFilter _eventFilter;
        private EcsPool<PowerUp> _powerUpPool;
        private EcsPool<Unlocked> _unlockedPool;
        private EcsPool<UpdateComponentViewEvent> _updateComponentViewPool;

        public BuyPowerUpSystem(EcsWorld world, IGameFactory factory, IStaticDataService staticDataService)
        {
            _world = world;
            _factory = factory;
            _staticDataService = staticDataService;
        }

        public void Init(IEcsSystems systems)
        {
            _eventFilter = _world.Filter<PowerUp>().Inc<BuyPowerUpEvent>().End();
            _businessFilter = _world.Filter<BusinessCard>().End();
            _powerUpPool = _world.GetPool<PowerUp>();
            _businessPool = _world.GetPool<BusinessCard>();
            _updateComponentViewPool = _world.GetPool<UpdateComponentViewEvent>();
            _unlockedPool = _world.GetPool<Unlocked>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var index in _eventFilter)
            {
                ref var powerUp = ref _powerUpPool.Get(index);

                var balance = _factory.Balance;
                if (!balance.HasEnoughCurrency(powerUp.Price)) { return; }

                balance.SpendCurrency(powerUp.Price);

                powerUp.Unlocked = true;
                _unlockedPool.Add(index);

                PowerUpBusinessIncome(ref powerUp);
            }
        }

        private void PowerUpBusinessIncome(ref PowerUp powerUp)
        {
            foreach (var index in _businessFilter)
            {
                ref var businessCard = ref _businessPool.Get(index);
                if (powerUp.BusinessId == businessCard.Id)
                {
                    businessCard.Income = businessCard.CalculateBusinessIncome(_staticDataService);

                    _updateComponentViewPool.Add(businessCard.EntityId);
                }
            }
        }
    }
}