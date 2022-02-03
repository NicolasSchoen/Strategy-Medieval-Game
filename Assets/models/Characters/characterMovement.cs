using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMovement : MonoBehaviour
{

    private GameObject selectionIndicator;
    private bool isSelected = false;
    private Rigidbody rbody;
    Animator anim;
    private Vector3 deltaPosition;
    public float speed = 1.5f;

    private bool isControlledManually = false;

    private bool hasReachedTarget = true;
    private Vector3 targetPosition;
    private Vector3 nextTargetPosion;
    private float deltaDistance = .25f;

    //required for astar pathfinding
    private int[] goalPos;
    private int mapSize;
    private int[,] currentMap;
    private List<positionNode> lstPath;
    private List<positionNode> lstChilds;
    public GameObject demoPath;

    // Start is called before the first frame update
    void Start()
    {
        selectionIndicator = transform.GetChild(2).gameObject;
        selectionIndicator.SetActive(false);
        isSelected = selectionIndicator.activeSelf;
        rbody = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hasReachedTarget && !isControlledManually) moveAround();
    }

    public List<positionNode> aStar(int[,] map, int[] start, int[] end)
    {
        int maxIterations = 1000;
        //lstPath
        //lstChilds
        mapSize = globalVariables.mapSize;
        currentMap = map;
        goalPos = end;
        positionNode startNode, currentNode, endNode;

        startNode = new positionNode(null,start[0],start[1]);
        startNode.g = 0;
        startNode.h = hFunction(start[0], start[1]);
        startNode.f = startNode.g + startNode.h;

        endNode = new positionNode(null, end[0], end[1]);
        endNode.g = endNode.h = endNode.f = 0;

        lstPath = new List<positionNode>();
        lstChilds = new List<positionNode>();

        lstChilds.Add(startNode);
        //Debug.Log("starting a*");

        if (currentMap[endNode.posx, endNode.posy] > 2 && currentMap[endNode.posx, endNode.posy] != 4) return null;
        if(currentMap[endNode.posx, endNode.posy] == 4)
        {
            deltaDistance = .4f;
        }
        else
        {
            deltaDistance = .25f;
        }

        while (lstChilds.Count > 0)
        {
            currentNode = lstChilds[0];
            lstChilds.RemoveAt(0);

            if (currentNode.isGoal(endNode))
            {
                lstPath = new List<positionNode>();
                while(currentNode.parent != null)
                {
                    lstPath.Add(currentNode);
                    currentNode = currentNode.parent;
                }
                lstPath.Reverse();
                return lstPath;
            }

            lstPath.Add(currentNode);
            expandNode(currentNode);
            maxIterations--;
            if (maxIterations <= 0) return null;
        }
        return null;
    }

    public void setReachedTarget()
    {
        hasReachedTarget = true;
    }

    private void expandNode(positionNode currentNode)
    {
        //int[,] directions = {{-1,0},{0,-1},{1,0},{0,1}};
        int[,] directions = {{-1,0},{0,-1},{1,0},{0,1},{-1,-1},{1,-1},{-1,1},{1,1}};   //also moving diagonal
        positionNode childNode;
        bool doContinue = false;
        float tentativeG;
        //Debug.Log("current Node: " + currentNode.posx.ToString() + currentNode.posy.ToString());

        for (int i = 0; i < 8; i++)
        {
            childNode = new positionNode(currentNode, currentNode.posx + directions[i, 0], currentNode.posy + directions[i, 1]);

            if(childNode.posx >=0 && childNode.posx < mapSize && childNode.posy >= 0 && childNode.posy < mapSize)
            {
                if(currentMap[childNode.posx,childNode.posy] <= 2 || currentMap[childNode.posx, childNode.posy] == 4)
                {
                    doContinue = false;

                    foreach (positionNode node in lstPath)
                    {
                        if (node.isGoal(childNode))
                        {
                            doContinue = true;
                        }
                    }
                    if (doContinue) continue;

                    tentativeG = currentNode.g + 1 + currentMap[childNode.posx, childNode.posy];
                    //tentativeG = currentNode.g + Mathf.Log10(MapController.MC.getHeight(childNode.posx,childNode.posy));

                    foreach (positionNode node in lstChilds)
                    {
                        if (node.isGoal(childNode) && (tentativeG >= node.g))
                        {
                            doContinue = true;
                        }
                    }
                    if (doContinue) continue;

                    childNode.parent = currentNode;
                    childNode.g = tentativeG;
                    childNode.f = tentativeG + hFunction(childNode.posx, childNode.posy);

                    lstChilds.Add(childNode);
                    //sort lstChilds
                    lstChilds.Sort((x,y)=>x.f.CompareTo(y.f));
                }
            }
        }
    }

    private float hFunction(int x, int y)
    {
        return Mathf.Sqrt(((goalPos[0]-x)*(goalPos[0]-x))+((goalPos[1]-y)*(goalPos[1]-y)));
    }

    public void setGoal(Vector3 target)
    {
        this.targetPosition = target;
        int[] startpos = { (int)transform.position.x, (int)transform.position.z };
        int[] goalpos = { (int)target.x, (int)target.z };
        if (aStar(MapController.MC.getMap(), startpos, goalpos) == null)
        {
            lstPath.Clear();
            return;
        }
        if (lstPath.Count < 1) return;


        /*if(lstPath != null)
        {
            foreach (positionNode posnode in lstPath)
            {
                Instantiate(demoPath, new Vector3(posnode.posx+.5f, MapController.MC.getHeight(posnode.posx, posnode.posy), posnode.posy+.5f), Quaternion.identity);
            }
        }*/
        

        int tposx = lstPath[0].posx;
        int tposz = lstPath[0].posy;
        nextTargetPosion = new Vector3(tposx + .5f, MapController.MC.getHeight(tposx, tposz), tposz + .5f);
        lstPath.RemoveAt(0);

        //moveTo(target);
        moveTo(nextTargetPosion);
    }

    private void moveAround()
    {
        if((nextTargetPosion - transform.position).magnitude < deltaDistance)
        {
            if(lstPath == null || lstPath.Count <= 0)
            {
                hasReachedTarget = true;
                if(anim != null)anim.SetBool("isRunning", false);
                rbody.useGravity = true;
                return;
            }
            else
            {
                //get next position to move to
                int tposx = lstPath[0].posx;
                int tposz = lstPath[0].posy;
                nextTargetPosion = new Vector3(tposx+.5f,MapController.MC.getHeight(tposx,tposz),tposz+.5f);
                lstPath.RemoveAt(0);
            }
            
        }
        else
        {
            deltaPosition = (nextTargetPosion - transform.position).normalized * speed * Time.deltaTime;
            rbody.useGravity = false;
            //rbody.velocity = new Vector3(0, .8f, 0); //make him jump to pass high slopes
            rbody.position = (rbody.position + deltaPosition);
            
            //rbody.MovePosition(rbody.position + deltaPosition);
            //rbody.MoveRotation(rbody.rotation * deltaPosition);
            //transform.localRotation *= Quaternion.FromToRotation(transform.position, targetPosition);
            //transform.rotation.eulerAngles.Set(0,targetPosition.y,0);
            //transform.Rotate(0,deltaPosition.,0);
            transform.rotation = Quaternion.LookRotation(deltaPosition.normalized);
            transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y,0);//keep the person facing up
            //transform.Rotate(0,transform.rotation.y,0);//to avoid falling over
        }
    }

    public bool selectPerson()
    {
        isSelected = !isSelected;
        selectionIndicator.SetActive(isSelected);
        return isSelected;
    }

    public void moveTo(Vector3 targetPosition)
    {
        hasReachedTarget = false;
        this.nextTargetPosion = targetPosition;
        if(anim != null) anim.SetBool("isRunning", true);
    }

    public void controlManually(bool isControlled)
    {
        isControlledManually = isControlled;
        rbody.useGravity = true;
        if (isControlled) hasReachedTarget = true;
    }
}
