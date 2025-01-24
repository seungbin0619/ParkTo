using Unity.VisualScripting;
using UnityEngine;

public class ProvisionalObject : MonoBehaviour
{
    void OnEnable() {
        Destroy(gameObject);
    }
}
