using UnityEngine;

public struct SoundRaycastResult
{
    public bool AnyHitPlayer;
    public Vector3 SoundOrigin;
    public float HitVolume;

    public SoundRaycastResult(bool anyHitPlayer, Vector3 soundOrigin, float hitVolume)
    {
        AnyHitPlayer = anyHitPlayer;
        SoundOrigin = soundOrigin;
        HitVolume = hitVolume;
    }
}
