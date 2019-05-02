using Imperium;
using Imperium.Economy;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResourcesUpdater : MonoBehaviour
{
    private Player player;

    private Dictionary<ResourceType, Text> texts = new Dictionary<ResourceType, Text>();


    public GameObject UIResourcePrefab;

    private void Start()
    {
        for (int j = 0; j < GameInitializer.Instance.gameSceneData.players.Count; j++)
        {
            if (GameInitializer.Instance.gameSceneData.players[j].player.PlayerType == PlayerType.Real)
            {
                player = GameInitializer.Instance.gameSceneData.players[j].player;
            }
        }

        //player = PlayerDatabase.INSTANCE.gameSceneData.RealPlayer;

        float x_axis_offset = UIResourcePrefab.GetComponent<RectTransform>().localPosition.x;
        if (x_axis_offset > 0)
        {
            x_axis_offset *= -1;
        }
        int i = 0;
        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
        {
            Resource resource = new Resource(resourceType);
            GameObject obj = Instantiate(UIResourcePrefab, transform);
            RectTransform rectTransform = obj.GetComponent<RectTransform>();

            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - x_axis_offset * i, rectTransform.localPosition.y, rectTransform.localPosition.z);

            obj.GetComponentInChildren<RawImage>().texture = resource.Icon;

            texts.Add(resource.Type, obj.GetComponentInChildren<Text>());

            i++;
        }
    }

    private void Update()
    {
        Dictionary<ResourceType, int> currentResources = PlayerDatabase.Instance.GetPlayerResources(player);
        foreach (KeyValuePair<ResourceType, int> entry in currentResources)
        {
            texts[entry.Key].text = entry.Value.ToString();
        }
    }
}