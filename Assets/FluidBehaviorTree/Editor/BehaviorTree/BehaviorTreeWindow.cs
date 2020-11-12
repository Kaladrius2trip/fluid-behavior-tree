using FluidBehaviorTree.Editor.BehaviorTree.Printer;
using FluidBehaviorTree.Runtime.BehaviorTree;

using UnityEditor;

using UnityEngine;

namespace FluidBehaviorTree.Editor.BehaviorTree
{
    public class BehaviorTreeWindow : EditorWindow
    {
        private string _name;
        private BehaviorTreePrinter _printer;

        private void Update()
        {
            if (Application.isPlaying)
            {
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                ClearView();
            }

            GUILayout.Label($"Behavior Tree: {_name}", EditorStyles.boldLabel);
            _printer?.Print(position.size);
        }

        public static void ShowTree(IBehaviorTree tree, string name)
        {
            var window = GetWindow<BehaviorTreeWindow>(false);
            window.titleContent = new GUIContent($"Behavior Tree: {name}");
            window.SetTree(tree, name);
        }

        private void SetTree(IBehaviorTree tree, string name)
        {
            _printer?.Unbind();
            _printer = new BehaviorTreePrinter(tree, position.size);
            _name = name;
        }

        private void ClearView()
        {
            _name = null;
            _printer = null;
        }
    }
}
