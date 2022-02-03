using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionNode
{
    public positionNode parent;
    public int posx;
    public int posy;
    public float g,h,f;

    public positionNode(positionNode parent, int posx, int posy)
    {
        this.parent = parent;
        this.posx = posx;
        this.posy = posy;
        g = h = f = 0f;
    }

    public bool isGoal(positionNode other)
    {
        return (posx == other.posx) && (posy == other.posy);
    }
}
