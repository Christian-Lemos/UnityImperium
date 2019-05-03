using Imperium;
using UnityEngine;

public class OutlineShaderController : MonoBehaviour
{
    private Player player;

    private Player FindPlayer()
    {
        Player player = PlayerDatabase.Instance.GetObjectPlayer(this.gameObject);
        if (player == null)
        {
            try
            {
                player = PlayerDatabase.Instance.GetObjectPlayer(GetComponentInParent<MapObject>().gameObject);
            }
            catch
            {
                return null;
            }
            

            return player;
        }
        else
        {
            return player;
        }
    }

    private void SetOutline()
    {
        Color color = player != null ? player.SerializableColor.Color : Color.white;

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
        foreach(MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.SetColor("_FirstOutlineColor", color);
        }

        
    }

    private void Start()
    {
        player = FindPlayer();

        SetOutline();
    }
}