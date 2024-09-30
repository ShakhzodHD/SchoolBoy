using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bullet_Master_3D.Scripts.Game
{
    [CreateAssetMenu(fileName = "LevelsSettings", menuName = "Settings/Level Settings", order = 0)]
    public class LevelsSettings : ScriptableObject
    {
        [Serializable]
        public class Level
        {
            [Serializable]
            public class WeaponSettings
            {
                public WeaponType WeaponType;
                [Range(1, Constants.MAXIMUM_CARTRIDGES)] public int CartridgesCount = 1;
            }

            [Header("Weapons that will be available at the level; spawn weapons starts at the end of the list")]
            [Header("WEAPONS")] 
            public List<WeaponSettings> Weapons;
            
            [Header("Star distribution that the player gets according to how many bullets player spent")]
            [Header("STARS")]
            [Range(1, Constants.MAXIMUM_CARTRIDGES)] public int CartridgesUsedForThreeStars;
            [Range(1, Constants.MAXIMUM_CARTRIDGES)] public int CartridgesUsedForTwoStars;
            [Range(1, Constants.MAXIMUM_CARTRIDGES)] public int CartridgesUsedForOneStar;
        }
        
        public List<Level> Levels = new List<Level>();
    }
}