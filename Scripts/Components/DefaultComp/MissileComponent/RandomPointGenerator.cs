
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class RandomPointGenerator 
{
    public static Vector3 GetRandomPoint(Vector3 centerPoint, float minRange, float maxRange, float maxDistance = 10f)
    {
        Vector3 spawnPoint;
        centerPoint.y = 0;
        spawnPoint = GetRandomPointInRadius(centerPoint, minRange, maxRange);
        if (!NavMesh.SamplePosition(spawnPoint, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            spawnPoint = centerPoint;
        }
        else
        {
            spawnPoint = hit.position;
        }
        return spawnPoint;
    }

    private static bool IsPointOnNavMesh(Vector3 position, float maxDistance)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, maxDistance, NavMesh.AllAreas);
    }

    private static Vector3 GetRandomPointInRadius(Vector3 center, float minRange, float maxRange)
    {
        float randomRadius = Mathf.Sqrt(Random.Range(minRange * minRange, maxRange * maxRange));
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float x = center.x + randomRadius * Mathf.Cos(angle);
        float z = center.z + randomRadius * Mathf.Sin(angle);
        return new Vector3(x, center.y, z);
    }
}
