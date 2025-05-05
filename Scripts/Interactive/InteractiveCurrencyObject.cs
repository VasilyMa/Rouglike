using Client;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Statement;

public class InteractiveCurrencyObject : InteractiveObject, IPool          //  , IItem ???  ����� ��� ����� ��� ���������
{
    public PlayerCurrency.CurrencyType _type;
    [SerializeField] private string _currencyName;
    [Tooltip("0 is not Random")]
    [SerializeField] private ParticleSystem[] _pickUpParticles;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _timer;
    private Collider _collider;
    [HideInInspector] public string CurrencyName => _currencyName;

    protected bool isAvaiable;
    public GameObject ThisGameObject => gameObject;

    public bool IsAvaiable { get => isAvaiable; set => isAvaiable = value; }

    public string PoolKeyID => KEY_ID;
    public string KEY_ID;

    [HideInInspector] public int Amount;
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

    protected override void Init()
    {

    }

    protected override void Dispose()
    {

    }

    protected override void Awake()
    {
        base.Awake();

        _state = State.Instance;
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        _pickUpParticles = GetComponentsInChildren<ParticleSystem>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _visualDamage = ConfigModule.GetConfig<ViewConfig>().VisualDamageConfig;
        _playerTransform = _state.EcsRunHandler.World.GetPool<TransformComponent>().Get(_state.GetEntity("PlayerEntity")).Transform;
    }

    public override void OnTriggerEnter(Collider collider)
    {
        StartCoroutine(JumpToPlayer());
    }
    private void OnTriggerStay(Collider other)
    {
        JumpToPlayer();
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

        Collect();
    }

    public void Collect()
    {
        switch (_type)
        {
            case PlayerCurrency.CurrencyType.Favour:
                PlayerEntity.Instance.Currency.FavourChange(Amount);
                break;
            case PlayerCurrency.CurrencyType.Effigies:
                PlayerEntity.Instance.Currency.EffigiesChange(Amount);
                break;
            case PlayerCurrency.CurrencyType.SkillShard:
                PlayerEntity.Instance.Currency.SkillShardChange(Amount);
                break;

        }

        _meshRenderer.enabled = false;
        foreach (var particle in _pickUpParticles)
        {
            particle.Play();
        }
        _timer = 0;
        _visualDamage.CurrencyPickup.Spawn(transform.position + new Vector3(0, 2, 0), Amount);

        StartCoroutine(ResolveCollecting());
    }
    public void Update()
    {
        this.transform.Rotate(Vector3.up * Time.deltaTime * _rotationSpeed);
        _timer += Time.deltaTime;
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
    public void InitPool()
    {

    }
    public void ReturnToPool()
    {
        PoolModule.Instance.ReturnToPool(this);
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
