using UnityEngine;
public class LevelState : MonoBehaviour {
    private Level _level;

    public void Initialize(Level level) {
        _level = level;
    }

    public bool CheckFinish() {
        return _level.Cars.Find(car => !car.Variables.isStop) == null;
    }
}