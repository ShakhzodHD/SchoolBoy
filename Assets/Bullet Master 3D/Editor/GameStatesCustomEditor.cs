using System.Linq;
using Bullet_Master_3D.Scripts;
using Bullet_Master_3D.Scripts.Game;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Bullet_Master_3D.Editor
{
    [CustomEditor(typeof(GameManager))]
    [CanEditMultipleObjects]
    public class GameStatesCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            var script = (GameManager) target;

            if (GUILayout.Button("Find all enemies and prisoners auto"))
            {
                script.AllEnemies = GameObject.FindGameObjectsWithTag(Constants.ENEMY_TAG).Select(gameObject => gameObject.GetComponent<Stickman>()).ToArray();
                script.Prisoners = GameObject.FindGameObjectsWithTag(Constants.PRISONER_TAG).Select(gameObject => gameObject.GetComponent<Stickman>()).ToArray();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(script);
                EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
            }
        }
    }
}