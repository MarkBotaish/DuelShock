using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : TurnObjectParentScript {

    public virtual void usePowerUp() { }
    public virtual void init(GameObject obj) { }
    public virtual Sprite getTexture() {
        return null;
    }
    
}
