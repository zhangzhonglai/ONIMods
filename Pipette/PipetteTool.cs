using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Pipette
{
    public class PipetteTool : InterfaceTool
    {
        protected Color RadiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);
        private int currentCell;
        public static InterfaceTool Instance { get; private set; }
        public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
        {
            colors = new HashSet<ToolMenu.CellColorData> {new ToolMenu.CellColorData(this.currentCell, this.RadiusIndicatorColor)};
        }

        public override void OnMouseMove(Vector3 cursorPos)
        {
            base.OnMouseMove(cursorPos);
            this.currentCell = Grid.PosToCell(cursorPos);
        }

        public override void OnLeftClickDown(Vector3 cursor_pos)
        {
            var cell = Grid.PosToCell(cursor_pos);
            if (Grid.IsValidCell(cell) && Grid.IsLiquid(cell))
                PipetteTool.Absorb(cell);
            else
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, (string)UI.DEBUG_TOOLS.INVALID_LOCATION, (Transform)null, cursor_pos, force_spawn: true); 
        }

        private static void Absorb(int cell)
        {
            UISounds.PlaySound(UISounds.Sound.ClickObject);
        }

        public override void OnLeftClickUp(Vector3 cursor_pos)
        {
            base.OnLeftClickUp(cursor_pos);
        }

        public override void OnRightClickDown(Vector3 cursor_pos, KButtonEvent e)
        {
            base.OnRightClickDown(cursor_pos, e);
        }

        public override void OnRightClickUp(Vector3 cursor_pos)
        {
            base.OnRightClickUp(cursor_pos);
        }

        public PipetteTool()
        {
            Instance = this;
        }

        public static void DestroyInstance()
        {
            Instance = null;
        }
    }
}