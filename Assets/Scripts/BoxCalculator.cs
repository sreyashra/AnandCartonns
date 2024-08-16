using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class BoxCalculator : MonoBehaviour
{
    //Box properties and dimension variables
    private float _boxLength = 23f;
    private float _boxBreadth = 15f;
    private float _boxHeight = 12f;
    private float _boxGSM = 200f;
    private int _numberOfPly = 5;
    private float _paperCost = 33.5f;

    void Start()
    {
        CalculateBoxWeight();
        CalculateBoxCost();
        Debug.Log(CalculateBoxCost());
    }

    void Update()
    {
        
    }

    float CalculateReelSize(float breadth, float height)
    {
        return breadth + height;
    }

    float CalculateCuttingSize(float length, float breadth)
    { 
        return length + breadth;
    }

    float CalculateBoxGSMSmallFlute(float gsm)
    {
        float FinalGSM;
        switch (_numberOfPly)
        {
            case 3:
                FinalGSM = gsm + (gsm + (gsm * 0.4f)) + gsm;
                break;

            case 5:
                FinalGSM = gsm + (gsm + (gsm * 0.4f)) + gsm + (gsm + (gsm * 0.4f)) + gsm;
                break;

            case 7:
                FinalGSM = gsm + (gsm + (gsm * 0.4f)) + gsm + (gsm + (gsm * 0.4f)) + gsm + (gsm + (gsm * 0.4f)) + gsm;
                break;

            default:
                return FinalGSM = 0;
        }
        return FinalGSM;
    }

    float CalculateBoxWeight()
    {
        float flopAllowance = 2f;
        float cuttingAllowance = 1.2f;

        float reelSize = CalculateReelSize(_boxBreadth, _boxHeight) + flopAllowance;
        float cuttingSize = CalculateCuttingSize(_boxLength, _boxBreadth) + cuttingAllowance;

        float BoxWeight = (((reelSize * cuttingSize * CalculateBoxGSMSmallFlute(_boxGSM))/3100f)/500f) * 2f;

        return BoxWeight;
    }

    float CalculateBoxCost()
    {
        return CalculateBoxWeight() * _paperCost;
    }
}
