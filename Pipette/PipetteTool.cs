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
        private Element selectedElement;
        private float mass = 0f;
        private float temperature = 0f;
        private const float MASS_STEP = 0.01f;
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

        private void Absorb(int cell)
        {
            UISounds.PlaySound(UISounds.Sound.ClickObject);
            if (this.selectedElement == null)
            {
                this.selectedElement = Grid.Element[cell];
                this.temperature = Mathf.Round(Grid.Temperature[cell] * 10f) / 10f;
            }
            else
            {
                this.temperature = Mathf.Round((Grid.Temperature[cell] + this.temperature) * 5f) / 10f;
            }
            this.mass += MASS_STEP;
            SimMessages.ConsumeMass(cell, this.selectedElement.id, MASS_STEP, 1);
        }

        private void Drip(int cell)
        {
            UISounds.PlaySound(UISounds.Sound.ClickObject);
            var sandBoxTool = CellEventLogger.Instance.SandBoxTool;
            var currentMass = 0f;
            if (Grid.IsLiquid(cell))
            {
                currentMass = Grid.Mass[cell];
            }
            SimMessages.ReplaceElement(cell, this.selectedElement.id, sandBoxTool, currentMass + MASS_STEP, this.temperature);
            this.mass -= Math.Min(this.mass, MASS_STEP);
            if (this.mass <= 0)
            {
                this.selectedElement = null;
                this.temperature = 0f;
            }
        }

        public override void OnLeftClickUp(Vector3 cursor_pos)
        {
            var cell = Grid.PosToCell(cursor_pos);
            if (Grid.IsValidCell(cell))
            {
                if (Grid.IsLiquid(cell) &&
                    (this.selectedElement == null || this.selectedElement.id == Grid.Element[cell].id))
                {
                    this.Absorb(cell);
                }
                else if (!Grid.IsSolidCell(cell) && this.selectedElement != null &&
                         (!Grid.IsLiquid(cell) || this.selectedElement.id == Grid.Element[cell].id))
                {
                    this.Drip(cell);
                }
                else
                {
                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative,
                        (string)PipetteConst.STRING_INVALID_LIQUID, (Transform)null, cursor_pos, force_spawn: true);
                }
            }
            else
            {
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative,
                    (string)UI.DEBUG_TOOLS.INVALID_LOCATION, (Transform)null, cursor_pos, force_spawn: true);
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