using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ImOverHere {
    [HarmonyPatch]
    public static class IncidentTargets_Patch {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Storyteller), nameof(Storyteller.AllIncidentTargets), MethodType.Getter)]
        public static void AllIncidentTargets(List<IIncidentTarget> __result) {
            var wealth = __result.OfType<Map>().ToDictionary(m => m, m => m.wealthWatcher.WealthTotal);
            float wealthCap = wealth.Values.Max() * 0.05f;

            __result.RemoveAll(target => 
                target is Map map && 
                wealth[map] < wealthCap && 
                !map.PlayerPawnsForStoryteller.Any());
        }
    }
}
