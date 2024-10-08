using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICharacter : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _attributeInput;

    public CharacterData _character;
    private UILogic _logic;

    private void OnEnable()
    {
        _nameInput.onValueChanged.AddListener(NameEntered);
        _attributeInput.onValueChanged.AddListener(AttributeChanged);
    }


    private void OnDisable()
    {
        _nameInput.onValueChanged.RemoveListener(NameEntered);
        _attributeInput.onValueChanged.RemoveListener(AttributeChanged);
    }

    private void AttributeChanged(string text)
    {
        _character.characterAttributes = text;
    }

    private void NameEntered(string characterName)
    {
        _character.characterName = characterName;
    }

    public void Init(CharacterData character, UILogic uiLogic)
    {
        _logic = uiLogic;
        _character = character;
        UpdateUI();
    }

    public void UpdateUI()
    {
        _nameInput.text = _character.characterName; 
        _attributeInput.text = _character.characterAttributes;
    }

    public void RemoveIt()
    {
        EventSystem.current?.SetSelectedGameObject(null);
        _logic.RemoveCharacter(this);
    }
}
