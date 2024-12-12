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

        bool preNamePassed = false;//������������� ����������� ������� ������� ����� �������������.
        bool pgnDetected = false;//������������� ����������� ������ ��������.
        bool tabProceed = false;//������������� ��������� ���������.

        int layer = 1;//�������� ����, ��������������� ��������� � ���������� �����.
        int prelayer = 1;//��������������� ���������� ��� �������� �������� ���� ����� ��������� � ������ �������� ����������, ���������� ��� ���������� ������� ����� �������� ���� (��� ���-���-���... �������� �����).


        for (int i = 0; i < input.Length; i++)//���������������� ��������� �������� ������� ������.
        {            
            if (input[i] == '\t') //���������� ���������.
            {                
                layer++; //���������� �������� ����. 
                Debug.Log("���������� ���������, ���������� layer" + " | layer: " + layer + " | prelayer: " + prelayer);
                tabProceed = true;//������������ �������� ��� ��������� ��������� ���������. 
            }
            else
            {
                if (tabProceed)//��������� ���������
                {
                    if (layer == prelayer)
                    {
                        Debug.Log("layer ����� prelayer, ������� ������������ ����" + " | layer: " + layer + " | prelayer: " + prelayer);
                        cnvstr += '\t' + ciTag + '\n' + '\n' + '\t' + oTag + '\n';

                    }
                    else if (layer > prelayer)
                    {
                        Debug.Log("layer ������ prelayer, ���������� prelayer � ������� ��������" + " | layer: " + layer + " | prelayer: " + prelayer);
                        prelayer++;
                    }
                    if (layer < prelayer)
                    {
                        Debug.Log("layer ������ prelayer, ������� ������������ ����." + " | layer: " + layer + " | prelayer: " + prelayer);
                        cnvstr += '\t' + ciTag + '\n' + '\n' + '\t' + oTag + '\n';

                        while (layer < prelayer)
                        {
                            Debug.Log("layer ������ prelayer, ������� ������������ ����." + " | layer: " + layer + " | prelayer: " + prelayer);
                            cnvstr += '\t' + ciTag + '\n' + '\n' + '\t' + oTag + '\n';
                            prelayer--;
                            Debug.Log("���������� prelayer" + " | layer: " + layer + " | prelayer: " + prelayer);
                        }
                    }
                    tabProceed = false;//����� � ��������� ��������� ���������, ����������� ������������� ��������� ���������.
                }               

                if (!preNamePassed) //����������� ���� ��� �������� ���� ����� ����� �������������.
                {
                    //cnvstr += '\n';

                    Debug.Log("������� ������� ������������");
                    cnvstr += preNameString; //����������� ���� ����� ����� �������������.
                    preNamePassed = true; //��������� ������������� ������� ����� ����� ����� �������������.
                }

                if (!pgnDetected) //����������� ����������� �������� ������������, �.�. �� ������ ��������
                {
                    if (input[i] == '\r' || input[i] == '\n')
                    {
                        //������� �������� ��� ������������� �������� ������.
                    }

                    else if (input[i] == divider) //���� ������ ����������� ������.
                    {
                        Debug.Log("������� ������� c�������");
                        cnvstr += prePageString; //����������� ���� ����� ����� ���������, ��� ������ ������������ ������������ �� �������������.
                        pgnDetected = true; //����������� ������, ������ ������������ ���� �� ����������� �� ���������� �������� ������, ��� ����� ������� ��������� ����, ��� ����������� ������ �������� ���������.                   
                    }

                    else
                    {
                        cnvstr += input[i]; //����������� ������� ������������.
                    }
                }
                else //�������� ������, ����������� ����������� �������� ������ ���. �� ���������� �������� ������.
                {
                    if (input[i] == ' ')
                    {
                        //������� �������� ��� ������������� �������.
                    }
                    else if (input[i] == '\r') 
                    {                        
                        //������� �������� ��� ������������� \r.
                    }
                    else if (input[i] == '\n') //��������� ������� ������, ��������� ����������� �������� ������ ��������. ����� ���� ���� �� ����� ��������� �� ����������� ��������.
                    {
                        Debug.Log("������� ������� ����");
                        cnvstr += preSubString; //����������� ���� ����� ����� ������� ����.
                        cnvstr += layer.ToString(); //����������� ����� ����.
                        Debug.Log("������� ������ ����");
                        cnvstr += endString; //����������� ���� ����� ����� ������ ����.

                        cnvstr += '\n';

                        pgnDetected = false; //����������� ������������� ����������� ��������.
                        preNamePassed = false; //����������� ������������� ����������� ������� ����� ����� �������������.                      
                        layer = 1; //����� �������� ����.
                        tabProceed = true; //��������� ������������� ��������� ���������.
                        Debug.Log("����� �������� ���� �� 1, ����� ���������" + " | layer: " + layer + " | prelayer: " + prelayer);
                    }
                    else
                    {
                        cnvstr += input[i]; //����������� ������� ������ ��������.
                    }
                }
            }
        }

        //����������� ����������� �������� ���������, ������� ����������� �����.
        cnvstr += preSubString;
        cnvstr += layer.ToString();
        cnvstr += endString;
        cnvstr += '\n';
        cnvstr += (endTag);

        return cnvstr;
    }   
}