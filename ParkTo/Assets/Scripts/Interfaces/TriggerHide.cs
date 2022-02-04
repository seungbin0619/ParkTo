using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHide : MonoBehaviour
{
    private Canvas canvas;

    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private UnityEngine.UI.Image hide;

    private Color color;

    private void Awake()
    {
        color = hide.color;
        canvas = GetComponent<Canvas>();
    }

    public void OnTriggerClick()
    {
        ChangeHide(true);
    }

    public void OnTriggerCancel()
    {
        ChangeHide(false);
    }

    public void OnTriggerStateChange()
    {
        canvas.sortingOrder = 10 * (TriggerSystem.instance.triggerMode ? 1 : -1);
    }

    private void ChangeHide(bool flag)
    {
        panel.SetActive(flag);
    }

    private void Update()
    {
        
    }
}
