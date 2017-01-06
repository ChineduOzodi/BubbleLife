using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIVisionController : MonoBehaviour {

    public AIVisionBlock[] vision;
	
	// Update is called once per frame
	public List<double> GetInputs()
    {
        List<double> inputs = new List<double>();
        foreach (AIVisionBlock block in vision)
        {
            inputs.Add(block.input);
            inputs.Add(block.vel.x);
            inputs.Add(block.vel.y);
            inputs.Add(block.distance);
        }

        return inputs;
    }
}
