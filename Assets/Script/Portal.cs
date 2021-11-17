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
    public List<Transform> ValidPoints;

    [SerializeField]private float m_Distance=200f;
    [SerializeField]private float m_ValidDot= 0.99f;
    [SerializeField] private float m_MaxValidDistance = 0.1f;
    public LayerMask m_LayerMask;

    public GameObject Renderer;

    private void Update()
    {
        Vector3 l_LocalPosition = m_OtherPortalTransform.InverseTransformPoint(m_Player.m_Camera.transform.position);
        m_OtherPortal.m_Camera.transform.position = m_OtherPortal.transform.TransformPoint(l_LocalPosition);

        Vector3 l_LocalDirection = m_OtherPortalTransform.InverseTransformDirection(m_Player.m_Camera.transform.forward);
        m_OtherPortal.m_Camera.transform.forward = m_OtherPortal.transform.TransformDirection(l_LocalDirection);

        float l_Distance = Vector3.Distance(m_OtherPortal.m_Camera.transform.position, m_OtherPortal.transform.position);
        m_OtherPortal.m_Camera.nearClipPlane = l_Distance + m_NearOffset;
    }

    public bool IsValidPosition(Vector3 Position, Vector3 Normal)        
    {
        transform.position = Position;
        transform.rotation = Quaternion.LookRotation(Normal);

        for (int i = 0; i < ValidPoints.Count; i++)
        {
            Vector3 l_Camera = m_Player.m_Camera.transform.position;
            Vector3 l_Direction = ValidPoints[i].transform.position - l_Camera;
            l_Direction.Normalize();
            Ray l_Ray = new Ray(l_Camera, l_Direction);
            RaycastHit l_RaycastHit;
            if (Physics.Raycast(l_Ray, out l_RaycastHit, m_Distance, m_LayerMask))
            {
                if (l_RaycastHit.collider.CompareTag("WallDrawable"))
                {
                    if (Vector3.Dot(Normal, l_RaycastHit.normal) > m_ValidDot)
                    {
                        float l_Distance = Vector3.Distance(ValidPoints[i].position, l_RaycastHit.point);
                        if (l_Distance >= m_MaxValidDistance)
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }
        return true;
    }

    public void ResetPortal()
    {
        gameObject.SetActive(false);
    }
}
