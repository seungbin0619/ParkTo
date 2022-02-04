using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private const float fixedDuration = 0.3f; // 1칸을 움직이는데 걸리는 시간
    private const float damage = 1f; // 충돌 시 충격을 얼마나 크게 할지

    public struct PathData
    {
        public Vector3Int position;
        public int rotation;

        public PathData(Vector3Int position, int rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

    private readonly static Vector3Int[] direction = new Vector3Int[4]
    {
        Vector3Int.up,
        Vector3Int.left,
        Vector3Int.down,
        Vector3Int.right
    };
    private readonly static Vector3[] rotate = new Vector3[5]
    {
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 90f),
        new Vector3(0, 0, 180f),
        new Vector3(0, 0, 270f),
        new Vector3(0, 0, 360f)
    };

    #region [ 기타 변수 ]

    [System.NonSerialized]
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid2d;
    private PolygonCollider2D pcollider2D;

    public int rotation;

    public Color32 color { private set; get; }

    public Vector3Int position { private set; get; }
    private List<Vector3Int> trace; // 이동 자취(undo 구현 시 사용)

    public List<PathData> path;
    public bool stopFlag = false;

    //private Vector3 velocity;

    public bool collided;
    public bool isOperatable { get { return path.Count > 1; } } // 다음에 움직일 수 있는지

    private int moveIndex;
    private float currentProgress;
    private float targetDuration;

    private const float accelDuration = 1f;

    #endregion

    private Vector3 targetScale = Vector3.one * 0.8f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        rigid2d = GetComponent<Rigidbody2D>();
        pcollider2D = GetComponent<PolygonCollider2D>();

        trace = new List<Vector3Int>();
        //velocity = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pcollider2D.isTrigger = false;
        collided = true;

        transform.position += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 0.1f;
    }

    public void Initialize(Vector3Int position, int rotation, Color32 color)
    {
        this.position = position;
        this.rotation = rotation;
        this.color = color;

        spriteRenderer.color = color;
        transform.eulerAngles = new Vector3(0, 0, rotation * 90f);
        trace.Add(position);
    }

    #region [ 경로 계산 ]

    public void InitPath()
    {
        moveIndex = 0;

        currentProgress = 0;
        targetDuration = accelDuration;

        stopFlag = false;

        path = new List<PathData>();
        path.Add(new PathData(position, rotation));
    }

    public void GetNextPath() // 다음 재생시 경로 구하기
    {
        if (stopFlag) return;

        PathData tmp = GetFront(path[path.Count - 1]);
        if (!MapSystem.IsValidPosition(tmp.position))
        {
            stopFlag = true;
            return;
        }

        switch (MapSystem.CurrentTriggers[tmp.position.y, tmp.position.x]) // 바닥에 따라 방향이 달라질 수 있음
        {
            case MapSystem.TRIGGER.TURNLEFT: // 우회전
                tmp.rotation += 1;
                tmp.rotation %= 4;

                break;
            case MapSystem.TRIGGER.TURNRIGHT: // 좌회전
                tmp.rotation -= 1;
                if (tmp.rotation < 0) tmp.rotation += 4;

                tmp.rotation %= 4;

                break;
            default: // 직진
                break;
        }

        for(int i = 0; i < MapSystem.CurrentCars.Count; i++)
        {
            Car dif = MapSystem.CurrentCars[i];
            PathData difPos = dif.path[Mathf.Min(path.Count, dif.path.Count) - 1];

            if (dif == this) continue;
            if (difPos.position != tmp.position) continue;
            if (MapSystem.IsValidPosition(GetFront(difPos).position)) continue;

            stopFlag = true;
            return;
        }

        path.Add(tmp);
    }

    private PathData GetFront(PathData bef)
    {
        return new PathData(bef.position + direction[bef.rotation], bef.rotation);
    }

    #endregion

    #region [ 이동 ]

    public bool MoveTo(float duration)
    {
        if (collided) return false;
        if (path.Count == 1) return false;

        duration /= fixedDuration;
        if (duration > targetDuration)
        {
            moveIndex++;

            currentProgress = targetDuration;
            targetDuration += 1f;
        }

        Vector3 position;
        float clamp = duration - currentProgress;

        if (moveIndex == 0)  // 처음 출발 시
        {
            position = path[0].position;
            position += 0.5f * Mathf.Pow(clamp, 2) * (Vector3)direction[path[0].rotation];
        }
        else if (moveIndex >= path.Count) return false; // 종료
        else
        {
            PathData bef = path[moveIndex - 1], cur = path[moveIndex];

            position = bef.position + cur.position;
            position *= 0.5f;

            if (moveIndex == path.Count - 1) // 감속
            {
                position += (clamp - 0.5f * Mathf.Pow(clamp, 2)) * (Vector3)direction[bef.rotation];

                switch (MapSystem.CurrentTriggers[cur.position.y, cur.position.x]) // 바닥에 따라 방향이 달라질 수 있음
                {
                    case MapSystem.TRIGGER.TURNLEFT:
                    case MapSystem.TRIGGER.TURNRIGHT:
                        rotation = cur.rotation;

                        if (bef.rotation == 3 && cur.rotation == 0) cur.rotation = 4;
                        if (bef.rotation == 0 && cur.rotation == 3) bef.rotation = 4;

                        transform.eulerAngles = Vector3.Lerp(rotate[bef.rotation], rotate[cur.rotation],
                            LineAnimation.Lerp(0, 1, clamp, 1, 0, 0.5f));

                        break;
                    default: // 직진
                        break;
                }
            }
            else // 중간
            {
                switch (MapSystem.CurrentTriggers[cur.position.y, cur.position.x]) // 바닥에 따라 방향이 달라질 수 있음
                {
                    case MapSystem.TRIGGER.TURNLEFT: position = GetTurnPosition(bef, cur, position, clamp, 1); break;
                    case MapSystem.TRIGGER.TURNRIGHT: position = GetTurnPosition(bef, cur, position, clamp, -1); break;
                    default: // 직진
                        position += clamp * (Vector3)direction[bef.rotation];

                        break;
                }
            }
        }

        transform.localPosition = position;
        transform.localPosition += new Vector3(0.5f, 0.5f); // 위치 조정

        this.position = Vector3Int.RoundToInt(position);

        return true;
    }

    private Vector3 GetTurnPosition(PathData bef, PathData cur, Vector3 position, float clamp, int dir)
    {
        Vector3 nxt = cur.position + (Vector3)direction[cur.rotation] * 0.5f;
        rotation = cur.rotation;

        if (bef.rotation == 3 && cur.rotation == 0) cur.rotation = 4;
        if (bef.rotation == 0 && cur.rotation == 3) bef.rotation = 4;

        transform.eulerAngles = Vector3.Lerp(rotate[bef.rotation], rotate[cur.rotation], clamp);

        Vector3 center = new Vector3();
        float A = Mathf.PI;

        if((position.y - nxt.y) / (position.x - nxt.x) * dir > 0)
        {
            center.x = position.x;
            center.y = nxt.y;

            A *= center.y < position.y ? 0.5f : 1.5f;
        }else
        {
            center.x = nxt.x;
            center.y = position.y;

            A *= center.x < position.x ? 0f : 1f;
        }

        float angle = A + 0.5f * Mathf.PI * dir * clamp;
        position.x = center.x + Mathf.Cos(angle) * 0.5f;
        position.y = center.y + Mathf.Sin(angle) * 0.5f;

        return position;
    }

    public void AfterMove()
    {
        transform.localPosition = position;
        transform.localPosition += new Vector3(0.5f, 0.5f); // 위치 조정
        transform.eulerAngles = rotate[rotation];
    }

    #endregion

    #region [ 트리거 적용 ]

    public void SetTrigger(Trigger trigger)
    {
        switch ((MapSystem.TRIGGER)trigger.index)
        {
            case MapSystem.TRIGGER.TURNLEFT:
                rotation += 1;

                break;
            case MapSystem.TRIGGER.TURNRIGHT:
                rotation -= 1;
                if (rotation < 0) rotation += 4;
                break;
            default:

                break;
        }

        rotation %= 4;
        transform.eulerAngles = rotate[rotation];
    }

    private void Update()
    {
        PreviewUpdate();
        //spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * 5f);

        if (!collided) return;

        rigid2d.velocity *= 0.95f;
        rigid2d.angularVelocity *= 0.95f;
    }

    private void PreviewUpdate()
    {
        if (MapSystem.MoveFlag) return;

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 10f);
    }

    public void PreviewTrigger(float size)
    {
        targetScale = Vector3.one * size;
    }

    public void OnTriggerCancel()
    {
        targetScale = Vector3.one * 0.8f;
        transform.eulerAngles = rotate[rotation];
    }

    #endregion
}
