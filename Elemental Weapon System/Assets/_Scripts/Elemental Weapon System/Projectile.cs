using UnityEngine;
using Elemental.Main;


namespace Elemental.WeaponSystem
{
    public class Projectile : MonoBehaviour
    {
        #region Construction Data

        public MeshRenderer[] _meshRenderers;
        private float _damage;
        private float _areaOfImpactRadius;
        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _direction;
        private LayerMask _layersToHit;
        [SerializeField] private Vector3 _spawnPosi;
        //private WeaponType Weapon;

        #endregion

        #region Local Variables

        private Rigidbody _rigidbody;

        [SerializeField] private bool _isMoving;
        private float _distanceTravelled;
        [SerializeField] private float _timeAlive;

        #endregion

        public void FeedData(float damage, float areaOfImpactRadius, float speed, Vector3 direction,
            LayerMask layersToHit,
            Vector3 spawnPosi)
        {
            _damage = damage;
            _areaOfImpactRadius = areaOfImpactRadius;
            _speed = speed;
            _direction = direction;
            _layersToHit = layersToHit;
            _spawnPosi = spawnPosi;
        }

        private void Start()
        {
            _isMoving = true;

            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_isMoving)
                _distanceTravelled = Vector3.Distance(_spawnPosi, transform.position);

            _timeAlive += Time.deltaTime;

            if (_timeAlive >= 35)
                Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if (_isMoving)
                _rigidbody.MovePosition(_rigidbody.position + _direction * (_speed * Time.fixedDeltaTime));
        }


        private void OnCollisionEnter(Collision other)
        {
            if (_layersToHit == (_layersToHit | 1 << other.gameObject.layer))
            {
                /*transform.GetComponent<MeshRenderer>().enabled = false;

                if (transform.GetComponentInChildren<MeshRenderer>() != null)
                    transform.GetComponentInChildren<MeshRenderer>().enabled = false;*/

                int maxColliders = 10;
                Collider[] hitColliders = new Collider[maxColliders];
                Physics.OverlapSphereNonAlloc(transform.position, _areaOfImpactRadius, hitColliders);

                foreach (Collider collider in hitColliders)
                {
                    if (collider != null && !collider.isTrigger)
                    {
                        if (collider.TryGetComponent(out UnitHealth TargetHealth))
                            TargetHealth.ReceiveDamage(_damage);


                        //Destroy(gameObject);
                    }
                }

                _isMoving = false;
                _rigidbody.isKinematic = true;
            }
        }
    }
}