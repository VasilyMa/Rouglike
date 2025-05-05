using System.Collections.Generic;

using UnityEngine;

using UniRx;

public class InterfaceObserver : MonoBehaviour
{
    public CompositeDisposable m_Disposable;
    public List<AbilityObserver> m_AbilitiesObserver;
    public PlayerObserver m_PlayerObserver;

    public void Init()
    {
        m_Disposable = new CompositeDisposable();
    }

    private void OnDestroy()
    {
        m_Disposable.Dispose();
    }
}
