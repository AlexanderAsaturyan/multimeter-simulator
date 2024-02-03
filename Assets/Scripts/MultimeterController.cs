using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MultimeterController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IScrollHandler
{
    private const float NeutralRotationZ = 61f;
    private const float DirectCurrentRotationZ = 15f;
    private const float AlternatingCurrentRotationZ = -75f;
    private const float CurrentStrengthRotationZ = -160f;
    private const float ResistanceRotationZ = -255f;
    private const float ScrollInterval = 0.5f;

    private const string NeutralMode = "NeutralMode";
    private const string DirectCurrentMode = "DirectCurrentMode";
    private const string AlternatingCurrentMode = "AlternatingCurrentMode";
    private const string CurrentStrengthMode = "CurrentStrengthMode";
    private const string ResistanceMode = "ResistanceMode";

    private const float Power = 400;
    private const float Resistance = 1000;
    
    [SerializeField] private GameObject handle;
    [SerializeField] private Light handleLight;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private TextMeshProUGUI leftCornerDirectCurrent;
    [SerializeField] private TextMeshProUGUI leftCornerCurrent;
    [SerializeField] private TextMeshProUGUI leftCornerAlternatingCurrent;
    [SerializeField] private TextMeshProUGUI leftCornerResistance;

    private float ResistanceOutput => Resistance;
    private float NeutralOutput => 0f;
    private float AlternatingCurrentOutput => 0.01f;
    private float DirectCurrentOutput => Mathf.Sqrt(Power * Resistance);
    private float CurrentStrengthOutput => Mathf.Sqrt(Power / Resistance);

    private float currentTime;
    private float previousTime;

    private string[] modes = new string[5] { NeutralMode, DirectCurrentMode,
        AlternatingCurrentMode, CurrentStrengthMode, ResistanceMode};

    private int _currentIndex;

    private void Start()
    {
        _currentIndex = 0;
    }
    
    private void ChangeHandleRotation(float rotationZ)
    {
        handle.transform.localRotation = Quaternion.Euler(-18f, 146f, rotationZ);
    }

    private void DisplayText(float output)
    {
        display.text = output.ToString();
    }

   
    public void OnScroll(PointerEventData eventData)
    {
        currentTime = Time.time;

        if (currentTime - previousTime < ScrollInterval)
        {
            return;
        }

        UpdateCurrentIndex(eventData.scrollDelta.y);
        HandleModeSwitch();
        previousTime = Time.time;
    }

    private void UpdateCurrentIndex(float scrollDelta)
    {
        if (scrollDelta > 0f)
        {
            _currentIndex = (_currentIndex + 1) % modes.Length;
        }
        else if (scrollDelta < 0f)
        {
            _currentIndex = (_currentIndex - 1 + modes.Length) % modes.Length;
        }
    }

    private void HandleModeSwitch()
    {
        switch (modes[_currentIndex])
        {
            case NeutralMode:
                HandleModeChange(NeutralRotationZ, NeutralOutput);
                break;
            case DirectCurrentMode:
                HandleModeChange(DirectCurrentRotationZ, DirectCurrentOutput, leftCornerDirectCurrent);
                break;
            case AlternatingCurrentMode:
                HandleModeChange(AlternatingCurrentRotationZ, AlternatingCurrentOutput, leftCornerAlternatingCurrent);
                break;
            case CurrentStrengthMode:
                HandleModeChange(CurrentStrengthRotationZ, CurrentStrengthOutput, leftCornerCurrent);
                break;
            case ResistanceMode:
                HandleModeChange(ResistanceRotationZ, ResistanceOutput, leftCornerResistance);
                break;
        }
    }

    private void HandleModeChange(float rotationZ, float output, TextMeshProUGUI leftCornerText = null)
    {
        float roundedOutput = (float)Math.Round(output, 2);
        ChangeHandleRotation(rotationZ);
        DisplayText(roundedOutput);
        ResetLeftCornerValues();
        if(leftCornerText != null)
        {
            leftCornerText.text = $"{roundedOutput}";
        }
    }

    private void ResetLeftCornerValues()
    {
        leftCornerDirectCurrent.text = "0";
        leftCornerCurrent.text = "0";
        leftCornerAlternatingCurrent.text = "0";
        leftCornerResistance.text = "0";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        handleLight.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        handleLight.gameObject.SetActive(false);
    }
}