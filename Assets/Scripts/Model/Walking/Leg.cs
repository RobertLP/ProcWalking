using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This model should only contain 1 foot and is reponsible for:
 * - Leg length
 * - Knee bending
 * It shoud also have one foot.
 */
public class Leg : BodyPart
{
    public Foot foot { get; private set; } = new Foot();
}
