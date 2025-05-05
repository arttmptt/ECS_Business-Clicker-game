using arttmptt.Data;

namespace arttmptt.Services
{
    public interface ISaveDataService
    {
        void Save(SaveData saveData);
        SaveData Load();
        SaveData NewSaveData();
    }
}