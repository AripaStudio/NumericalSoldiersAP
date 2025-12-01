using GameConfig;
using Google.FlatBuffers;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalSoldiersAP.Service
{
    public class GetGameConstantsClass
    {
        public static GameConstants GetGameConstants()
        {
            byte[] buffer = File.ReadAllBytes("game_config.bin");

            ByteBuffer bb = new ByteBuffer(buffer);

            return GameConstants.GetRootAsGameConstants(bb);
        }

      
    }
}
