using System.Collections.Generic;
using System.Linq;
using arttmptt.Components;
using arttmptt.UnityComponents;
using Leopotam.EcsLite;

namespace arttmptt.Systems
{
    public class UpdateBusinessCardViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly UiRoot _uiRoot;
        private EcsPool<BusinessCard> _businessCardPool;
        private List<BusinessCardView> _businessCardViewList;
        private EcsFilter _filter;

        public UpdateBusinessCardViewSystem(EcsWorld world, UiRoot uiRoot)
        {
            _world = world;
            _uiRoot = uiRoot;
        }

        public void Init(IEcsSystems systems)
        {
            _filter = _world.Filter<BusinessCard>().Inc<UpdateComponentViewEvent>().End();
            _businessCardPool = _world.GetPool<BusinessCard>();
            _businessCardViewList = _uiRoot.Canvas.GetComponentsInChildren<BusinessCardView>().ToList();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var index in _filter)
            {
                ref var businessCard = ref _businessCardPool.Get(index);
                foreach (var businessCardView in _businessCardViewList)
                {
                    if (businessCardView.Id == businessCard.Id)
                    {
                        businessCardView.Income.text = $"${businessCard.Income:#,#}";
                        businessCardView.Level.text = businessCard.Level.ToString();
                        businessCardView.LevelUpPrice.text = $"${businessCard.LevelUpPrice:#,#}";
                    }
                }
            }
        }
    }
}