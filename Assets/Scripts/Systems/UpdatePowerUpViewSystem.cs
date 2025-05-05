using System.Collections.Generic;
using System.Linq;
using arttmptt.Components;
using arttmptt.Services;
using arttmptt.UnityComponents;
using Leopotam.EcsLite;

namespace arttmptt.Systems
{
    public class UpdatePowerUpViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly UiRoot _uiRoot;
        private EcsFilter _filter;
        private EcsPool<PowerUp> _powerUpPool;
        private List<PowerUpView> _powerUpViewList;

        private EcsPool<UpdateBalanceViewEvent> _updateBalancePool;

        public UpdatePowerUpViewSystem(EcsWorld world, UiRoot uiRoot)
        {
            _world = world;
            _uiRoot = uiRoot;
        }

        public void Init(IEcsSystems systems)
        {
            _filter = _world.Filter<PowerUp>().Inc<Unlocked>().End();

            _powerUpViewList = _uiRoot.Canvas.GetComponentsInChildren<PowerUpView>().ToList();
            _powerUpPool = _world.GetPool<PowerUp>();

            _updateBalancePool = _world.GetPool<UpdateBalanceViewEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var index in _filter)
            {
                ref var powerUp = ref _powerUpPool.Get(index);

                foreach (var powerUpView in _powerUpViewList)
                {
                    if (powerUpView.Id == powerUp.Id)
                    {
                        powerUpView.IncomeTransform.gameObject.SetActive(false);
                        powerUpView.PriceTransform.gameObject.SetActive(false);
                        powerUpView.BoughtLabel.gameObject.SetActive(true);
                        powerUpView.Button.interactable = false;
                    }
                }

                _updateBalancePool.Add(_world.NewEntity());
            }
        }
    }
}