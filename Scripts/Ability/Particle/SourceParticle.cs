using UnityEngine;
using UnityEngine.VFX;
using Leopotam.EcsLite;
using FMODUnity;
using Statement;

namespace Client
{
    public class SourceParticle : MonoBehaviour, IPool
    {
        private EcsPool<VisualEffectsComponent> _visualPool = null;
        [SerializeField] protected VisualEffect effect;
        [SerializeField] protected ParticleSystem particles;
        [SerializeField] private bool _turnsOffOnlyAfterTimerExpired;
        public float Timer = 0f;
        private bool IsInvoked = false;
        public EventReference eventReference;
        public bool followTransform;
        protected bool isAvaiable;
        private StopDelegate Stop;
        private PlayDelegate Play;
        private SetTimerDelegate SetTimer;
        public GameObject ThisGameObject => gameObject;

        public bool IsAvaiable { get => isAvaiable; set => isAvaiable = value; }

        public string PoolKeyID => KEY_ID;
        public string KEY_ID;
        
        public void AttachVisualEffectToEntity(Vector3 startPos, Quaternion startRotation, EcsPackedEntity targetPackedEntity, Transform parent = null, float time = 5f)
        {
            var world = State.Instance.EcsRunHandler.World;
            transform.SetParent(parent);
            transform.position = startPos;
            transform.rotation = startRotation;
            if (targetPackedEntity.Unpack(world, out int targetEntity))
            {
                _visualPool = world.GetPool<VisualEffectsComponent>();
                ref var visualComp = ref _visualPool.Get(targetEntity);
                visualComp.SourceParticles.Add(this);
            }
            Invoke(time);
        }

        public void PlaySound()
        {
            if (followTransform)
            {
                SoundEntity.Instance.PlayAudioAttached(eventReference, this.transform);
            }
            else
            {
                SoundEntity.Instance.PlayAudioAtPosition(eventReference, this.transform.position);
            }
        }
        public VisualEffect GetEffect()
        {
            return effect;
        }
        

        
#if UNITY_EDITOR
        private void OnValidate()
        {
           effect = GetComponent<VisualEffect>();

            if (!string.IsNullOrEmpty(name))
            {
                KEY_ID = name;
            }
        }
#endif
        private void Update()
        {
            if(!IsInvoked) return;
            if (Timer > 0)
                Timer -= Time.deltaTime;
            else
            {
                Dispose();
            }
        }
        private delegate void StopDelegate();
        private delegate void PlayDelegate();
        private delegate void SetTimerDelegate(float time = 5f);
        public void Invoke(float time = 5f)
        {
            SetTimer(time);
            isAvaiable = false;
            IsInvoked = true;
            PlaySound();
            Play();
        }
        public void Dispose()
        {
            if (_turnsOffOnlyAfterTimerExpired && Timer > 0) return;
            Stop();
            IsInvoked = false;
            ReturnToPool();
        }
        private void DisposeVFX()
        {
            effect.Stop();
        }
        private void InvokeVFX()
        {
            effect.Play();
        }
        private void DisposeParticles()
        {
            particles.Stop();
        }
        private void InvokeParticles()
        {
            particles.Play();
        }
        public void SetParticlesTimer(float time = 5f)
        {
            Timer = time;
        }

        private void SetVFXTimer(float time = 5f)
        {
            if (GetEffect().HasFloat("Lifetime"))
            {
                Timer = GetEffect().GetFloat("Lifetime");
            }
            else
            {
                Timer = time;
            }
        }
        public void InitPool()
        {
            if (TryGetComponent<VisualEffect>(out effect))
            {
                Play = InvokeVFX;
                Stop = DisposeVFX;
                SetTimer = SetVFXTimer;
                return;
            }
            if (TryGetComponent<ParticleSystem>(out particles))
            {
                Play = InvokeParticles;
                Stop = DisposeParticles;
                SetTimer = SetParticlesTimer;
            }
        }
        public void ReturnToPool()
        {
            PoolModule.Instance.ReturnToPool(this);
        }
    }
}

