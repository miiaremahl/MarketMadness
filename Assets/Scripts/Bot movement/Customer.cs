using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Customer behavious class.
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 24.1.2021
 * 
 * References:
 * 1. Unity NavMesh tutorial: https://learn.unity.com/tutorial/unity-navmesh#5c7f8528edbc2a002053b497 , examples on how to make the navmesh agent move
 * 2. Brackeys - ENEMY AI - Making an RPG in Unity (E10) : https://www.youtube.com/watch?v=xppompv1DBg, took tips on how to 
 * 3. bigbat - How to get a random point on NavMesh? : https://answers.unity.com/questions/475066/how-to-get-a-random-point-on-navmesh.html, took an idea on a random place to walk in navmesh
 */

namespace Customers
{
    public class Customer : MonoBehaviour
    {
        #region Variables

        #region Object references
        [Header(header: "Referenced objects")]
        //Ref to nasmesh agent
        public NavMeshAgent agent;

        //responsible for animation changes
        public AnimationHandler animationHandler;

        //Item selector reference
        public Items.ItemHandler itemHandler;

        //Player
        public GameObject player;

        //graphics of the customer
        public Transform graphics;

        //reference to hand
        public GameObject hand;
        #endregion

        #region destinations

        //walking radius for random destination
        public float randomWalkDist = 30f;

        [Header(header: "Adjustments")]
        public float cashDistAdj = 1f; //adjust how close the customer has to be to the cashier

        //the destination to go when leacing store
        public Transform leavingDestination;

        //Cashier
        public Transform cashier;

        //destination of navigation
        public Vector3 navigationDest;

        #endregion

        #region items
        //what we are focusing on 
        private Interactable focus;

        //item carried
        private Items.Item pickedItem;

        //item type that customer is looking for 
        public ItemData lookedItemType;

        //does customer have an item
        public bool carryingItem;

        //Item to pick
        public Items.Item destinationItem;

        //is item destination set
        public bool itemDestSet;
        #endregion

        //state of the customer
        public CustomerState currentState;

        //state options
        public enum CustomerState
        {
            NotMoving,
            MovingToItemGroup,
            MovingToPlayer,
            MovingToCashier,
            MovingToItem,
            LeavingStore,
            MovingToRandom,
            InFight
        }

        //is game paused / ended
        private bool paused = false;
        private bool gameEnded = false;

        [Header(header: "Raycast")]
        //How long distance do we count for the raycast to hit object
        public float maxRayHitDist = 3f;

        //is player fighting
        public bool isFighting = false;

        //rotated
        private bool rotated;

        //aisle helper to get forbidden areas of the store
        public ProceduralLogic.AisleHelper aislehelper;

        //store cordinates
        public float zMaxlimit;
        public float zMinlimit;
        public float xMinLimit;
        public float xMaxLimit;


        //ref to face
        //public Renderer face;

        ////faces
        //public Material smileFace;
        //public Material sadFace;
        //public Material angryFace;


        #region audio
        //falling voice
        public AudioSource fall;

        //hitting audio
        public AudioSource hit;

        //fighting audio
        public AudioSource fighting;

        //laughing/being angry
        public AudioSource laugh;
        public AudioSource anger;
        #endregion

        #endregion

        void Start()
        {
            currentState = CustomerState.NotMoving;
            zMaxlimit = aislehelper.zMaxlimit;
            zMinlimit = aislehelper.zMinlimit;
            xMinLimit = aislehelper.xMinLimit;
            xMaxLimit = aislehelper.xMaxLimit;
        }

