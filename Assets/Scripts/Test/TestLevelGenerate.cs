using UnityEngine;

public class TestLevelGenerate : MonoBehaviour
{
    public LevelState levelState;
    public LevelPack pack;
    void Start() {
        levelState.Initialize(pack, 0);
    }
}
