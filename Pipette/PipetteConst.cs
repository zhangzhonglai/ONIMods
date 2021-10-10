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

        public static string PIPETTE_TOOL_NAME = "PipetteTool";
        public static string PIPETTE_ICON_NAME = "sample";  // game original icon
        public static PAction PIPETTE_ACTION;
        public static ToolMenu.ToolCollection PIPETTE_TOOL_COLLECTION;

        public static Color PIPETTE_COLOR_DRAG = new Color32(255, 140, 105, 255);

        public static string PIPETTE_PATH_CONFIG_FOLDER;
        public static string PIPETTE_PATH_CONFIG_FILE;
        public static string PIPETTE_PATH_KEYCODES_FILE;
        
    }
}