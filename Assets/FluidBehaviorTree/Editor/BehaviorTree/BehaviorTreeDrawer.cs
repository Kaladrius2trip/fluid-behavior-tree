using FluidBehaviorTree.Runtime.BehaviorTree;

using UnityEditor;

using UnityEngine;

namespace FluidBehaviorTree.Editor.BehaviorTree
{
    [CustomPropertyDrawer(typeof(Runtime.BehaviorTree.BehaviorTree))]
    public class BehaviorTreeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            GUI.enabled = Application.isPlaying;
            if (GUI.Button(position, "View Tree"))
            {
                var tree = fieldInfo.GetValue(property.serializedObject.targetObject) as IBehaviorTree;
                BehaviorTreeWindow.ShowTree(tree, tree.Name ?? property.displayName);
            }

            GUI.enabled = true;

            EditorGUI.EndProperty();
        }
    }
}
