using UnityEngine;

public class ConvertController : MonoBehaviour
{
    [SerializeField] MainController mc;

    [SerializeField] string startTag = "";

    [TextArea(2, 8)]
    [SerializeField] string endTag = "";

    [TextArea(2,8)]
    [SerializeField] string preNameString = "";
    [TextArea(2, 8)]
    [SerializeField] string prePageString = "";
    [TextArea(2, 8)]
    [SerializeField] string preSubString = "";
    [TextArea(2, 8)]
    [SerializeField] string endString = "";

    [SerializeField] string ciTag = "";
    [SerializeField] string oTag = "";  

    string cnvstr;
    char divider;

    public string ConvertString(string input)
    {
        divider = mc.charToReplaceIF.text[0];
        cnvstr = startTag + '\n';

        bool preNamePassed = false;//Переключатель прохождения вставки претега перед наименованием.
        bool pgnDetected = false;//Переключатель обнаружения номера страницы.
        bool tabProceed = false;//Переключатель обработки табуляции.

        int layer = 1;//Значение слоя, подразумевается подраздел в содержании книги.
        int prelayer = 1;//Дополнтительная переменная для хранения значения слоя после обработки и сброса основной переменной, необходима для корреткной вставки тегов закрытия слоя (для под-под-под... разделов книги).


        for (int i = 0; i < input.Length; i++)//Последовательная обработка символов входной строки.
        {            
            if (input[i] == '\t') //Обнаружена табуляция.
            {                
                layer++; //Увеличение значения слоя. 
                Debug.Log("Обнаружена табуляция, увеличение layer" + " | layer: " + layer + " | prelayer: " + prelayer);
                tabProceed = true;//Переключение значения для активации обработки табуляции. 
            }
            else
            {
                if (tabProceed)//Обработка табуляции
                {
                    if (layer == prelayer)
                    {
                        Debug.Log("layer равен prelayer, вставка закрывающего тега" + " | layer: " + layer + " | prelayer: " + prelayer);
                        cnvstr += '\t' + ciTag + '\n' + '\n' + '\t' + oTag + '\n';

                    }
                    else if (layer > prelayer)
                    {
                        Debug.Log("layer больше prelayer, увеличение prelayer и пропуск итерации" + " | layer: " + layer + " | prelayer: " + prelayer);
                        prelayer++;
                    }
                    if (layer < prelayer)
                    {
                        Debug.Log("layer меньше prelayer, вставка закрывающего тега." + " | layer: " + layer + " | prelayer: " + prelayer);
                        cnvstr += '\t' + ciTag + '\n' + '\n' + '\t' + oTag + '\n';

                        while (layer < prelayer)
                        {
                            Debug.Log("layer меньше prelayer, вставка закрывающего тега." + " | layer: " + layer + " | prelayer: " + prelayer);
                            cnvstr += '\t' + ciTag + '\n' + '\n' + '\t' + oTag + '\n';
                            prelayer--;
                            Debug.Log("уменьшение prelayer" + " | layer: " + layer + " | prelayer: " + prelayer);
                        }
                    }
                    tabProceed = false;//Поиск и обработка табуляции завершена, деактивация переключателя обработки табуляции.
                }               

                if (!preNamePassed) //Выполняется если еще вставлен блок тегов перед наименованием.
                {
                    //cnvstr += '\n';

                    Debug.Log("Вставка претега наименования");
                    cnvstr += preNameString; //Вставляется блок тегов перед наименованием.
                    preNamePassed = true; //Активация переключателя вставки блока тегов перед наименованием.
                }

                if (!pgnDetected) //Поочередное копирование символов наименования, т.к. не найден делитель
                {
                    if (input[i] == '\r' || input[i] == '\n')
                    {
                        //Пропуск итерации для игнорирования переноса строки.
                    }

                    else if (input[i] == divider) //Если символ разделителя найден.
                    {
                        Debug.Log("Вставка претега cтраницы");
                        cnvstr += prePageString; //Вставляется блок тегов перед страницей, сам символ разделителся пропускается за ненадобностью.
                        pgnDetected = true; //Разделитель найден, больше родительский блок не выполняется до нахождения переноса строки, что будет являтся признаком того, что копирования номера страницы завершено.                   
                    }

                    else
                    {
                        cnvstr += input[i]; //копирование символа наименования.
                    }
                }
                else //Делитель найден, поочередное копирование символов номера стр. до нахождения переноса строки.
                {
                    if (input[i] == ' ')
                    {
                        //Пропуск итерации для игнорирования пробела.
                    }
                    else if (input[i] == '\r') 
                    {                        
                        //Пропуск итерации для игнорирования \r.
                    }
                    else if (input[i] == '\n') //Обнаружен перенос строки, завершено копирование символов номера страницы. Более этот блок не будет выполнтся до обнаружения делителя.
                    {
                        Debug.Log("Вставка претега слоя");
                        cnvstr += preSubString; //Вставляется блок тегов перед номером слоя.
                        cnvstr += layer.ToString(); //Вставляется номер слоя.
                        Debug.Log("Вставка затега слоя");
                        cnvstr += endString; //Вставляется блок тегов после номера слоя.

                        cnvstr += '\n';

                        pgnDetected = false; //Деактивация переключателя обнаружения делителя.
                        preNamePassed = false; //Деактивация переключателя обнаружения вставки тегов перед намиенованием.                      
                        layer = 1; //Сброс значения слоя.
                        tabProceed = true; //Активация переключателя обработки табуляции.
                        Debug.Log("Сброс значения слоя на 1, поиск табуляции" + " | layer: " + layer + " | prelayer: " + prelayer);
                    }
                    else
                    {
                        cnvstr += input[i]; //Копирование символа номера страницы.
                    }
                }
            }
        }

        //Поочередное копирование символов завершено, вставка закрывающих тегов.
        cnvstr += preSubString;
        cnvstr += layer.ToString();
        cnvstr += endString;
        cnvstr += '\n';
        cnvstr += (endTag);

        return cnvstr;
    }   
}