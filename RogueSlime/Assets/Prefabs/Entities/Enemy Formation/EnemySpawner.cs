using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public float width = .32f;
	public float height = .32f;

    public bool preset = false;
    public bool spawned = false;
    public bool enable_doors;
    public GameObject[] doors = { null, null, null, null };
	private Position[] positions;
	private PlayerEntity player;

	void Start () {
		player = GameObject.FindObjectOfType<PlayerEntity> ();
	}

    private void Update()
    {
        if(EnemiesAreDead())
        {
            for (int i = 0; i < 4; i++)  //LOCK DOORS ON ENTRY
            {
                if (doors[i] != null)
                    doors[i].GetComponent<Doors>().unlockDoors();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<PlayerEntity>())
            for(int i = 0; i < 4; i++)  //LOCK DOORS ON ENTRY
            {
                if (doors[i] != null)
                    doors[i].GetComponent<Doors>().lockDoors();
            }
            if (!preset)
            {
                PlayerEntity player = col.GetComponent<PlayerEntity>();
                if (player && !spawned)
                {
                    float difference_x = transform.position.x - player.transform.position.x;
                    float difference_y = transform.position.y - player.transform.position.y;
                    foreach (Transform child in transform)
                    {
                        Vector3 new_position = child.transform.position;
                        if (Mathf.Abs(difference_x) > Mathf.Abs(difference_y))
                            if (difference_x > 0)
                                new_position.x += 0.28f;
                            else
                                new_position.x -= 0.28f;
                        else
                            if (difference_y > 0)
                            new_position.y += 0.28f;
                        else
                            new_position.y -= 0.28f;
                        child.position = new_position;
                    }
                    SpawnEnemies();
                }
            }
            else
                SpawnEnemies();

    }
    void OnDrawGizmos()
	{
		Gizmos.DrawWireCube (transform.position, new Vector3 (width, height));
	}

	public bool EnemiesAreDead()
	{
		foreach (Transform child in transform)
            foreach (Transform child2 in transform)
			    if (child2.childCount > 0)
				    return false;
		return true;
	}

	void SpawnEnemies()
	{
        spawned = true;
		foreach (Transform child in transform) {
            if (!child.gameObject.GetComponent<Position>().born)
            {
                child.gameObject.GetComponent<Position>().born = true;
                GameObject enemy = Instantiate(child.gameObject.GetComponent<Position>().enemy[Random.Range(0, child.gameObject.GetComponent<Position>().enemy.Length)], child.transform.position, Quaternion.identity) as GameObject;
                enemy.transform.parent = child;
            }
		}
	}
}