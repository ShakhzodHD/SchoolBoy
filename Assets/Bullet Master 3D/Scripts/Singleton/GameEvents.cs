using System;

namespace Bullet_Master_3D.Scripts.Singleton
{
    public class GameEvents
    {
        public Action OnLevelStart;
        public Action OnLevelLose;
        public Action OnLevelComplete;
        public Action OnLevelRestart;
        public Action<int> OnOpenLevelFromList;
    }
}