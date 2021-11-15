using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : LaserController, IDeath
{
    public float m_DotAlife = 0.7f;

    public void Death()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        float l_DotAngle = Vector3.Dot(transform.up, Vector3.up);
        m_LineRenderer.gameObject.SetActive(l_DotAngle > m_DotAlife);

        ShootLaser();
    }
}
