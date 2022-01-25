using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Pipette
{
    public class PipetteTool : InterfaceTool
    {
        protected Color RadiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);
        private int coloredCell;
        private int selectedCell;
        private Element selectedElement;
        private float mass;
        private float temperature;
        public static InterfaceTool Instance { get; private set; }
        public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
        {
            colors = new HashSet<ToolMenu.CellColorData> {new ToolMenu.CellColorData(coloredCell, RadiusIndicatorColor)};
        }

        public override void OnMouseMove(Vector3 cursorPos)
        {
            base.OnMouseMove(cursorPos);
            coloredCell = Grid.PosToCell(cursorPos);
        }

        private void Absorb(int cell)
        {
            UISounds.PlaySound(UISounds.Sound.ClickObject);
            selectedElement = Grid.Element[cell];
            selectedCell = cell;
            mass = Math.Min(Grid.Mass[cell], Integration.Settings.Capacity);
            temperature = Mathf.Round(Grid.Temperature[cell] * 10f) / 10f;
            SimMessages.ConsumeMass(cell, selectedElement.id, mass, 1);
        }

        private void Drip(int cell)
        {
            UISounds.PlaySound(UISounds.Sound.ClickObject);
            var sandBoxTool = CellEventLogger.Instance.SandBoxTool;
            if (Grid.IsLiquid(cell))
            {
                if (selectedCell != cell)
                {
                    mass += Grid.Mass[cell];
                }
                else
                {
                    mass = Grid.Mass[cell];
                }
            }
            SimMessages.ReplaceElement(cell, selectedElement.id, sandBoxTool, mass, temperature);
            selectedElement = null;
            mass = 0f;
        }

        public override void OnLeftClickDown(Vector3 cursor_pos)
        {
            var cell = Grid.PosToCell(cursor_pos);
            if (Grid.IsValidCell(cell) && Grid.IsLiquid(cell))
            {
                if (selectedElement == null)
                {
                    Absorb(cell);
                    RadiusIndicatorColor = new Color(0.5f, 0.5f, 0.7f, 0.2f);
                }
                else
                {
                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative,
                        PipetteConst.STRING_PIPETTE_NOT_EMPTY, null, cursor_pos, force_spawn: true);
                }
            }
            else
            {
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative,
                    PipetteConst.STRING_INVALID_LIQUID, null, cursor_pos, force_spawn: true);
            }
        }

        public override void OnLeftClickUp(Vector3 cursor_pos)
        {
            var cell = Grid.PosToCell(cursor_pos);
            if (Grid.IsValidCell(cell) && !Grid.IsSolidCell(cell))
            {
                if (selectedElement != null)
                {
                    if (!Grid.IsLiquid(cell) || selectedElement.id == Grid.Element[cell].id)
                    {
                        Drip(cell);
                        RadiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);
                    }
                    else
                    {
                        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative,
                            PipetteConst.STRING_INCOMPATIBLE_LIQUID, null, cursor_pos, force_spawn: true);
                    }
                }
                else
                {
                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative,
                        PipetteConst.STRING_PIPETTE_EMPTY, null, cursor_pos, force_spawn: true);
                }
            }
            else
            {
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative,
                    UI.DEBUG_TOOLS.INVALID_LOCATION, null, cursor_pos, force_spawn: true);
            }
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