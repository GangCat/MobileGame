using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public int TotalCnt => ttlCnt;
    public int ActiveCnt => activeCnt;
    public GameObject PoolObject => poolObject;

    /// <summary>
    /// �Է¹��� ������Ʈ�� ������� �޸�Ǯ ����
    /// </summary>
    /// <param name="_poolObject"></param>
    public ObjectPool(GameObject _poolObject, Transform _parentTr = null, int _increaseCnt = 5)
    {
        ttlCnt = 0;
        activeCnt = 0;

        poolObject = _poolObject;
        parentTr = _parentTr;
        increaseCnt = Mathf.Max(_increaseCnt,1);

        arrList = new List<GameObject>[10];
        for(int i = 0; i < 10; ++i)
            arrList[i] = new List<GameObject>(30);

        poolListEnable = new List<GameObject>();
        poolQueueDisable = new Queue<GameObject>();

        InstantiateObjects();
    }

    /// <summary>
    /// increaseCnt ������ ������Ʈ�� ����
    /// </summary>
    public void InstantiateObjects()
    {
        for (int i = 0; i < increaseCnt; ++i)
        {
            GameObject poolGo = GameObject.Instantiate(poolObject);
            poolGo.SetActive(false);
            poolGo.transform.parent = parentTr;

            poolQueueDisable.Enqueue(poolGo);
        }

        ttlCnt += increaseCnt;
    }

    /// <summary>
    /// ���� �������� ��� ������Ʈ�� '����'
    /// ���� �ٲ�ų� ������ ����� �� �� ���� ȣ��
    /// </summary>
    public void DestroyObjects()
    {
        if (poolListEnable == null || poolQueueDisable == null) return;

        int cnt = poolListEnable.Count;
        for (int i = 0; i < cnt; ++i)
            GameObject.Destroy(poolListEnable[i]);

        cnt = poolQueueDisable.Count;
        for(int i = 0; i < cnt; ++i)
            GameObject.Destroy(poolQueueDisable.Dequeue());

        poolListEnable.Clear();
        poolQueueDisable.Clear();
    }

    /// <summary>
    /// �ش� ������Ʈ�� Ȱ��ȭ
    /// </summary>
    /// <returns></returns>
    public GameObject ActivatePoolItem()
    {
        if (poolListEnable == null || poolQueueDisable == null) return null;

        if (poolQueueDisable.Count <= 0)
            InstantiateObjects();

        GameObject poolGo = poolQueueDisable.Dequeue();
        poolListEnable.Add(poolGo);

        poolGo.SetActive(true);

        ++activeCnt;

        return poolGo;
    }

        /// <summary>
    /// �ش� ������Ʈ�� ��Ȱ��ȭ
    /// </summary>
    /// <param name="_removeObject"></param>
    public void DeactivatePoolItem(GameObject _removeObject)
    {
        if (poolListEnable == null || poolQueueDisable == null || _removeObject == null) return;

        int cnt = poolListEnable.Count;
        for (int i = 0; i < cnt; ++i)
        {
            GameObject poolGo = poolListEnable[i];
            if (poolGo == _removeObject)
            {
                poolGo.transform.parent = parentTr;
                poolGo.SetActive(false);
                poolListEnable.Remove(poolGo);
                poolQueueDisable.Enqueue(poolGo);
                //poolGo.transform.position = tempStorePos;

                --activeCnt;

                return;
            }
        }
    }

    /// <summary>
    /// ��� ������Ʈ�� ��Ȱ��ȭ
    /// </summary>
    public void DeactivateAllPoolItems()
    {
        if (poolListEnable == null || poolQueueDisable == null) return;

        int cnt = poolListEnable.Count;
        for (int i = 0; i < cnt; ++i)
        {
            GameObject poolGo = poolListEnable[i];
            poolGo.transform.parent = parentTr;
            poolGo.SetActive(false);
            poolGo.transform.position = tempStorePos;

            poolListEnable.Remove(poolGo);
            poolQueueDisable.Enqueue(poolGo);
        }

        activeCnt = 0;
    }

    public bool IsEnableListEmpty()
    {
        return poolListEnable.Count < 1;
    }

    private int ttlCnt = 0;
    private int activeCnt = 0;
    private int increaseCnt = 0;

    private Vector3 tempStorePos = new Vector3(-3000f, 0f, 0f);
    private GameObject poolObject = null; // ������Ʈ Ǯ������ �����ϴ� ������Ʈ ������
    private Transform parentTr = null; // ���� �޸�Ǯ�� ���� �θ� Ʈ������

    private List<GameObject>[] arrList = null;
    private List<GameObject> poolListEnable = null;
    private Queue<GameObject> poolQueueDisable = null;
}
