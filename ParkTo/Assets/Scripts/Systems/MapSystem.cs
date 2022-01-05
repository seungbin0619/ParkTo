using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem : MonoBehaviour
{
    #region [ 인스턴스 초기화 ]

    public static MapSystem instance;
    private void Awake()
    {
        if (instance == null) instance = this;

        drawFlag = new WaitWhile(() => isDrawed);
    }

    #endregion

    #region [ 맵 정보들 ]

    private bool isDrawed; // 맵이 그려져있는가?

    private WaitWhile drawFlag;

    [SerializeField]
    private ThemeBase[] themes;

    [SerializeField]
    private LevelBase[] levels;

    [SerializeField]
    private Tile[] triggers;

    [SerializeField]
    private Tile car;

    public static LevelBase CurrentLevel;
    public static ThemeBase CurrentTheme;

    public static int[,] CurrentGrounds;
    public static int[,] CurrentTriggers;

    public static List<Car> CurrentCars;
    public static List<Goal> CurrentGoals;

    #endregion

    #region [ 오브젝트 ]

    [SerializeField]
    private Grid mapGrid;

    [SerializeField]
    private Tilemap mapTile;

    [SerializeField]
    private Tilemap lineTile;

    [SerializeField]
    private Tilemap carTile;

    [SerializeField]
    private Tilemap triggerTile;

    [SerializeField]
    private UnityEngine.UI.Image background;

    #endregion

    public void PrevSelectLevel(int index)
    {
        IEnumerator CPrevSelectLevel()
        {
            if (isDrawed) StartCoroutine(PrevResetLevel());
            yield return drawFlag; // 기존 맵이 지워질 때까지 기다리기

            SelectLevel(index);
            InitializeLevel();
        }

        StartCoroutine(CPrevSelectLevel());
    }

    private IEnumerator PrevResetLevel()
    {
        isDrawed = false;

        yield return null;
    }

    private void SelectLevel(int index)
    {
        if (index < 0 || index >= levels.Length) return;

        CurrentLevel = levels[index];
        CurrentTheme = themes[CurrentLevel.theme];

        Vars.instance.OnThemeChanged.Raise();

        CurrentGrounds = CurrentLevel.ToArray(CurrentLevel.grounds);
        CurrentTriggers = CurrentLevel.ToArray(CurrentLevel.triggers);
    }

    private void InitializeLevel()
    {
        if (CurrentLevel == null) return;

        CurrentCars = new List<Car>();
        CurrentGoals = new List<Goal>();

        Random.InitState(CurrentLevel.seed);

        #region [ 맵 배치 ]

        // 중앙으로
        mapGrid.transform.position = new Vector3(-CurrentLevel.size.x * 0.5f, -CurrentLevel.size.y * 0.5f);

        int[,] tmpGroundRotation = CurrentLevel.ToArray(CurrentLevel.groundRotations);
        int[,] tmpTriggerInfo = CurrentLevel.ToArray(CurrentLevel.triggerInformations);

        int[,] tmpLines = CurrentLevel.ToArray(CurrentLevel.line);
        int[,] tmpLineRotation = CurrentLevel.ToArray(CurrentLevel.lineRotations);

        int[,] tmpCars = CurrentLevel.ToArray(CurrentLevel.cars);
        int[,] tmpCarRotation = CurrentLevel.ToArray(CurrentLevel.carRotations);

        Color32[] shuffle = CurrentTheme.carColors;
        for(int i = 0; i < 10; i++)
        {
            int r1 = Random.Range(0, CurrentTheme.carColors.Length);
            int r2 = Random.Range(0, CurrentTheme.carColors.Length);

            Color32 tmp = shuffle[r1];
            shuffle[r1] = shuffle[r2];
            shuffle[r2] = tmp;
        }


        for (int y = 0; y < CurrentLevel.size.y; y++)
            for (int x = 0; x < CurrentLevel.size.x; x++)
            {
                Vector3Int targetPosition = new Vector3Int(x, y, 0);

                #region [ 바닥 타일 배치 ]

                if (CurrentGrounds[y, x] >= 0)
                {
                    mapTile.SetTile(targetPosition, CurrentTheme.grounds[CurrentGrounds[y, x]]);
                    lineTile.SetTile(targetPosition, CurrentTheme.outlines[tmpLines[y, x]]);

                    Matrix4x4 targetMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, tmpGroundRotation[y, x] * 90f), Vector3.one);
                    mapTile.SetTransformMatrix(targetPosition, targetMatrix);

                    //Debug.Log(tmpLineRotation[y, x]);
                    lineTile.GetInstantiatedObject(targetPosition).transform.Rotate(0, 0, tmpLineRotation[y, x] * 90f);
                }

                #endregion

                #region [ 차 배치 ]

                if(tmpCars[y, x] == 1)
                {
                    carTile.SetTile(targetPosition, car);
                    Car tmpCar = carTile.GetInstantiatedObject(targetPosition).GetComponent<Car>();

                    tmpCar.Initialize(targetPosition, tmpCarRotation[y, x], shuffle[CurrentCars.Count]);
                    CurrentCars.Add(tmpCar);
                }

                #endregion
            }

        #region [ 트리거 배치 ]
        for (int y = 0; y < CurrentLevel.size.y; y++)
            for (int x = 0; x < CurrentLevel.size.x; x++)
            {
                Vector3Int targetPosition = new Vector3Int(x, y, 0);

                if (CurrentTriggers[y, x] == 0)
                {
                    triggerTile.SetTile(targetPosition, triggers[CurrentTriggers[y, x]]);

                    Goal tmpGoal = triggerTile.GetInstantiatedObject(targetPosition).GetComponent<Goal>();
                    tmpGoal.Initialize(targetPosition, tmpTriggerInfo[y, x]);

                    CurrentGoals.Add(tmpGoal);
                }
                else if (CurrentTriggers[y, x] > 0)
                {

                }
            }
        #endregion

        for (int i = 0; i < CurrentCars.Count; i++)
            CurrentCars[i].GetNextPath();

        #endregion
    }

    public static bool IsValidPosition(Vector3Int position)
    {
        int x = position.x, y = position.y;

        if (x < 0 || x > CurrentLevel.size.x - 1 || y < 0 || y > CurrentLevel.size.y - 1) return false;
        if (CurrentGrounds[y, x] < 0) return false;

        /*
        
        switch(CurrentTriggers[y, x])
        {
          
        }

        */

        return true;
    }

    public void Move()
    {
        WaitForEndOfFrame delay = new WaitForEndOfFrame();
        float progress = 0;

        IEnumerator CMove()
        {
            bool completeFlag = false;
            while (!completeFlag)
            {
                completeFlag = true;

                for (int i = 0; i < CurrentCars.Count; i++)
                    completeFlag = !CurrentCars[i].MoveTo(progress) && completeFlag;

                yield return delay;
                progress += Time.deltaTime;
            }

            Vars.instance.AfterMove.Raise();
        }
        StartCoroutine(CMove());
    }

    public void AfterMove() // 이동이 종료되었을 때
    {
        Vars.instance.OnChanged.Raise();

    }

    public void OnChanged() // 어떠한 변화가 생긴 경우
    {
        bool valid = true;
        for (int i = 0; i < CurrentCars.Count; i++)
        {
            CurrentCars[i].GetNextPath();
            valid = CurrentCars[i].isOperatable || valid;
        }


    }

    private void DrawPathPredictor() // 예상 경로를 그려줌
    {

    }
}
