#pragma warning disable IDE1006

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.SceneManagement {
    public class SceneGroupLoader : MonoBehaviour
    {
        private static readonly List<SceneGroupLoader> _sceneGroupLoaders = new();
        public static SceneGroupLoader current => _sceneGroupLoaders.FirstOrDefault();

        private readonly SceneGroupManager _manager = new();
        [SerializeField] SceneGroup firstLoaded;
        [SerializeField] SceneGroup[] sceneGroups;
        private readonly HashSet<string> _activeScenes = new();

        void Awake() {
            _manager.OnSceneLoaded += (name) => _activeScenes.Add(name);
            _manager.OnSceneUnloaded += (name) => _activeScenes.Remove(name);
        }

        void Start() {
            if(firstLoaded) {
                LoadSceneGroup(firstLoaded);
            }
        }

        public async void LoadSceneGroup(SceneGroup sceneGroup) {
            await _manager.LoadSceneGroup(sceneGroup);
        }

        public async void LoadSceneGroup(int index) {
            await _manager.LoadSceneGroup(sceneGroups[index]);
        }

        public bool IsSceneActive(string sceneName) {
            return _activeScenes.Contains(sceneName);
        }
    }
}