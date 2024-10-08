using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UILogic : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _answerText;
    [SerializeField] private TMP_InputField _requestText;
    [SerializeField] private TMP_InputField _modelField;
    [SerializeField] private UICharacter _characterPrefab;

    [SerializeField] private RectTransform _resultScreen;
    [SerializeField] private RectTransform _analisyngScreen;
    [SerializeField] private RectTransform _charactersViewScreen;
    [SerializeField] private RectTransform _charactersScrollParent;

    [SerializeField] private DataAnalyser _dataAnalyser;

    public List<UICharacter> characters = new ();

    public void Error(string error)
    {
        _analisyngScreen.gameObject.SetActive(false);
        _resultScreen.gameObject.SetActive(true);
        _answerText.text = "Error! Try Again. \nMessage: " + error;
    }

    public void SaveValues()
    {
        Database.Instance.characters = new();
        foreach (var character in characters)
        {
            Database.Instance.characters.Add(character._character);
        }
    }

    public void Find()
    {
        if(!string.IsNullOrEmpty(_requestText.text) && _requestText.text.Trim().Length > 0)
        {
            _analisyngScreen.gameObject.SetActive(true);
            _dataAnalyser.SendRequest(_requestText.text);
        }
    }

    public void SaveModel()
    {
        _dataAnalyser.SetModel(_modelField.text.Trim());
    }
    public void GetModels()
    {
        Application.OpenURL("https://openrouter.ai/docs/models");
    }

    public void Answer(string answer)
    {
        if(int.TryParse(ToDigits(answer), out var result))
        {
            _analisyngScreen.gameObject.SetActive(false);
            _resultScreen.gameObject.SetActive(true);
            if(result != -1)
            {
                var character = Database.Instance.characters.FirstOrDefault(s => s.id == result);

                if (character != null)
                {
                    _answerText.text = "I think it's " + character.characterName;
                }
                else
                {
                    Error("Please ask the question differently and don't write bad words.");
                }
            }
            else
            {
                _answerText.text = "There are no matching characters";
            }
        }
        else
        {
            Error("Please ask the question differently and don't write bad words.");
        }
    }

    private static string ToDigits(string input)
    {
        return new string(input.Where(c => char.IsDigit(c) || c == '-').ToArray());
    }

    public void AddCharacter()
    {
        var c = Instantiate(_characterPrefab, _charactersScrollParent);
        c.Init(new CharacterData(characters.Count, "", ""), this);
        characters.Add(c);
    }

    public void RemoveCharacter(UICharacter UIcharacter)
    {
        characters.Remove(UIcharacter);
        Destroy(UIcharacter.gameObject);
    }

    private void Start()
    {
        _charactersViewScreen.gameObject.SetActive(false);
        int i = 0;
        foreach (var character in Database.Instance.characters)
        {
            var c = Instantiate(_characterPrefab, _charactersScrollParent);
            c.Init(character, this);
            characters.Add(c);
            i++;
        }
    }
}
