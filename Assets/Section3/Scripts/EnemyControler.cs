using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class EnemyControler : MonoBehaviour
    {
        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private Transform[] m_WayPoints;
        [SerializeField] private ProjectileControler m_Projectile;
        [SerializeField] private Transform m_FiringPoint;
        [SerializeField] private float m_MinFiringCooldown;
        [SerializeField] private float m_MaxFiringCooldown;
        [SerializeField] private int m_Hp;

        private int m_CurrentHp;
        private float m_TempCooldown;
        private int m_CurrentWayPointIndex;
        private bool m_Active;
        private float m_CurMoveSpeed;
        private float m_SpeedMultiplayer;

        // private SpawnManager m_SpawnManager;
        // private GameManager m_GameManager;
        private AudioManager m_AudioManager;

        // Start is called before the first frame update
        void Start()
        {
            // m_SpawnManager = FindObjectOfType<SpawnManager>();
            // m_GameManager = FindObjectOfType<GameManager>();
            // m_AudioManager = FindObjectOfType<AudioManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if(!m_Active)
                return;
            int nextWayPoint = m_CurrentWayPointIndex + 1;
            // neu diem tiep theo la cuoi cung thi quay lai diem bat dau
            if (nextWayPoint > m_WayPoints.Length - 1)
                nextWayPoint = 0;

            transform.position = Vector3.MoveTowards(transform.position, m_WayPoints[nextWayPoint].position, m_CurMoveSpeed * Time.deltaTime);
            // kiem tra xem enemy da den diem tiep theo chua
            if (transform.position == m_WayPoints[nextWayPoint].position)
            {
                m_CurrentWayPointIndex = nextWayPoint;
            }

            // vector huong tu vi tri hien tai toi vi tri tiep theo 
            Vector3 direction = m_WayPoints[nextWayPoint].position - transform.position;

            // tinh goc giua vector direction va truc x
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // cap nhat goc quay theo huong di chuyen cua enemy
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

            
                if (m_TempCooldown <= 0)
                {
                    Fire();
                    m_TempCooldown = Random.Range(m_MinFiringCooldown,m_MaxFiringCooldown);
                }
            
                m_TempCooldown -= Time.deltaTime;
        }

        public void Init(Transform[] Waypoints, float speedmultiplayer)
        {
            m_WayPoints = Waypoints;
            m_SpeedMultiplayer = speedmultiplayer;
            m_CurMoveSpeed = m_MoveSpeed * speedmultiplayer;
            m_Active = true;
            transform.position = m_WayPoints[0].position;
            m_TempCooldown = Random.Range(m_MinFiringCooldown, m_MaxFiringCooldown) / speedmultiplayer;
            m_CurrentHp = m_Hp;
        }

        public void Fire()
        {
            ProjectileControler projectile = SpawnManager.instance.SpawnEnemyProjectile(m_FiringPoint.position);
            projectile.Fire(m_SpeedMultiplayer);

            AudioManager.Instance.PlayPlasmaSFX();
        }

        public void Hit(int damage)
        {
            m_CurrentHp -= damage;
            if(m_CurrentHp <= 0)
            {
                //Destroy(gameObject);
                SpawnManager.instance.RealeaseEnemyControler(this);
                // m_GameManager.Addscore(1);
                GameManager.instance.Addscore(1);
                AudioManager.Instance.PlayExplosionSFX();
            }
            AudioManager.Instance.PlayHitSFX();
        }
    }
}
