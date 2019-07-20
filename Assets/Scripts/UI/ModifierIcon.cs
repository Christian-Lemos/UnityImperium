using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Assets.Lib.Civilization;
using System.IO;
using UnityEngine.UI;
using Assets.Lib.Events;

[RequireComponent(typeof(RawImage))]
public class ModifierIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private static Dictionary<string, Texture> texture_cache = new Dictionary<string, Texture>();

    [SerializeField]
    private Texture texture;

    [SerializeField]
    public Modifier modifier;

    private RawImage rawImage;

    [SerializeField]
    private GameObject panel;

    private Text text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.text = modifier.Name;
        panel.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        if (panel == null)
        {
            panel.transform.GetChild(0);
        }
        panel.SetActive(false);
        
        
        text = panel.GetComponentInChildren<Text>();
        LoadTexture(out this.texture);

        this.rawImage = GetComponent<RawImage>();
        this.rawImage.texture = this.texture;

        /*modifier.OnDestroyEvent(() =>
        {
            Destroy(this.gameObject);
        });*/
        
    }

    private void LoadTexture(out Texture texture)
    {
        if(texture_cache.ContainsKey(this.modifier.Icon))
        {
            texture = texture_cache[this.modifier.Icon];
        }
        else
        {
            texture = Resources.Load(Path.Combine("icons", "modifiers", this.modifier.Icon)) as Texture;
            texture_cache.Add(this.modifier.Icon, this.texture);
        }
    }
}
