using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This Model will only check if its grounded.
 * A controller will call monobehaviour
 * from BodyPart parent to translate this object.
 */
public class Foot : BodyPart
{
    private const float FOOT_COL_CHECK_DIST = 1;
    private const float FOOT_COL_GROUNDED_DIST = 0.5f;

    public bool grounded { get; private set; }

    public void Update(LayerMask mask)
    {
        RaycastHit hit;
        bool inRange = UnityEngine.Physics.Raycast(this.transform.position, -this.transform.up, out hit, FOOT_COL_CHECK_DIST, mask );
        grounded = inRange && hit.distance <= FOOT_COL_GROUNDED_DIST;
        if (grounded)
            CorrectPosition(hit.point);
    }

    /**
     * Function correct the foot position so clipping is avoided.
     */
    private void CorrectPosition(Vector3 hitPos)
    {
        // TODO: Correct my foot position   
    }

    
}
