using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ldw
{
    // ǻ��ڽ� VFX �����ϴ� �Լ�
    public class FuseBoxController : MonoBehaviour
    {
        public ParticleSystem[] sparkleFuseVFX;
        public ParticleSystem[] switchedOnVFX;
        public ParticleSystem[] switchedOffVFX;

        bool m_FusePresent = false;

        public void Switched(int step)
        {
            if (!m_FusePresent)
                return;

            if (step == 0)
            {
                foreach (var s in switchedOffVFX)
                {
                    s.Play();
                }
            }
            else
            {
                foreach (var s in switchedOnVFX)
                {
                    s.Play();
                }
            }
        }

        public void FuseSocketed(bool socketed)
        {
            m_FusePresent = socketed;

            if (m_FusePresent)
            {
                foreach (var s in sparkleFuseVFX)
                {
                    s.Play();
                }
            }
        }
    }
}