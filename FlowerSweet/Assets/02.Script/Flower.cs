using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.GraphicsBuffer;

public class Flower : Unit
{

    public FlowerData FlowerData;

    public PetalSlot[] petalSlots;

    [SerializeField] float detectionRadius;
    private bool isSweet = false;
    public Slot flowerSlot = null;

    private Dictionary<int, PetalSlot> listSlots;
    public Dictionary<int, Unit> listDetectFlower;


    public float scaleSpeed = 0.01f;

    [SerializeField] private AudioClip SweetSound;

    public Action OnSweet;
    private void Start()
    {
        PetalSlot[] childSlots = GetComponentsInChildren<PetalSlot>();
        flowerSlot = gameObject.transform.parent.GetComponent<Slot>();
        petalSlots = petalSlots.Concat(childSlots).ToArray();

        listSlots = new Dictionary<int, PetalSlot>();

        for (int i = 0; i < petalSlots.Length; i++)
        {
            petalSlots[i].id = i;
            listSlots.Add(i, petalSlots[i]);
        }

        if (flowerSlot.slotType == SlotType.StartPlayable)
        {
            TransferChildBegin();
            flowerSlot.ChangeTypeTo(SlotType.Playable);
        }
        else
            TransferChildTo();

        InvokeRepeating(nameof(UpdateListFlower), 0, 1f);
    }
    private void Update()
    {
        if (AllSlotsOccupied() && IsSweet() && !isSweet)
        {
            OnSweet?.Invoke();
            var stringJson = PlayerPrefs.GetString(GameData.PP_USER_DATA);
            Debug.Log(" UserData json : " + stringJson);
        }
        if (AllSlotsEmpty())
        {
            Destroy(gameObject, 1f);
            transform.parent.GetComponent<Slot>().ChangeStateTo(SlotState.Empty);
        }
        if (IsParentInSlots() && gameObject.GetComponent<Unit>().FlowerState == FlowerState.Prepare)
        {
            //UnitDetectAround();
        }
    }


