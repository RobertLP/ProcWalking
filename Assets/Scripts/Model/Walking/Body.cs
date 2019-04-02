using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class only contains legs and should only contain 
 * the methods to translate based on information of an leg
 */
public class Body : BodyPart
{
    public Leg[] Legs { get; private set; } = { new Leg(), new Leg() };
}
