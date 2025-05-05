using Client;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Statement;

public class InteractiveHealObject : InteractiveObject, IPool
{
    public GameObject ThisGameObject => gameObject;
    protected bool isAvaiable;
    public bool IsAvaiable { get => isAvaiable; set => isAvaiable = value; }

    public string PoolKeyID => KEY_ID;
    public string KEY_ID;
    [SerializeField] private ParticleSystem[] _pickUpParticles;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _timer;
    private Collider _collider;
    [HideInInspector] public float Amount;
    State _state;
    Transform _parentTransform;
    Transform _playerTransform;
    Vector3 _playerPosition;
    VisualDamageConfig _visualDamage;
    public void Invoke(Transform poolTransform)
    {
        _parentTransform = poolTransform;
        _collider.enabled = true;
    }
    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        _pickUpParticles = GetComponentsInChildren<ParticleSystem>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _state = State.Instance;
        _visualDamage = ConfigModule.GetConfig<ViewConfig>().VisualDamageConfig;
        _playerTransform = _state.EcsRunHandler.World.GetPool<TransformComponent>().Get(_state.GetEntity("PlayerEntity")).Transform;
    }

    public void InitPool()
    {

    }

    public void ReturnToPool()
    {
        PoolModule.Instance.ReturnToPool(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void OnTriggerEnter(Collider collider)
    {
        StartCoroutine(JumpToPlayer());
    }
    private void OnTriggerStay(Collider collider)
    {
        StartCoroutine(JumpToPlayer());
    }
    IEnumerator JumpToPlayer()
    {
        _collider.enabled = false;
        _collider.isTrigger = true;

        Vector3 startPos = transform.position;
        float timer = 0f;

        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            float progress = timer / 0.5f;

            transform.position = Vector3.Lerp(startPos, _playerTransform.position, progress)
                              + Vector3.up * Mathf.Sin(progress * Mathf.PI) * 1f;

            yield return null;
        }

        Heal();
    }
    private void Heal()
    {
        if(!_state.EcsRunHandler.World.GetPool<HealEvent>().Has(_state.GetEntity("PlayerEntity")))
        {
            _state.EcsRunHandler.World.GetPool<HealEvent>().Add(_state.GetEntity("PlayerEntity"));
        }
        ref var healEvt = ref _state.EcsRunHandler.World.GetPool<HealEvent>().Get(_state.GetEntity("PlayerEntity"));
        healEvt.Heal += Amount;
        _meshRenderer.enabled = false;
        foreach (var particle in _pickUpParticles)
        {
            particle.Play();
        }
        _timer = 0;
        
        _visualDamage.PositiveHealthChange.Spawn(_playerTransform.position + new Vector3(0, 2, 0), healEvt.Heal);
        StartCoroutine(ResolveCollecting());
        
    }
    IEnumerator ResolveCollecting()
    {
        yield return new WaitForSeconds(0.4f);
        foreach (var particle in _pickUpParticles)
        {
            particle.Stop();
        }
        _meshRenderer.enabled = true;
        this.transform.parent = _parentTransform;
        ReturnToPool();
    }


    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.up * Time.deltaTime * _rotationSpeed);
        _timer += Time.deltaTime;
    }
    protected override void Init()
    {

    }

    protected override void Dispose()
    {

    }
    #if UNITY_EDITOR
    private void OnValidate()
    {

        if (!string.IsNullOrEmpty(name))
        {
            KEY_ID = name;
        }
    }

#endif
}
