using System;
using UnityEngine;
// per utilizzare un tipo di metodo abstact, bisogna usare una classe con il tipo abstact
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float cooldown = 1f;
    [SerializeField] protected float damage = 1f;
    [SerializeField] protected float range = 1f;
    protected AutoTargeter targeter;
    private float timer;    // non modifidcabile
    protected virtual bool NeedsTarget => true;     // serve per alcune armi
    protected abstract void Fire();     // metodo dichiarato nella classe padre

    protected virtual void Awake()
    {
        // non sa se è su un player o su una torre:
        // gli basta che ci sia un AutoTargeter
        if (targeter == null)
        {
            targeter = GetComponent<AutoTargeter>();
        }
    }

    protected virtual void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0) return;

        if (NeedsTarget && targeter.CurrentTarget == null) return;
        timer = cooldown;
        Fire(); // quale Fire? Lo decide la classe vera
    }
}
