using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; // 仅在你准备切换场景时需要
using TMPro;

public class IntroManager : MonoBehaviour
{
    public TextMeshProUGUI displayText; // 在Inspector中关联Text组件
    public string fullText; // 要显示的文本行
    public GameObject clickTips;
    public float delayBetweenChars = 5f; // 字符之间的延迟
    public float delayBeforeNextAction = 1f; // 行之间的延迟

    private void Start()
    {
        // 先显示黑屏
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        yield return StartCoroutine(ShowText(fullText)); // 显示完整文本

        yield return new WaitForSeconds(0.8f);
        // 显示点击继续提示
        clickTips.gameObject.SetActive(true); // 激活继续提示
        yield return new WaitForSeconds(delayBeforeNextAction); // 稍等再显示提示

        // 等待鼠标点击
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        SceneManager.LoadScene(2); // 跳转到下一个场景
    }

    private IEnumerator ShowText(string text)
    {
        displayText.text = ""; // 清空文本
        text = text.Replace("_NEWLINE_", "\n"); // 将特殊标记替换为换行符

        foreach (char character in text)
        {
            displayText.text += character; // 逐字添加字符
            //Debug.Log($"Displaying character: {character}");
            yield return new WaitForSeconds(delayBetweenChars); // 字符之间的延迟
        }
    }

}