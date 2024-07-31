using UnityEngine;

public class Cam : MonoBehaviour, IPlayerMoveObserver
{
    [SerializeField]
    private Transform playerTr = null;

    private Vector3 camOffset = Vector3.zero;

    private void Start()
    {
        camOffset = transform.position - playerTr.position;
    }

    private void LateUpdate()
    {
        transform.position = playerTr.position + camOffset;
    }

    public void OnNotify(EBlockType _blockType)
    {
        throw new System.NotImplementedException();
    }
}
