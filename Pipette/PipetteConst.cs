using PeterHan.PLib.Actions;
using UnityEngine;

namespace Pipette
{
    static class PipetteConst
    {
        public static LocString STRING_PIPETTE_NAME = "Pipette";
        public static LocString STRING_PIPETTE_TOOLTIP = "Absorb liquid {0}";
        public static LocString STRING_PIPETTE_TOOLTIP_TITLE = "Pipette";
        public static LocString STRING_PIPETTE_ACTION_DRAG = "DRAG";
        public static LocString STRING_PIPETTE_ACTION_BACK = "BACK";
        public static LocString STRING_PIPETTE_NOT_EMPTY = "Pipette Is Not Empty";
        public static LocString STRING_PIPETTE_EMPTY = "Pipette Is Empty";
        public static LocString STRING_INVALID_LIQUID = "Invalid Liquid";

        public static string PIPETTE_TOOL_NAME = "PipetteTool";
        public static string PIPETTE_ICON_NAME = "sample";  // game original icon
        public static PAction PIPETTE_ACTION;

        public static string PIPETTE_PATH_CONFIG_FOLDER;
        public static string PIPETTE_PATH_CONFIG_FILE;
        public static string PIPETTE_PATH_KEYCODES_FILE;
        
    }
}