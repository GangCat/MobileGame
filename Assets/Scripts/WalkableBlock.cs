using System.Collections;
using UnityEngine;

public class WalkableBlock : MonoBehaviour, IWalkableBlock
{
    [SerializeField]
    private GameObject arrowTestMR;
    [SerializeField]
    private EBlockType blockType;
    [SerializeField]
    private Renderer myRenderer;

    private EnemyController enemyController = null;

    public Vector3 Forward => transform.forward;
    public Vector2 Position { get; private set; }
    public EBlockType BlockType => blockType;
    public EnemyController EnemyController 
    { 
        set
        {
            enemyController = value;
        } 
    }

    private ObjectPoolManager poolManager;

    public void Init(Vector2 _position, EBlockType _blockType, ObjectPoolManager _poolManager, Vector2 _forward)
    {
        Position = _position;
        blockType = _blockType;
        transform.position = new Vector3(_position.x, -1, _position.y);
        transform.forward = new Vector3(_forward.x, 0, _forward.y);
        poolManager = _poolManager;

        arrowTestMR = transform.GetChild(0).gameObject;

        //switch (blockType)
        //{
        //    case EBlockType.NORMAL:
        //        GetComponent<Renderer>().material.color = Color.green;
        //        break;
        //    case EBlockType.DOUBLE_SCORE:
        //        GetComponent<Renderer>().material.color = Color.yellow;
        //        break;
        //    case EBlockType.TRIPLE_SCORE:
        //        GetComponent<Renderer>().material.color = Color.cyan;
        //        break;
        //    case EBlockType.INVINCIBLE_BUFF:
        //        GetComponent<Renderer>().material.color = Color.white;
        //        break;
        //}


        StartCoroutine(nameof(AppearBlockCoroutine));
    }

    public void OnPositionChanged()
    {
        StartCoroutine(nameof(DisappearBlockCoroutine));
    }

    public void OnPositioned()
    {
        // 플레이어가 블럭으로 이동할 때 블럭이 위아래로 잠깐 움직이는 효과
        // 블럭 위아래 이동
        StartCoroutine(WiggleCoroutine());
        // 이때 적에게 신호
        enemyController?.Die();
        enemyController = null;
    }

    /// <summary>
    /// ResetBlock할 때 즉, 게임 재시작할 때 호출될 함수
    /// </summary>
    public void Destroy()
    {
        poolManager.ReturnObj(gameObject);
        enemyController?.Return();
    }

    private IEnumerator WiggleCoroutine()
    {
        float wiggleTime = 0.3f;
        float wiggleFactor = 1;
        float elapsedTime = 0;
        Vector3 originPos = transform.position;
        while (elapsedTime < wiggleTime)
        {
            elapsedTime += Time.deltaTime;
            float y = Mathf.PingPong(elapsedTime ,wiggleTime * 0.5f) / wiggleTime * 0.5f * wiggleFactor;
            transform.position = new(originPos.x, originPos.y - y, originPos.z);

            yield return null;
        }
    }

    private IEnumerator AppearBlockCoroutine()
    {
        float wiggleTime = 0.2f;
        float elapsedTime = 0;
        var mat = myRenderer.material;
        while (elapsedTime < wiggleTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / wiggleTime);
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, alpha);

            yield return null;
        }
    }

    private IEnumerator DisappearBlockCoroutine()
    {
        float wiggleTime = 0.2f;
        float elapsedTime = 0;
        var mat = myRenderer.material;
        while (elapsedTime < wiggleTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / wiggleTime);
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, alpha);

            yield return null;
        }

        poolManager.ReturnObj(gameObject);

    }

}