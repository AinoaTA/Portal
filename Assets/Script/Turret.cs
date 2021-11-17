using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : LaserController, IDeath
{
    public float m_DotAlife = 0.7f;
    private Vector2 InitialPos;
    private Quaternion InitialRot;
    public Transform Parent;


    void Start()
    {
        InitialPos = transform.position;
        InitialRot = transform.rotation;
    }

    public void Death()
    {
        GameController.GetGameController().GetLevelData().m_TurretDestroyed.Add(this);
        transform.SetParent(Parent);
        StartCoroutine(DelayDeath());
    }

    public IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds(0.2f);
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
        transform.position = InitialPos;
        transform.rotation = InitialRot;
    }
}
