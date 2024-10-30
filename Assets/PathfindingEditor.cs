using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Pathfinding))]
public class PathfindingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SerializedProperty obstacleLocationProperty = serializedObject.FindProperty("NewObstacleLocation");

        if (obstacleLocationProperty != null)
        {
            EditorGUILayout.PropertyField(obstacleLocationProperty);
            obstacleLocationProperty.serializedObject.ApplyModifiedProperties();
        }

        // Draw selection GUI in horizontal pattern
        using (new EditorGUILayout.HorizontalScope())
        {
            Pathfinding pathfinding = (Pathfinding)serializedObject.targetObject;
            if (GUILayout.Button("Place obstacle"))
            {
                pathfinding.AddObstacle(obstacleLocationProperty.vector2IntValue);
            }

            if (GUILayout.Button("Generate new grid"))
            {
                pathfinding.GenerateNewGrid();
            }
        }
    }
}
