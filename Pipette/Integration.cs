using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using PeterHan.PLib.Actions;
using PeterHan.PLib.Core;
using PeterHan.PLib.Database;
using UnityEngine;

namespace Pipette
{
    public sealed class Integration : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            PUtil.InitLibrary();
            new PLocalization().Register();

            PipetteConst.PIPETTE_PATH_CONFIG_FOLDER = Path.Combine(path, "config");
            PipetteConst.PIPETTE_PATH_CONFIG_FILE =
                Path.Combine(PipetteConst.PIPETTE_PATH_CONFIG_FOLDER, "config.json");
            PipetteConst.PIPETTE_PATH_KEYCODES_FILE =
                Path.Combine(PipetteConst.PIPETTE_PATH_CONFIG_FILE, "keycodes.txt");

            PipetteConst.PIPETTE_ACTION = new PActionManager().CreateAction("Pipette.OpenTool", PipetteConst.STRING_PIPETTE_NAME, new PKeyBinding(KKeyCode.None, Modifier.None));

        }

        [HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
        public static class PlayerController_OnPrefabInit
        {
            public static void Postfix(PlayerController __instance)
            {
                var interfaceTools = new List<InterfaceTool>(__instance.tools);
                
                var pipetteTool = new GameObject(PipetteConst.PIPETTE_TOOL_NAME);
                pipetteTool.AddComponent<PipetteTool>();

                pipetteTool.transform.SetParent(__instance.gameObject.transform);
                pipetteTool.gameObject.SetActive(true);
                pipetteTool.gameObject.SetActive(false);

                interfaceTools.Add(pipetteTool.GetComponent<InterfaceTool>());


                __instance.tools = interfaceTools.ToArray();
            }
        }

        [HarmonyPatch(typeof(ToolMenu), "CreateBasicTools")]
        public static class ToolMenu_CreateBasicTools
        {
            public static void Prefix(ToolMenu __instance)
            {
                __instance.basicTools.Add(
                    ToolMenu.CreateToolCollection(
                        PipetteConst.STRING_PIPETTE_NAME,
                        PipetteConst.PIPETTE_ICON_NAME,
                        PipetteConst.PIPETTE_ACTION.GetKAction(),
                        PipetteConst.PIPETTE_TOOL_NAME,
                        string.Format(PipetteConst.STRING_PIPETTE_TOOLTIP, "{Hotkey}"), 
                        false
                    )
                );
            }
        }

        [HarmonyPatch(typeof(Game), "DestroyInstances")]
        public static class Game_DestroyInstances
        {
            public static void Postfix()
            {
                PipetteTool.DestroyInstance();
            }
        }
    }
}