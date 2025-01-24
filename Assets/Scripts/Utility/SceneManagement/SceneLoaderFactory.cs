using System.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Core.SceneManagement {
    public class SceneLoaderFactory {
        public SceneLoader CreateSceneLoader(SceneReference scene) {
            SceneLoader sceneLoader = null;
            switch(scene.State) {
                case SceneReferenceState.Regular:
                    sceneLoader = new RegularSceneLoader(scene);
                    break;
                case SceneReferenceState.Addressable: {
                    sceneLoader = new AddressablesSceneLoader(scene);
                    break;
                }
            }

            return sceneLoader;
        }
    }

    public abstract class SceneLoader { 
        public SceneReference Reference { get; private set; }
        public Scene Scene => SceneManager.GetSceneByPath(Reference.Path);
        public bool IsLoaded => Scene.isLoaded;
        
        public SceneLoader(SceneReference reference) { 
            Reference = reference; 
        }

        public abstract void Load();
        public abstract void Unload();
        public abstract bool IsDone();

    }
    public class RegularSceneLoader : SceneLoader {
        private AsyncOperation _operation = null;
        public RegularSceneLoader(SceneReference reference) : base(reference) { }

        public override void Load() => _operation = SceneManager.LoadSceneAsync(Reference.Path, LoadSceneMode.Additive);
        public override void Unload() => _operation = SceneManager.UnloadSceneAsync(Scene);
        public override bool IsDone() => _operation?.isDone ?? true;
    }

    public class AddressablesSceneLoader : SceneLoader {
        private AsyncOperationHandle<SceneInstance> _handle;
        public AddressablesSceneLoader(SceneReference reference) : base(reference) { }

        public override void Load() => _handle = Addressables.LoadSceneAsync(Reference.Path, LoadSceneMode.Additive);
        public override void Unload() {
            if(!_handle.IsValid()) return;
            _handle = Addressables.UnloadSceneAsync(_handle);
        }
        public override bool IsDone() => !_handle.IsValid() || _handle.IsDone;
    }
}