using System.Collections.Generic;
using UnityEngine;

public class AreaEffect : MonoBehaviour
{

    public enum AreaEffectType
    {
        Damage,
        Heal
    }

    public enum SpeedModifier
    {
        None,
        Slow,
        Boost
    }

    /// <summary>
    /// Area con collider trigger. Applica danno o cura periodica al giocatore
    /// che ci sta dentro, più effetti di movimento opzionali (scivolamento, rallentamento, accelerazione).
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class AreaEffect : MonoBehaviour
    {
        [Header("Effetto principale")]
        [SerializeField] private AreaEffectType effectType = AreaEffectType.Damage;
        [SerializeField, Tooltip("Danno o cura applicati a ogni tick")] private float amountPerTick = 5f;
        [SerializeField, Tooltip("Secondi tra un tick e il successivo")] private float tickInterval = 1f;

        [Header("Filtro")]
        [SerializeField] private LayerMask playerLayer;

        [Header("Effetti opzionali di movimento")]
        [SerializeField] private bool applySlide;
        [SerializeField] private SpeedModifier speedModifier = SpeedModifier.None;
        [SerializeField, Range(0.1f, 1f)] private float slowMultiplier = 0.5f;
        [SerializeField, Min(1f)] private float boostMultiplier = 1.6f;
        [SerializeField, Tooltip("Durata degli effetti dopo l'ultimo contatto")] private float optionalEffectDuration = 5f;

        // Prossimo istante utile per il tick, per ogni giocatore dentro l'area.
        private readonly Dictionary<Collider, float> nextTickTime = new Dictionary<Collider, float>();

        private void Reset()
        {
            // Imposta il collider come trigger appena aggiungi il componente.
            GetComponent<Collider>().isTrigger = true;
        }

        private bool IsPlayer(Collider other)
        {
            return (playerLayer.value & (1 << other.gameObject.layer)) != 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsPlayer(other)) return;

            nextTickTime[other] = Time.time + tickInterval;
            ApplyOptionalEffects(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!IsPlayer(other)) return;

            // Finché il giocatore è sopra l'area, gli effetti opzionali vengono rinnovati.
            ApplyOptionalEffects(other);

            if (!nextTickTime.TryGetValue(other, out float next))
            {
                next = Time.time + tickInterval;
            }

            if (Time.time < next)
            {
                nextTickTime[other] = next;
                return;
            }

            if (other.TryGetComponent(out Health health) && !health.IsDead)
            {
                if (effectType == AreaEffectType.Damage)
                {
                    health.TakeDamage(amountPerTick);
                }
                else
                {
                    health.Heal(amountPerTick);
                }
            }

            nextTickTime[other] = Time.time + tickInterval;
        }

        private void OnTriggerExit(Collider other)
        {
            nextTickTime.Remove(other);
        }

        private void OnDisable()
        {
            nextTickTime.Clear();
        }

        private void ApplyOptionalEffects(Collider other)
        {
            if (!other.TryGetComponent(out PlayerController player)) return;

            if (applySlide)
            {
                player.ApplySlide(optionalEffectDuration);
            }

            switch (speedModifier)
            {
                case SpeedModifier.Slow:
                    player.ApplySpeedMultiplier(slowMultiplier, optionalEffectDuration);
                    break;
                case SpeedModifier.Boost:
                    player.ApplySpeedMultiplier(boostMultiplier, optionalEffectDuration);
                    break;
            }
        }
    }
}