using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class BackGroundControler : MonoBehaviour
    {
        [SerializeField] private Material m_BigStarBg;
        [SerializeField] private Material m_MedStarBg;
        [SerializeField] private Material m_NebulaBg;
        [SerializeField] private float m_BigStarBgScrollSpeed;
        [SerializeField] private float m_MedStarBgScrollSpeed;
        [SerializeField] private float m_NebulaBgSpeed;

        private int m_MainTexId;

        // Start is called before the first frame update
        void Start()
        {
            m_MainTexId = Shader.PropertyToID("_MainTex");
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 offset = m_BigStarBg.GetTextureOffset(m_MainTexId);
            offset += new Vector2(0, m_BigStarBgScrollSpeed * Time.deltaTime);
            m_BigStarBg.SetTextureOffset(m_MainTexId, offset);

            offset = m_MedStarBg.GetTextureOffset(m_MainTexId);
            offset += new Vector2(0, m_MedStarBgScrollSpeed * Time.deltaTime);
            m_MedStarBg.SetTextureOffset(m_MainTexId, offset);

            offset = m_NebulaBg.GetTextureOffset(m_MainTexId);
            offset += new Vector2(0, m_NebulaBgSpeed * Time.deltaTime);
            m_NebulaBg.SetTextureOffset(m_MainTexId, offset);
        }
    }
}