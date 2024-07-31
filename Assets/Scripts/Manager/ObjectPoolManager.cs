using UnityEngine;
using Unity;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public class ObjectPoolManager :MonoBehaviour
{
    Dictionary<string, ObjectPool> assetPathToObjectPool = new Dictionary<string, ObjectPool>();

    Dictionary<GameObject, string> pathToObject = new();


    public ObjectPool PrepareObjects(string path,int increaseCnt=5)
    {

        if (assetPathToObjectPool.ContainsKey(path))
        {
            throw new System.ArgumentException($"이미 풀이 등록되어있습니다 [{path}]");
        }

        var poolGameObject= new GameObject();


        var delimiterIdx= path.LastIndexOf("/");
        if (delimiterIdx == -1)
        {
            poolGameObject.name = path;
        }
        else
        {
            poolGameObject.name = path.Substring(delimiterIdx+1);
        }
        

        poolGameObject.transform.SetParent(this.transform);
        var poolObj=Addressables.LoadAssetAsync<GameObject>(path).WaitForCompletion();
        var objPool=new ObjectPool(poolObj, poolGameObject.transform, increaseCnt);

        assetPathToObjectPool.Add(path, objPool);

        return objPool;
    }


    public GameObject GetObject(string path)
    {
        if (assetPathToObjectPool.TryGetValue(path, out var objectPool) == false)
        {
            objectPool=PrepareObjects(path);
        }

        var obj= objectPool.ActivatePoolItem();

        pathToObject.Add(obj,path);

        return obj;
    }

    public void ReturnObj(GameObject obj)
    {
        if(pathToObject.TryGetValue(obj,out var path) == false)
        {
            throw new System.ArgumentException($"오브젝트가 풀에 등록되어있지 않습니다. [{obj.name}]");
        }

        pathToObject.Remove(obj);

        assetPathToObjectPool[path].DeactivatePoolItem(obj);
    }

    public void ReturnAllObjects(string path)
    {
        if (assetPathToObjectPool.TryGetValue(path, out var objectPool))
        {
            objectPool.DeactivateAllPoolItems();
        }
        else
        {
            throw new System.ArgumentException($"해당 경로의 오브젝트 풀이 존재하지 않습니다. [{path}]");
        }

        List<GameObject> objectsToRemove = new List<GameObject>();
        foreach (var kvp in pathToObject)
        {
            if (kvp.Value == path)
            {
                objectsToRemove.Add(kvp.Key);
            }
        }

        foreach (var obj in objectsToRemove)
        {
            pathToObject.Remove(obj);
        }
    }

    public void OnDestroy()
    {
        foreach (ObjectPool op in assetPathToObjectPool.Values)
            Addressables.Release(op.PoolObject);
    }



}
