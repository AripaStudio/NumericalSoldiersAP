using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameConfig;

namespace NumericalSoldiersAP.Service
{
    public enum GameStateEnum
    {
        Win,
        Lose
    }

    public struct WarStateStruct
    {
        public int ArmyPlayer;
        public int ArmyEnemy;
        public int HealthsBuilding;
    }
    public static class WarManager
    {
        public static GameStateEnum StartWar(WarStateStruct stateWar)
        {
            int currentEnemy = stateWar.ArmyEnemy;
            currentEnemy -= stateWar.ArmyPlayer;
            if (currentEnemy <= 0)
            {
                return GameStateEnum.Win;
            }
            else
            {
                int currentHealthBuilding = stateWar.HealthsBuilding * 2;
                currentEnemy -= currentHealthBuilding;
                if (currentHealthBuilding <= 0)
                {
                    return GameStateEnum.Win;
                }
                else
                {
                    return GameStateEnum.Lose;
                }
            }
        }
    }
}