        void Update()
        {
            if (!paused && !gameEnded) //are we playing
            {
                RaycastHit hit;
                if (Physics.SphereCast(transform.position, 1.3f, transform.TransformDirection(Vector3.forward), out hit, 0.5f)) //is SphereCast hitting something
                {
                    if (!carryingItem) //only if we are not carrying anything
                    {
                        Interactable interactable = hit.transform.gameObject.GetComponent<ItemPickUp>();
                        if (interactable != null)
                        {
                            Items.Item item = interactable.GetComponent<Items.Item>();
                            if (hit.distance < maxRayHitDist && lookedItemType.type == item.itemData.type) //is the hit within the distance and interactable
                            {
                                if (!item.carried) //check if someone else has it
                                {
                                    CarryItem(interactable);
                                }
                            }
                        }

                    }
                }
                switch (currentState) //check state
                {
                    case CustomerState.NotMoving:
                        break;
                    case CustomerState.MovingToItemGroup:
                        NavigateToItemGroup();
                        break;
                    case CustomerState.MovingToPlayer:
                        MoveToPlayer();
                        break;
                    case CustomerState.MovingToCashier:
                        MoveToCashier();
                        break;
                    case CustomerState.MovingToItem:
                        NavigateToItem();
                        break;
                    case CustomerState.MovingToRandom:
                        CheckIfReached();
                        break;
                    case CustomerState.InFight:
                        CheckIfAdjusted();
                        break;
                    default:
                        break;
                }
                #region OLD code
                // customer moves w mouse clicks
                //if (!item)
                //{
                //    if (Input.GetMouseButtonDown(0))
                //    {
                //        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                //        RaycastHit hit;

                //        if (Physics.Raycast(ray, out hit))
                //        {
                //            //Move agent
                //            agent.SetDestination(hit.point);
                //            Debug.Log(hit.point);
                //        }
                //    }
                //}
                //else{
                //    if (!navigating)
                //    {
                //        NavigateToItem();
                //    }
                //}
                #endregion
            }
        }

        //lose item in fight
        public Items.Item LoseItem()
        {
            return pickedItem;
        }

        //Carry given item
        public void CarryItem(Interactable interactable) {
            animationHandler.CarryShelfItem();
            pickedItem = interactable.gameObject.GetComponent<Items.Item>();
            itemHandler.ItemPicked(pickedItem); //remove from free to pick list
            pickedItem.carried = true; //set the item to be carried
            StartMovingToCashier(); //move to cashier
            carryingItem = true;
            interactable.gameObject.transform.parent = hand.transform; //visually change the item to be in hand
            interactable.gameObject.transform.position = hand.transform.position;
        }

        //get item from fight
        public void GainItemFromFight(Interactable interactable)
        {
            StartMovingToCashier(); //move to cashier
            carryingItem = true;
            pickedItem = interactable.gameObject.GetComponent<Items.Item>();
            pickedItem.carried = true;
            interactable.gameObject.transform.parent = hand.transform; //visually change the item to be in hand
            interactable.gameObject.transform.position = hand.transform.position;
        }

        //Checks if customer has reached the random position they were suppose to
        private void CheckIfReached()
        {
            float distance = Vector3.Distance(navigationDest, transform.position);
            if (distance <= agent.stoppingDistance+1)
            {
                if (player.GetComponent<Player.PlayerBehaviour>().carryingItem) //player might be carrying item we want
                {
                    if (player.GetComponent<Player.PlayerBehaviour>().itemCarried.itemData == lookedItemType)
                    {
                        if (player.GetComponent<Player.PlayerBehaviour>().isChased < 3)
                        {
                            StartMovingToPlayer(); //navigate to player (if they have the wanted item)
                        }
                        else
                        {
                            FindNewDestination(); //new destination
                        }
                    }
                    else
                    {
                        FindNewDestination(); //new destination
                    }
                }
                else
                {
                    FindNewDestination();
                }
            }
        }

        //check if customer has responded to player's fight (adjust the rotation etc)
        private void CheckIfAdjusted()
        {
            float distance = Vector3.Distance(navigationDest, transform.position);
            if (distance <= agent.stoppingDistance + 0.1) //if reached then stay still
            {
                navigationDest = transform.position;
                agent.SetDestination(navigationDest);
            }
        }

        //start carrying an item
        public void StartMovingToCashier()
        {
            rotated = false;
            currentState = CustomerState.MovingToCashier;
        }

        //start moving towards player
        public void StartMovingToPlayer()
        {
            rotated = false;
            currentState = CustomerState.MovingToPlayer;
            player.GetComponent<Player.PlayerBehaviour>().BeingChased();
        }

        //Start movement to given item group
        public void StartMovingToItemGroup()
        {
            animationHandler.StartRunningToItem();
            itemDestSet = false;
            rotated = false;
            currentState = CustomerState.MovingToItemGroup;
        }

