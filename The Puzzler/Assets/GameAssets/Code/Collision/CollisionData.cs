using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColiderType
{
    PLAYER,
    ENVIROMENT,
    PROP,
    TRIGGER
}

// this is a class a struct is not nullable
// http://answers.unity3d.com/questions/362799/nullable-struct.html
public class CollisionData
{
    public int m_id;
    public ColiderType m_type;

    public float m_posX;
    public float m_posY;
    public float m_width;
    public float m_heght;

    public float m_newPosX;
    public float m_newPosY;

    public bool m_collisionTop;

    public float m_colisionPosX;
    public float m_colisionPosY;

    public bool m_colidedHorizontal;
    public bool m_colidedVertical;
}

