using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class LoadSelect : MonoBehaviour
{
    #region [ 오브젝트 ]
    [SerializeField]
    private Tilemap tile;
    [SerializeField]
    private Tilemap line;

    private int index;
    private ThemeBase theme;

    private List<Button> buttons;
    private List<Transform> lines;

    [SerializeField]
    private Image car;

    [SerializeField]
    private RectTransform panel;

    [SerializeField]
    private TMPro.TMP_Text text;

    #endregion

    private int genIndex;
    private int levelIndex;
    //private int page;

    private Vector3 targetPosition;

    [SerializeField]
    private Button[] moveTheme;

    public static int tmpIndex = -1;

    private void Awake()
    {
        #region [ 길 오브젝트 ]
        buttons = new List<Button>();
        lines = new List<Transform>();

        for(int i = 0; i < panel.childCount; i++)
        {
            if (i % 2 == 0)
            {
                Button button = panel.GetChild(i).GetComponent<Button>();
                button.interactable = false;

                buttons.Add(button);
            }
            else
            {
                Transform transform = panel.GetChild(i);
                foreach (Transform child in transform)
                    child.gameObject.SetActive(false);
                lines.Add(transform);
            }
        }
        #endregion
    }

    private void Start()
    {
        IEnumerator waitFrame()
        {
            yield return new WaitForEndOfFrame();
            if (ThemeSystem.CurrentTheme != null)
            {
                OnThemeChanged();

                if (tmpIndex >= 0) SetLevelBase(tmpIndex, false);
                else
                {
                    int cleared = DataSystem.GetData("Puzzle", ThemeSystem.CurrentTheme.name, -1);
                    if (cleared == LevelSystem.instance.levelCount[ThemeSystem.CurrentTheme.index] - 1)
                        cleared = -1;

                    SetLevelBase(cleared + 1, false);
                }

                tmpIndex = -1;
            }
        }
        StartCoroutine(waitFrame());
    }

    public void OnThemeChanged()
    {
        LoadLevels(ThemeSystem.CurrentTheme.index);

        moveTheme[0].interactable = index > 0;

        bool flag = index < ThemeSystem.instance.themes.Length - 1;
        flag = flag && DataSystem.GetData("Puzzle", ThemeSystem.CurrentTheme.name) >= LevelSystem.instance.levelCount[ThemeSystem.CurrentTheme.index] - 1;

        moveTheme[1].interactable = flag;
    }

    public void LoadLevels(int index)
    {
        if (index < 0 || index >= ThemeSystem.instance.themes.Length) return;

        const int RANGE = 10;

        this.index = index;
        theme = ThemeSystem.instance.themes[index];

        Random.InitState((int)(Time.deltaTime * 1000));

        car.color = theme.carColors[Random.Range(0, theme.carColors.Length - 1)];

        for (int i = -RANGE; i <= RANGE; i++)
        {
            Vector3Int target = new Vector3Int(i, 0, 0);

            tile.SetTile(target, theme.grounds[0]);
            line.SetTile(target, theme.outlines[0]);
        }

        int cleared = DataSystem.GetData("Puzzle", theme.name);
        buttons[0].interactable = true;
        for (int i = 1; i < LevelSystem.instance.levelCount[index]; i++)
        {
            if (cleared < i - 1) break;

            buttons[i].interactable = true;
            foreach (Transform child in lines[i - 1].transform)
                child.gameObject.SetActive(true);
        }
    }

    public void SetLevel(int index)
    {
        SetLevelBase(index);
    }

    private void SetLevelBase(int index, bool move = true)
    {
        if(index < 0) // 이전 테마
        {
            ChangeTheme(-1);

            return;
        }else if(index >= LevelSystem.instance.levelCount[ThemeSystem.CurrentTheme.index]) // 다음 테마
        {
            ChangeTheme(1);

            return;
        }

        if (!buttons[index].interactable) return;

        genIndex = levelIndex = index;
        // genIndex => 진짜 맵 번호, levelIndex = 테마에서의 맵 번호

        for (int i = 0; i < this.index; i++) genIndex += LevelSystem.instance.levelCount[i];
        if (genIndex >= LevelSystem.instance.levels.Length) return;

        LevelBase level = LevelSystem.instance.levels[genIndex];
        text.text = level.id;

        LevelSystem.instance.SelectedLevel = genIndex;

        targetPosition = buttons[levelIndex].transform.position;
        if (!move) car.transform.position = targetPosition;
    }

    public void ChangeTheme(int delta)
    {
        if (index + delta < 0 || index + delta >= ThemeSystem.instance.themes.Length) return;

        tmpIndex = delta > 0 ? 0 : 7;
        ThemeSystem.CurrentTheme = ThemeSystem.instance.themes[index + delta];

        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 1);
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Move, "Select");
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 0);

        ActionSystem.instance.Play();
    }

    #region [ 키 리드 ]

    private void Update()
    {
        car.transform.position = Vector3.Lerp(car.transform.position, targetPosition, Time.deltaTime * 5f);

        if (!ActionSystem.instance.IsCompleted) return;
        if (SettingSystem.IsOpen) return;

        Key_LevelChange();
        Key_LevelEnter();
    }

    private void Key_LevelChange()
    {
        int delta = 0;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) delta -= 1;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) delta += 1;

        if (delta == 0) return;

        SetLevel(levelIndex + delta);
    }

    private void Key_LevelEnter()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        LevelEnter();
    }

    #endregion

    public void LevelEnter()
    {
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 1);
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Move, "Game");
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 0);

        ActionSystem.instance.Play();
    }
}
