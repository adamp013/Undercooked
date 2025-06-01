using UnityEngine;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    public bool isPlayerOne;
    public int[,] tileMap;
    public GameObject[,] stMap;
    public List<int> walkableIndices;
    public float speed = 5f;
    public float width = 0.9f;
    public bool interacting = false;
    public Stanica interactingStation;
    public Food holdingFood;
    public bool holding = false;
    public bool cooking = false;
    public int obs = 0;
    public float scale;

    public Transform holdPoint;
    private GameObject heldObject;
    private Vector3 lastInputDirection = Vector3.forward;
    private bool grab;

    void Update()
    {
        KeyCode[] keycodes = new KeyCode[4] {
            KeyCode.N, KeyCode.Q,
            KeyCode.M, KeyCode.E
        };

        if (tileMap == null) return;

        if (holding && Input.GetKeyDown(KeyCode.Backspace))
        {
            DropHeldItem();
        }

        if (heldObject != null)
        {
            heldObject.transform.position = holdPoint.position;
            heldObject.transform.rotation = holdPoint.rotation;
        }

        if (interacting || cooking)
        {
            bool interactPustenie = isPlayerOne ? Input.GetKeyUp(keycodes[0]) : Input.GetKeyUp(keycodes[1]);
            bool interactAktivne = isPlayerOne ? Input.GetKey(keycodes[0]) : Input.GetKey(keycodes[1]);

            if (interacting)
            {
                if (interactAktivne) interactingStation.Select();
                if (interactPustenie)
                {
                    holdingFood = interactingStation.EndSelect(isPlayerOne);
                    holding = holdingFood != null;

                    if (holding)
                    {
                        heldObject = Instantiate(holdingFood.gameObject, holdPoint.position, Quaternion.identity);
                        heldObject.transform.SetParent(holdPoint);
                    }

                    interactingStation = null;
                    interacting = false;
                }
            }
            else if (cooking)
            {
                if (interactAktivne) interactingStation.Interact();
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

            if (input != Vector3.zero)
            {
                lastInputDirection = input;
            }

            Vector3 move = GetAllowedMovement(dir);
            transform.position += move * scale;

            float buffer = 0.01f;
            List<(int, int)> places = new List<(int, int)>();
            int px = Mathf.RoundToInt(transform.localPosition.x);
            int pz = Mathf.RoundToInt(transform.localPosition.z);

            if (h > buffer) { places.Add((1, 0)); if (v > buffer) places.Add((1, 1)); else if (v < -buffer) places.Add((1, -1)); }
            else if (h < -buffer) { places.Add((-1, 0)); if (v > buffer) places.Add((-1, 1)); else if (v < -buffer) places.Add((-1, -1)); }
            if (v > buffer) places.Add((0, 1));
            else if (v < -buffer) places.Add((0, -1));

            bool interact = isPlayerOne ? Input.GetKey(keycodes[0]) : Input.GetKey(keycodes[1]);

            places.Sort((a, b) =>
            {
                float da = (a.Item1 - px) * (a.Item1 - px) + (a.Item2 - pz) * (a.Item2 - pz);
                float db = (b.Item1 - px) * (b.Item1 - px) + (b.Item2 - pz) * (b.Item2 - pz);
                return da.CompareTo(db);
            });
            while (places.Count > 0) //stanica na ktoru sa hrac pozera
            {
                int x = places[0].Item1 + px;
                int z = places[0].Item2 + pz;
                if (x < 0 || z < 0 || x >= stMap.GetLength(0) || z >= stMap.GetLength(1))
                {
                    places.RemoveAt(0);
                    continue;
                }

                Stanica st = stMap[x, z]?.GetComponent<Stanica>();
                if (st == null)
                {
                    places.RemoveAt(0);
                    continue;
                }

                float dist = Vector3.Distance(transform.localPosition, st.transform.localPosition);
                if (dist > 1.5f)
                {
                    places.RemoveAt(0);
                    continue;
                }
                grab = isPlayerOne ? Input.GetKeyDown(keycodes[2]) : Input.GetKeyDown(keycodes[3]); //grab je true ked je drzane e(z nejakeho dovodu polovicu casu)

                if (interact)
                {
                    if (holding) { places.RemoveAt(0); continue; }

                    if (st.hasOutputs)
                    {
                        interacting = true;
                        st.StartSelect();
                        interactingStation = st;
                        break;
                    }
                    else if (st.activneInteractable && !st.activne && st.hasInput)
                    {
                        cooking = true;
                        st.StartInteract();
                        interactingStation = st;
                        break;
                    }
                }
                else if (grab)
                {
                    if (st.hasOutput)
                    {
                        holdingFood = st.Grab();
                        if (holdingFood != null)
                        {
                            holding = true;
                            heldObject = Instantiate(holdingFood.gameObject, holdPoint.position, Quaternion.identity);
                            heldObject.transform.SetParent(holdPoint);
                        }
                    }
                    else if (!st.activne && st.free && holding && holdingFood != null)
                    {
                        st.Place(holdingFood);
                        holding = false;
                        holdingFood = null;

                        if (heldObject != null)
                        {
                            Destroy(heldObject);
                            heldObject = null;
                        }
                    }
                    break;
                }
                places.RemoveAt(0);
            }
        }
    }

    void DropHeldItem()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);
            heldObject.transform.position = transform.position + lastInputDirection.normalized;
            heldObject = null;
        }

        holding = false;
        holdingFood = null;
    }

    Vector3 GetAllowedMovement(Vector3 movementDelta)
    {
        Vector3 pos = transform.localPosition;
        float halfWidth = width / 2f;

        Vector3[] cp = new Vector3[4];
        cp[0] = new Vector3(pos.x - halfWidth, 0, pos.z - halfWidth);
        cp[1] = new Vector3(pos.x + halfWidth, 0, pos.z - halfWidth);
        cp[2] = new Vector3(pos.x + halfWidth, 0, pos.z + halfWidth);
        cp[3] = new Vector3(pos.x - halfWidth, 0, pos.z + halfWidth);

        Vector3 xMovement = new Vector3(movementDelta.x, 0, 0);
        Vector3 zMovement = new Vector3(0, 0, movementDelta.z);

        bool xBlocked = false;
        bool zBlocked = false;

        for (int i = 0; i < 4; i++)
        {
            xBlocked = xBlocked || IsColliding(cp[i] + xMovement);
            zBlocked = zBlocked || IsColliding(cp[i] + zMovement);
        }

        return (!xBlocked ? xMovement : Vector3.zero) + (!zBlocked ? zMovement : Vector3.zero);
    }

    bool IsColliding(Vector3 center)
    {
        int x = Mathf.RoundToInt(center.x);
        int z = Mathf.RoundToInt(center.z);

        if (x < 0 || z < 0 || x >= tileMap.GetLength(0) || z >= tileMap.GetLength(1))
            return true;

        return !walkableIndices.Contains(tileMap[x, z]);
    }
}
