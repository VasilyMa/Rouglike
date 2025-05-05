using FMODUnity;
using UnityEngine;

namespace Client
{
    struct PlaySoundEvent
    {
        public Transform SoundTransform;
        public EventReference eventReference;
        public int entity;
        public int entitySender;

        public void Invoke(EventReference reference, Transform soundTransform = null)
        {
            eventReference = reference;
            SoundTransform = soundTransform;
        }
    }
}
