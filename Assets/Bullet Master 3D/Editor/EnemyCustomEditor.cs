using Bullet_Master_3D.Scripts.Game;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Bullet_Master_3D.Editor
{
    [CustomEditor(typeof(StandingEnemy))] [CanEditMultipleObjects]
    public class EnemyCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            var script = (StandingEnemy) target;

            if (GUILayout.Button("Find all rigidbodies auto")) 
            {
                script.AllRigidbodies = script.gameObject.GetComponentsInChildren<Rigidbody>();
            }

            if (GUI.changed) {
                EditorUtility.SetDirty(script);
                EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
            }
        }
    }
}