using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class ProjectileControler : MonoBehaviour
    {
        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private Vector2 m_Direction;
        [SerializeField] private int m_Damage;

        private bool m_FromPlayer;
        // private SpawnManager m_SpawnManager;
        private float m_LifeTime;
        private float m_CurMovSpeed;
        // Start is called before the first frame update
        void Start()
        {
            // m_SpawnManager = FindObjectOfType<SpawnManager>();
        }

        // Update is called once per frame
        void Update()
        {
            // di chuyen dan theo huong va toc do
            transform.Translate(m_Direction * m_CurMovSpeed * Time.deltaTime);
            if (m_LifeTime <= 0)
            {
                Realese();
                
            }
            m_LifeTime -= Time.deltaTime;
        }

        public void Fire(float speedmultiplayer)
        {
            m_LifeTime = 10f / speedmultiplayer;
            m_CurMovSpeed = speedmultiplayer * m_MoveSpeed;
        }

        private void Realese()
        {
            if (m_FromPlayer)
                SpawnManager.instance.RealeasePlayerProjectile(this);
            else
                SpawnManager.instance.RealeaseEnemyProjectile(this);
        }

        public void SetFromPlayer(bool value)
        {
            m_FromPlayer = value;
        }
        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    Debug.Log(collision.gameObject.name);
        //}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Trigger" + collision.gameObject.name);

            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (m_FromPlayer)
                {
                    SpawnManager.instance.RealeasePlayerProjectile(this);
                }
                else
                    SpawnManager.instance.RealeaseEnemyProjectile(this);

                Vector3 hitPos = collision.ClosestPoint(transform.position);
                SpawnManager.instance.SpawnHitFx(hitPos);

                EnemyControler enemy;
                collision.gameObject.TryGetComponent(out enemy);
                enemy.Hit(m_Damage);
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                if (m_FromPlayer)
                {
                    SpawnManager.instance.RealeasePlayerProjectile(this);
                }
                else
                    SpawnManager.instance.RealeaseEnemyProjectile(this);

                Vector3 hitPos = collision.ClosestPoint(transform.position);
                SpawnManager.instance.SpawnHitFx(hitPos);

                PlayerControler player;
                collision.gameObject.TryGetComponent(out player);
                player.Hit(m_Damage);
            }
        }


    }
}
