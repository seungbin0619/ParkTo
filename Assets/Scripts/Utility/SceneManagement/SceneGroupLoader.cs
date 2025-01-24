#pragma warning disable IDE1006

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.SceneManagement {
    public class SceneGroupLoader : MonoBehaviour
    {
        private static readonly List<SceneGroupLoader> _sceneGroupLoaders = new();
        public static SceneGroupLoader current => _sceneGroupLoaders.FirstOrDefault();

        [SerializeField] bool loadDefaultGroupOnPlay = true;
        private readonly SceneGroupManager _manager = new();
        [SerializeField] SceneGroup[] sceneGroups;

        void Awake() {
            _manager.OnSceneLoaded += (name) => Debug.Log(name + " loaded");
            _manager.OnSceneUnloaded += (name) => Debug.Log(name + " unloaded");
        }

        void Start() {
            if(loadDefaultGroupOnPlay) {
                LoadSceneGroup(0);
            }

        }

        public async void LoadSceneGroup(int index) {
            await _manager.LoadSceneGroup(sceneGroups[index]);
        }
    }
}