using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    // Start is called before the first frame update
    bool m_TeleporteActive = true;
    Rigidbody rb;
    public float m_DotToEnterPortal = 0.5f;
    public Vector3 InitialPos;
    public Quaternion InitialRot;

    public Transform Parent;
    void Start()
    {
        InitialPos = transform.position;
        InitialRot = transform.rotation;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public bool CanBeTeleported()
    {
        return m_TeleporteActive;
    }

    public void SetTelportActive(bool TeleportActive)
    {
        m_TeleporteActive = TeleportActive;
    }

    public void Teleport(Portal PortalToTeleported)
    {
        if (CanBeTeleported())
        {
            Vector3 l_PortalZX = PortalToTeleported.transform.forward;
            l_PortalZX.y = 0;
            l_PortalZX.Normalize();
            Vector2 l_VelocityXZ = rb.velocity;
            l_VelocityXZ.y = 0;
            l_VelocityXZ.Normalize();
            float l_Dot = Vector3.Dot(l_PortalZX, -l_VelocityXZ);

            print(l_Dot + "y " + m_DotToEnterPortal);
            if (l_Dot > m_DotToEnterPortal)
            {
                print("cojones");
                Vector3 l_Velocity = PortalToTeleported.m_OtherPortalTransform.transform.InverseTransformDirection(rb.velocity);
                rb.isKinematic = true;
                Vector3 l_LocalPos = PortalToTeleported.transform.InverseTransformPoint(transform.position);
                transform.position = PortalToTeleported.m_OtherPortal.transform.TransformPoint(l_LocalPos);
                rb.isKinematic = false;
                rb.velocity = PortalToTeleported.m_OtherPortal.transform.TransformDirection(l_Velocity);
                transform.localScale *= (PortalToTeleported.m_OtherPortal.transform.localScale.x / PortalToTeleported.transform.localScale.x);

            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
            Teleport(other.GetComponent<Portal>());
    }

    public void ResetCompanion()
    {
        transform.position= InitialPos;
        transform.rotation=InitialRot;
     }
}
