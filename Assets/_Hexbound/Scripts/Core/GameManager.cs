using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hexbound.Core
{
    /// <summary>
    /// Persistent singleton that owns the GameState and loads the matching scene.
    /// Lives in the Boot scene and survives every scene load after that.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public const string HubScene = "Hub";
        public const string RunScene = "Run";

        public static GameManager Instance { get; private set; }

        public GameState CurrentState { get; private set; } = GameState.Boot;

        /// <summary>Raised after the state changes. Payload is the new state.</summary>
        public event Action<GameState> StateChanged;

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

        private void Start()
        {
            // Boot's only job: hand off to the Hub.
            if (CurrentState == GameState.Boot)
                ChangeState(GameState.Hub);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        public void ChangeState(GameState newState)
        {
            if (newState == CurrentState)
                return;

            Debug.Log($"GameState: {CurrentState} → {newState}");
            CurrentState = newState;

            string scene = SceneFor(newState);
            if (scene != null)
                SceneManager.LoadScene(scene);

            StateChanged?.Invoke(newState);
        }

        private static string SceneFor(GameState state) => state switch
        {
            GameState.Hub => HubScene,
            GameState.Run => RunScene,
            _ => null // Results is an overlay inside Run for now; Boot is never re-entered.
        };
    }
}
