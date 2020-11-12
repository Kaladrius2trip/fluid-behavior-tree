using UnityEditor;

using UnityEngine;

namespace FluidBehaviorTree.Editor.BehaviorTree.Printer.Graphics
{
    public class TextureLoader
    {
        public TextureLoader(string spritePath)
        {
            Texture = AssetDatabase.LoadAssetAtPath<Texture2D>(
                spritePath.Replace("ROOT", AssetPath.BasePath));
        }

        public Texture2D Texture { get; }

        public void Paint(Rect rect, Color color)
        {
            Color oldColor = GUI.color;
            GUI.color = color;

            if (Texture == null)
            {
                return;
            }

            GUI.Label(rect, Texture);

            GUI.color = oldColor;
        }
    }
}
