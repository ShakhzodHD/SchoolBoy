using System;
using UnityEngine;

namespace Bullet_Master_3D.Scripts.Editor
{
    [CreateAssetMenu(fileName = "Level Editor Settings", menuName = "Settings/Level Editor Settings", order = 0)]
    public class LevelEditorSettings : ScriptableObject
    {
        [Serializable]
        public class PrefabSettings
        {
            public GameObject Prefab;
            public Vector3 DefaultLocalPosition;
            public Vector3 DefaultLocalEulerAngles;
            public bool UsesDesignColor;
        }
        
        public Sprite Preview;
        public GameObject[] DefaultSceneObjectsPrefabs;
        
        [Header("DESIGNS")]
        public Sprite[] DesignsPreviews;
        public PrefabSettings[] DesignsPrefabs;
        public Material[] DesignsMaterials;
        
        [Header("ENVIRONMENT")]
        public PrefabSettings[] EnvironmentObjectsPrefabs;
    }
}