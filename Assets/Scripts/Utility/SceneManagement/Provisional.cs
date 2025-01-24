using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Additive 모드로 로드된 씬이 아닌 경우에만 화면에 표시되도록 하기 위한 컴포넌트
/// </summary>
public class Provisional : MonoBehaviour
{
    void OnEnable() {
        if(SceneManager.GetActiveScene() == gameObject.scene) {
            return;
        }

        Destroy(gameObject);
    }
}