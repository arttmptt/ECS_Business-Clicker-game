using arttmptt.Components;
using arttmptt.UnityComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine.Scripting;

namespace arttmptt.Systems
{
    public class UiClickEventSystem : EcsUguiCallbackSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter _businessCardFilter;
        private readonly EcsPool<BusinessCard> _businessCardPool;
        private readonly EcsFilter _powerUpFilter;
        private readonly EcsPool<PowerUp> _powerUpPool;

        public UiClickEventSystem(EcsWorld world)
        {
            _world = world;

            _businessCardFilter = _world.Filter<BusinessCard>().End();
            _powerUpFilter = _world.Filter<PowerUp>().End();

            _businessCardPool = _world.GetPool<BusinessCard>();
            _powerUpPool = _world.GetPool<PowerUp>();
        }

        [Preserve]
        [EcsUguiClickEvent]
        private void OnAnyClick(in EcsUguiClickEvent e)
        {
            if (e.Sender.TryGetComponent(out LevelUpButtonView levelUpButtonView))
            {
                var businessCardView = levelUpButtonView.BusinessCardView;

                if (businessCardView == null) { return; }

                foreach (var index in _businessCardFilter)
                {
                    ref var entityBusinessCard = ref _businessCardPool.Get(index);
                    if (businessCardView.Id == entityBusinessCard.Id)
                    {
                        ref var buyLevelUpEvent = ref _world.GetPool<BuyLevelUpEvent>().Add(index);

                        buyLevelUpEvent.Id = businessCardView.Id;
                    }
                }
            }
            else if (e.Sender.TryGetComponent(out PowerUpView powerUpView))
            {
                foreach (var index in _powerUpFilter)
                {
                    ref var entityPowerUp = ref _powerUpPool.Get(index);

                    if (entityPowerUp.Unlocked) { continue; }

                    if (powerUpView.Id == entityPowerUp.Id)
                    {
                        ref var buyPowerUpEvent = ref _world.GetPool<BuyPowerUpEvent>().Add(index);
                        buyPowerUpEvent.Id = powerUpView.Id;
                    }
                }
            }
        }
    }
}