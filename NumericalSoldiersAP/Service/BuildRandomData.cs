using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameConfig;
using Google.FlatBuffers;

namespace NumericalSoldiersAP.Service
{
    
    public struct NumberSoldiersStruct
    {
        public int PlayerArmy;
        public int EnemyArmy;
    }

    public enum AttackEnemyPathEnum
    {
        Path1, Path2
    }
    public class BuildRandomData
    {
        public const int NUM_BUILDINGS  = 4;
        public static VectorOffset CreateRandomBuildingTypeVector(FlatBufferBuilder builder)
        {
            
            Offset<BuildingType>[] buildingOffsets = new Offset<BuildingType>[NUM_BUILDINGS];

            Random random = new Random();

            for (short i = 0; i < NUM_BUILDINGS; i++)
            {
                TypeOfCapability capability = (TypeOfCapability)random.Next(0, 2);

                Offset<BuildingType> buildingOffset = BuildingType.CreateBuildingType(builder,
                    id: i,
                    type_of_capability: capability);

                buildingOffsets[i] = buildingOffset;
            }

            return builder.CreateVectorOfTables(buildingOffsets);
        }


        public static NumberSoldiersStruct CreateRandomNumberSoldiers()
        {
            var gameConstants = GetGameConstantsClass.GetGameConstants();
            var min_player = gameConstants.MinCapacityPalyer;
            var max_player = gameConstants.MaxCapacityPlayer;
            var min_enemy = gameConstants.MinCapacityEnemy;
            var max_enemy = gameConstants.MaxCapacityEnemy;

            Random random = new Random();
            NumberSoldiersStruct numberSoldiers = new NumberSoldiersStruct();
            numberSoldiers.EnemyArmy = random.Next(min_enemy, max_enemy);
            numberSoldiers.PlayerArmy = random.Next(min_player, max_player);
            return numberSoldiers;
        }

        public static AttackEnemyPathEnum CreateRandomAttackEnemyPath()
        {
            Random random = new Random();
            return (AttackEnemyPathEnum)random.Next(0, 2);
        }
    }
}
