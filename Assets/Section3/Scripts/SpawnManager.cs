using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{

    [System.Serializable]
    public class EnemiesPool
    {
        public EnemyControler prefab;
        public List<EnemyControler> inactiveObject;
        public List<EnemyControler> activeObject;
        public EnemyControler Spawn(Vector3 position, Transform parent)
        {
            if(inactiveObject.Count == 0)
            {
                EnemyControler newObj = GameObject.Instantiate(prefab, parent);
                newObj.transform.position = position;
                activeObject.Add(newObj);
                return newObj;
            }
            else
            {
                EnemyControler oldObj = inactiveObject[0];
                oldObj.gameObject.SetActive(true);
                oldObj.transform.SetParent(parent);
                oldObj.transform.position = position;
                activeObject.Add(oldObj);
                inactiveObject.RemoveAt(0);
                return oldObj;
            }
        }

        public void Realease(EnemyControler obj)
        {
            if (activeObject.Contains(obj))
            {
                activeObject.Remove(obj);
                inactiveObject.Add(obj);
                obj.gameObject.SetActive(false);
            }
        }

        public void clear()
        {
            while (activeObject.Count > 0)
            {
                EnemyControler obj = activeObject[0];
                obj.gameObject.SetActive(false);
                activeObject.RemoveAt(0);
                inactiveObject.Add(obj);
            }
        }
    }

    [System.Serializable]
    public class ProjectilesPool
    {
        public ProjectileControler prefab;
        public List<ProjectileControler> inactiveObject;
        public List<ProjectileControler > activeObject;
        public ProjectileControler Spawn(Vector3 position, Transform parent)
        {
            if (inactiveObject.Count == 0)
            {
                ProjectileControler newObj = GameObject.Instantiate(prefab, parent);
                newObj.transform.position = position;
                activeObject.Add(newObj);
                return newObj;
            }
            else
            {
                ProjectileControler oldObj = inactiveObject[0];
                oldObj.gameObject.SetActive(true);
                oldObj.transform.SetParent(parent);
                oldObj.transform.position = position;
                activeObject.Add(oldObj);
                inactiveObject.RemoveAt(0);
                return oldObj;
            }
        }
        public void Realease(ProjectileControler obj)
        {
            if (activeObject.Contains(obj))
            {
                activeObject.Remove(obj);
                inactiveObject.Add(obj);
                obj.gameObject.SetActive(false);
            }
        }
        public void clear()
        {
            while (activeObject.Count > 0)
            {
                ProjectileControler obj = activeObject[0];
                obj.gameObject.SetActive(false);
                activeObject.RemoveAt(0);
                inactiveObject.Add(obj);
            }
        }

    }

    [System.Serializable]
    public class ParticalFxPool
    {
        public ParticalFx prefab;
        public List<ParticalFx> inactiveObject;
        public List<ParticalFx> activeObject;
        public ParticalFx Spawn(Vector3 position, Transform parent)
        {
            if (inactiveObject.Count == 0)
            {
                ParticalFx newObj = GameObject.Instantiate(prefab, parent);
                newObj.transform.position = position;
                activeObject.Add(newObj);
                return newObj;
            }
            else
            {
                ParticalFx oldObj = inactiveObject[0];
                oldObj.gameObject.SetActive(true);
                oldObj.transform.SetParent(parent);
                oldObj.transform.position = position;
                activeObject.Add(oldObj);
                inactiveObject.RemoveAt(0);
                return oldObj;
            }
        }
        public void Realease(ParticalFx obj)
        {
            if (activeObject.Contains(obj))
            {
                activeObject.Remove(obj);
                inactiveObject.Add(obj);
                obj.gameObject.SetActive(false);
            }
        }

        public void clear()
        {
            while(activeObject.Count > 0)
            {
                ParticalFx obj = activeObject[0];
                obj.gameObject.SetActive(false);
                activeObject.RemoveAt(0);
                inactiveObject.Add(obj);
            }
        }
    }
    public class SpawnManager : MonoBehaviour
    {
        private static SpawnManager m_Instance;
        public static SpawnManager instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = FindObjectOfType<SpawnManager>();
                }
                return m_Instance;
            }
        } 
        [SerializeField] private bool m_Active;
        ///[SerializeField] private EnemyControler m_EnemyPreFab;
        [SerializeField] private EnemiesPool m_EnemiesPool;
        [SerializeField] private ProjectilesPool m_PlayerProjectilesPool;
        [SerializeField] private ProjectilesPool m_EnemyProjectilesPool;
        [SerializeField] private int m_MinTotalEnemies;
        [SerializeField] private int m_MaxTotalEnemies;
        [SerializeField] private float m_EnemySpawnInteval;
        [SerializeField] private EnemyPath[] m_Path;
        [SerializeField] private int m_TotalGroup;
        [SerializeField] private ParticalFxPool m_HitFxPool;
        [SerializeField] private ParticalFxPool m_ShootingFxPool;
        [SerializeField] private PlayerControler m_PlayerControlerPrefab;

        public PlayerControler Player => m_Player;

        private bool m_IsSpawn;
        private PlayerControler m_Player;
        private WaveData m_Curwave;
        // Start is called before the first frame update

        private void Awake()
        {
            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject); 
        }

        public void StartBattle(WaveData wave, bool ResetPosition)
        {
            m_Curwave = wave;
            m_MinTotalEnemies = m_Curwave.minTotalEnemies;
            m_MaxTotalEnemies = m_Curwave.maxTotalEnemies;
            m_TotalGroup = m_Curwave.totalGroup;

            if (m_Player == null)
                m_Player = Instantiate(m_PlayerControlerPrefab);
            if(ResetPosition)
                m_Player.transform.position = Vector3.zero;
            StartCoroutine(IESpawnGroup(m_TotalGroup));
        }
        // Update is called once per frame
       

        private IEnumerator IESpawnGroup(int SpGroups)
        {
            m_IsSpawn = true;
            for(int i = 0; i < SpGroups; i++)
            {
                int TotalEnemies = Random.Range(m_MinTotalEnemies, m_MaxTotalEnemies);
                EnemyPath path = m_Path[Random.Range(0, m_Path.Length)];
                yield return StartCoroutine(IESpawnEnemies(TotalEnemies,path));
                if(i < SpGroups - 1)
                    yield return new WaitForSeconds(2f / m_Curwave.speedMultiplayer);
            }
            m_IsSpawn = false;
        }
        private IEnumerator IESpawnEnemies(int TotalEnemies, EnemyPath path)
        {
          
            for(int i=0;i< TotalEnemies; i++)
            {
                yield return new WaitUntil(() => m_Active);
                yield return new WaitForSeconds(m_EnemySpawnInteval / m_Curwave.speedMultiplayer);

                //EnemyControler enemy =  Instantiate(m_EnemyPreFab, transform);
                EnemyControler enemy = m_EnemiesPool.Spawn(path.WayPoints[0].position, transform);
                enemy.Init(path.WayPoints,m_Curwave.speedMultiplayer);
            }
        }

        public void RealeaseEnemy(EnemyControler obj)
        {
            m_EnemiesPool.Realease(obj);
        }

        public void RealeaseEnemyControler(EnemyControler enemy)
        {
            m_EnemiesPool.Realease(enemy);
        }

        public ProjectileControler SpawnEnemyProjectile(Vector3 position)
        {
            ProjectileControler obj = m_EnemyProjectilesPool.Spawn(position, transform);
            obj.SetFromPlayer(false);
            return obj;
        }

        public void RealeaseEnemyProjectile(ProjectileControler projectile)
        {
            m_EnemyProjectilesPool.Realease(projectile);
        }

        public ProjectileControler SpawnPlayerProjectile(Vector3 position)
        {
            ProjectileControler obj = m_PlayerProjectilesPool.Spawn(position, transform);
            obj.SetFromPlayer(true);
            return obj;
        }

        public void RealeasePlayerProjectile(ProjectileControler projectile)
        {
            m_PlayerProjectilesPool.Realease(projectile);
        }

        public ParticalFx SpawnHitFx(Vector3 position)
        {
           ParticalFx fx =  m_HitFxPool.Spawn(position, transform);
            fx.Setpool(m_HitFxPool);
            return fx;
        }

        public void RealeaseHitFx(ParticalFx obj)
        {
            m_HitFxPool.Realease(obj);
        }

        public ParticalFx SpawnShootingFx(Vector3 position)
        {
            ParticalFx fx = m_ShootingFxPool.Spawn(position, transform);
            fx.Setpool(m_ShootingFxPool);
            return fx;
        }

        public void RealeaseShootingFx(ParticalFx obj)
        {
            m_ShootingFxPool.Realease(obj);
        }

        public bool isclear()
        {
            if(m_IsSpawn || m_EnemiesPool.activeObject.Count > 0)
            {
                return false;
            }
            return true;
        }

        public void Clear()
        {
            m_EnemiesPool.clear();
            m_EnemyProjectilesPool.clear();
            m_HitFxPool.clear();
            m_PlayerProjectilesPool.clear();
            m_ShootingFxPool.clear();
            StopAllCoroutines();
        }
    }
}
