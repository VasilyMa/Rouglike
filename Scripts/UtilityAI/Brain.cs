using UnityEngine;

public class Brain : MonoBehaviour {
    public RequiredContext RequiredContext;
}
[System.Flags]
public enum RequiredContext
{
    Nothing = 0,
    General = 1,
    Attacks = 2,
    Support = 4,
    Dodging = 8,
    Everything = 0b1111
}
public enum AIState
{
    Idle,
    MoveTo,
    Attack,
    Defend,
    Support,
    Terrorize,
    KeepAtRange,
    Wandering,
    None
}