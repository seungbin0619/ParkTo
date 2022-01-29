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

    private static Vector3Int[] direction = new Vector3Int[4]
    {
        Vector3Int.up,
        Vector3Int.left,
        Vector3Int.down,
        Vector3Int.right
    };

    [System.NonSerialized]
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid2d;
    private PolygonCollider2D pcollider2D;

    private int rotation;

    public Color32 color { private set; get; }
    public Vector3Int position { private set; get; }
    private List<Vector3Int> trace; // 이동 자취(undo 구현 시 사용)

    public List<PathData> path;
    public bool stopFlag = false;

    //private Vector3 velocity;

    public bool collided;
    public bool isOperatable { get { return path.Count > 1; } } // 다음에 움직일 수 있는지

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        rigid2d = GetComponent<Rigidbody2D>();
        pcollider2D = GetComponent<PolygonCollider2D>();

        trace = new List<Vector3Int>();
        //velocity = Vector3.zero;
    }

    private void Update()
    {
        if (!collided) return;
        
        rigid2d.velocity *= 0.95f;
        rigid2d.angularVelocity *= 0.95f;
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

    public void InitPath()
    {
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

        //switch (MapSystem.CurrentTriggers[tmp.position.y, tmp.position.x]) // 바닥에 따라 방향이 달라질 수 있음
        //{

        //}

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

    public bool MoveTo(float duration)
    {
        if (collided) return false;
        if (path.Count == 1) return false;

        duration /= fixedDuration;
        float expDuration = path.Count - 0.2f; // 0.8(가감속) - 1(타일 수 -1만큼 이동)

        #region [ 차가 움직이는 수식들 ]
        // 처음 0.8초, 마지막 0.8초는 가감속(원래보다 0.4초 길게 계산됨)

        int index; float clamp;
        if (duration < 0.8f) // 가속
        {
            index = 0;
            clamp = duration - index;
        }
        else if(duration > expDuration - 0.8f) // 감속
        {
            index = path.Count - 2;
            clamp = duration - (int)duration;
        }
        else // 중간
        {
            index = (int)(duration - 0.4f);
            clamp = duration - 0.4f - index;
        }

        if (duration >= expDuration) // 종료
        {
            position = path[path.Count - 1].position;
            trace.Add(position);

            transform.localPosition = path[path.Count - 1].position;
            transform.localPosition += new Vector3(0.5f, 0.5f); // 위치 조정

            return false;
        }

        Vector3 tmpPosition = path[index].position;
        Vector3 tmpVelocity = direction[rotation];

        if (duration < 0.8f) // 처음 가속
            tmpPosition += 5f / 8f * Mathf.Pow(clamp, 2) * tmpVelocity; //s = 5/8t^2
        else if(duration > expDuration - 0.8f)
            tmpPosition += (0.6f + clamp - 5f / 8f * Mathf.Pow(clamp, 2)) * tmpVelocity; //s = t - 5/8t^2
        else tmpPosition += clamp * tmpVelocity; //s = t

        //velocity = tmpPosition - transform.localPosition - new Vector3(0.5f, 0.5f);

        position = Vector3Int.RoundToInt(tmpPosition);

        transform.localPosition = tmpPosition;
        transform.localPosition += new Vector3(0.5f, 0.5f); // 위치 조정

        #endregion

        //Debug.Log(position);

        return true;
    }
}
