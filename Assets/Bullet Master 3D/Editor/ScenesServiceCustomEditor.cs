using Bullet_Master_3D.Scripts.Menu;
using UnityEditor;
using UnityEngine;

namespace Bullet_Master_3D.Editor
{
    [CustomEditor(typeof(ScenesService))][CanEditMultipleObjects]
    public class ScenesServiceCustomEditor : UnityEditor.Editor
    {
        private ScenesService _script;
        public void OnEnable()
        {
            _script = (ScenesService) target;
        }

        public void OnDisable()
        {
            _script.TestModeEnabled = false;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Play"))
            {
                _script.TestModeEnabled = true;
                EditorApplication.isPlaying = true;
            }
        }
    }
}