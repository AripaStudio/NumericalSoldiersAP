using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameConfig;
using Google.FlatBuffers;

namespace NumericalSoldiersAP.Service
{
    public class DataBuilder
    {
        public static void BuildGameData()
        {
            FlatBufferBuilder builder = new FlatBufferBuilder(1024);

            Offset<GameConstants> constantsOffset = GameConstants.CreateGameConstants(builder,
                soldier_health_factor: 0.1f,
                shooter_troop_factor: 0.2f,
                num_paths: 2,
                building_defense_multiplier: 2,
                max_capacity_player: 3000,
                min_capacity_palyer: 500, 
                max_capacity_enemy: 2000,
                min_capacity_enemy: 800,
                building_typesOffset:BuildRandomData.CreateRandomBuildingTypeVector(builder));

            builder.Finish(constantsOffset.Value);

            string outputPath = "game_config.bin";

            byte[] buffer = builder.SizedByteArray();

            File.WriteAllBytes(outputPath, buffer);


        }
    }
}
