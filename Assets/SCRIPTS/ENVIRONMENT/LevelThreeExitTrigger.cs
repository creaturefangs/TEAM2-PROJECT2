using UnityEngine;

public class LevelThreeExitTrigger : MonoBehaviour
{
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.enabled = false;
    }

    public void OnBossDeathEnableCollider()
    {
        _boxCollider.enabled = true;
    }
}
