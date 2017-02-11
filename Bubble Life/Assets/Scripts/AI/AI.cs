using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using NeuralNetwork;

public class AI : BasePlayer {

    public AnimationCurve attackCurve;
    public AnimationCurve avoidCurve;
    public AnimationCurve wanderCurve;

    internal NeuralNet neuralNet;
    internal int index;

    //private AIVisionController aiVision;
    internal float fitness = 0;
    //internal List<double> inputs;

    private float fireTime;
    private float repeatFreq = .1f;
    private float attackValue;
    private float avoidValue;
    private float wanderValue = .5f;

	// Use this for initialization
	void Start () {
        //aiVision = GetComponentInChildren<AIVisionController>();
		//InvokeRepeating ("ScanSurroundings", 0, repeatFreq);
	}
	
	// Update is called once per frame
	void Update(){

        //---------------Create the input list--------------------------------------------\\
        //inputs = aiVision.GetInputs();
        //inputs.Add(1);

        //---------------Get the ouput list by inputing the input list--------------------\\
        //List<double> output =  neuralNet.Update(inputs);
        List<double> output = AIBrain();

        SendFitness();
        SetOutputs(output);
        CheckBorder();
        CheckHealth();

    }

    private List<double> AIBrain()
    {
        List<double> output = new List<double>();
        SetChoiceValues();

        return output;
    }

    private void SetChoiceValues()
    {
        float enemyDistance = GetClosestEnemy();
    }

    private float GetClosestEnemy()
    {
        throw new NotImplementedException();
    }

    private void SetOutputs(List<double> output)
    {
        float transX = (float)(output[0] - output[1]) * turnSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, -transX));

        if (output[2] > .5f && playerFlame.activeSelf == false)
        {
            playerFlame.gameObject.SetActive(true);
            source.Play();
            Move();
        }
        else if (output[2] > .5f)
        {
            Move();
        }
        else if (playerFlame.activeSelf == true)
        {
            playerFlame.gameObject.SetActive(false);
            source.Stop();
        }

        if (output[3] > .5f && fireTime < Time.time)
        {
            Attack();
            fireTime = Time.time + .1f;
        }
    }

    private void SendFitness()
    {
        levelScript.genAlg.population[index] = new Genome(levelScript.genAlg.population[index].weights, fitness);
    }

    protected override void Attack()
    {
        base.Attack();
    }
}
