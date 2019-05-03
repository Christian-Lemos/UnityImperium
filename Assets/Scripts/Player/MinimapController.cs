using Imperium;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public ICollection<GameObject> visibleObjects = new HashSet<GameObject>();

    private void LateUpdate()
    {
        ICollection<GameObject> visibleNow = MapObjecsRenderingController.Instance.visibleObjects;

        foreach (GameObject gameObject in visibleObjects)
        {
            if (gameObject != null && !visibleNow.Contains(gameObject))
            {
                SetRender(false, gameObject);
            }
        }

        foreach (GameObject gameObject in visibleNow)
        {
            if (!visibleObjects.Contains(gameObject))
            {
                SetRender(true, gameObject);
            }
        }

        visibleObjects = visibleNow;
    }

    private void SetRender(bool value, GameObject gameObject)
    {
        MiniMapIcon miniMapIcon = gameObject.GetComponentInChildren<MiniMapIcon>();

        if (miniMapIcon != null)
        {
            if (value)
            {
                /*Color color = Color.red;
                Player goPlayer = PlayerDatabase.Instance.GetObjectPlayer(gameObject);
                for (int i = 0; i < MapObjecsRenderingController.Instance.players.Length; i++)
                {
                    if (goPlayer == MapObjecsRenderingController.Instance.players[i])
                    {
                        color = Color.blue;
                        break;
                    }
                }
                miniMapIcon.GetComponent<SpriteRenderer>().color = color;*/
                miniMapIcon.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                miniMapIcon.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}