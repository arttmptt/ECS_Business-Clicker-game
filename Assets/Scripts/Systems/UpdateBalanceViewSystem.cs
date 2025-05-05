using arttmptt.Components;
using arttmptt.Services;
using arttmptt.UnityComponents;
using Leopotam.EcsLite;

namespace arttmptt.Systems
{
    public class UpdateBalanceViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly IGameFactory _factory;
        private readonly UiRoot _uiRoot;
        private BalanceView _balanceView;
        private EcsFilter _componentEventFilter;
        private EcsFilter _balanceEventFilter;

        public UpdateBalanceViewSystem(EcsWorld world, IGameFactory factory, UiRoot uiRoot)
        {
            _world = world;
            _factory = factory;
            _uiRoot = uiRoot;
        }

        public void Init(IEcsSystems systems)
        {
            _componentEventFilter = _world.Filter<UpdateComponentViewEvent>().End();
            _balanceEventFilter = _world.Filter<UpdateBalanceViewEvent>().End();
            _balanceView = _uiRoot.Canvas.GetComponentInChildren<BalanceView>();
        }

        public void Run(IEcsSystems systems)
        {
            if (_balanceEventFilter.GetEntitiesCount() <= 0 && _componentEventFilter.GetEntitiesCount() <= 0)
            {
                return;
            }

            _balanceView.Value.text = $"${_factory.Balance.Value:#,#}";
        }
    }
}