using System.Linq;

using FluidBehaviorTree.Editor.BehaviorTree.Printer.Containers;
using FluidBehaviorTree.Editor.BehaviorTree.Printer.Graphics;
using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.Tasks;

using UnityEngine;

namespace FluidBehaviorTree.Editor.BehaviorTree.Printer
{
    public class NodePrintController
    {
        private readonly VisualTask _node;
        private readonly IGraphBox _box;
        private readonly IGraphBox _divider;
        private readonly NodeFaders _faders = new NodeFaders();

        private readonly TextureLoader _iconMain;

        private Texture2D _dividerGraphic;
        private Texture2D _verticalBottom;
        private Texture2D _verticalTop;

        public NodePrintController(VisualTask node)
        {
            _node = node;
            _box = node.Box;
            _divider = node.Divider;
            _iconMain = new TextureLoader(_node.Task.IconPath);
        }

        private static GuiStyleCollection Styles => BehaviorTreePrinter.SharedStyles;

        public void Print(TaskStatus taskStatus)
        {
            if (!(_node.Task is TaskRoot))
            {
                PaintVerticalTop();
            }

            _faders.Update(taskStatus);

            PaintBody();

            if (_node.Children.Count > 0)
            {
                PaintDivider();
                PaintVerticalBottom();
            }
        }

        private void PaintBody()
        {
            var rect = new Rect(
                _box.GlobalPositionX + _box.PaddingX,
                _box.GlobalPositionY + _box.PaddingY,
                _box.Width - _box.PaddingX,
                _box.Height - _box.PaddingY);

            if (_node.Task.HasBeenActive)
            {
                Color prevBackgroundColor = GUI.backgroundColor;
                GUI.backgroundColor = _faders.BackgroundFader.CurrentColor;
                GUI.Box(rect, GUIContent.none, Styles.BoxActive.Style);
                GUI.backgroundColor = prevBackgroundColor;

                PrintLastStatus(rect);
            }
            else
            {
                GUI.Box(rect, GUIContent.none, Styles.BoxInactive.Style);
            }

            PrintIcon();

            Styles.Title.normal.textColor = _faders.TextFader.CurrentColor;
            GUI.Label(rect, _node.Task.Name, Styles.Title);
        }

        private void PrintLastStatus(Rect rect)
        {
            const float sidePadding = 1.5f;

            TextureLoader icon = BehaviorTreePrinter.StatusIcons.GetIcon(_node.Task.LastStatus);
            icon.Paint(
                new Rect(
                    rect.x + rect.size.x - icon.Texture.width - sidePadding,
                    rect.y + rect.size.y - icon.Texture.height - sidePadding,
                    icon.Texture.width, icon.Texture.height),
                new Color(1, 1, 1, 0.7f));
        }

        private void PrintIcon()
        {
            const float iconWidth = 35;
            const float iconHeight = 35;
            _iconMain.Paint(
                new Rect(
                    _box.GlobalPositionX + _box.PaddingX / 2 + _box.Width / 2 - iconWidth / 2 + _node.Task.IconPadding / 2,
                    _box.GlobalPositionY + _box.PaddingX / 2 + 3 + _node.Task.IconPadding / 2,
                    iconWidth - _node.Task.IconPadding,
                    iconHeight - _node.Task.IconPadding),
                _faders.MainIconFader.CurrentColor);
        }

        private void PaintDivider()
        {
            const int graphicSizeIncrease = 5;

            if (_dividerGraphic == null)
            {
                _dividerGraphic = CreateTexture(
                    (int) _divider.Width + graphicSizeIncrease,
                    1,
                    Color.black);
            }

            var position = new Rect(
                _divider.GlobalPositionX + _box.PaddingY / 2 + _node.DividerLeftOffset - 2,
                _divider.GlobalPositionY,
                _divider.Width + graphicSizeIncrease,
                10);

            GUI.Label(position, _dividerGraphic);
        }

        private void PaintVerticalBottom()
        {
            if (_verticalBottom == null)
            {
                _verticalBottom = CreateTexture(1, (int) _box.PaddingY, Color.black);
            }

            var position = new Rect(
                _box.GlobalPositionX + _node.Width / 2 + _box.PaddingX - 2,
                _box.GlobalPositionY + _node.Height + _box.PaddingY - 1,
                _node.Width,
                _box.PaddingY - 2);

            GUI.Label(position, _verticalBottom);
        }

        private void PaintVerticalTop()
        {
            if (_verticalTop == null)
            {
                _verticalTop = CreateTexture(1, Mathf.RoundToInt(_box.PaddingY / 2), Color.black);
            }

            var position = new Rect(
                _box.GlobalPositionX + _node.Width / 2 + _box.PaddingX - 2,
                _box.GlobalPositionY + 2,
                100,
                _box.PaddingY);

            GUI.Label(position, _verticalTop);
        }

        private static Texture2D CreateTexture(int width, int height, Color color)
        {
            var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.SetPixels(Enumerable.Repeat(color, width * height).ToArray());
            texture.Apply();

            return texture;
        }
    }
}
