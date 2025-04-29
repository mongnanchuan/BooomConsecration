using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipsManager : MonoBehaviour
{
    public static TipsManager Instance { get; private set; }


    public GameObject tipsPrefab;
    public int maxTipsCount = 4; // 最大提示数量
    public float displayTime = 2f; // 提示显示时间
    public float moveSpeed;
    public float moveTime;
    // 文本队列
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
            // 超过最大数量时，销毁最早的提示
            var (oldTip, oldCoroutine) = tipsQueue.Dequeue(); // 取出最早的提示和其对应的协程
            StopCoroutine(oldCoroutine); // 停止对应的协程
            Destroy(oldTip); // 销毁提示
        }

        // 创建新的提示文本
        GameObject newTip = Instantiate(tipsPrefab, transform);
        TextMeshProUGUI tipText = newTip.GetComponent<TextMeshProUGUI>();
        if (tipText != null)
        {
            tipText.text = message; // 设置提示文本
            Coroutine tipCoroutine = StartCoroutine(DisplayTip(newTip.transform as RectTransform, tipText)); // 启动协程并保存
            tipsQueue.Enqueue((newTip, tipCoroutine)); // 添加到队列
        }
    }

    private IEnumerator DisplayTip(RectTransform tipTransform, TextMeshProUGUI tipText)
    {
        // 初始透明度
        Color color = tipText.color;
        color.a = 0;
        tipText.color = color;

        // 渐显效果
        for (float t = 0; t < moveTime; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, 1, t);
            tipText.color = color;
            tipTransform.anchoredPosition += new Vector2(0, Time.deltaTime * 50); // 向上移动
            yield return null;
        }

        color.a = 1;
        tipText.color = color;

        // 持续显示
        yield return new WaitForSeconds(displayTime);

        // 渐隐效果
        for (float t = 0; t < moveTime; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(1, 0, t);
            tipText.color = color;
            //tipTransform.anchoredPosition += new Vector2(0, Time.deltaTime * 50); // 向上移动
            yield return null;
        }

        Destroy(tipTransform.gameObject); // 完全消失后销毁
    }

}
