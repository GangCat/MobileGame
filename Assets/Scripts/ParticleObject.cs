using System.Collections;
using UnityEngine;

public class ParticleObject : MonoBehaviour
{
    public void Init(Vector3 _position, ObjectPoolManager _poolManager)
    {
        transform.position = _position;
        StartCoroutine(nameof(AutoDestroyCoroutine), _poolManager);
    }

    private IEnumerator AutoDestroyCoroutine(ObjectPoolManager _poolManager)
    {
        var particleSys = GetComponent<ParticleSystem>();
        particleSys.Play();
        while (particleSys.isPlaying)
        {
            yield return null;
        }
        _poolManager.ReturnObj(gameObject);
    }
}
