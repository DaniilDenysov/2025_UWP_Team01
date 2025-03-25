using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Wave
{
    public List<WaveEnemy> Enemies = new List<WaveEnemy>();
    public float Duration;
}

[System.Serializable]
public class WaveEnemy
{
    //TODO: [DD] change to EnemyMovement
    public GameObject EnemyPrefab;
    public uint Amount;
}

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private List<Wave> waves = new List<Wave>();

    private IEnumerator Start()
    {
        for (int i = 0;i<waves.Count;i++)
        {

            yield return null;
        }
    }


}
