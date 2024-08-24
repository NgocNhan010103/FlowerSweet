using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public enum GameState
{
    Pause,
    Playing,
    Winning,
    Losing
}

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public int level;
    public Slot[] slotsPlay;
    public Slot[] slots;
    private Vector3 _target;
    public Unit carryingUnit;
    public GameObject panelGameOver;
    public GameState gameState = GameState.Playing;

    private Dictionary<int, Slot> slotDictionary;

    private Slot originalSlot;

    [SerializeField] private AudioClip putFlowerSound;


    public Action OnGameOver;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //CreateData();
    }

    private void Start()
    {
        StartCoroutine(LoadSlot());

        slotDictionary = new Dictionary<int, Slot>();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotId = i;
            slotDictionary.Add(i, slots[i]);
        }
        for (int i = 0; i < slotsPlay.Length; i++)
        {
            slotsPlay[i].slotId = i;
        }
        FlowerBegin();
        UnitUtils.InitResources();
        
    }

    IEnumerator LoadSlot()
    {
        foreach (Slot slot in slotsPlay)
        {
            slot.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.09f);
        }
    }

   

    private void OnEnable()
    {
        OnGameOver += GameOver;

        UserData.data.CoinsInCache = 0;
        UserData.data.ScoreInCache = 0;
        UserData.data.LevelInCache = 0;
    }

    private void OnDisable()
    {
        OnGameOver -= GameOver;
    }

    void FlowerBegin()
    {
        for (int i = 0;i < slotsPlay.Length;i++)
        {
            if (slotsPlay[i].slotType == SlotType.StartPlayable)
            {
                var indexValue = UnityEngine.Random.Range(0, UnitUtils.InitResources().items.Count);
                slotsPlay[i].CreateUnit(indexValue);
            }
        }
    }


    private void Update()
    {
        if (gameState != GameState.Pause)
        {
            if (slots[0].currentUnit == null && slots[1].currentUnit == null && slots[2].currentUnit == null)
            {
                PlaceRandom();
            }

            if (Input.GetMouseButtonDown(0))
            {
                SendRayCast();

            }

            if (Input.GetMouseButton(0) && carryingUnit)
            {
                OnItemSelected();
            }

            if (Input.GetMouseButtonUp(0))
            {
                //Drop item
                SendRayCast();
                originalSlot = null;
            }
            if (CheckGameOver() && gameState != GameState.Losing)
            {
                OnGameOver?.Invoke();
                ChangeGameState(GameState.Losing);
            }
        }
    }

    public void ChangeGameState(GameState targetState)
    {
        gameState = targetState;
    }

    void GameOver()
    {
        UIController.instance.GameOver();
        Time.timeScale = 0; 
    }

    bool CheckGameOver()
    {
        foreach (var slot in slotsPlay)
        {
            if (slot.slotState == SlotState.Empty)
            {
                return false;
            }
        }
        return true;
    }
    
    void SendRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var slot = hit.transform.GetComponent<Slot>();

            if (slot != null)
            {
                if (slot.slotState == SlotState.Full && carryingUnit == null)
                {
                    if (IsSlotInList(slot))
                    {
                        carryingUnit = slot.currentUnit;
                        originalSlot = slot;
                        //originalSlot.currentUnit.transform.SetParent(null);
                        originalSlot.ChangeStateTo(SlotState.Empty);
                        SoundFXManager.Instance.PlaySoundFXClip(putFlowerSound, 2f);
                    }
                }
                else if (slot.slotState == SlotState.Empty && carryingUnit != null)
                {
                    SoundFXManager.Instance.PlaySoundFXClip(putFlowerSound, 2f);
                    if (slot != originalSlot)
                    {
                        slot.transform.position = new Vector3(slot.transform.position.x, slot.transform.position.y, 0.28f);
                        originalSlot.currentUnit.transform.SetParent(slot.transform);
                        originalSlot.currentUnit.ChangeFlowerState(FlowerState.Prepare);
                        originalSlot.currentUnit.transform.localPosition = new Vector3(0, 1.37f, 0);
                        slot.currentUnit = originalSlot.currentUnit;
                        originalSlot.ItemPlaced();
                        slot.ChangeStateTo(SlotState.Full);
                    }
                    else
                    {
                        originalSlot.currentUnit.transform.SetParent(originalSlot.transform);
                        originalSlot.currentUnit.transform.localPosition = new Vector3(0, 1.37f, 0);
                        originalSlot.ChangeStateTo(SlotState.Full);
                    }
                    carryingUnit = null;
                    originalSlot = null;
                }
                else if (slot.slotState == SlotState.Full && carryingUnit != null)
                {
                    if (slot != originalSlot)
                    {
                        originalSlot.currentUnit.transform.SetParent(originalSlot.transform);
                        originalSlot.currentUnit.transform.localPosition = new Vector3(0, 1.37f, 0);
                        originalSlot.ChangeStateTo(SlotState.Full);
                    }
                    carryingUnit = null;
                    originalSlot = null;
                }
            }
            else
            {
                OnItemCarryFail();
            }

        }
        else
        {
            if (!carryingUnit)
            {
                return;
            }
            OnItemCarryFail();
        }
    }

    bool IsSlotInList(Slot slot)
    {
        return Array.IndexOf(slots, slot) != -1;
    }

    void OnItemCarryFail()
    {
        if (originalSlot != null)
        {
            originalSlot.currentUnit.transform.SetParent(
                originalSlot.transform);
            originalSlot.currentUnit.transform.localPosition = new Vector3(0, 1.37f, 0);
            originalSlot?.ChangeStateTo(
                SlotState.Full);
        }
        carryingUnit = null;
        originalSlot = null;
    }

    void OnItemSelected()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.forward, 0f);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            _target = ray.GetPoint(distance);
            _target.z = originalSlot.currentUnit.transform.position.z;
            float moveSpeed = 100f; 
            float delta = moveSpeed * Time.deltaTime;
            originalSlot.currentUnit.transform.position = Vector3.MoveTowards(
                carryingUnit.transform.position, _target, delta);
            originalSlot.currentUnit.ChangeFlowerState(FlowerState.Ready);
        }
    }


    public void PlaceRandom()
    {
        for (int i = 0;i < slots.Length;i++)
        {
            if (AllSlotsOccupied())
            {
                Debug.Log("No empty slot available!");
                return;
            }

            var rand = UnityEngine.Random.Range(0, slots.Length);
            var slot = GetSlotById(rand);

            while (slot.slotState == SlotState.Full)
            {
                rand = UnityEngine.Random.Range(0, slots.Length);
                slot = GetSlotById(rand);
            }

            var indexValue = UnityEngine.Random.Range(0, UnitUtils.InitResources().items.Count);
            slot.CreateUnit(indexValue);
        }
    }

    public bool AllSlotsOccupied()
    {
        foreach (var slot in slots)
        {
            if (slot.slotState == SlotState.Empty)
            {
                return false;
            }
        }
        return true;
    }

    Slot GetSlotById(int id)
    {
        return slotDictionary[id];
    }
}