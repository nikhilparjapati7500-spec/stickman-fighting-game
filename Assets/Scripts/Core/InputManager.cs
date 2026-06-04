using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Handles all player input for PC and mobile platforms.
/// Includes input buffering for responsive combo detection.
/// </summary>
public class InputManager : MonoBehaviour
{
    [SerializeField] private float inputBufferTime = 0.1f;
    
    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>
    {
        { "MoveLeft", KeyCode.A },
        { "MoveRight", KeyCode.D },
        { "MoveForward", KeyCode.W },
        { "MoveBack", KeyCode.S },
        { "Jump", KeyCode.Space },
        { "Block", KeyCode.Q },
        { "Punch", KeyCode.E },
        { "Kick", KeyCode.R },
        { "Special", KeyCode.F },
        { "Ultimate", KeyCode.G }
    };
    
    private List<InputBufferData> inputBuffer = new List<InputBufferData>();
    private static InputManager instance;
    
    public enum InputType
    {
        MoveLeft,
        MoveRight,
        MoveForward,
        MoveBack,
        Jump,
        Block,
        Punch,
        Kick,
        Special,
        Ultimate,
        None
    }
    
    // Events
    public static event Action<InputType> OnInputPressed;
    public static event Action<InputType> OnInputReleased;
    public static event Action<Vector2> OnMovementInput;
    
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InputManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("InputManager");
                    instance = go.AddComponent<InputManager>();
                }
            }
            return instance;
        }
    }
    
    private struct InputBufferData
    {
        public InputType input;
        public float time;
    }
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    
    private void Update()
    {
        ProcessKeyboardInput();
        UpdateInputBuffer();
        HandleMovement();
    }
    
    private void ProcessKeyboardInput()
    {
        // Check all input types
        foreach (var binding in keyBindings)
        {
            if (Input.GetKeyDown(binding.Value))
            {
                InputType inputType = GetInputTypeFromString(binding.Key);
                OnInputPressed?.Invoke(inputType);
                AddToInputBuffer(inputType);
            }
            
            if (Input.GetKeyUp(binding.Value))
            {
                InputType inputType = GetInputTypeFromString(binding.Key);
                OnInputReleased?.Invoke(inputType);
            }
        }
    }
    
    private void HandleMovement()
    {
        Vector2 movementInput = Vector2.zero;
        
        if (Input.GetKey(keyBindings["MoveRight"]))
            movementInput.x += 1;
        if (Input.GetKey(keyBindings["MoveLeft"]))
            movementInput.x -= 1;
        if (Input.GetKey(keyBindings["MoveForward"]))
            movementInput.y += 1;
        if (Input.GetKey(keyBindings["MoveBack"]))
            movementInput.y -= 1;
        
        if (movementInput != Vector2.zero)
        {
            movementInput.Normalize();
            OnMovementInput?.Invoke(movementInput);
        }
    }
    
    private void UpdateInputBuffer()
    {
        // Remove old inputs outside buffer window
        inputBuffer.RemoveAll(input => Time.time - input.time > inputBufferTime);
    }
    
    private void AddToInputBuffer(InputType input)
    {
        inputBuffer.Add(new InputBufferData
        {
            input = input,
            time = Time.time
        });
    }
    
    private InputType GetInputTypeFromString(string input)
    {
        return System.Enum.TryParse(input, out InputType result) ? result : InputType.None;
    }
    
    public List<InputType> GetInputBuffer()
    {
        List<InputType> result = new List<InputType>();
        foreach (var input in inputBuffer)
        {
            result.Add(input.input);
        }
        return result;
    }
    
    public void ClearInputBuffer()
    {
        inputBuffer.Clear();
    }
    
    public bool IsInputPressed(InputType input)
    {
        return Input.GetKey(GetKeyCodeForInput(input));
    }
    
    private KeyCode GetKeyCodeForInput(InputType input)
    {
        return keyBindings.ContainsKey(input.ToString()) ? keyBindings[input.ToString()] : KeyCode.None;
    }
    
    // Allow rebinding
    public void RebindKey(string action, KeyCode newKey)
    {
        if (keyBindings.ContainsKey(action))
        {
            keyBindings[action] = newKey;
        }
    }
}
