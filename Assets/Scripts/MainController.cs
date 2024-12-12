using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MainController : MonoBehaviour
{
    [SerializeField] TMP_InputField inputBookmark;
    [SerializeField] TMP_InputField outputBookmark;

    [SerializeField] InputField charToRemoveIF;
    public InputField charToReplaceIF;
    [SerializeField] Text ctrIgnoreLenghtText;
    [SerializeField] Button increaseCTRIBtn;
    [SerializeField] Button decreaseCTRIBtn;

    [SerializeField] int minctrIgnore = 0;
    [SerializeField] int maxctrIgnore = 3;    

    public char charToRemove =  '.';
    public char charToPlace = ';';
    public int ctrIgnoreLenght = 1;

    string inputString;
    string outputString;
    string exportString;

    [SerializeField] Text console;

    [SerializeField] ConvertController convcon;

    private void Start()
    {
        charToRemoveIF.text = charToRemove.ToString();
        charToReplaceIF.text = charToPlace.ToString();
        ctrIgnoreLenghtText.text = ctrIgnoreLenght.ToString();
        CheckCTRIButton();
    }

    public void IncreaseCTRI()
    {
        ctrIgnoreLenght++;
        ctrIgnoreLenghtText.text = ctrIgnoreLenght.ToString();
        if (decreaseCTRIBtn.interactable == false) decreaseCTRIBtn.interactable = true;
        CheckCTRIButton();
    }

    public void DecreaseCTRI()
    {
        ctrIgnoreLenght--;
        ctrIgnoreLenghtText.text = ctrIgnoreLenght.ToString();
        if (increaseCTRIBtn.interactable == false) increaseCTRIBtn.interactable = true;
        CheckCTRIButton();        
    }

    void CheckCTRIButton()
    {
        if (ctrIgnoreLenght == minctrIgnore) decreaseCTRIBtn.interactable = false;
        if (ctrIgnoreLenght == maxctrIgnore) increaseCTRIBtn.interactable = false;
    }

    public void ExportText()
    {
        if (!String.IsNullOrEmpty(outputBookmark.text)) WriteToFile(convcon.ConvertString(outputBookmark.text));
        else
        {
            console.text = "Export error, output text is empty. Please process the content text before exporting.";
        }      
    }

    public void ProcessText()
    {
        if (charToRemoveIF.text.Length < 1)
        {
            console.text = "Error! Symbol to delete set is empty!";
        }
        else if(charToReplaceIF.text.Length < 1)
        {
            console.text = "Error! Symbol to replace is empty!";
        }
        else
        {
            outputString = "";

            if (String.IsNullOrEmpty(inputBookmark.text))
            {
                console.text = "Processing error, input text is empty. Insert the book's content text into the left column";
                Debug.Log("Input is empty");
                outputBookmark.text = outputString;
            }
            else
            {
                inputString = inputBookmark.text;

                bool ctrDetected = false;

                for (int i = 0; i < inputString.Length; i++)
                {
                    if (inputString[i] != charToRemoveIF.text[0])
                    {
                        if (ctrDetected)
                        {
                            outputString += charToReplaceIF.text;

                            if (inputString[i] != ' ')
                            {
                                outputString += ' ';
                            }

                            ctrDetected = false;
                        }

                        outputString += inputString[i];
                    }
                    else
                    {
                        if (ctrIgnoreLenght != 0)
                        {
                            if (!ctrDetected)
                            {
                                for (int j = ctrIgnoreLenght; j > 0; j--)
                                {
                                    if (inputString.Length > i + j)
                                    {
                                        if (inputString[i + j] != charToRemoveIF.text[0])
                                        {
                                            outputString += inputString[i];
                                            break;
                                        }
                                        ctrDetected = true;
                                        break;
                                    }
                                    else
                                    {
                                        Debug.Log("Out of array");
                                    }
                                }
                            }
                        }

                        else
                        {
                            ctrDetected = true;
                        }
                    }
                }
                outputBookmark.text = outputString;
                console.text = "Processing is complete, before exporting to txt, check the result in the right column";
            }
        }                
    }  

    void WriteToFile(string export)
    {
        if (String.IsNullOrEmpty(export))
        {            
            console.text = "Export error, output text is empty. Please process content text before exporting.";
        }
        else
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine("BookmarkExport.txt"), false))
            {
                Debug.Log("File saved!");
                outputFile.WriteLine(export);
                console.text = "Export is complete. The file is located in the program folder. (ContentExport.txt)";
            }
        }        
    }
}