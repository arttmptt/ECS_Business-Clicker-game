using arttmptt.Components;
using arttmptt.Data;
using arttmptt.Services;
using arttmptt.Systems;
using arttmptt.UnityComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;

namespace arttmptt
{
    public class EcsStartup : MonoBehaviour
    {
        [SerializeField] private UiRoot uiRoot;

        private EcsWorld _world;
        private IEcsSystems _systems;

        private EcsUguiEmitter _uiEmitter;
        private IStaticDataService _staticDataService;
        private ISaveDataService _saveDataService;
        private SaveData _saveData;
        private IGameFactory _factory;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            InitServices();

            InitSystems();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }

            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }

        private void InitServices()
        {
            _uiEmitter = uiRoot.UiEmitter;
            _staticDataService = new StaticDataService();
            _staticDataService.Load();

            _saveDataService = new SaveDataService(_staticDataService);
            _saveData =
                _saveDataService.Load()
                ?? _saveDataService.NewSaveData();

            _factory = new GameFactory(_world, uiRoot, _saveData, _staticDataService);
        }

        private void InitSystems()
        {
            _systems
                .Add(new UiClickEventSystem(_world))
                .Add(new InitGameWorldSystem(_factory))
                .Add(new IncomeSystem(_world, _factory, _staticDataService))
                .Add(new BuyPowerUpSystem(_world, _factory, _staticDataService))
                .Add(new BuyLevelUpSystem(_world, _factory, _staticDataService))
                .Add(new UpdateBalanceViewSystem(_world, _factory, uiRoot))
                .Add(new UpdateBusinessCardViewSystem(_world, uiRoot))
                .Add(new UpdatePowerUpViewSystem(_world, uiRoot))
                .Add(new UpdateSliderSystem(_world, uiRoot))
                .Add(new SaveGameSystem(_world, _saveDataService, _factory))

                .DelHere<UpdateComponentViewEvent>()
                .DelHere<UpdateBalanceViewEvent>()
                .DelHere<BuyPowerUpEvent>()
                .DelHere<BuyLevelUpEvent>()
                .DelHere<SaveEvent>()

#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem())
#endif

                .InjectUgui(_uiEmitter)
                .Init();
        }
    }
}