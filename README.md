# Business-Clicker game on ECS

A prototype of the game with an emphasis on technical implementation using ECS architecture.
* ECS framework: [LeoEcsLite](https://github.com/Leopotam/ecslite).
* MonoBehaviour just for EcsStartup and UnityComponents which are still just components of data.
* Initialization of the game field by Factory Design Pattern.
* Sevices implemented with the Dependency inversion principle (DIP).
* Configs implemented using ScriptableObjects, in StaticData.
* No DI frameworks used, dependencies obtained through constructors and Init methods.
* Unity's [C# Style Guide](https://unity.com/resources/c-sharp-style-guide-unity-6).
