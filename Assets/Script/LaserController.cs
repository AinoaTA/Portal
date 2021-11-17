using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public LineRenderer m_LineRenderer;
    public LayerMask m_LayerMask;
    public float m_LaserMax = 250f;

    public virtual void ShootLaser()
    {
        
        Ray l_Ray = new Ray(m_LineRenderer.transform.position, m_LineRenderer.transform.forward);
        float l_Distance = m_LaserMax;
        if (Physics.Raycast(l_Ray, out RaycastHit l_RayCastHit, m_LaserMax, m_LayerMask.value))
        {
            if (l_RayCastHit.collider.CompareTag("Turret"))
            {
                if(m_LineRenderer != l_RayCastHit.collider.GetComponent<Turret>().m_LineRenderer)
                    l_RayCastHit.collider.GetComponent<IDeath>().Death();

            }
            if (l_RayCastHit.collider.CompareTag("Player"))
                l_RayCastHit.collider.GetComponent<IDeath>().Death(); 
            

            l_Distance = l_RayCastHit.distance;
            if (l_RayCastHit.collider.CompareTag("RefractionCube"))
                l_RayCastHit.collider.GetComponent<RefractionCube>().ShootLaser();

            if(l_RayCastHit.collider.GetComponent<ButtonEvent>())
                l_RayCastHit.collider.GetComponent<ButtonEvent>().m_Event?.Invoke();
            
           
        }
        m_LineRenderer.SetPosition(1, new Vector3(0f, 0f, l_Distance));
    }
    
           
}
