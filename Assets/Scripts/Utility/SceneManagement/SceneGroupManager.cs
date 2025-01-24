using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.SceneManagement {
    public class SceneGroupManager
    {
        public event Action<string> OnSceneLoaded = delegate {};
        public event Action<string> OnSceneUnloaded = delegate {};


        public SceneGroup ActiveSceneGroup { get; private set; } = null;
        private readonly SceneLoaderFactory _sceneLoaderFactory = new();
        private readonly List<SceneLoader> _sceneLoaders = new();
        

        public async Task LoadSceneGroup(SceneGroup sceneGroup) {
            if(ActiveSceneGroup == sceneGroup) return;
            await UnloadSceneGroup();

            ActiveSceneGroup = sceneGroup;
            
            foreach(var scene in sceneGroup.scenes) {
                var sceneLoader = _sceneLoaderFactory.CreateSceneLoader(scene);
                sceneLoader.Load();

                _sceneLoaders.Add(sceneLoader);
            }
            
            while(!_sceneLoaders.All(o => o.IsDone()))
                await Task.Delay(100);

            // set active scene
            SceneManager.SetActiveScene(_sceneLoaders[0].Scene);
            OnSceneLoaded.Invoke(sceneGroup.Name);
        }

        private async Task UnloadSceneGroup() {
            if(!ActiveSceneGroup) return;
            
            foreach(var sceneLoader in _sceneLoaders) {
                if(!sceneLoader.Scene.isLoaded) continue;
                sceneLoader.Unload();
            }

            while(!_sceneLoaders.All(o => o.IsDone()))
                await Task.Delay(100);
            _sceneLoaders.Clear();

            OnSceneUnloaded.Invoke(ActiveSceneGroup.Name);
            ActiveSceneGroup = null;
        }
    }
}