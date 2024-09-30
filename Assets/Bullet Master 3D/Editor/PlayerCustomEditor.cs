using Bullet_Master_3D.Scripts.Game;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Bullet_Master_3D.Editor
{
    [CustomEditor(typeof(Player))]
    [CanEditMultipleObjects]
    public class PlayerCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = (Player) target;

            if (GUILayout.Button("Find all rigidbodies auto")) 
            {
                script.AllRigidbodies = script.gameObject.GetComponentsInChildren<Rigidbody>();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(script);
                EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
            }
        }
    }
}