using arttmptt.Data;

namespace arttmptt.Services
{
    public interface IGameFactory
    {
        public Currency Balance { get; set; }
        public void CreateBalance();
        public void CreateBusinessCards();
    }
}