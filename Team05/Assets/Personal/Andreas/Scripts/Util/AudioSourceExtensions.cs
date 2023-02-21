using UnityEngine;

namespace Util
{
    public static class AudioSourceExtensions
    {
        public static void SetMaxDistance(this AudioSource source, float distance)
        {
            source.maxDistance = distance;
            source.spatialBlend = 1f;
            source.rolloffMode = AudioRolloffMode.Custom;
        }
    }
}