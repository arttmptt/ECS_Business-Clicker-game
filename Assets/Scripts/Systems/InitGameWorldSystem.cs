using arttmptt.Data;
using arttmptt.Services;
using arttmptt.UnityComponents;
using Leopotam.EcsLite;

namespace arttmptt.Systems
{
    public class InitGameWorldSystem : IEcsInitSystem
    {
        private readonly IGameFactory _factory;
        private readonly SaveData _saveData;
        private readonly IStaticDataService _staticDataService;
        private readonly UiRoot _uiRoot;

        public InitGameWorldSystem(IGameFactory factory)
        {
            _factory = factory;
        }

        public void Init(IEcsSystems systems)
        {
            _factory.CreateBalance();
            _factory.CreateBusinessCards();
        }
    }
}