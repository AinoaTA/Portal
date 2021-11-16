using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : LaserController, IDeath
{
    public float m_DotAlife = 0.7f;
    public Transform InitialPos;

    public void Death()
    {
        GameController.GetGameController().GetLevelData().m_TurretDestroyed.Add(this);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        float l_DotAngle = Vector3.Dot(transform.up, Vector3.up);
        m_LineRenderer.gameObject.SetActive(l_DotAngle > m_DotAlife);

        ShootLaser();
    }

    public void ResetTurret()
    {
        transform.position = InitialPos.position;
        transform.rotation = InitialPos.rotation;
    }
}
