using UnityEngine;
using UnityEngine.Events;


namespace Elemental.Main
{
    public class UnitHealth : MonoBehaviour
    {
        #region Local Variables

        [SerializeField] private float currentHealth = 100;
        [SerializeField] private float maxHealth = 100;

        private bool isDead;

        #endregion

        #region Public properties

        /// <summary>
        /// Object or Character's current Health Points
        /// </summary>
        public float CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = Mathf.Max(0, value, maxHealth); }
        }


        /// <summary>
        /// Object or Character's Maximum Health Points
        /// </summary>
        public float MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }


        /// <summary>
        /// Object or Character's eliminated state boolean
        /// </summary>
        public bool IsDead
        {
            get { return isDead; }
            set { IsDead = value; }
        }

        #endregion

        #region Unity Events

        [HideInInspector] public UnityEvent OnHealthGainEvent;
        [HideInInspector] public UnityEvent OnTakeDamageEvent;
        [HideInInspector] public UnityEvent OnDeathEvent;

        #endregion


        /// <summary>
        /// Reduce Current Health as well as invoke OnDamage Events
        /// </summary>
        /// <param name="DamageAmt"></param>
        public void ReceiveDamage(float DamageAmt)
        {
            if (isDead)
                return;


            OnTakeDamageEvent?.Invoke();

            CurrentHealth -= DamageAmt;

            if (CurrentHealth <= 0)
            {
                IsDead = true;

                OnDeathEvent?.Invoke();
                OnTakeDamageEvent.RemoveAllListeners();
                OnDeathEvent.RemoveAllListeners();
            }
        }
    }
}