using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public enum PortalType
    {
        BLUE=0,
        ORANGE
    }

    public PortalType m_PortalType;

    public Camera m_Camera;
    public Transform m_OtherPortalTransform;
    public Portal m_OtherPortal;
    public float m_NearOffset = 0.3f;
    public PlayerController m_Player;

    private void Update()
    {
        Vector3 l_LocalPosition = m_OtherPortalTransform.InverseTransformPoint(m_Player.m_Camera.transform.position);
        m_OtherPortal.m_Camera.transform.position = m_OtherPortal.transform.TransformPoint(l_LocalPosition);

        Vector3 l_LocalDirection = m_OtherPortalTransform.InverseTransformDirection(m_Player.m_Camera.transform.forward);
        m_OtherPortal.m_Camera.transform.forward = m_OtherPortal.transform.TransformDirection(l_LocalDirection);

        float l_Distance = Vector3.Distance(m_OtherPortal.m_Camera.transform.position, m_OtherPortal.transform.position);
        m_OtherPortal.m_Camera.nearClipPlane = l_Distance + m_NearOffset;
    }
}
