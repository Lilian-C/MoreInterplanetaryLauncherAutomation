using HarmonyLib;
using Database;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using System.Reflection;
using InterPlanetaryLauncherAutomationMod;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using InterPlanetaryLauncherAutomation;

namespace InterPlanetaryLauncherAutomation
{
    public class RailGun_Patches
    {
        [HarmonyPatch(typeof(RailGunConfig))]
        [HarmonyPatch("CreateBuildingDef")]
        public class RailGunConfig_CreateBuildingDef_Patch
        {
            public static void Postfix(ref BuildingDef __result)
            {
                __result.LogicOutputPorts.Add(
                    LogicPorts.Port.OutputPort(SmartReservoir.PORT_ID, new CellOffset(0, 1),  (string) STRINGS.LOGIC.INTERPLANETARYLAUNCHERADJUST.TITLE, (string) STRINGS.LOGIC.INTERPLANETARYLAUNCHERADJUST.ACTIVE, (string) STRINGS.LOGIC.INTERPLANETARYLAUNCHERADJUST.INACTIVE)
                );
                __result.LogicOutputPorts.Add(
                    LogicPorts.Port.OutputPort(InterplanetaryLauncherAutomation.RADBOLTS_EMPTY_PORT_ID, new CellOffset(1, 1), (string) STRINGS.LOGIC.INTERPLANETARYLAUNCHERADJUST.RADBOLTS_EMPTY_TITLE, (string) STRINGS.LOGIC.INTERPLANETARYLAUNCHERADJUST.RADBOLTS_EMPTY_ACTIVE, (string) STRINGS.LOGIC.INTERPLANETARYLAUNCHERADJUST.RADBOLTS_EMPTY_INACTIVE)
                );
            }
        }

        [HarmonyPatch(typeof(RailGunConfig))]
        [HarmonyPatch("ConfigureBuildingTemplate")]
        public class RailGunConfig_ConfigureBuildingTemplate_Patch
        {
            static void Postfix(GameObject go, Tag prefab_tag)
            {
                go.AddOrGet<InterplanetaryLauncherAutomation>();
            }
        }
    }
}
