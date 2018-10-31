using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class CloneController : MonoBehaviour
{
    private GameObject right_click;
    private GameObject enemy;
    public GameObject cloneInstance;
    private float slider_value;
    private float increment = .005f;
    private float moveSpeed;
    private float attackAnimationDelay;
    private float attackSpeed;
    private float DEFAULT_SPEED;
    private GameObject equip;
    private bool enemy_stored;
    private bool clone_ready;
    

    public GameObject cloneTemplate;
    public enum Mode { Empty, Stored, Spawned };
    public Mode currentMode;
    public float recorded_value;
    public bool active;


    // Use this for initialization
    void Start()
    {
        currentMode = Mode.Empty;
        right_click = FindObjectOfType<RightClickSlider>().gameObject;
        enemy_stored = false;
        clone_ready = false;
    }

    // Update is called once per frame
    void Update()
    {
        slider_value = right_click.GetComponent<RightClickSlider>().slider.value;
        enemy = right_click.GetComponent<RightClickSlider>().enemy;
        if (cloneInstance == null && enemy_stored == false)
        {
            currentMode = Mode.Empty;
        }
        else if (cloneInstance == null)
        {
            currentMode = Mode.Stored;
        }
        else
        {
            currentMode = Mode.Spawned;
            if (cloneInstance.GetComponent<PlayerAttackPattern>().entity == null)
            {
                cloneInstance.GetComponent<PlayerAttackPattern>().entity = cloneInstance.GetComponent<Entity>();
                cloneInstance.GetComponent<MovementPatternAbstract>().entity = cloneInstance.GetComponent<Entity>();
            }
            cloneInstance.GetComponent<CloneEntity>().setTargetVector(Input.mousePosition);
        }
        switch (currentMode)
        {
            case Mode.Empty:
                active = true;
                if (enemy != null)
                {
                    EmptyFill();
                    if (recorded_value == 1)
                    {
                        cloneTemplate = enemy.GetComponent<PrefabHolder>().prefab;
                        moveSpeed = enemy.GetComponent<Entity>().moveSpeed;
                        attackAnimationDelay = enemy.GetComponent<Entity>().attackAnimationDelay;
                        attackSpeed = enemy.GetComponent<Entity>().attackSpeed;
                        DEFAULT_SPEED = enemy.GetComponent<Entity>().DEFAULT_SPEED;
                        equip = enemy.GetComponent<Entity>().equip;

                        cloneTemplate = cloneTemplate.GetComponent<PrefabHolder>().prefab;
                        //Debug.Log(equip);
                        recorded_value = 0;
                        enemy_stored = true;
                    }
                }
                else
                {
                    setSliderPos2Mouse();
                }
                break;
            case Mode.Stored:
                active = true;
                StoredFill();
                if (cloneInstance == null && clone_ready)
                {
                    cloneInstance = Instantiate(cloneTemplate, transform.position, Quaternion.identity);
                    Destroy(cloneInstance.GetComponent<MovementPatternAbstract>());
                    Destroy(cloneInstance.GetComponent<AttackPatternAbstract>());
                    Entity prevEntity = cloneInstance.GetComponent<Entity>();
                    //Destroy(cloneInstance.GetComponent<Entity>());
                    if (cloneInstance.GetComponent<MeleeMovement>())
                        Destroy(cloneInstance.GetComponent<MeleeMovement>());

                    CloneEntity currentEntity = cloneInstance.AddComponent<CloneEntity>();

                    currentEntity.equip = prevEntity.equip;

                    /*
                    cloneInstance.GetComponent<Entity>().moveSpeed = this.moveSpeed;
                    cloneInstance.GetComponent<Entity>().attackAnimationDelay = this.attackAnimationDelay;
                    cloneInstance.GetComponent<Entity>().attackSpeed = this.attackSpeed;
                    cloneInstance.GetComponent<Entity>().DEFAULT_SPEED = this.DEFAULT_SPEED;
                    cloneInstance.GetComponent<Entity>().equip = this.equip;
                    cloneInstance.GetComponent<Entity>().createWeaponInstance();
                    //Debug.Log(cloneInstance.GetComponent<Entity>().equip);
                    */

                    cloneInstance.AddComponent<CloneMovement>();
                    cloneInstance.AddComponent<PlayerAttackPattern>();
                    cloneInstance.GetComponent<PlayerAttackPattern>().entity = cloneInstance.GetComponent<Entity>();
                    cloneInstance.GetComponent<Entity>().createWeaponInstance();
                    cloneInstance.GetComponent<Entity>().setTargetVector(Input.mousePosition);
                    //cloneInstance.GetComponent<PlayerAttackPattern>().setEntity(cloneInstance.GetComponent<Entity>());
                    
                    cloneInstance.layer = 9;
                    cloneInstance.tag = "Clone";
                    setCloneHealth();
                    cloneInstance.GetComponent<CloneEntity>().setColor();
                    Destroy(prevEntity);

                }

                recorded_value = 0;
                break;
            case Mode.Spawned:
                break;

        }
    }

    private void setCloneHealth()
    {
        cloneInstance.GetComponent<Entity>().max_health = GetComponent<PlayerEntity>().max_health;
        cloneInstance.GetComponent<Entity>().healthPoints = GetComponent<PlayerEntity>().healthPoints * recorded_value;
        GetComponent<PlayerEntity>().healthPoints -= cloneInstance.GetComponent<Entity>().healthPoints;
        cloneInstance.GetComponent<Entity>().checkHealth();
    }

    void EmptyFill()
    {
        if (Input.GetMouseButton(1) && active)
        {
            right_click.transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position);
            Vector3 new_position = (enemy.transform.position - right_click.transform.position);
            right_click.GetComponent<RightClickSlider>().circle.offset = new_position;
            right_click.GetComponent<RightClickSlider>().updateValue(slider_value + increment);
        }
        else
        {
            setSliderPos2Mouse();
        }
        if (Input.GetMouseButtonUp(1))
        {
            recorded_value = slider_value;
            right_click.GetComponent<RightClickSlider>().setZero();
        }
    }

    void StoredFill()
    {
        right_click.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 new_position = Camera.main.ScreenToWorldPoint(transform.position) - transform.position;
        right_click.GetComponent<RightClickSlider>().circle.offset = new_position;
        if (Input.GetMouseButton(1) && active)
        {
            if (slider_value == 1)
                slider_value = 0;
            right_click.GetComponent<RightClickSlider>().updateValue(slider_value + increment);
        }
        if (Input.GetMouseButtonUp(1))
        {
            recorded_value = slider_value;
            right_click.GetComponent<RightClickSlider>().setZero();
            clone_ready = true;
        }
    }

    void setSliderPos2Mouse()
    {
        right_click.transform.position = Input.mousePosition;
        Vector3 new_position = Camera.main.ScreenToWorldPoint(right_click.transform.position) - right_click.transform.position;
        right_click.GetComponent<RightClickSlider>().circle.offset = new_position;
    }
    PolygonCollider2D CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as PolygonCollider2D;
    }
}
