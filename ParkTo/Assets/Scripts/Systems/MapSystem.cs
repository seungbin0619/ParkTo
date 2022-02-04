using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem : MonoBehaviour
{
    #region [ 인스턴스 초기화 ]

    public static MapSystem instance;

    private WaitWhile drawFlag;

    private WaitWhile AnimateFlag;
    private void Awake()
    {
        if (instance == null) instance = this;

        drawFlag = new WaitWhile(() => isDrawed);
        AnimateFlag = new WaitWhile(() => isAnimated);
    }

    #endregion

    public enum TRIGGER
    {
        NORMAL = -1,
        GOAL,
        TURNLEFT,
        TURNRIGHT,
        STOP
    }

    #region [ 맵 정보들 ]

    [SerializeField]
    private ThemeBase[] themes;

    [SerializeField]
    private LevelBase[] levels;

    [SerializeField]
    private Tile[] triggers;

    [SerializeField]
    private Tile car;

    [SerializeField]
    private GameObject predictor;

    public static LevelBase CurrentLevel;
    public static ThemeBase CurrentTheme;

    public static int[,] CurrentGrounds;
    public static TRIGGER[,] CurrentTriggers;

    public static List<Car> CurrentCars;
    public static List<Goal> CurrentGoals;

    public static bool MoveFlag = false;

    #endregion

    #region [ 변수 ]

    public static int MapIndex = -1; // 현재 맵 인덱스
    private static bool isDrawed; // 맵이 그려져있는가?
    public static bool isPlayable; // 재생 가능한가?
    public static bool isAnimated; // 애니메이션 재생중인가?

    private static bool isPredictorInstantiated; // 경로 표시 중인가

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
    private Grid predictTile;

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
        // 초기화 애니메이션

        yield return AnimateFlag;

        carTile.ClearAllTiles();
        mapTile.ClearAllTiles();
        lineTile.ClearAllTiles();
        triggerTile.ClearAllTiles();
        //predictTile.ClearAllTiles();
        foreach (Transform child in predictTile.transform)
            Destroy(child.gameObject);

        TriggerSystem.instance.ResetTriggers();

        isDrawed = false;

        yield return null;
    }

    private void SelectLevel(int index)
    {
        if (index < 0 || index >= levels.Length) return;

        MapIndex = index;
        CurrentLevel = levels[index];
        CurrentTheme = themes[CurrentLevel.theme];

        Vars.instance.OnThemeChanged.Raise();

        CurrentGrounds = CurrentLevel.ToArray(CurrentLevel.grounds);
        CurrentTriggers = new TRIGGER[CurrentLevel.size.y, CurrentLevel.size.x];

        var tmp = CurrentLevel.ToArray(CurrentLevel.triggers);

        for (int y = 0; y < CurrentLevel.size.y; y++)
            for (int x = 0; x < CurrentLevel.size.x; x++)
                CurrentTriggers[y, x] = (TRIGGER)tmp[y, x];
    }

    private void InitializeLevel()
    {
        if (CurrentLevel == null) return;

        CurrentCars = new List<Car>();
        CurrentGoals = new List<Goal>();

        Random.InitState(CurrentLevel.seed);

        #region [ 맵 배치 ]

        #region [ 데이터 배열 ]

        // 중앙으로 배치 
        mapGrid.transform.position = new Vector3(-CurrentLevel.size.x * 0.5f, -CurrentLevel.size.y * 0.5f);

        int[,] tmpGroundRotation = CurrentLevel.ToArray(CurrentLevel.groundRotations);
        int[,] tmpTriggerInfo = CurrentLevel.ToArray(CurrentLevel.triggerInformations);

        int[,] tmpLines = CurrentLevel.ToArray(CurrentLevel.line);
        int[,] tmpLineRotation = CurrentLevel.ToArray(CurrentLevel.lineRotations);

        int[,] tmpCars = CurrentLevel.ToArray(CurrentLevel.cars);
        int[,] tmpCarRotation = CurrentLevel.ToArray(CurrentLevel.carRotations);

        Color32[] shuffle = new Color32[CurrentTheme.carColors.Length];
        System.Array.Copy(CurrentTheme.carColors, shuffle, shuffle.Length);

        for (int i = 0; i < 10; i++)
        {
            int r1 = Random.Range(0, CurrentTheme.carColors.Length);
            int r2 = Random.Range(0, CurrentTheme.carColors.Length);

            Color32 tmp = shuffle[r1];
            shuffle[r1] = shuffle[r2];
            shuffle[r2] = tmp;
        }

        #endregion

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
                    triggerTile.SetTile(targetPosition, triggers[0]);

                    Goal tmpGoal = triggerTile.GetInstantiatedObject(targetPosition).GetComponent<Goal>();
                    tmpGoal.Initialize(targetPosition, tmpTriggerInfo[y, x]);

                    CurrentGoals.Add(tmpGoal);
                }
                else if (CurrentTriggers[y, x] > 0) 
                    SetTrigger(targetPosition, (int)CurrentTriggers[targetPosition.y, targetPosition.x]);
            }
        #endregion

        TriggerSystem.instance.InitializeTriggers(CurrentLevel.hadTriggers);

        #endregion

        isDrawed = true;

        Vars.instance.OnChanged.Raise();
    }

    public void SetTrigger(Vector3Int targetPosition, int index)
    {
        triggerTile.SetTile(targetPosition, triggers[1]);
        TileTrigger tile = triggerTile.GetInstantiatedObject(targetPosition).GetComponent<TileTrigger>();

        tile.Initialize(index);

        CurrentTriggers[targetPosition.y, targetPosition.x] = (TRIGGER)index;
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
            Vars.instance.PrevMove.Raise();

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

        MoveFlag = true;
        StartCoroutine(CMove());
    }

    public void AfterMove() // 이동이 종료되었을 때
    {
        if(CurrentCars.FindAll(k => k.collided).Count > 0) // 충돌이 일어난 경우
        {
            // 다시 시작하기
            Debug.Log("실패!");

            return;
        }

        Vars.instance.OnChanged.Raise();

        MoveFlag = false;
        TriggerBar.instance.IsHide = false;
    }

    public void OnChanged() // 어떠한 변화가 생긴 경우
    {
        isPlayable = false;

        #region [ 클리어 체크 ]

        if (CurrentGoals.FindAll(k => k.IsArrived).Count == CurrentGoals.Count)
        {
            // 클리어
            Debug.Log("클리어!");
            PrevSelectLevel(MapIndex + 1);

            return;
        }

        #endregion

        #region [ 다음 경로 ] 

        for (int i = 0; i < CurrentCars.Count; i++)
            CurrentCars[i].InitPath();

        while (true)
        {
            bool pFlag = false;
            for (int i = 0; i < CurrentCars.Count; i++)
            {
                CurrentCars[i].GetNextPath();
                pFlag = pFlag || !CurrentCars[i].stopFlag;
            }

            if (!pFlag) break;
        }
        for (int i = 0; i < CurrentCars.Count; i++) // 차 이동 가능 체크
            isPlayable = CurrentCars[i].isOperatable || isPlayable;

        #endregion

        DrawPathPredictor();

        Vars.instance.AfterChange.Raise();
    }

    private void Update()
    {
        if (!isPredictorInstantiated) return;

    }

    private void DrawPathPredictor() // 예상 경로를 그려줌
    {
        foreach (Transform child in predictTile.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < CurrentCars.Count; i++)
            for (int j = 1; j < CurrentCars[i].path.Count; j++)
            {
                Vector3Int targetPosition = CurrentCars[i].path[j].position;
                Predictor tmpPredictor = Instantiate(predictor, predictTile.transform).gameObject.GetComponent<Predictor>();
                tmpPredictor.Initialize(CurrentCars[i].color, j * 2, targetPosition);

                tmpPredictor = Instantiate(predictor, predictTile.transform).gameObject.GetComponent<Predictor>();
                tmpPredictor.Initialize(CurrentCars[i].color, j * 2 - 1, (Vector3)(CurrentCars[i].path[j - 1].position + targetPosition) * 0.5f, true);
            }

        isPredictorInstantiated = true;
    }
}
