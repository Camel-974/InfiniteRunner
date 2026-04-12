using UnityEngine;
using System.Collections.Generic;

public class TrackController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float _translationSpeed = 5f;
    [SerializeField] private int _activeChunkCount = 4;
    [SerializeField] private float _behindPlayerDistance = -10f;

    [Header("Components")]
    [SerializeField] private ChunkController[] _chunksPool;

    // Singleton
    public static TrackController Instance { get; private set; }
    
    // get the track speed
    public float TrackSpeed => _translationSpeed;

    // List of all currently active chunks
    private List<ChunkController> _instancedChunks = new List<ChunkController>();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        AddBaseChunk();
    }

    private void Update()
    {
        TranslateChunks();
        UpdateChunks();
    }

    private void TranslateChunks()
    {
        // Move all active chunks toward the player
        foreach (var chunk in _instancedChunks)
        {
            chunk.transform.Translate(Vector3.back * _translationSpeed * Time.deltaTime);
        }
    }

    private void UpdateChunks()
    {
        // Find chunks that are behind the player
        List<ChunkController> behindChunks = new List<ChunkController>();

        foreach (var chunk in _instancedChunks)
        {
            if (chunk.IsBehindPlayer())
            {
                behindChunks.Add(chunk);
            }
        }

        // Delete chunks that are too far behind
        foreach (var chunk in behindChunks)
        {
            _instancedChunks.Remove(chunk);
            Destroy(chunk.gameObject);
        }

        // Add new chunks to replace deleted ones
        int missingChunkCount = _activeChunkCount - _instancedChunks.Count;

        for (int i = 0; i < missingChunkCount; i++)
        {
            if (_instancedChunks.Count == 0) break;
            var chunk = AddChunk(LastActiveChunk().GetNextChunkPosition());
            _instancedChunks.Add(chunk);
        }
    }

    private void AddBaseChunk()
    {
        for (int i = 0; i < _activeChunkCount; i++)
        {
            if (i == 0)
            {
                // First chunk at origin
                ChunkController baseChunk = Instantiate(_chunksPool[0], transform.position, Quaternion.identity);
                _instancedChunks.Add(baseChunk);
                continue;
            }

            // Next chunks placed after the previous one
            var chunk = AddChunk(LastActiveChunk().GetNextChunkPosition());
            _instancedChunks.Add(chunk);
        }
    }

    private ChunkController AddChunk(Vector3 position)
    {
        if (_chunksPool.Length == 0)
        {
            Debug.LogError("No chunks in pool !");
            return null;
        }

        // Pick a random chunk from the pool
        var index = Random.Range(1, _chunksPool.Length);
        ChunkController chunk = Instantiate(_chunksPool[index], position, Quaternion.identity);

        return chunk;
    }

    private ChunkController LastActiveChunk()
    {
        return _instancedChunks[_instancedChunks.Count - 1];
    }

}
