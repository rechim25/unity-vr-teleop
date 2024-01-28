using System.Collections.Concurrent;
using UnityEngine;

public class BytesBuffer
{
    private ConcurrentQueue<byte[]> buffer = new ConcurrentQueue<byte[]>();

    public void Enqueue(byte[] bytes)
    {   
        buffer.Enqueue(bytes);
    }
    
    public bool TryDequeue(out byte[] bytes)
    {   
        if(buffer.TryDequeue(out byte[] bs))
        {   
            bytes = bs;
            return true;
        }
        else
        {
            bytes = null;
            return false; 
        }
    }

    public int Count
    {
        get { return buffer.Count; }
    }
}