        //Start movement to given item
        public void StartMovingToItem()
        {
            itemDestSet = false;
            rotated = false;
            currentState = CustomerState.MovingToItem;
        }

        //Object navigates to player
        //refs: 2. Brackeys, tips on how to move towards player
        private void MoveToPlayer()
        {
            if (player.transform.position != navigationDest) //check if the player pos is same
            {
                navigationDest = player.GetComponent<Player.PlayerBehaviour>().orientation.position;
                agent.SetDestination(navigationDest); //set destination
            }

            float distance = Vector3.Distance(navigationDest, transform.position);
            if (distance <= agent.stoppingDistance+0.5) //rotate if we are close
            {
                agent.SetDestination(transform.position);
                navigationDest = transform.position;
                if (!rotated)
                {
                    rotated = true;
                    FaceObj(navigationDest); //face the object navigated towards to

                    Player.PlayerBehaviour playerBehav = player.GetComponent<Player.PlayerBehaviour>();
                    if (playerBehav.carryingItem && playerBehav.isFighting == false) //player might be carrying item we want
                    {
                        if (playerBehav.itemCarried.itemData == lookedItemType)
                        {
                            //face.materials[1] = angryFace;
                            animationHandler.StartFighting();
                            isFighting = true; //start a fight about the item
                            playerBehav.StartFighting(gameObject.GetComponent<Customer>());
                            hit.Play(); //audio
                            fighting.Play();
                        }
                        else
                        {
                            FindNewDestination();
                        }
                    }
                    else
                    {
                        FindNewDestination();
                    }
                }
            }
            
        }

        //Get into fight
        public void GetIntoFight(Transform playerpos)
        {
            navigationDest = player.GetComponent<Player.PlayerBehaviour>().orientation.position;
            agent.SetDestination(navigationDest); //stay still (when player reached)
            currentState = CustomerState.InFight;
            animationHandler.FightWithItem(); //animation
            hit.Play(); //audio
            fighting.Play();
        }

        //win the item fight
        public void FightWon(Interactable interactable)
        {
            hit.Pause();
            fighting.Pause();
            laugh.Play();
            animationHandler.StartRunningAfterWon(); //animation
            //face.materials[1] = smileFace;
            GainItemFromFight(interactable); //get item
            isFighting = false;
            player.GetComponent<Player.PlayerBehaviour>().NotChasedAnyMore();
        }

        //win fight after you allready had an item
        public void FightWonWhenChallenged()
        {
            animationHandler.WinFightWithItem(); //animation
            hit.Pause();
            fighting.Pause();
            laugh.Play();
            StartMovingToCashier(); //go the cashier
            isFighting = false;
        }

        //lose the item fight
        public void FightLost()
        {
            //face.materials[1] = sadFace;
            pickedItem = null;
            hit.Pause();//audio
            fighting.Pause();
            anger.Play();
            animationHandler.Fall(); //animation
            StartCoroutine(Fall());
            isFighting = false;
            currentState = CustomerState.NotMoving; //state
            player.GetComponent<Player.PlayerBehaviour>().NotChasedAnyMore();
            if (carryingItem) //lose item if you had one
            {
                carryingItem = false;
            }
        }

        //find new destination, refs: 3. bigbat 
        public void FindNewDestination()
        {
            currentState = CustomerState.MovingToRandom; //random destination

            bool destFound = false;
            while (!destFound)
            {
                Vector3 randomDirection = (Random.insideUnitSphere * randomWalkDist) + transform.position;
                NavMeshHit navmeshHit;
                NavMesh.SamplePosition(randomDirection, out navmeshHit, randomWalkDist, 1);
                Vector3 position = navmeshHit.position;

                if (IsInsideStore(position))
                {
                    destFound = true;
                    navigationDest = navmeshHit.position;
                    agent.SetDestination(navigationDest);
                }
            }
           
        }

