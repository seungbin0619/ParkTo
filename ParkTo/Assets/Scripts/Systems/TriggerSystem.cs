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
    private RectTransform barContent;

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

    public Sprite banTrigger;

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
        //scrollRect.movementType = UnityEngine.UI.ScrollRect.MovementType.
    }

    public void AddTrigger(int trigger, int index = -1)
    {
        Trigger trig = Instantiate(this.trigger, barContent);
        trig.Initialize(trigger, scrollRect);

        if (index == -1 || triggers.Count < index)
            triggers.Add(trig);
        else
        {
            triggers.Insert(index, trig);
            trig.transform.SetSiblingIndex(index);
        }

        UpdateTriggerBar();
    }

    private void UpdateTriggerBar()
    {
        int cnt = triggers.Count;
        noTrigger.SetActive(cnt == 0);

        bar.targetSizeDelta = new Vector2(Mathf.Clamp(cnt, 1, 3.5f) * 200 + 40, 240);
    }

    public void OnLevelLoaded()
    {
        IEnumerator Co()
        {
            yield return new WaitForEndOfFrame();
            barContent.anchoredPosition = new Vector2(barContent.sizeDelta.x * 0.5f, 0);
        }
        StartCoroutine(Co());
    }

    public void Select(Trigger trigger)
    {
        if (MapSystem.isGameOver) return;

        prevTrigger.gameObject.SetActive(true);
        selectedTrigger = trigger;

        prevTrigger.sprite = triggerSprites[selectedTrigger.index];

        triggerMode = false;

        //RemoveTrigger(trigger);

        Vars.instance.OnTriggerClick.Raise();
        Vars.instance.OnTriggerStateChange.Raise();
    }

    public void UnSelect()
    {
        prevTrigger.gameObject.SetActive(false);
        selectedTrigger = null;

        Vars.instance.OnTriggerCancel.Raise();
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

                if(mb0Click) SFXSystem.instance.PlaySound(8);
            }
            else
            {
                if (!mb0Click)
                {
                    prevTrigger.gameObject.SetActive(false);

                    car.PreviewTrigger(1f);
                }
                else
                {
                    // 효과 적용
                    car.SetTrigger(selectedTrigger);

                    //
                    MapSystem.instance.AddBehavior(
                        MapSystem.Behavior.BehaviorType.STATE, 
                        selectedTrigger.index, 
                        selectedTrigger.transform.GetSiblingIndex(),
                        car);

                    UseTrigger(selectedTrigger);
                    SFXSystem.instance.PlaySound(7);

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
                    tileValid = MapSystem.CurrentTriggers[tilePosition.y, tilePosition.x] == MapSystem.TRIGGER.NORMAL;
                }
                else if (MapSystem.CurrentTriggers[tilePosition.y, tilePosition.x] == MapSystem.TRIGGER.NORMAL)
                {
                    MapSystem.instance.SetTrigger(tilePosition, selectedTrigger.index);

                    //
                    MapSystem.instance.AddBehavior(
                        MapSystem.Behavior.BehaviorType.TRIGGER,
                        selectedTrigger.index,
                        selectedTrigger.transform.GetSiblingIndex(),
                        tilePosition);

                    UseTrigger(selectedTrigger);
                    SFXSystem.instance.PlaySound(7);

                    Vars.instance.OnTriggerCancel.Raise();
                }
                else SFXSystem.instance.PlaySound(8);
            }
            #endregion
        }

        prevTrigger.color = tileValid ? Color.white : Color.red * 0.5f;
    }
}
