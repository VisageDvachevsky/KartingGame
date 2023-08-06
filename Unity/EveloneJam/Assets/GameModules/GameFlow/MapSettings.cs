using UnityEngine;

namespace Project.GameFlow
{
    public class MapSettings : MonoBehaviour
    {
        [field: Min(1)]
        [field: SerializeField] public int LapCount { get; private set; } = 3;
        [field: Min(0)]
        [field: SerializeField] public int BotCount { get; private set; } = 4;
    }
}