        //Checks if the position is inside the store
        public bool IsInsideStore(Vector3 position)
        {
            //out of the store
            if (zMaxlimit-1 > position.z && xMaxLimit-1 > position.x && xMinLimit+1 < position.x && zMinlimit+1 < position.z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Face the object (Rotate the graphics child)
        //refs: 2. Brackeys, on how to rotate correctly
        private void FaceObj(Vector3 pos)
        {
            Vector3 direction = (pos - graphics.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        //rotate towards item
        private void RotateToItem(Vector3 pos)
        {
            Vector3 direction = (pos - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
        }

        //Object navigates to cashier
        private void MoveToCashier()
        {
            if (cashier.position != navigationDest) //check destination
            {
                navigationDest = cashier.position;
                agent.SetDestination(navigationDest); //set destination
            }

            float distance = Vector3.Distance(navigationDest, transform.position);
            if (distance <= agent.stoppingDistance + cashDistAdj)
            {
                if (!rotated)
                {
                    rotated = true;
                    //face the object navigated towards to
                    FaceObj(navigationDest);
                }
            }
        }

        //Object navigates to certain item group
        //refs:1 Unity NavMesh tutorial, agent movement tips
        private void NavigateToItemGroup()
        {
            if (!itemDestSet)
            {
                navigationDest = itemHandler.getItemGroupDestination();
                agent.SetDestination(navigationDest);
                itemDestSet = true;
                lookedItemType = itemHandler.GetDestinationItemType();
            }
        }

        //Object navigates to certain item
        private void NavigateToItem()
        {
            if (itemDestSet && destinationItem == null)
            {
                FindNewDestination();
              
            }

            if (destinationItem != null)
            {
                if (destinationItem.carried)//need a new item destination
                {
                    FindNewDestination();
                }
            }
         
            if (!itemDestSet)
            {
                if (!itemDestSet)
                {
                    if (itemHandler.ItemsAvailable() == true) //items still left
                    {
                        destinationItem = itemHandler.getDestinationItem(); //get destination to item
                        navigationDest = destinationItem.gameObject.transform.position;
                        agent.SetDestination(navigationDest);
                        itemDestSet = true;
                    }
                    else
                    {
                        if (player.GetComponent<Player.PlayerBehaviour>().carryingItem) //player might be carrying item we want
                        {
                            if (player.GetComponent<Player.PlayerBehaviour>().itemCarried.itemData == lookedItemType)
                            {
                                if (player.GetComponent<Player.PlayerBehaviour>().isChased < 3)
                                {
                                    StartMovingToPlayer(); //no items left -> navigate to player (if they have the wanted item)
                                }
                                FindNewDestination();
                            }
                            FindNewDestination();
                        }
                        else
                        {
                            FindNewDestination();
                        }
                    }
                }

                float distance = Vector3.Distance(navigationDest, transform.position);
                if (distance <= agent.stoppingDistance) //rotate
                {
                    RotateToItem(navigationDest);
                }
            }
        }

        //Leave the store
        private void LeaveStore()
        {
            currentState = CustomerState.LeavingStore;
            navigationDest = leavingDestination.position;
            agent.SetDestination(navigationDest);
        }

        //Trigger enter
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("ItemTrigger")) //item
            {
                if (currentState == CustomerState.MovingToItemGroup)
                {
                    StartMovingToItem();
                }
            }
            else if (other.gameObject.CompareTag("Cashier")) //cashier
            {
                if (pickedItem != null)
                {
                    itemHandler.ItemPaid(pickedItem);
                    LeaveStore();
                }
            }
            else if (other.gameObject.CompareTag("RemovingSpot") && currentState == CustomerState.LeavingStore) //customer gets destroyed
            {
                Destroy(gameObject);
            }
        }

        //Coroutine for the fall timing
        IEnumerator Fall()
        {
            fall.Play();
            yield return new WaitForSeconds(1f);
            animationHandler.GetUp();
            yield return new WaitForSeconds(3f);
            animationHandler.StandStillAfterGU();
            yield return new WaitForSeconds(1f);
            animationHandler.StartRunningFromId();
            FindNewDestination();
        }

        #region game state
        //game paused
        public void PauseGame()
        {
            paused = true;
        }

        //game resuming
        public void ResumeGame()
        {
            paused = false;
        }

        //game ended
        public void EndGame()
        {
            gameEnded = true;
        }
        #endregion
    }
}
