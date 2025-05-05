using AbilitySystem;
using Client;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using Statement;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class InteractionEnvironment : MonoBehaviour
{
    public InteractionType interactionType;
    [ShowIf("interactionType", InteractionType.Destructible)]
    [SerializeField] private Transform targetEnvironment;
    [ShowIf("interactionType", InteractionType.Lustre)]
    [SerializeField] private SourceParticle hitParticle;
    [ShowIf("interactionType", InteractionType.Lustre)]
    [SerializeField] private int Damage;

    private float fadeOutDuration = 10f;

    public void Start()
    {
        if (interactionType == InteractionType.Destructible)
            gameObject.tag = "InteractiveEnvironment";
    }
    public void Demolish() /////////////////TEST/////////////////
    {
        switch (interactionType)
        {
            case InteractionType.Destructible:
                if (targetEnvironment.TryGetComponent<InteractionEnvironment>(out InteractionEnvironment interactionEnvironment))
                {
                    hitParticle = GameObject.Instantiate(hitParticle, gameObject.transform.position, Quaternion.identity);
                    hitParticle.gameObject.SetActive(true);
                    interactionEnvironment.Demolish();
                    gameObject.SetActive(false);
                    if(gameObject.activeSelf) StartCoroutine(FadeOutAndDeactivate());
                }
                break;
            case InteractionType.Lustre:
                if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                {
                    if (gameObject.activeSelf) StartCoroutine(FadeOutAndDeactivate());
                    rigidbody.isKinematic = false;
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude < 0.1f) return;
        if (interactionType == InteractionType.Destructible) return;

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            var unitMB = other.GetComponent<UnitMB>();
            State.Instance.EcsRunHandler.World.GetPool<TakeDamageComponent>().Add(unitMB._entity).Damage = Damage;
            hitParticle = GameObject.Instantiate(hitParticle, other.transform.position, Quaternion.identity);
            hitParticle.gameObject.SetActive(true);
            if (gameObject.activeSelf) StartCoroutine(FadeOutAndDeactivate());
        }
    }

    private IEnumerator FadeOutAndDeactivate()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hitParticle.Dispose();
        gameObject.SetActive(false);
    }

    public enum InteractionType
    {
        Lustre,
        Destructible
    }
}
