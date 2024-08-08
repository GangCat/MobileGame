using System.Collections;
using UnityEngine;

public class WalkableBlock : MonoBehaviour, IWalkableBlock
{
    [SerializeField]
    private GameObject arrowTestMR;

    public Vector2 Position { get; private set; }
    public EBlockType BlockType { get; private set; }

    private ObjectPoolManager poolManager;

    public void Init(Vector2 _position, EBlockType _blockType, ObjectPoolManager _poolManager, Vector2 _forward)
    {
        Position = _position;
        BlockType = _blockType;
        transform.position = new Vector3(_position.x, -1, _position.y);
        transform.forward = new Vector3(_forward.x, 0, _forward.y);
        poolManager = _poolManager;

        arrowTestMR = transform.GetChild(0).gameObject;
        

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
    }

    /// <summary>
    /// ResetBlock할 때 즉, 게임 재시작할 때 호출될 함수
    /// </summary>
    public void Destroy()
    {
        poolManager.ReturnObj(gameObject);
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
        var mat = GetComponent<MeshRenderer>().material;
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
        var mat = GetComponent<MeshRenderer>().material;
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