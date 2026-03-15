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
                    LogicPorts.Port.OutputPort(SmartReservoir.PORT_ID, new CellOffset(0, 1), (string)STRINGS.CUSTOMUI.UISIDESCREENS.INTERPLANETARYLAUNCHERADJUST.STORAGE, (string)STRINGS.CUSTOMUI.UISIDESCREENS.INTERPLANETARYLAUNCHERADJUST.ACTIVE, (string)STRINGS.CUSTOMUI.UISIDESCREENS.INTERPLANETARYLAUNCHERADJUST.INACTIVE)
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

/*
        [HarmonyPatch(typeof(RailGun))]
        [HarmonyPatch("LaunchProjectile")]
        public class RailGun_LaunchProjectile_Patch
        {
            static void Postfix(GameObject go)
            {
                InterplanetaryLauncherAutomation ila = go.AddOrGet<InterplanetaryLauncherAutomation>();
                if (ila != null)
                    ila.UpdateLogicCircuit((object) null);
            }
        }
        */
    }

}
