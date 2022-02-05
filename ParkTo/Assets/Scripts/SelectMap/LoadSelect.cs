using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class LoadSelect : MonoBehaviour
{
    [SerializeField]
    private Tilemap tile;
    [SerializeField]
    private Tilemap line;

    private int index;
    private ThemeBase theme;

    private List<Button> buttons;
    private List<Transform> transforms;

    [SerializeField]
    private Image car;

    [SerializeField]
    private RectTransform panel;

    public List<int> levelCount;

    private void Awake()
    {
        buttons = new List<Button>();
        transforms = new List<Transform>();

        for(int i = 0; i < panel.childCount; i++)
        {
            if (i % 2 == 0) buttons.Add(panel.GetChild(i).GetComponent<Button>());
            else transforms.Add(panel.GetChild(i));
        }
    }

    private void Start()
    {
        if(ThemeSystem.CurrentTheme != null)
            OnThemeChanged();
    }

    public void OnThemeChanged()
    {
        LoadLevels(0);
    }

    public void LoadLevels(int index)
    {
        const int RANGE = 10;

        this.index = index;
        theme = ThemeSystem.instance.themes[index];

        Random.InitState((int)(Time.deltaTime * 1000));
        //Debug.Log(theme.carColors.Length - 1);

        //car.color = theme.carColors[Random.Range(0, theme.carColors.Length - 1)];

        for (int i = -RANGE; i <= RANGE; i++)
        {
            Vector3Int target = new Vector3Int(i, 0, 0);

            tile.SetTile(target, theme.grounds[0]);
            line.SetTile(target, theme.outlines[0]);
        }

        for (int i = 1; i < levelCount[index]; i++)
        {

        }
    }
}
