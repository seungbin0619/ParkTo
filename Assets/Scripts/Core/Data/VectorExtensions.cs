using UnityEngine;

public static class VectorExtensionMethods {
    public static Vector3 XZY(this Vector3 v) {
        (v.y, v.z) = (v.z, v.y);
        return v;
    }

    public static Vector3Int XZY(this Vector3Int v) {
        (v.y, v.z) = (v.z, v.y);
        return v;
    }
}