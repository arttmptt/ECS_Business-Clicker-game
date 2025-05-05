using System;
using arttmptt.StaticData;

namespace arttmptt.Data
{
    [Serializable]
    public class PowerUpData
    {
        public PowerUpId Id;
        public BusinessTypeId BusinessId;
        public bool Unlocked;
    }

    public enum PowerUpId
    {
        PowerUp_1_1,
        PowerUp_1_2,

        PowerUp_2_1,
        PowerUp_2_2,

        PowerUp_3_1,
        PowerUp_3_2,

        PowerUp_4_1,
        PowerUp_4_2,

        PowerUp_5_1,
        PowerUp_5_2
    }
}