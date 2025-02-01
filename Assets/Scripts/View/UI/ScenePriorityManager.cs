#pragma warning disable IDE1006

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePriorityManager : MonoBehaviour {
    private static readonly List<ScenePriorityManager> _scenePriorityManagers = new();
    public static ScenePriorityManager current => _scenePriorityManagers.FirstOrDefault();

    public event Action OnScenePriorityChanged = delegate {};

    readonly Dictionary<string, int> _scenePriority = new();

    void OnEnable() {
        _scenePriorityManagers.Add(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        // OnScenePriorityChanged += () => {
        //     foreach(var k in _scenePriority) {
        //         Debug.Log(k.Key + " " + k.Value);
        //     }
        // };
    }

    void OnDisable() {
        _scenePriorityManagers.Remove(this);
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if(mode == LoadSceneMode.Single) {
            _scenePriority.Clear();
        }
        
        _scenePriority.Add(scene.name, 0);
    }

    private void OnSceneUnloaded(Scene scene) {
        _scenePriority.Remove(scene.name);

        OnScenePriorityChanged?.Invoke();
    }

    public void SetPriority(string sceneName, int priority) {
        if(!_scenePriority.ContainsKey(sceneName)) return;

        _scenePriority[sceneName] = priority;
        OnScenePriorityChanged?.Invoke();
    }

    public void SetHighestPriority(string sceneName) {
        SetPriority(sceneName, GetHighestPriority() + 1);
    }

    public void ResetPriority(string sceneName) {
        if(!_scenePriority.ContainsKey(sceneName)) return;

        _scenePriority[sceneName] = 0;
        OnScenePriorityChanged?.Invoke();
    }

    public bool IsHighestPriority(string sceneName) {
        if(!_scenePriority.ContainsKey(sceneName)) return false;

        return _scenePriority[sceneName] == GetHighestPriority();
    }

    public void ResetAllPriorities() {
        foreach(var sceneName in _scenePriority.Keys) {
            _scenePriority[sceneName] = 0;
        }

        OnScenePriorityChanged?.Invoke();
    }

    private int GetHighestPriority() {
        return _scenePriority.Values.Count == 0 ? 0 : _scenePriority.Values.Max();
    }
}