using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BoxCalculator : MonoBehaviour
{
    // Input References
    public TMP_InputField lengthInput;
    public TMP_InputField breadthInput;
    public TMP_InputField heightInput;
    public TMP_InputField gsmInput;
    public TMP_Dropdown plyDropdown;
    public TMP_Dropdown fluteSizeDropdown;
    public TMP_InputField paperCostInput;
    public Button submitButton;

    // Output References (Results Panel)
    public TMP_Text reelSizeValueText;
    public TMP_Text cuttingSizeValueText;
    public TMP_Text boxWeightValueText;
    public TMP_Text boxProductionCostValueText;
    public TMP_Text totalGSMValueText;

    // Box properties and dimension variables
    private float _boxLength;
    private float _boxBreadth;
    private float _boxHeight;
    private float _boxGSM;
    private int _numberOfPly;
    private float _paperCost;

    // Allowances
    private const float FlopAllowance = 2f;
    private const float CuttingAllowance = 1.2f;

    public void OnSubmit()
    {
        // Parse input values from the UI
        _boxLength = float.Parse(lengthInput.text);
        _boxBreadth = float.Parse(breadthInput.text);
        _boxHeight = float.Parse(heightInput.text);
        _boxGSM = float.Parse(gsmInput.text);
        int[] plyOptions = { 3, 5, 7 };
        _numberOfPly = plyOptions[plyDropdown.value];
        Debug.Log("Selected Number of Ply: " + _numberOfPly);
        _paperCost = float.Parse(paperCostInput.text);

        // Determine which flute size is selected
        string fluteSize = fluteSizeDropdown.options[fluteSizeDropdown.value].text;

        // Calculate based on the flute size
        float finalGSM = (fluteSize == "Small") ? CalculateBoxGSMSmallFlute(_boxGSM) : CalculateBoxGSMLargeFlute(_boxGSM);
        float boxWeight = CalculateBoxWeight(finalGSM);
        float boxCost = CalculateBoxCost(boxWeight);

        float reelSize = CalculateReelSize(_boxBreadth, _boxHeight);
        float cuttingSize = CalculateCuttingSize(_boxLength, _boxBreadth);

        // Update the results panel with calculated values
        reelSizeValueText.text = reelSize.ToString("F2");
        cuttingSizeValueText.text = cuttingSize.ToString("F2");
        boxWeightValueText.text = boxWeight.ToString("F2");
        boxProductionCostValueText.text = boxCost.ToString("F2");
        totalGSMValueText.text = finalGSM.ToString("F2");
    }

    float CalculateReelSize(float breadth, float height)
    {
        return breadth + height + FlopAllowance;
    }

    float CalculateCuttingSize(float length, float breadth)
    {
        return length + breadth + CuttingAllowance;
    }

    float CalculateBoxGSMSmallFlute(float gsm)
    {
        float finalGSM = 0f;
        float fluteGSM = gsm * 1.4f;

        for (int i = 0; i < _numberOfPly; i++)
        {
            finalGSM += (i % 2 == 0) ? gsm : fluteGSM;
        }

        return finalGSM;
    }

    float CalculateBoxGSMLargeFlute(float gsm)
    {
        float finalGSM = 0f;

        for (int i = 0; i < _numberOfPly; i++)
        {
            if (i % 2 == 0)
            {
                finalGSM += gsm;
            }
            else
            {
                finalGSM += gsm * ((i == 1) ? 1.5f : 1.4f);
            }
        }

        return finalGSM;
    }

    float CalculateBoxWeight(float finalGSM)
    {
        float reelSize = CalculateReelSize(_boxBreadth, _boxHeight);
        float cuttingSize = CalculateCuttingSize(_boxLength, _boxBreadth);

        return (reelSize * cuttingSize * finalGSM / 3100f) / 500f * 2f;
    }

    float CalculateBoxCost(float boxWeight)
    {
        return boxWeight * _paperCost;
    }
}
