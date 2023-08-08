using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class ParticalFx : MonoBehaviour
    {
        [SerializeField] private float m_LifeTime;

        private float m_CurrentLifeTime;
        private ParticalFxPool m_Pool;

        private void OnEnable()
        {
            m_CurrentLifeTime = m_LifeTime;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(m_CurrentLifeTime <= 0)
            {
                m_Pool.Realease(this);
            }
            m_CurrentLifeTime -= Time.deltaTime;
        }

        public void Setpool(ParticalFxPool pool)
        {
            m_Pool = pool;
        }
    }
}
