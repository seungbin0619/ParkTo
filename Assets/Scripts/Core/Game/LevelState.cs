using System.Collections;
using UnityEngine;
public class LevelState : MonoBehaviour {
    private Level _level;

    public void Initialize(Level level) {
        _level = level;
    }

    public IEnumerator Play() {
        while(CheckFinish()) {
            foreach(Car car in _level.Cars) {
                car.Move();
            }

            yield return new WaitForSeconds(1f);
        }


        yield return null;
    }

    public bool CheckFinish() {
        return _level.Cars.Find(car => !car.Variables.isStop) == null;
    }
}