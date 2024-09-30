using Bullet_Master_3D.Scripts.Game;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Bullet_Master_3D.Editor
{
    [CustomEditor(typeof(MovableWall))]
    [CanEditMultipleObjects]
    public class MovableWallCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = (MovableWall) target;

            if (GUILayout.Button("Set start point"))
            {
                script.StartPoint = script.transform.position;
            }

            if (GUILayout.Button("Set end point"))
            {
                script.EndPoint = script.transform.position;
            }

            if (GUILayout.Button("Move to start point"))
            {
                script.transform.position = script.StartPoint;
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(script);
                EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
            }
        }
    }
}