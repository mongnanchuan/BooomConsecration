using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; // ������׼���л�����ʱ��Ҫ
using TMPro;

public class IntroManager : MonoBehaviour
{
    public TextMeshProUGUI displayText; // ��Inspector�й���Text���
    public string fullText; // Ҫ��ʾ���ı���
    public GameObject clickTips;
    public float delayBetweenChars = 5f; // �ַ�֮����ӳ�
    public float delayBeforeNextAction = 1f; // ��֮����ӳ�

    private void Start()
    {
        // ����ʾ����
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        yield return StartCoroutine(ShowText(fullText)); // ��ʾ�����ı�

        yield return new WaitForSeconds(0.8f);
        // ��ʾ���������ʾ
        clickTips.gameObject.SetActive(true); // ���������ʾ
        yield return new WaitForSeconds(delayBeforeNextAction); // �Ե�����ʾ��ʾ

        // �ȴ������
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        SceneManager.LoadScene(2); // ��ת����һ������
    }

    private IEnumerator ShowText(string text)
    {
        displayText.text = ""; // ����ı�
        text = text.Replace("_NEWLINE_", "\n"); // ���������滻Ϊ���з�

        foreach (char character in text)
        {
            displayText.text += character; // ��������ַ�
            //Debug.Log($"Displaying character: {character}");
            yield return new WaitForSeconds(delayBetweenChars); // �ַ�֮����ӳ�
        }
    }

}