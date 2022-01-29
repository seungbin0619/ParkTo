using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSystem : MonoBehaviour
{
    #region [ 인스턴스 초기화 ]

    public static TriggerSystem instance;
    private void Awake()
    {
        if (instance == null) instance = this;

        barRect = bar.GetComponent<RectTransform>();
        scrollRect = bar.GetComponent<UnityEngine.UI.ScrollRect>();
    }

    #endregion

    [SerializeField]
    private TriggerBar bar;
    private RectTransform barRect;
    private UnityEngine.UI.ScrollRect scrollRect;

    public Sprite[] triggerSprites;

    public static bool trigBarHide;

    //private int triggerCount => triggers.Count;
    private List<Trigger> triggers;

    [SerializeField]
    private Transform barContent;
    [SerializeField]
    private GameObject noTrigger;

    [SerializeField]
    private Trigger trigger;

    public void InitializeTriggers(int[] triggers)
    {
        this.triggers = new List<Trigger>();
        //this.triggers.AddRange(triggers);
        foreach (int trigger in triggers)
            AddTrigger(trigger);

        UpdateTriggerBar();
    }

    public void AddTrigger(int trigger)
    {
        Trigger trig = Instantiate(this.trigger, barContent);
        trig.Initialize(trigger, scrollRect);
        
        triggers.Add(trig);

        UpdateTriggerBar();
    }

    private void UpdateTriggerBar()
    {
        int cnt = triggers.Count;
        noTrigger.SetActive(cnt == 0);

        barRect.sizeDelta = new Vector2(Mathf.Clamp(cnt, 1, 3.5f) * 200 + 80, 240);
    }
}
