using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyBlock: MonoBehaviour{
        public int blockId;
        public int durablitySeconds;
    
        public ParticleSystem particles;
    public bool tryBreak(float time)
    {
        if (time > durablitySeconds)
        {
            Destroy(this.gameObject);
            return true;
        }
        return false;
    }
}