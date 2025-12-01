using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GameConfig;

namespace NumericalSoldiersAP.Service
{
    public static class CurrentBuildings
    {
        public static Building[] buildings = new Building[4];
    }

    public struct CurrentTroopInPathStruct
    {
        public int currentAllocatedTroops;
        public int currentHealth;
    }
    public class BuildingManager
    {
        public bool SetSolders(int id, int solders)
        {
            var targetBuilding = CurrentBuildings.buildings.FirstOrDefault(b => b.id == id);
            if (targetBuilding == null) return false;
            if (id <= 0 || solders > targetBuilding.MaxCapacityBuilding || solders < targetBuilding.MinCapacityBuilding)
            {
                return false;
            }

            targetBuilding.AllocatedTroops = solders;
            return true;
        }

        public CurrentTroopInPathStruct GetTroopsPlayerOnBuilding(AttackEnemyPathEnum attackEnemyPath)
        {
            if (attackEnemyPath == AttackEnemyPathEnum.Path1)
            {
                var currentAllocatedTroops1 = CurrentBuildings.buildings[0].AllocatedTroops;
                var currentHealth1 = CurrentBuildings.buildings[0].CurrentHealth;

                var currentAllocatedTroops2 = CurrentBuildings.buildings[2].AllocatedTroops;
                var currentHealth2 = CurrentBuildings.buildings[2].CurrentHealth;

                CurrentTroopInPathStruct output = new CurrentTroopInPathStruct()
                {
                    currentAllocatedTroops = currentAllocatedTroops1 + currentAllocatedTroops2,
                    currentHealth = currentHealth1 + currentHealth2
                };
                return output;

            }
            else
            {
                var currentAllocatedTroops1 = CurrentBuildings.buildings[1].AllocatedTroops;
                var currentHealth1 = CurrentBuildings.buildings[1].CurrentHealth;

                var currentAllocatedTroops2 = CurrentBuildings.buildings[3].AllocatedTroops;
                var currentHealth2 = CurrentBuildings.buildings[3].CurrentHealth;

                CurrentTroopInPathStruct output = new CurrentTroopInPathStruct()
                {
                    currentAllocatedTroops = currentAllocatedTroops1 + currentAllocatedTroops2,
                    currentHealth = currentHealth1 + currentHealth2
                };
                return output;
            }
        }


}


    public class Building
    {
        public short id;

        public int MaxHealth;

        public int CurrentHealth;

        public int MaxCapacityBuilding;

        public int MinCapacityBuilding;

        public TypeOfCapability Capability;

        public int AllocatedTroops;

        public short PathId;

        public Building(BuildingType staticData, short pathId)
        {
            this.id = staticData.Id;
            this.MaxHealth = staticData.BaseHealth;
            this.CurrentHealth = staticData.BaseHealth;
            this.Capability = staticData.TypeOfCapability;
            this.MaxCapacityBuilding = staticData.MaxCapacityBuilding;
            this.MinCapacityBuilding = staticData.MinCapacityBuilding;

            if (Capability == TypeOfCapability.TypeSoldier)
            {
                this.MaxCapacityBuilding = 1000;
                this.MaxHealth = 200;
            }
            else 
            {
                this.MaxCapacityBuilding = 1200;
            }

            this.AllocatedTroops = 0;
            this.PathId = pathId;
        }

    }
}
