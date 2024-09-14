using System.Collections;
using UnityEngine;

public class WalkableBlock : MonoBehaviour
{
    [SerializeField]
    private EBlockType blockType;
    [SerializeField]
    private Renderer myRenderer;

    private OOBController enemyController = null;

    [SerializeField]
    private float intensityFactor = 4f;

    [SerializeField]
    private int idx = 0;

    public Vector3 Forward => transform.forward;
    public Vector2 Position { get; private set; }
    public EBlockType BlockType => blockType;
    public OOBController OOBController 
    { 
        set
        {
            enemyController = value;
        } 
    }

    private ObjectPoolManager poolManager;

    public void Init(Vector2 _position, EBlockType _blockType, ObjectPoolManager _poolManager, Vector2 _forward, int _idx)
    {
        Position = _position;
        blockType = _blockType;
        transform.position = new Vector3(_position.x, -1, _position.y);
        transform.forward = new Vector3(_forward.x, 0, _forward.y);
        poolManager = _poolManager;
        idx = _idx;

        myRenderer.material.SetColor("_EmissionColor", Color.red * intensityFactor);


        StartCoroutine(nameof(AppearBlockCoroutine));
    }

    /// <summary>
    /// 플레이어가 해당 블럭의 다음 블럭으로 이동했을 때 호출될 함수
    /// </summary>
    public void OnPositionChanged()
    {
        StartCoroutine(nameof(DisappearBlockCoroutine));
    }

    /// <summary>
    /// 플레이어가 해당 블럭으로 이동할 때 호출될 함수
    /// </summary>
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

    public void UpdateIdx()
    {
        --idx;
        if(idx == 3)
            myRenderer.material.SetColor("_EmissionColor", Color.yellow * intensityFactor);
        else if(idx < 3)
            myRenderer.material.SetColor("_EmissionColor", Color.green * intensityFactor);
    }

    /// <summary>
    /// 처음 생성되는 블럭들 0, 1, 2, 3 등 주기 위한 함수
    /// </summary>
    /// <param name="_newIdx"></param>
    public void SetNewIdx(int _newIdx)
    {
        idx = _newIdx;
    }
}