using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(MoveSO))]
public class MoveSOEditor : Editor
{
    /*
    public override void OnInspectorGUI()
    {
        // Draw the default Inspector for MoveSO
        DrawDefaultInspector();

        // Get the references to Move from MoveSO
        MoveSO moveSO = (MoveSO)target;
        List<Move> moveDataList = moveSO.moveActions;

        // If the list is not null, show the properties of each element in the Inspector
        if (moveDataList != null)
        {
            EditorGUI.BeginChangeCheck();

            // Iterate over each element in the list
            for (int i = 0; i < moveDataList.Count; i++)
            {
                Move moveData = moveDataList[i];

                // If the element is null, skip this iteration
                if (moveData == null)
                {
                    continue;
                }

                ScriptableObject move = moveData.effect;

                // If the element is null, show a placeholder message
                if (move == null)
                {
                    EditorGUILayout.HelpBox("Element " + i + " is null", MessageType.Warning);
                    continue;
                }

                // Create a SerializedObject for the ScriptableObject in the MoveData
                SerializedObject serializedMove = new SerializedObject(move);

                // Get the SerializedProperty for Move's properties
                SerializedProperty prop = serializedMove.GetIterator();
                prop.NextVisible(true);

                // Draw the properties of Move
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.LabelField("Element " + i + ": " + moveData.name, EditorStyles.boldLabel);
                EditorGUILayout.ObjectField("Effect", move, typeof(ScriptableObject), false);
                EditorGUI.indentLevel++;
                while (prop.NextVisible(false))
                {
                    if (prop.name == "effectType")
                    {
                        continue;
                    }

                    EditorGUILayout.PropertyField(prop, true);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();

                // Apply changes to Move's SerializedObject
                if (EditorGUI.EndChangeCheck())
                {
                    serializedMove.ApplyModifiedProperties();
                }
            }

            // If there are no elements in the list, show a message
            if (moveDataList.Count == 0)
            {
                EditorGUILayout.HelpBox("Move list is empty", MessageType.Info);
            }
            // If the list is not empty, provide a button to add a new element
            else
            {
                EditorGUILayout.Space();
                if (GUILayout.Button("Add Move"))
                {
                    moveDataList.Add(new Move());
                }
            }
        }
    }

    */
}
