using System.Collections;
using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;

namespace Core.SceneManagement {
    [CreateAssetMenu(fileName = "SceneGroup", menuName = "SceneGroup/New Group")]
    public class SceneGroup : ScriptableObject
    {
        public string Name;    
        public List<SceneReference> scenes;
    }
}