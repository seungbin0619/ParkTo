using UnityEngine;

/// <summary>
/// 레벨 도중 발생하는 여러 이펙트, 애니메이션 등을 정의한 클래스
/// </summary>
public class LevelView : MonoBehaviour {
    private LevelState _levelState;

    void Awake() {
        _levelState = GetComponent<LevelState>();
    }
}