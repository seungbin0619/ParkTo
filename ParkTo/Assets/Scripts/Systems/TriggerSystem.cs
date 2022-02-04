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

    #region [ 기타 변수, 오브젝트 ]

    [SerializeField]
    private TriggerBar bar;
    private RectTransform barRect;
    private UnityEngine.UI.ScrollRect scrollRect;

    public Sprite[] triggerSprites;

    public static bool trigBarHide;

    //private int triggerCount => triggers.Count;
    private List<Trigger> triggers;

    [SerializeField]
    private UnityEngine.Tilemaps.Tilemap triggerTile;

    [SerializeField]
    private Transform barContent;
    [SerializeField]
    private GameObject noTrigger;

    [SerializeField]
    private Trigger trigger;

    [SerializeField]
    private SpriteRenderer prevTrigger;

    [System.NonSerialized]
    public Trigger selectedTrigger;

    #endregion

    [System.NonSerialized]
    public bool triggerMode = false;
    private bool tileValid = false;


    public void ResetTriggers()
    {
        foreach (Transform child in barContent)
        {
            if (child.gameObject == noTrigger) continue;
            Destroy(child.gameObject);
        }
    }

    public void InitializeTriggers(int[] triggers)
    {
        this.triggers = new List<Trigger>();
        //this.triggers.AddRange(triggers);
        foreach (int trigger in triggers)
            AddTrigger(trigger);

        UpdateTriggerBar();
        TriggerBar.instance.IsHide = triggers.Length == 0;
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

        barRect.sizeDelta = new Vector2(Mathf.Clamp(cnt, 1, 3.5f) * 200 + 40, 240);
    }

    public void Select(Trigger trigger)
    {
        prevTrigger.gameObject.SetActive(true);
        selectedTrigger = trigger;

        prevTrigger.sprite = triggerSprites[selectedTrigger.index];

        triggerMode = false;

        //RemoveTrigger(trigger);

        Vars.instance.OnTriggerClick.Raise();
        Vars.instance.OnTriggerStateChange.Raise();
    }

    public void UseTrigger(Trigger trigger)
    {
        prevTrigger.gameObject.SetActive(false);

        triggers.Remove(trigger);
        Destroy(trigger.gameObject);

        selectedTrigger = null;
        TriggerBar.instance.IsHide = triggers.Count == 0;

        UpdateTriggerBar();
        Vars.instance.OnChanged.Raise();
    }

    private void Update()
    {
        TriggerFollow();
    }

    private void TriggerFollow()
    {
        if (selectedTrigger == null) return;

        if (Input.GetMouseButtonDown(1))
        {
            triggerMode = !triggerMode;
            Vars.instance.OnTriggerStateChange.Raise();
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); mousePosition.z = 0;
        Vector3Int tilePosition = triggerTile.WorldToCell(mousePosition);

        if(!prevTrigger.gameObject.activeSelf) prevTrigger.gameObject.SetActive(true);

        bool mb0Click = Input.GetMouseButtonDown(0);

        if (triggerMode)
        {
            #region [ 트리거 - 차 ]
            Car car = null;
            foreach(Car tmp in MapSystem.CurrentCars)
            {
                if (tilePosition != tmp.position) continue;

                car = tmp;
                break;
            }

            foreach (Car tmp in MapSystem.CurrentCars)
                tmp.PreviewTrigger(0.8f);

            if (car == null)
            {
                prevTrigger.transform.position = mousePosition;
                tileValid = false;
            }
            else
            {
                if (!mb0Click)
                {
                    // 효과 적용
                    prevTrigger.gameObject.SetActive(false);

                    car.PreviewTrigger(1f);
                }else
                {
                    car.SetTrigger(selectedTrigger);
                    UseTrigger(selectedTrigger);

                    Vars.instance.OnTriggerCancel.Raise();
                }
            }
            #endregion
        }
        else
        {
            #region [ 트리거 - 타일 ]
            if (!MapSystem.IsValidPosition(tilePosition))
            {
                prevTrigger.transform.position = mousePosition;
                tileValid = false;
            }
            else
            {
                if (!mb0Click)
                {
                    prevTrigger.transform.localPosition = tilePosition + new Vector3(0.5f, 0.5f, 0);
                    tileValid = MapSystem.CurrentTriggers[tilePosition.y, tilePosition.x] < 0;
                }else if(MapSystem.CurrentTriggers[tilePosition.y, tilePosition.x] < 0)
                {
                    MapSystem.instance.SetTrigger(tilePosition, selectedTrigger.index);
                    UseTrigger(selectedTrigger);

                    Vars.instance.OnTriggerCancel.Raise();
                }
            }
            #endregion
        }

        prevTrigger.color = tileValid ? Color.white : Color.red * 0.5f;
    }
}
