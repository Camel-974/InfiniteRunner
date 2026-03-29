using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField] private float _chunkLength = 30f;

    // Returns the position where the next chunk should spawn
    public Vector3 GetNextChunkPosition()
    {
        return transform.position + new Vector3(0, 0, _chunkLength);
    }

    // Returns true if this chunk is behind the player
    public bool IsBehindPlayer()
    {
        return transform.position.z < -_chunkLength;
    }
}
