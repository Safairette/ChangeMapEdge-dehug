using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ChangeMapEdge
{
    public class ChangeMapEdge_Settings : ModSettings
    {
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref noBuildLimit, "noBuildLimit", 0, false);
            Scribe_Values.Look(ref noZoneLimit, "noZoneLimit", 0, false);
        }
        public static int noBuildLimit = 0;
        public static int noZoneLimit = 0;
    }
}
