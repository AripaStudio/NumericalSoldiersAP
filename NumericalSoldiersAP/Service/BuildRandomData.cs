using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameConfig;
using Google.FlatBuffers;

namespace NumericalSoldiersAP.Service
{
    public class BuildRandomData
    {
        public static VectorOffset CreateRandomBuildingTypeVector(FlatBufferBuilder builder)
        {
            const int NUM_BUILDINGS = 4;
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
    }
}
