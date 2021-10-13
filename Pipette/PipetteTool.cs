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
            this.selectedElement = Grid.Element[cell];
            this.temperature = Mathf.Round(Grid.Temperature[cell] * 10f) / 10f;
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
            this.selectedElement = null;
        }

        public override void OnLeftClickDown(Vector3 cursor_pos)
        {
            var cell = Grid.PosToCell(cursor_pos);
            if (Grid.IsValidCell(cell) && Grid.IsLiquid(cell))
            {
                if (this.selectedElement == null)
                {
                    this.Absorb(cell);
                    this.RadiusIndicatorColor = new Color(0.5f, 0.5f, 0.7f, 0.2f);
                }
                else
                {
                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative,
                        (string)PipetteConst.STRING_PIPETTE_NOT_EMPTY, (Transform)null, cursor_pos, force_spawn: true);
                }
            }
            else
            {
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative,
                    (string)PipetteConst.STRING_INVALID_LIQUID, (Transform)null, cursor_pos, force_spawn: true);
            }
        }

        public override void OnLeftClickUp(Vector3 cursor_pos)
        {
            var cell = Grid.PosToCell(cursor_pos);
            if (Grid.IsValidCell(cell) && !Grid.IsSolidCell(cell))
            {
                if (this.selectedElement != null)
                {
                    if (!Grid.IsLiquid(cell) || this.selectedElement.id == Grid.Element[cell].id)
                    {
                        this.Drip(cell);
                        this.RadiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);
                    }
                    else
                    {
                        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative,
                            (string)PipetteConst.STRING_INCOMPATIBLE_LIQUID, (Transform)null, cursor_pos, force_spawn: true);
                    }
                }
                else
                {
                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative,
                        (string)PipetteConst.STRING_PIPETTE_EMPTY, (Transform)null, cursor_pos, force_spawn: true);
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