using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    #region [ 인스턴스 초기화 ]

    public static LevelSystem instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    #endregion

    public List<int> levelCount;
    public LevelBase[] levels;

    public int SelectedLevel = -1;

    public void ClearedLevelSave(int index)
    {
        //if (SelectedLevel != index) return;

        ThemeBase theme = null;
        for(int i= 0; i < levelCount.Count; i++)
            if(levelCount[i] <= index) index -= levelCount[i];
            else
            {
                theme = ThemeSystem.instance.themes[i];
                break;
            }

        if (theme == null) return;

        int curData = DataSystem.GetData("Puzzle", theme.name);
        if (curData >= index) return;

        DataSystem.SetData("Puzzle", theme.name, index);
        DataSystem.SaveData();
    }
}
