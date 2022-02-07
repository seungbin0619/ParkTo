using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSystem : MonoBehaviour
{
    #region [ 인스턴스 초기화 ]

    public static ActionSystem instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        actions = new List<Action>();
    }

    #endregion
    public struct Action
    {
        public enum ActionType
        {
            Move,  // 씬 이동
            Fade
        }
        public ActionType type;
        public List<object> args;
    }
    private List<Action> actions;
    private static Action currentAction;

    public bool IsCompleted { get { return actions.Count == 0; } }

    private WaitWhile wait = null;

    private static AsyncOperation operation;
    private static readonly WaitWhile waitMove = new WaitWhile(() => !operation.isDone);

    private static readonly WaitWhile waitFade = new WaitWhile(() => FadeSystem.instance.isAnimated);

    public void AddAction(Action.ActionType type, params object[] args)
    {
        Action action = new Action();
        action.type = type;
        action.args = new List<object>();
        action.args.AddRange(args);

        actions.Add(action);
    }

    public void Play()
    {
        IEnumerator CoPlay()
        {
            while (!IsCompleted)
            {
                Next();
                yield return wait;
                actions.RemoveAt(0);

                switch (currentAction.type)
                {
                    case Action.ActionType.Move: break;
                }
            }
        }
        StartCoroutine(CoPlay());
    }

    private void Next()
    {
        if (IsCompleted) return;
        currentAction = actions[0];

        switch (currentAction.type)
        {
            case Action.ActionType.Move:
                string SceneName = (string)currentAction.args[0];
                operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneName);

                wait = waitMove;

                break;
            case Action.ActionType.Fade:
                float target = float.Parse(currentAction.args[0].ToString());
                FadeSystem.instance.StartFade(target);

                wait = waitFade;

                break;
        }
    }
}
