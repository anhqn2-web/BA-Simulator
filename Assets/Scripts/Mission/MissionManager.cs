using UnityEngine;

namespace BASimulator.Mission
{
    /// <summary>
    /// Tracks active missions and objectives.
    /// </summary>
    public class MissionManager : MonoBehaviour
    {
        public static MissionManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Completes an objective by index.
        /// </summary>
        public void CompleteObjective(int index)
        {
            Debug.Log($"[MissionManager] Objective {index} completed!");
            // Implementation logic for missions would go here
        }
    }
}
