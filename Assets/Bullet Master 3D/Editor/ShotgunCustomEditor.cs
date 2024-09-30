using Bullet_Master_3D.Scripts.Game;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Bullet_Master_3D.Editor
{
    [CustomEditor(typeof(Shotgun))]
    [CanEditMultipleObjects]
    public class ShotgunCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = (Shotgun) target;

            if (GUILayout.Button("Set position and rotation to weapon auto")) {
                script.transform.localPosition = script.DefaultLocalPosition;
                script.transform.localEulerAngles = script.DefaultLocalEulerAngles;
            }
            
            if (GUILayout.Button("Save weapon position and rotation auto")) {
                script.DefaultLocalPosition = script.transform.localPosition;
                script.DefaultLocalEulerAngles = script.transform.localEulerAngles;
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(script);
                EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
            }
        }
    }
}