    public void InitFLowerData(int slotId, int id, FlowerState flowerState)
    {
        FlowerData.Id = id;
        FlowerData.SlotId = slotId;
    }
    private void OnEnable()
    {
        OnSweet += Sweet;
    }
    private void OnDisable()
    {
        OnSweet -= Sweet;
    }
    public int ReturnNumberPetal()
    {
        foreach (PetalSlot petal in petalSlots)
        {
            if (petal.slotState == State.Full)
            {
                FlowerData.PetalNumber++;
            }
        }
        return FlowerData.PetalNumber;
    }
    private void Sweet()
    {
        isSweet = true;
        Destroy(gameObject, 1f);
        IncreaseCoinsOverTime(10);
        ProgressBar.Instance.TickScore();
        UserData.data.CoinsInCache += 10;
        transform.parent.GetComponent<Slot>().ChangeStateTo(SlotState.Empty);
        //Play VFX sound
        SoundFXManager.Instance.PlaySoundFXClip(SweetSound, 1f);
    }
    void IncreaseCoinsOverTime(int coins)
    {
        int currentCoins = 0;
        while (currentCoins < coins)
        {
            currentCoins++;
            UserData.data.Coins += currentCoins;
            GameSharedUI.Instance.UpdateUIText();
        }
    }
    public bool IsParentInSlots()
    {
        if (flowerSlot != null)
            ChangeParentSlot(gameObject.transform.parent.GetComponent<Slot>());
        if (flowerSlot != null)
        {
            if (flowerSlot.slotType == SlotType.Playable)
            {
                return true;
            }
        }
        return false;
    }
    private bool IsSweet()
    {
        Unit unitFirst = petalSlots[0].transform.GetChild(0).GetComponent<Unit>();

        for (int i = 1; i < petalSlots.Length; i++)
        {
            Unit unit = petalSlots[i].transform.GetChild(0).GetComponent<Unit>();

            if (unit.color != unitFirst.color)
            {
                return false;
            }
        }
        return true;
    }
    bool AllSlotsEmpty()
    {
        foreach (var slot in petalSlots)
        {
            if (slot.slotState == State.Full)
            {
                return false;
            }
        }
        return true;
    }
    public void TransferChildTo()
    {
        List<int> index = new List<int>();
        int pistilId = FlowerData.Id;
        index.Add(pistilId);
        int ran = UnityEngine.Random.Range(1, petalSlots.Length - 3);

        for (int i = 0; i < ran; i++)
        {

            int indexValue = UnityEngine.Random.Range(0, UnitUtils.InitChildResources().items.Count);

            index.Add(indexValue);
        }

        index.Sort();

        for (int i = 0; i < index.Count; i++)
        {
            petalSlots[i].CreateChild(index[i]);
            PetalDefind petalDefind = new PetalDefind();
            petalDefind.Id = index[i];
            petalDefind.IdSlot = i;
            switch (index[i])
            {
                case 0: petalDefind.PetalColor = Color.Blue; break;
                case 1: petalDefind.PetalColor = Color.Green; break;
                case 2: petalDefind.PetalColor = Color.Purple; break;
                case 3: petalDefind.PetalColor = Color.Red; break;
                case 4: petalDefind.PetalColor = Color.White; break;
                case 5: petalDefind.PetalColor = Color.Yelow; break;
            }
            FlowerData.Name = gameObject.name;
            FlowerData.PetalNumber = index.Count;
            FlowerData.ListPetalDefind.Add(petalDefind);
        }

    }
    private void TransferChildBegin()
    {
        int pistilId = FlowerData.Id;
        for (int i = 0; i < petalSlots.Length - 3; i++)
        {
            petalSlots[i].CreateChild(pistilId);
        }
        int indexValue2 = UnityEngine.Random.Range(0, UnitUtils.InitChildResources().items.Count);
        while (pistilId == indexValue2)
            indexValue2 = UnityEngine.Random.Range(0, UnitUtils.InitChildResources().items.Count);

        for (int i = 3; i < petalSlots.Length; i++)
        {
            petalSlots[i].CreateChild(indexValue2);
        }

    }
    public bool AllSlotsOccupied()
    {
        foreach (var slot in petalSlots)
        {
            if (slot.slotState == State.Empty)
            {
                return false;
            }
        }
        return true;
    }
    PetalSlot GetSlotById(int index)
    {
        return listSlots[index];
    }   

    private void UpdateListFlower()
    {
        
    }

