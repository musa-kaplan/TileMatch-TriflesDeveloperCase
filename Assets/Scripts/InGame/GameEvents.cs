using System;
using MusaUtils.Templates.HyperCasual;

namespace InGame
{
    public static class GameEvents
    {
        public static event Action<BlockBehaviours> onBlockFlied;
        public static void BlockFlied(BlockBehaviours blockBehaviours) => onBlockFlied?.Invoke(blockBehaviours);
        
        public static event Action<BlockBehaviours> onBlockBlasted;
        public static void BlockBlasted(BlockBehaviours blockBehaviours) => onBlockBlasted?.Invoke(blockBehaviours);
        
        public static event Action onCheckLevelWin;
        public static void CheckLevelWin() => onCheckLevelWin?.Invoke();
        
        public static event Action<GameStates> onStateChanged;
        public static void StateChanged(GameStates state) => onStateChanged?.Invoke(state);
    }
}
