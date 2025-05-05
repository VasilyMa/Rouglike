using Client;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphingUnitMB : UnitMB // ���� ����
{
    [HideInInspector] public GameObject TeleGO;// ����� ����
    [HideInInspector] public GameObject MegaTeleGO; // ����� ����
    [HideInInspector] public ParticleSystem DeathParticle;
    [HideInInspector] public Color AgroColor;
    [HideInInspector] public Color AvoidColor;
    [HideInInspector] public Color MainAttackColor;
    [HideInInspector] public Color SecondAttackColor;
    private GameObject[] _teleGO;
    private GameObject[] _megaTeleGO;
    private Dictionary<MemberTypes, Transform> _membersOfBody = new Dictionary<MemberTypes, Transform>();
    private Rigidbody _bodyRigidbody;
    public override void Init(int entity)
    {
        base.Init(entity);
        InitMembersOfBody();
        InitTelegraphingPoints();
    }
    public void DeactiveTeleGO() => ProgressingTelegraphing((tele, megaTele) => { tele.SetActive(false); megaTele.SetActive(false); });
    public void DeactiveSmallTeleGO() => ProgressingTelegraphing((tele, megaTele) => tele.SetActive(false));
    public void TelegraphingDanger() => ProgressingTelegraphing((tele, megaTele) => tele.SetActive(true));
    public void MegaTelegraphingDanger()
    {
        ProgressingTelegraphing((tele, megaTele) => { tele.SetActive(false); megaTele.SetActive(true); });
    }
    public void ProgressingTelegraphing(Action<GameObject, GameObject> processFunction)
    {
        if (_teleGO != null)
        {
            for (int i = 0; i < _teleGO.Length; i++)
            {
                processFunction(_teleGO[i], _megaTeleGO[i]);
            }
        }
    }
    public void InitTelegraphingPoints()
    {
        List<Transform> vfxHolders = new List<Transform>();
        if (_membersOfBody.ContainsKey(MemberTypes.LeftHand)) vfxHolders.Add(GetMemberOfBodyByType(MemberTypes.LeftHand));
        if (_membersOfBody.ContainsKey(MemberTypes.RightHand)) vfxHolders.Add(GetMemberOfBodyByType(MemberTypes.RightHand));
        if (TeleGO != null && vfxHolders.Count != 0)
        {
            _teleGO = new GameObject[vfxHolders.Count];
            _megaTeleGO = new GameObject[vfxHolders.Count];
            int i = 0;
            foreach (var item in vfxHolders)
            {
                _teleGO[i] = GameObject.Instantiate(TeleGO, vfxHolders[i].position, Quaternion.identity);
                _megaTeleGO[i] = GameObject.Instantiate(MegaTeleGO, vfxHolders[i].position, Quaternion.identity);
                _teleGO[i].transform.SetParent(vfxHolders[i]);
                _megaTeleGO[i].transform.SetParent(vfxHolders[i]);
                _teleGO[i].SetActive(false);
                _megaTeleGO[i].SetActive(false);
                i++;
            }
        }
    }
    public Transform GetMemberOfBodyByType(MemberTypes type)
    {
        if (_membersOfBody.ContainsKey(type)) return _membersOfBody[type];
        else return null;

    }
    public void PushForceBody(Vector3 direction, ForceMode forceMove )
    {
        if (_bodyRigidbody)
            _bodyRigidbody.AddForce(direction, forceMove);

    }
    public void InitMembersOfBody()
    {
        _membersOfBody.Clear();
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if (child.TryGetComponent<HEAD>(out HEAD head))
            {
                _membersOfBody.Add(MemberTypes.Head, child);
            }
            else if (child.TryGetComponent<BODY>(out BODY body))
            {
                _membersOfBody.Add(MemberTypes.Body, child);
                body.TryGetComponent<Rigidbody>(out _bodyRigidbody);
            }
            else if (child.TryGetComponent<LHAND>(out LHAND lHand))
            {
                _membersOfBody.Add(MemberTypes.LeftHand, child);
            }
            else if (child.TryGetComponent<RHAND>(out RHAND rHand))
            {
                _membersOfBody.Add(MemberTypes.RightHand, child);
            }
        }
    }
    #if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
    }
    #endif
}
public enum MemberTypes
{
    Head, Body, LeftHand, RightHand
}
