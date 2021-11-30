using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public bool selected;
    GameMaster gm;

    public int tileSpeed;
    public bool hasMoved;
    public float moveSpeed;

    public int playerNumber;
    public int attackRange;
    List<Unit> enemiesInRange = new List<Unit>();
    public bool hasAttacked;

    public GameObject weaponIcon;
    Body unitBody;

    public int health;
    public int attackDamage;
    public int defenseDamage;
    public int armor;

    //public GameObject bloodPrefab;
    public DamageIcon damageIcon;

    public Text queenHealth;
    public bool isQueen;

    public GameObject explosionPrefab;
    Animator myAnimator;
    AudioManager myAudioManager;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myAudioManager = FindObjectOfType<AudioManager>();
        unitBody = GetComponentInChildren<Body>();
        if(playerNumber == 2)
        {
            unitBody.transform.Rotate(0, -90, 0);


        }
        else if(playerNumber ==1)
        {
            unitBody.transform.Rotate(0, 90, 0);
         
        }
        gm = FindObjectOfType<GameMaster>();
        UpdateQueenHealth();
    }
    
    public void UpdateQueenHealth()
    {
        if (isQueen == true)
        {
            queenHealth.text = health.ToString();
        }
    }

    private void OnMouseOver()
    {
        //if (Input.GetMouseButton(1))
        //{
        //    gm.ToggleStatsPanel(this);
        //}
        //gm.ToggleStatsPanel(this);
    }

    private void OnMouseEnter()
    {
        //if (Input.GetMouseButton(1))
        //{
        //    gm.ToggleStatsPanel(this);
        //}
        if (!gm.selectedUnit)
        {
            gm.ToggleStatsPanel(this);
        }
        else
        {
            return;
        }
    }

    private void OnMouseExit()
    {
        if (!gm.selectedUnit)
        {
            gm.ToggleStatsPanel(this);
        }
        else
        {
            return;
        }
    }

    private void OnMouseDown()
    {
        gm.RemoveStatsPanel(this);

            ResetWeaponIcons();
            if (selected == true)
            {
                selected = false;
                gm.selectedUnit = null;
                gm.ResetTiles();
            }
            else
            {
                if (playerNumber == gm.playerTurn)
                {
                    if (gm.selectedUnit != null)
                    {
                        gm.selectedUnit.selected = false;
                    }
                    selected = true;
                    gm.selectedUnit = this;
                    gm.ResetTiles();
                    GetEnemies();
                    GetWalkableTiles();
                }
            }
            Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 30f);
            Unit unit = col.GetComponent<Unit>();

            if (gm.selectedUnit != null)
            {
                if (gm.selectedUnit.enemiesInRange.Contains(unit) && gm.selectedUnit.hasAttacked == false)
                {
                    StartCoroutine(ProcessAtack(unit));

                }
            }
        
    }

    private void Attack(Unit enemy)
    {
        
        hasAttacked = true;
        int enemyDamage = attackDamage - enemy.armor;
        int myDamage = enemy.defenseDamage - armor;
        if (enemyDamage >= 1)
        {
            DamageIcon instance = Instantiate(damageIcon, new Vector3(enemy.transform.position.x, enemy.transform.position.y-.3f, -1.5f), Quaternion.identity);
            instance.Setup(enemyDamage);
            //Instantiate(bloodPrefab, new Vector3(enemy.transform.position.x, enemy.transform.position.y, -.7f), Quaternion.identity);
            enemy.health -= enemyDamage;
            enemy.UpdateQueenHealth();
        }

        if(myDamage >= 1)
        {
            DamageIcon instance = Instantiate(damageIcon, new Vector3(transform.position.x, transform.position.y - .3f, -1.5f), Quaternion.identity) ;
            //Instantiate(bloodPrefab, new Vector3(transform.position.x, transform.position.y, -.7f), Quaternion.identity);
            instance.Setup(myDamage);
            health -= myDamage;
            UpdateQueenHealth();
        }

        if(enemy.health <= 0)
        {
            myAudioManager.PlaySFX(1);
            enemy.gameObject.SetActive(false);
            Instantiate(explosionPrefab, new Vector3(enemy.transform.position.x, enemy.transform.position.y, -.7f), Quaternion.identity);
            GetWalkableTiles();
            gm.RemoveStatsPanel(enemy);
        }

        if(health <= 0)
        {
            gm.ResetTiles();
            gm.RemoveStatsPanel(this);
            gameObject.SetActive(false);
            myAudioManager.PlaySFX(1);
            Instantiate(explosionPrefab, new Vector3(enemy.transform.position.x, enemy.transform.position.y, -.7f), Quaternion.identity);
            
        }
        gm.UpdateStatsPanel();
        if (selected == true)
        {
            selected = false;
            gm.selectedUnit = null;
            gm.ResetTiles();
        }
    }

    private void GetWalkableTiles()
    {
        if (hasMoved == true)
        {
            return;
        }
        foreach(Tile tile in FindObjectsOfType<Tile>())
        {
            if (Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y) <= tileSpeed)
            {
                if (tile.IsClear() == true)
                {
                    tile.Highlight();
                }
            }
        }
    }



    private void GetEnemies()
    {
        enemiesInRange.Clear();
        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            if (Mathf.Abs(transform.position.x - unit.transform.position.x) + Mathf.Abs(transform.position.y - unit.transform.position.y) <= attackRange)
            {
                if(unit.playerNumber != gm.playerTurn && hasAttacked == false)
                {
                    enemiesInRange.Add(unit);
                    unit.weaponIcon.SetActive(true);
                }
            }
        }
    }

    public void ResetWeaponIcons()
    {
        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            unit.weaponIcon.SetActive(false);
        }
    }

    public void Move(Vector2 tilePositon)
    {
        gm.ResetTiles();
        StartCoroutine(StartMovement(tilePositon));
    }

    IEnumerator StartMovement(Vector2 tilePos)
    {
        while(transform.position.x != tilePos.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(tilePos.x, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
            yield return null;         
        }

        while (transform.position.y != tilePos.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, tilePos.y, transform.position.z), moveSpeed * Time.deltaTime);
            yield return null;
        }      
        hasMoved = true;
        ResetWeaponIcons();
        GetEnemies();
        gm.MoveStatsPanel(this);
    }

    IEnumerator ProcessAtack(Unit unit)
    {
        gm.selectedUnit.myAnimator.SetTrigger("Attack");
        myAudioManager.PlaySFX(2);
        yield return new WaitForSeconds(1f);
        gm.selectedUnit.Attack(unit);
    }
}