    /*
    
    public void UnitDetectAround()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        

        bool anyColliderWithUnit = colliders.Any(collider => collider != null && collider.TryGetComponent(out Flower unit));

        if (anyColliderWithUnit)
        {
            foreach (Collider collider in colliders)
            {
                if (collider != null && collider.TryGetComponent(out Flower unit))
                {
                    if (unit.Type == Type.Pistil 
                        && unit.FlowerData.SlotId != FlowerData.SlotId
                        && unit.FlowerState == FlowerState.Prepare)
                    {
                            bool unitExists = flowerList.Any(
                                existingUnit => existingUnit.FlowerData.SlotId == unit.FlowerData.SlotId);

                            if (!unitExists)
                            {
                                flowerList.Add(unit);
                            }
                        for (int i = 0; i < flowerList.Count; i++)
                        {
                            if (flowerList[i] == null)
                            {
                                flowerList.Remove(flowerList[i]);
                                return;
                            }
                            if (flowerList[i] != null && flowerList[i] != gameObject)
                            {
                                if (gameObject.transform.parent != null && flowerList[i].transform.parent != null)
                                {
                                    Flower mainFlower = gameObject.GetComponent<Flower>();
                                    Flower otherFlower = flowerList[i].GetComponent<Flower>();

                                    if (mainFlower != null && otherFlower != null
                                        && mainFlower.transform.parent.GetComponent<Slot>().slotType == SlotType.Playable
                                        && otherFlower.transform.parent.GetComponent<Slot>().slotType == SlotType.Playable)
                                    {
                                        ComparePetal(otherFlower);
                                    }
                                }
                            }
                        }
                    }
                } 
                else if (collider == null)
                {
                    return;
                }
            }
        }
    }

    */
    private void ComparePetal(Flower otherFlower)
    {
        Flower pistilMain = gameObject.GetComponent<Flower>();
        Flower pistilOther = otherFlower.GetComponent<Flower>();
       

        /*
            for (int i = 0; i < petalSlots.Length; i++)
            {
                if (petalSlots[i].slotState == State.Full)
                {
                for (int j = 0; j < otherFlower.petalSlots.Length; j++)
                {
                    if (otherFlower.petalSlots[j].slotState == State.Full)
                    {
                        Unit petalMain = petalSlots[i].transform.GetChild(0).GetComponent<Unit>();
                        Unit petalOther = otherFlower.petalSlots[j].transform.GetChild(0).GetComponent<Unit>();
                     
                            if (petalMain.Color == petalOther.Color)
                            {
                                MovePetalToEmptyAdjacentSlot(i, j, otherFlower);
                            }
                            if (petalMain.Color != pistilOther.Color)
                            {
                                return;
                            }

                            if (pistilMain.Color == petalMain.Color && petalMain.Color == petalOther.Color)
                            {
                                MovePetalToEmptyAdjacentSlot(i, j, otherFlower);
                            }
                            if (pistilMain.Color == petalOther.Color)
                            {
                                MovePetalToEmptyAdjacentSlot(i, j, otherFlower);
                            }
                            if (petalMain.Color == petalOther.Color)
                            {
                                MovePetalToEmptyAdjacentSlot(i, j, otherFlower);
                            }

                            if ( petalMain.Color == petalOther.Color)
                            {
                                MovePetalToEmptyAdjacentSlot(i, j, otherFlower);
                            }
                        
                        
                    }
                }
            }
        }
*/
    }

    
    private void MovePetalToEmptyAdjacentSlot(int i, int j, Flower flower)
    {
        PetalSlot slotMain = GetEmptyAdjacentSlot(i);

        if (slotMain != null && slotMain.slotState == State.Empty)
        {
            Transform targetPetal = flower.petalSlots[j].transform.GetChild(0);

            if (targetPetal != null)
            {
                if (flower.petalSlots[j].slotState == State.Full)
                {
                    Vector3 targetPos = flower.petalSlots[j].transform.position;
                    targetPetal.parent = slotMain.transform;
                    targetPetal.localPosition = slotMain.transform.InverseTransformPoint(targetPos);
                    targetPetal.localRotation = Quaternion.Euler(-89.98f, 0, 0);
                    StartCoroutine(MoveToOrigin(targetPetal, Vector3.zero));
                    slotMain.slotState = State.Full;
                    flower.petalSlots[j].slotState = State.Empty;
                }
            }   
        }
        else
        {
            Debug.Log("No empty slot available or target slot is full!");
        }
    }
    IEnumerator MoveToOrigin(Transform transform, Vector3 target)
    {
        float duration = 0.5f;
        float elapsed = 0.0f;
        Vector3 originalPosition = transform.localPosition;
        float progress = 0.0f;

        while (elapsed < duration)
        {
            progress = Mathf.Clamp01(elapsed / duration);
            transform.localPosition = Vector3.Lerp(originalPosition, target, progress);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = target;

    }
    PetalSlot GetEmptyAdjacentSlot(int currentIndex)
    {
        // Tìm kiếm BloomSlot trống kế cạnh
        for (int k = currentIndex + 1; k < petalSlots.Length; k++)
        {
            if (petalSlots[k].slotState == State.Empty)
            {
                return petalSlots[k];
            }

        }
        if (currentIndex != 0)
        {
            for (int l = currentIndex - 1; l >= 0; l--)
            {
                if (petalSlots[l].slotState == State.Empty)
                {
                    return petalSlots[l];
                }
            }
        }

        return null;
    }

    public void ChangeParentSlot(Slot targetSlot)
    {
        flowerSlot = targetSlot;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
