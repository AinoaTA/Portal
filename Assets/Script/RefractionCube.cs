using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractionCube : LaserController
{
    public bool m_IsRefracting=false;

    public Vector3 InitialPos;
    public Quaternion InitialRot;
    void Start()
    {
        InitialPos = transform.position;
        InitialRot = transform.rotation;
    }
    public override void ShootLaser()
    {
        if (m_IsRefracting)
            return;
        m_IsRefracting = true;
        m_LineRenderer.gameObject.SetActive(true);
        base.ShootLaser();
        
    }

    private void Update()
    {
        m_IsRefracting = false;
        m_LineRenderer.gameObject.SetActive(false);
    }

    public void ResetRefractions()
    {
        transform.position = InitialPos;
        transform.rotation = InitialRot;
    }
}
