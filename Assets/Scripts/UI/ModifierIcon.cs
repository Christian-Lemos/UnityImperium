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
    private Modifier modifier;

    private RawImage rawImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
       Debug.Log("Hello");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pog");
    }

    // Use this for initialization
    void Start()
    {
        LoadTexture(out this.texture);

        this.rawImage = GetComponent<RawImage>();
        this.rawImage.texture = this.texture;

        modifier.OnDestroyEvent(() =>
        {
            Destroy(this.gameObject);
        });
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
