using UnityEngine;

namespace BASimulator.Gameplay
{
    /// <summary>
    /// Manages player progression, level, and experience points.
    /// </summary>
    public class ExperienceManager : MonoBehaviour
    {
        public static ExperienceManager Instance { get; private set; }

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
    }
}
