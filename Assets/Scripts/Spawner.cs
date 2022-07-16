using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public DicePool Pool { get; private set; }
    Bounds spawnBounds;

    [SerializeField, Min(1)] int initialCount;

    void Start()
    {
        Pool = GetComponent<DicePool>();
        spawnBounds = GetComponent<Collider>().bounds;

        Spawn(initialCount);
    }

    public void Spawn(int number)
    {
        for (int i = 0; i < number; i++)
        {
            Pool.GetPooledObject().transform.SetPositionAndRotation(RandomPointInBox(spawnBounds, doZ: false), Random.rotation);
        }
    }

    private Vector3 RandomPointInBox(Bounds b, bool doX = true, bool doY = true, bool doZ = true)
    {
        return b.center + new Vector3(
           doX ? (Random.value - 0.5f) * b.size.x : 0,
           doY ? (Random.value - 0.5f) * b.size.y : 0,
           doZ ? (Random.value - 0.5f) * b.size.z : 0
        );
    }

}
