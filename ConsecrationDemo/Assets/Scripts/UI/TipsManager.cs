using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipsManager : MonoBehaviour
{
    public static TipsManager Instance { get; private set; }


    public GameObject tipsPrefab;
    public int maxTipsCount = 4; // �����ʾ����
    public float displayTime = 2f; // ��ʾ��ʾʱ��
    public float moveSpeed;
    public float moveTime;
    // �ı�����
    private Queue<(GameObject tipObject, Coroutine tipCoroutine)> tipsQueue = new Queue<(GameObject, Coroutine)>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowTip(string message)
    {
        if (tipsQueue.Count >= maxTipsCount)
        {
            // �����������ʱ�������������ʾ
            var (oldTip, oldCoroutine) = tipsQueue.Dequeue(); // ȡ���������ʾ�����Ӧ��Э��
            StopCoroutine(oldCoroutine); // ֹͣ��Ӧ��Э��
            Destroy(oldTip); // ������ʾ
        }

        // �����µ���ʾ�ı�
        GameObject newTip = Instantiate(tipsPrefab, transform);
        TextMeshProUGUI tipText = newTip.GetComponent<TextMeshProUGUI>();
        if (tipText != null)
        {
            tipText.text = message; // ������ʾ�ı�
            Coroutine tipCoroutine = StartCoroutine(DisplayTip(newTip.transform as RectTransform, tipText)); // ����Э�̲�����
            tipsQueue.Enqueue((newTip, tipCoroutine)); // ��ӵ�����
        }
    }

    private IEnumerator DisplayTip(RectTransform tipTransform, TextMeshProUGUI tipText)
    {
        // ��ʼ͸����
        Color color = tipText.color;
        color.a = 0;
        tipText.color = color;

        // ����Ч��
        for (float t = 0; t < moveTime; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, 1, t);
            tipText.color = color;
            tipTransform.anchoredPosition += new Vector2(0, Time.deltaTime * 50); // �����ƶ�
            yield return null;
        }

        color.a = 1;
        tipText.color = color;

        // ������ʾ
        yield return new WaitForSeconds(displayTime);

        // ����Ч��
        for (float t = 0; t < moveTime; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(1, 0, t);
            tipText.color = color;
            //tipTransform.anchoredPosition += new Vector2(0, Time.deltaTime * 50); // �����ƶ�
            yield return null;
        }

        Destroy(tipTransform.gameObject); // ��ȫ��ʧ������
    }

}
