using System;
using System.Collections.Generic;
using System.Linq;
using arttmptt.Components;
using arttmptt.UnityComponents;
using Leopotam.EcsLite;

namespace arttmptt.Systems
{
    public class UpdateSliderSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly UiRoot _uiRoot;
        private EcsFilter _businessCardFilter;
        private List<BusinessCardView> _businessCardViewList;
        private EcsPool<IncomeTimer> _incomeTimerPool;

        public UpdateSliderSystem(EcsWorld world, UiRoot uiRoot)
        {
            _world = world;
            _uiRoot = uiRoot;
        }

        public void Init(IEcsSystems systems)
        {
            _businessCardFilter = _world.Filter<BusinessCard>().Inc<IncomeTimer>().End();
            _incomeTimerPool = _world.GetPool<IncomeTimer>();
            _businessCardViewList = _uiRoot.Canvas.GetComponentsInChildren<BusinessCardView>().ToList();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var index in _businessCardFilter)
            {
                ref var incomeTimer = ref _incomeTimerPool.Get(index);
                ref var businessCard = ref _world.GetPool<BusinessCard>().Get(index);

                if (businessCard.Level <= 0) { continue; }

                foreach (var businessCardView in _businessCardViewList)
                {
                    if (businessCardView.Id == businessCard.Id)
                    {
                        businessCardView.Slider.value = CalculateSliderValue(incomeTimer, businessCard);
                    }
                }
            }
        }

        private float CalculateSliderValue(IncomeTimer incomeTimer, BusinessCard businessCard)
        {
            return Math.Abs(incomeTimer.Timer - businessCard.IncomeDelay) / businessCard.IncomeDelay;
        }
    }
}