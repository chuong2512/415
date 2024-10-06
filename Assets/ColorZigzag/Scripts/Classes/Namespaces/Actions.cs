using UnityEngine;
using System.Collections;

namespace Actions {
    
    public delegate void VoidVoid ();
    public delegate void VoidFloat (float f);
    public delegate void VoidInt (int i);
    public delegate float FloatFloat (float f);
    public delegate void VoidVector2 (Vector2 v);
    public delegate void VoidGameObject (GameObject go);
    public delegate GameObject GameObjectVoid ();
    public delegate void VoidAction (VoidVoid a);
    public delegate bool BoolVoid ();
    
    public delegate void VoidCollision (Collision c);
    public delegate void VoidCollider (Collider c);
}
