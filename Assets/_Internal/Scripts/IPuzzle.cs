using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("s"))
        {
            IPuzzle CheckingPuzzle = new BasePuzzle();
            CheckingPuzzle.Condition.CheckIfPassed();
        }
        
    }

    public class BasePuzzle : IPuzzle
    {
        public ICondition Condition { get; set;}


        public void GiveReward()
        {
             Console.WriteLine("Puzzle Passed");
        }

        public bool IsComplited()
        {
            throw new System.NotImplementedException();
        }
    }

    public class BaseCondition : ICondition

    {
        public bool CheckIfPassed()
        {
             throw new System.NotImplementedException();
        }
    }


    public interface IPuzzle 
    {
        bool IsComplited();

        void GiveReward();

        ICondition Condition { get; set; }
    }

    public interface ICondition 
    {
         bool CheckIfPassed();

    }

}

