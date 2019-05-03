using Imperium;
using UnityEngine;

public class MiniMapIcon : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Player player = PlayerDatabase.Instance.GetObjectPlayer(gameObject.transform.parent.gameObject);

        Color color = player != null ? player.SerializableColor.Color : Color.white;

        GetComponent<SpriteRenderer>().color = color;
    }
}