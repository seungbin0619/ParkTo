using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingSound : MonoBehaviour
{
    [SerializeField]
    private float weight = 1;

    private AudioSource sound;
    private void Awake() { sound = GetComponent<AudioSource>(); }

    void Start() { OnSoundChanged(); }

    public void OnSoundChanged() { sound.volume = weight * DataSystem.GetData("Setting", "Sound", 50) * 0.01f; }
}
