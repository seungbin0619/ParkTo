using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpSystem : MonoBehaviour
{
    #region [ 인스턴스 초기화 ]

    public static HelpSystem instance;
    private void Awake()
    {
        if (instance == null) instance = this;

        DontDestroyOnLoad(canvas);
    }

    #endregion

    #region [ 오브젝트 ]

    [SerializeField]
    private Sprite[] helpImages;

    [SerializeField, TextArea]
    private string[] helpTexts;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    TMPro.TMP_Text progress;

    [SerializeField]
    TMPro.TMP_Text descript;

    [SerializeField]
    private UnityEngine.UI.Image help;

    [SerializeField]
    private UnityEngine.UI.Button[] buttons;

    #endregion

    int current;

    public void OpenCanvas(int index = -1)
    {
        SFXSystem.instance.PlaySound(1);

        canvas.gameObject.SetActive(true);
        SetHelp(index);
    }

    public void HideCanvas()
    {
        canvas.gameObject.SetActive(false);
    }

    public void ChangeHelp(int delta)
    {
        SetHelp(current + delta);
    }

    private void SetHelp(int index)
    {
        index = Mathf.Clamp(index, -1, helpImages.Length - 1);

        int cnt = DataSystem.GetData("Setting", "Help", 0);
        if (index == -1) index = cnt;

        current = index;

        progress.text = index + 1 + " / " + (cnt + 1);

        help.sprite = helpImages[index];
        descript.text = helpTexts[index];

        buttons[0].interactable = index > 0;
        buttons[1].interactable = index < cnt;
    }

    public void SaveHelp(int index)
    {
        int cnt = DataSystem.GetData("Setting", "Help", 0);
        if (cnt >= index) return;

        DataSystem.SetData("Setting", "Help", index);
        DataSystem.SaveData();
    }
}
