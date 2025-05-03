using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropSelect : MonoBehaviour
{
    public int dropType; //1-altar 2-token
    public int Index = 1;
    public GameObject PrefabObject;
    private Image Icon;
    private Text Text;
    private Text Title;
    public LevelManager lm;
    // Start is called before the first frame update
    void Start()
    {
        lm = GameObject.Find("EventSystem").GetComponent<LevelManager>();
        Icon = transform.Find("Image")?.gameObject.GetComponent<Image>();
        Text = transform.Find("Text")?.gameObject.GetComponent<Text>();
        Title = transform.Find("Title")?.gameObject.GetComponent<Text>();
        Icon.sprite = PrefabObject.GetComponent<SpriteRenderer>().sprite;
        if (dropType == 1)
        {
            AltarsConfig altar = ConfigManager.Instance.GetConfig<AltarsConfig>(PrefabObject.GetComponent<Altar>().currentID);
            SkillsConfig skill1 = ConfigManager.Instance.GetConfig<SkillsConfig>(altar.Skill1);
            SkillsConfig skill2 = ConfigManager.Instance.GetConfig<SkillsConfig>(altar.Skill1);
            Title.text = altar.name;
            Text.text = skill1.desc + "\n\n"
                + "Ï×¼Àºó£º" + skill2.desc + "\n\n"
                + "ÀäÈ´£º" + skill1.cooldown;
        }
        else if (dropType == 2)
        {
            TokensConfig token = ConfigManager.Instance.GetConfig<TokensConfig>(PrefabObject.GetComponent<Token>().currentID);
            Title.text = token.name;
            Text.text = token.desc;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelected()
    {
        lm.SelectIndex = Index;
    }
}
