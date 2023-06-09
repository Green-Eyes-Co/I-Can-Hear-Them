using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundRaycaster : MonoBehaviour
{
    [SerializeField] int raysToShoot = 30;
    [SerializeField] float maxLength = 300f;
    [SerializeField] int reflections = 1;
    [SerializeField] LayerMask layerMask;

    [SerializeField] AudioSource sourcePrefab;
    [SerializeField] AudioClip[] sounds;
    [SerializeField] float soundIntervalMin;
    [SerializeField] float soundIntervalMax;

    [SerializeField] bool showDebugLines = false;
    [SerializeField] bool updateEveryFrame = false;
    [SerializeField] bool playOnAwake = false;
    [SerializeField] string playerTag;

    float soundInterval;
    Transform targetLook;

    void Start()
    {
        targetLook = GameObject.FindWithTag(playerTag).transform;
        StartCoroutine(SoundCoroutine());
    }

    void Update()
    {
        if (updateEveryFrame)
        {
            ShootRaycasts();
        }
    }

    IEnumerator SoundCoroutine()
    {
        while (true)
        {
            soundInterval = Random.Range(soundIntervalMin, soundIntervalMax);
            if (playOnAwake)
            {
                soundInterval = 0f;
                playOnAwake = false;
            }
            yield return new WaitForSeconds(soundInterval);
            var result = ShootRaycasts();
            if (result.AnyHitPlayer)
            {
                var source = Instantiate(sourcePrefab, result.SoundOrigin, Quaternion.identity);
                source.volume = result.HitVolume;

                var sound = sounds[Random.Range(0, sounds.Length)];
                source.PlayOneShot(sound);

                Destroy(source.gameObject, sound.length);
            }
        }
    }


    SoundRaycastResult ShootRaycasts()
    {
        if (Physics.Raycast(transform.position, targetLook.position - transform.position, out var hit, maxLength, layerMask) && hit.collider.CompareTag(playerTag))
        {
            if (showDebugLines)
            {
                Debug.DrawLine(transform.position, hit.point, Color.green, updateEveryFrame ? 0f : soundInterval);
            }
            return new SoundRaycastResult(true, transform.position, Mathf.InverseLerp(maxLength, 0, Vector3.Distance(transform.position, hit.point)));
        }

        var angle = 0f;
        List<HitOrigin> playerHitOrigins = new();
        for (int i = 0; i < raysToShoot; i++)
        {
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);
            angle += 2 * Mathf.PI / raysToShoot;

            var ray = new Ray(transform.position, new Vector3(x, 0, z));
            var remainingLength = maxLength;
            for (int j = 0; j <= reflections; j++)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength, layerMask))
                {
                    var hitPlayer = hit.collider.CompareTag(playerTag);
                    if (showDebugLines)
                    {
                        Debug.DrawLine(ray.origin, hit.point, hitPlayer ? Color.yellow : Color.red, updateEveryFrame ? 0f : soundInterval);
                    }

                    remainingLength -= Vector3.Distance(ray.origin, hit.point);
                    if (hitPlayer)
                    {
                        playerHitOrigins.Add(new HitOrigin(ray.origin, maxLength - remainingLength));
                        break;
                    }
                    ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                }
                else
                {
                    if (showDebugLines)
                    {
                        Debug.DrawLine(ray.origin, ray.origin + ray.direction * remainingLength, Color.blue, updateEveryFrame ? 0f : soundInterval);
                    }
                    break;
                }
            }
        }

        if (playerHitOrigins.Any())
        {
            var averageOrigin = AverageOrigin(playerHitOrigins);
            var hitVolume = Mathf.InverseLerp(maxLength, 0, playerHitOrigins.Average(x => x.Distance));
            return new SoundRaycastResult(true, new Vector3(averageOrigin.x, 1, averageOrigin.z), hitVolume);
        }

        return new SoundRaycastResult();
    }

    private struct HitOrigin
    {
        public Vector3 Position;
        public float Distance;

        public HitOrigin(Vector3 position, float distance)
        {
            Position = position;
            Distance = distance;
        }
    }

    private Vector3 AverageOrigin(List<HitOrigin> origins)
    {
        var count = 0;
        var valueSum = Vector3.zero;
        var weightSum = 0f;

        if (origins.Count == 1)
        {
            return origins.Single().Position;
        }

        foreach (var origin in origins)
        {
            count++;
            valueSum += origin.Position * origin.Distance;
            weightSum += origin.Distance;
        }

        return valueSum / weightSum;
    }
}
