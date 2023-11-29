using System;
using Verse;
using HarmonyLib;
using System.Reflection;
using RimWorld;
using UnityEngine;

namespace ChangeMapEdge
{
    [StaticConstructorOnStartup]
    static class ChangeMapEdgeHarmony
    {
        static ChangeMapEdgeHarmony()
        {
            Harmony harmony = new Harmony("rimworld.kapitanoczywisty.changemapedge");

            MethodInfo nobuild_targetmethod = AccessTools.Method(typeof(Verse.GenDraw), "DrawNoBuildEdgeLines");
            HarmonyMethod nobuild_prefixmethod = new HarmonyMethod(typeof(ChangeMapEdgeHarmony).GetMethod("DrawNoBuildEdgeLines_Prefix"));
            harmony.Patch(nobuild_targetmethod, nobuild_prefixmethod, null);

            MethodInfo nozone_targetmethod = AccessTools.Method(typeof(Verse.GenDraw), "DrawNoZoneEdgeLines");
            HarmonyMethod nozone_prefixmethod = new HarmonyMethod(typeof(ChangeMapEdgeHarmony).GetMethod("DrawNoZoneEdgeLines_Prefix"));
            harmony.Patch(nozone_targetmethod, nozone_prefixmethod, null);

            HarmonyMethod ignorefunction_prefixmethod = new HarmonyMethod(typeof(ChangeMapEdgeHarmony).GetMethod("IgnoreFunction"));

            MethodInfo innobuild_targetmethod = AccessTools.Method(typeof(Verse.GenGrid), "InNoBuildEdgeArea");
            HarmonyMethod innobuild_postfixmethod = new HarmonyMethod(typeof(ChangeMapEdgeHarmony).GetMethod("InNoBuildEdgeArea_Postfix"));
            harmony.Patch(innobuild_targetmethod, ignorefunction_prefixmethod, innobuild_postfixmethod);

            MethodInfo innozone_targetmethod = AccessTools.Method(typeof(Verse.GenGrid), "InNoZoneEdgeArea");
            HarmonyMethod innozone_postfixmethod = new HarmonyMethod(typeof(ChangeMapEdgeHarmony).GetMethod("InNoZoneEdgeArea_Postfix"));
            harmony.Patch(innozone_targetmethod, ignorefunction_prefixmethod, innozone_postfixmethod);

            MethodInfo cellrect_innobuild_targetmethod = AccessTools.Method(typeof(Verse.CellRect), "InNoBuildEdgeArea");
            HarmonyMethod cellrect_innobuild_postfixmethod = new HarmonyMethod(typeof(ChangeMapEdgeHarmony).GetMethod("CellRectInNoBuildEdgeArea_Postfix"));
            harmony.Patch(cellrect_innobuild_targetmethod, ignorefunction_prefixmethod, cellrect_innobuild_postfixmethod);
        }

        static Material LineMatMetaOverlay = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.MetaOverlay);

        private static void DrawMapEdgeLines(int edgeDist)
        {
            float y = AltitudeLayer.MetaOverlays.AltitudeFor();
            IntVec3 size = Find.CurrentMap.Size;
            Vector3 vector = new Vector3((float)edgeDist, y, (float)edgeDist);
            Vector3 vector2 = new Vector3((float)edgeDist, y, (float)(size.z - edgeDist));
            Vector3 vector3 = new Vector3((float)(size.x - edgeDist), y, (float)(size.z - edgeDist));
            Vector3 vector4 = new Vector3((float)(size.x - edgeDist), y, (float)edgeDist);
            GenDraw.DrawLineBetween(vector, vector2, ChangeMapEdgeHarmony.LineMatMetaOverlay, 0.2f);
            GenDraw.DrawLineBetween(vector2, vector3, ChangeMapEdgeHarmony.LineMatMetaOverlay, 0.2f);
            GenDraw.DrawLineBetween(vector3, vector4, ChangeMapEdgeHarmony.LineMatMetaOverlay, 0.2f);
            GenDraw.DrawLineBetween(vector4, vector, ChangeMapEdgeHarmony.LineMatMetaOverlay, 0.2f);
        }

        public static bool IgnoreFunction()
        {
            return false;
        }

        public static bool DrawNoBuildEdgeLines_Prefix()
        {
            int limit = ChangeMapEdge_Settings.noBuildLimit;
            if (limit > 0)
                ChangeMapEdgeHarmony.DrawMapEdgeLines(limit);
            return false;
        }
        public static bool DrawNoZoneEdgeLines_Prefix()
        {
            int limit = ChangeMapEdge_Settings.noZoneLimit;
            if (limit > 0)
                ChangeMapEdgeHarmony.DrawMapEdgeLines(limit);
            return false;
        }
        public static void InNoBuildEdgeArea_Postfix(this IntVec3 c, Map map, ref bool __result)
        {
            __result = c.CloseToEdge(map, ChangeMapEdge_Settings.noBuildLimit);
        }
        public static void InNoZoneEdgeArea_Postfix(this IntVec3 c, Map map, ref bool __result)
        {
            __result = c.CloseToEdge(map, ChangeMapEdge_Settings.noZoneLimit);
        }
        public static void CellRectInNoBuildEdgeArea_Postfix(Verse.CellRect __instance, Map map, ref bool __result)
        {
            int limit = ChangeMapEdge_Settings.noBuildLimit;
            __result = !__instance.IsEmpty && (__instance.minX < limit || __instance.minZ < limit || __instance.maxX >= map.Size.x - limit || __instance.maxZ >= map.Size.z - limit);
        }
    }
}