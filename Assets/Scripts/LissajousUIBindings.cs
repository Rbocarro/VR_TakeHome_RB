using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]

//helper class for deluttering LissajousUI elements
public class LissajousUIBindings
{   

    //UI elements
    public TMP_Dropdown dropdown;
    public Slider frequencyX;
    public Slider frequencyY;
    public Slider amplitudeX;
    public Slider amplitudeY;
    public Slider phaseAngle;
    public Slider movementSpeed;
    public Slider animationScale;
    public Button randomiseButton;
    public Button ResetButton;

    public void BindTo(LissajousMovement movement)
    {
        // Populate dropdown with enum values
        dropdown.ClearOptions();
        var enumNames = System.Enum.GetNames(typeof(LissajousMovement.LissajousCurveType));
        dropdown.AddOptions(new List<string>(enumNames));

        // Set the dropdown to the current type
        dropdown.value = (int)movement.curveType;
        dropdown.RefreshShownValue();

        // Listen for changes and update the movement type
        dropdown.onValueChanged.AddListener((index) =>
        {
            movement.curveType = (LissajousMovement.LissajousCurveType)index;
        });

        //add listeners
        frequencyX.onValueChanged.AddListener((value) => movement.frequencyX = value);
        frequencyY.onValueChanged.AddListener((value) => movement.frequencyY = value);
        amplitudeX.onValueChanged.AddListener((value) => movement.amplitudeX = value);
        amplitudeY.onValueChanged.AddListener((value) => movement.amplitudeY = value);
        phaseAngle.onValueChanged.AddListener((value) => movement.phaseAngle = value);
        movementSpeed.onValueChanged.AddListener((value) => movement.movementSpeed = value);
        animationScale.onValueChanged.AddListener((value) => movement.animationScale = value);

        // Bind randomise and reset buttons
        randomiseButton.onClick.AddListener(() => Randomise());
        ResetButton.onClick.AddListener(()=>ResetSliders());
    
        // Set initial values
        frequencyX.value = movement.frequencyX;
        frequencyY.value = movement.frequencyY;
        amplitudeX.value = movement.amplitudeX;
        amplitudeY.value = movement.amplitudeY;
        phaseAngle.value = movement.phaseAngle;
        movementSpeed.value = movement.movementSpeed;
        animationScale.value = movement.animationScale;
    }

    public void Randomise()
    {
        frequencyX.value = Random.Range(frequencyX.minValue, frequencyX.maxValue);
        frequencyY.value = Random.Range(frequencyY.minValue, frequencyY.maxValue);
        amplitudeX.value = Random.Range(amplitudeX.minValue, amplitudeX.maxValue);
        amplitudeY.value = Random.Range(amplitudeY.minValue, amplitudeY.maxValue);
        phaseAngle.value = Random.Range(phaseAngle.minValue, phaseAngle.maxValue);
        movementSpeed.value = Random.Range(movementSpeed.minValue, movementSpeed.maxValue);
    }

    public void ResetSliders()
    {
        frequencyX.value = 1;
        frequencyY.value = 1;
        amplitudeX.value = 1;
        amplitudeY.value = 1;
        phaseAngle.value = 1;
        movementSpeed.value = 2;
        animationScale.value = 1;
        //curveTypeDropdown.value = (int)lissajous.curveType;
    }


}
