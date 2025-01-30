using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SceneState", menuName = "Global/SceneState")]
    public class SceneState : ScriptableObject
    {
        public bool isFovEnabled = true;
    }
}