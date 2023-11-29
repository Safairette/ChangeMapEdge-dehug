using System;
using Verse;
using UnityEngine;
using RimWorld;

namespace ChangeMapEdge
{
    public class ChangeMapEdge : Mod
    {
        public ChangeMapEdge(ModContentPack content) : base(content)
        {
            base.GetSettings<ChangeMapEdge_Settings>();
        }

        public override void DoSettingsWindowContents(Rect rect)
        {
            base.DoSettingsWindowContents(rect);
            Listing_Standard list = new Listing_Standard();
            list.Begin(rect);
            ChangeMapEdge_Settings.noBuildLimit = (int)list.SliderLabeled("No-build edge size " + ChangeMapEdge_Settings.noBuildLimit.ToString(), Mathf.Round(ChangeMapEdge_Settings.noBuildLimit), 0f, 20f, tooltip: "Restrict building to X tiles from edge. Game default: 10");
            ChangeMapEdge_Settings.noZoneLimit = (int)list.SliderLabeled("No-zone edge size " + ChangeMapEdge_Settings.noZoneLimit.ToString(), Mathf.Round(ChangeMapEdge_Settings.noZoneLimit), 0f, 20f, tooltip: "Restrict zones to X tiles from edge. Game default: 5");
            list.End();
        }
        public override string SettingsCategory()
        {
            return "Change map edge limit";
        }
    }
}
