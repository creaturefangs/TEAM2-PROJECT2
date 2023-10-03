using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillstreakManager : MonoBehaviour
{
    public KillstreakSO[] killStreaks;
    public UnityEvent killStreakEvent;

    public void GrantKillstreak()
    {
        killStreakEvent?.Invoke();
    }
}
