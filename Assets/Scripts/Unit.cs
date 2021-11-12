using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public DamageIcon damageIcon;

    // Start is called before the first frame update
    void Start()
    {
        unitBody = GetComponentInChildren<Body>();
        if(transform.position.x >= 0)
        {
            unitBody.transform.Rotate(0, 0, 0);

        }
        else if(transform.position.x <0)
        {
            unitBody.transform.Rotate(0, 180, 0);
        }
        gm = FindObjectOfType<GameMaster>();
    }

    
    private void OnMouseDown()
    {
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
        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 10f);   
            Unit unit = col.GetComponent<Unit>();

            if (gm.selectedUnit != null)
            {
                if (gm.selectedUnit.enemiesInRange.Contains(unit) && gm.selectedUnit.hasAttacked == false)
                {
                    gm.selectedUnit.Attack(unit);
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
            DamageIcon instance = Instantiate(damageIcon, new Vector3(enemy.transform.position.x, enemy.transform.position.y, -.7f), Quaternion.identity);
            instance.Setup(enemyDamage);
            enemy.health -= enemyDamage;
        }

        if(myDamage >= 1)
        {
            DamageIcon instance = Instantiate(damageIcon, new Vector3(transform.position.x, transform.position.y, -.7f), Quaternion.identity);
            instance.Setup(myDamage);
            health -= myDamage;
        }

        if(enemy.health <= 0)
        {
            enemy.gameObject.SetActive(false);
            GetWalkableTiles();
        }

        if(health <= 0)
        {
            gm.ResetTiles();
            Destroy(this.gameObject);
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
    }
}
