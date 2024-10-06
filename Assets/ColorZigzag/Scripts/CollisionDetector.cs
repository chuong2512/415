using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour {
    
    private Actions.VoidCollision onCollision;
    private Actions.VoidVoid onCollisionExit;

    private Actions.VoidCollider onTrigger;

	private void OnCollisionEnter (Collision collision) {

        if (onCollision != null) {

            onCollision (collision);
        }
    }

    private void OnCollisionStay (Collision collision) {

        OnCollisionEnter (collision);
    }

    private void OnCollisionExit () {

        if (onCollisionExit != null) {

            onCollisionExit ();
        }
    }

    private void OnTriggerEnter (Collider collider) {

        if (onTrigger != null) {

            onTrigger (collider);
        }
    }

    public CollisionDetector SetListeners (Actions.VoidCollision _onCollision = null, Actions.VoidVoid _onCollisionExit = null) {

        onCollision = _onCollision;
        onCollisionExit = _onCollisionExit;

        return this;
    }

    
    public CollisionDetector SetTriggerListeners (Actions.VoidCollider _onTrigger = null) {

        onTrigger = _onTrigger;

        return this;
    }
}
