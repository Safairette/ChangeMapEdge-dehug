
using System;
using Verse;
using UnityEngine;
using RimWorld;
using HugsLib;
using HugsLib.Settings;

namespace ChangeMapEdge
{
    public class ChangeMapEdge : ModBase
    {

        public static ChangeMapEdge Instance { get; private set; }

        public ChangeMapEdge()
        {
            Instance = this;
        }
        public override string ModIdentifier
        {
            get { return "ChangeMapEdge"; }
        }
        private SettingHandle<int> noBuildLimit;
        private SettingHandle<int> noZoneLimit;
        public override void DefsLoaded()
        {
            noBuildLimit = Settings.GetHandle<int>("noBuildEdge", "No-build edge size", "Restrict building to X tiles from edge. Game default: 10", 0, Validators.IntRangeValidator(0, 20));
            noZoneLimit = Settings.GetHandle<int>("noZoneEdge", "No-zone edge size", "Restrict zones to X tiles from edge. Game default: 5", 0, Validators.IntRangeValidator(0, 20));
        }

        public int GetNoBuildLimit() => noBuildLimit.Value;
        public int GetNoZoneLimit() => noZoneLimit.Value;

    }
}
