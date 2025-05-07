using UnityEngine;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    public bool isPlayerOne;//prvy hrac su cispky
    public int[,] tileMap;
    public GameObject[,] stMap;
    public List<int> walkableIndices;
    public float speed = 5f;
    public float width = 0.9f; // šírka hráča (používa sa na kolízie)
    public bool interacting = false; //interacting vyberanie suroviny
    public Stanica interactingStation;
    public Food holdingFood;
    public bool holding = false;//drzis objekt neskor pridaj jedlo class premennu na vec co drzis
    public bool cooking = false; //interacting with station
    public int obs = 0;
    void Update()
    {

        KeyCode[] keycodes = new KeyCode[4]{
            KeyCode.N, KeyCode.Q,
            KeyCode.M, KeyCode.E
        };
        if (tileMap == null) { return; }
        if (holding)
        {
            //bool grabPustenie = isPlayerOne ? Input.GetKeyUp(keycodes[2]) : Input.GetKeyUp(keycodes[3]);
            //bool grabAktivne = isPlayerOne ? Input.GetKey(keycodes[2]) : Input.GetKey(keycodes[3]);
            //zatial nic
        }
        if (interacting || cooking)
        {
            bool interactPustenie = isPlayerOne ? Input.GetKeyUp(keycodes[0]) : Input.GetKeyUp(keycodes[1]);
            bool interactAktivne = isPlayerOne ? Input.GetKey(keycodes[0]) : Input.GetKey(keycodes[1]);
            if (interacting)
            {
                if (interactAktivne)
                {
                    interactingStation.Player1isTouching = isPlayerOne;
                    interactingStation.Select();
                }
                if (interactPustenie)
                {
                    interactingStation.Player1isTouching = isPlayerOne;
                    holdingFood = interactingStation.EndSelect(isPlayerOne);
                    holding = true;
                    interactingStation = null;
                    interacting = false;
                }
            }
            else if (cooking)
            {
                if (interactAktivne)
                {
                    interactingStation.Interact();
                }
                if (interactPustenie)
                {
                    interactingStation.EndInteract();
                    cooking = false;
                    interactingStation = null;
                }
            }
        }
        else
        {
            obs = tileMap.GetLength(0) + tileMap.GetLength(1);
            float h = isPlayerOne ? Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal2");
            float v = isPlayerOne ? Input.GetAxis("Vertical") : Input.GetAxis("Vertical2");

            Vector3 input = new Vector3(h, 0, v).normalized;
            Vector3 dir = input * speed * Time.deltaTime;

            float buffer = 0.01f;
            List<(int, int)> places = new List<(int, int)>();
            if (h > buffer)
            {
                places.Add((1, 0));
                if (v > buffer)
                {
                    places.Add((1, 1));
                }
                else if (v < -buffer)
                {
                    places.Add((1, -1));
                }
            }
            else if (h < -buffer)
            {
                places.Add((-1, 0));
                if (v > buffer)
                {
                    places.Add((-1, 1));
                }
                else if (v < -buffer)
                {
                    places.Add((-1, -1));
                }
            }
            if (v > buffer)
            {
                places.Add((0, 1));
            }
            else if (v < -buffer)
            {
                places.Add((0, -1));
            }

            bool interact = isPlayerOne ? Input.GetKeyDown(keycodes[0]) : Input.GetKeyDown(keycodes[1]);
            bool grab = isPlayerOne ? Input.GetKeyDown(keycodes[2]) : Input.GetKeyDown(keycodes[3]);


            if (!interact)
            {
                transform.position += GetAllowedMovement(dir);
                if (!grab)
                {
                    return;
                }
            }

            int px = Mathf.RoundToInt(transform.position.x);
            int pz = Mathf.RoundToInt(transform.position.z);
            for (int i = 0; i < places.Count; i++)
            {
                int x = places[i].Item1 + px;
                int z = places[i].Item2 + pz;
                places[i] = (x, z);
            }
            places.Sort((x, z) =>
            {
                float dx = Mathf.Pow((float)x.Item1 - (float)px, 2f) + Mathf.Pow((float)x.Item2 - (float)pz, 2f);
                float dz = Mathf.Pow((float)z.Item1 - (float)px, 2f) + Mathf.Pow((float)z.Item2 - (float)pz, 2f);
                return dx.CompareTo(dz); // porovná floaty a vráti int
            });
            (int, int) end = (-1, -1);
            while (places.Count > 0)
            {
                Stanica st = stMap[places[0].Item1, places[0].Item2].GetComponent<Stanica>();
                if (st == null)
                {
                    places.RemoveAt(0);
                    continue;
                }
                if (interact)
                {
                  
                    if (holding)
                    {
                        places.RemoveAt(0);
                        continue;
                    }
                   
                    if (st.hasOutputs)
                    {
                        Debug.Log("Outputy");
                        interacting = true;
                        interactingStation.Player1isTouching = isPlayerOne;
                        st.StartSelect();
                        interactingStation = st;
                        //tu spavnuj rozoberanie polic
                    }
                    else if (st.activneInteractable && !st.activne && st.hasInput)
                    {
                        Debug.Log("rezanie");
                        cooking = true;
                        st.StartInteract();
                        interactingStation = st;
                    }
                    //tu je interact
                }
                else
                {
                    if (st.hasOutput)
                    {
                        holdingFood = st.Grab();
                        holding = (holdingFood != null);
                    }
                    else if (!st.activne && st.free && holding)
                    {
                        if (holdingFood != null)
                        {
                            st.Place(holdingFood);
                        }
                        holding = false;
                        holdingFood = null;
                    }
                    //tu je grab
                }
                places.RemoveAt(0);
            }
        }
    }

    Vector3 GetAllowedMovement(Vector3 movementDelta)
    {
        Vector3 pos = transform.position;
        float halfWidth = width / 2f;


        Vector3[] cp = new Vector3[4];
        cp[0] = new Vector3(pos.x - halfWidth, 0, pos.z - halfWidth);
        cp[1] = new Vector3(pos.x + halfWidth, 0, pos.z - halfWidth);
        cp[2] = new Vector3(pos.x + halfWidth, 0, pos.z + halfWidth);
        cp[3] = new Vector3(pos.x - halfWidth, 0, pos.z + halfWidth);

        Vector3 xMovement = new Vector3(movementDelta.x, 0, 0);
        Vector3 zMovement = new Vector3(0, 0, movementDelta.z);

        bool xDih = false;
        bool zDih = false;

        Vector3[] g = cp;
        for (int i = 0; i < 4; i++)
        {
            xDih = xDih || IsColliding(g[i] + xMovement);
        }

        for (int i = 0; i < 4; i++)
        {
            zDih = zDih || IsColliding(g[i] + zMovement);
        }

        return (!xDih ? xMovement : new Vector3(0, 0, 0)) + (!zDih ? zMovement : new Vector3(0, 0, 0));
    }

    bool IsColliding(Vector3 center)
    {
        int x = Mathf.RoundToInt(center.x);
        int z = Mathf.RoundToInt(center.z);

        if (x < 0 || z < 0 || x >= tileMap.GetLength(0) || z >= tileMap.GetLength(1))
        {
            return true;
        }

        return !walkableIndices.Contains(tileMap[x, z]);
    }
}