using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileColliderManager : MonoBehaviour {

	// All gameobjects with one of these tags will be included when creating colliders
	public string[] tagsOfTiles;
	// Tag to apply to collider
	public string colliderTag = "GroundCollider";
	// Physics2D material for colliders
	public PhysicsMaterial2D colliderMaterial;

	// Width of a tile
	public float tileWidth = 1;
	public float tileHeight = 1;

	// ExtraMethods script
	public ExtraMethods eMethods;

	// Tiles to manage
	private List<GameObject> tiles = new List<GameObject> ();
	// Colliders
	private List<BoxCollider2D> cols = new List<BoxCollider2D> ();
	
	/*
	 * This script is for detecting all tiles (as specified in Inspector) and creating
	 * colliders for them, then merging them as necessary so as to minimize the total
	 * colliders in the scene. While this is meant to be on a Game Controller, this is not necessary.
	 * 
	 * Minimizing colliders helps in two ways. First, it fixes the common 'tripping' problem
	 * encountered in 2D platformers when multiple box colliders are sitting right next to each other.
	 * Secondly, it increases performance as the physics engine won't need to check as many colliders
	 * for collisions.
	 */

	// Initialization
	void Start () 
	{
		if (colliderMaterial == null)
			Debug.LogWarning ("Collider Material not specified. None will be applied.");

		// Retrieve all objects specified for management
		foreach (string tag in tagsOfTiles)
		{
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag (tag))
			{
				tiles.Add (obj);
			}
		}

		while (tiles.Count > 0)
		{
			// Create a new empty GameObject to hold the collider, and place it at the position
			// of the first box
			GameObject colParent = new GameObject();
			colParent.transform.position = tiles[0].transform.position;
			colParent.tag = colliderTag;

			// Create the collider
			BoxCollider2D col = colParent.AddComponent<BoxCollider2D> ();
			col.size = new Vector2 (tileWidth, tileHeight);

			if (colliderMaterial != null)
				col.sharedMaterial = colliderMaterial;

			// Remove tile from list, as it now has a collider covering it
			tiles.RemoveAt (0);

			// Whether or not to continue while loop
			bool cont = true;

			while (cont && tiles.Count > 0)
			{
				Vector3 parentPos = colParent.transform.position;

				// Check for adjacent block
				for (int i = 0; i < tiles.Count; i++)
				{
					Vector3 pos = tiles[i].transform.position;
					// Positions to test for tiles at
					Vector2 check = new Vector2 ((parentPos.x-col.size.x/2)-tileWidth/2,
					                             (parentPos.x+col.size.x/2)+tileWidth/2);

					// Searching a small range prevents rounding errors and human error from messing things up.
					if (eMethods.inRange (pos.x, check.x - 0.01f, check.x + 0.01f)
					    && eMethods.inRange (pos.y, parentPos.y - 0.01f, parentPos.y + 0.01f))
					{
						// Left
						// Make sure loop continues
						cont = true;
						// Move parent gameObject
						colParent.transform.position = new Vector3 (colParent.transform.position.x - tileWidth/2,
						                                            colParent.transform.position.y,
						                                            colParent.transform.position.z);
						// Adjust collider size
						col.size = new Vector2 (col.size.x + tileWidth, col.size.y);
						// Remove tile as it now has a collider
						tiles.RemoveAt (i);
						// Break for loop
						break;
					}
					else if (eMethods.inRange (pos.x, check.y - 0.01f, check.y + 0.01f)
					         && eMethods.inRange (pos.y, parentPos.y - 0.01f, parentPos.y + 0.01f))
					{
						// Right
						cont = true;
						colParent.transform.position = new Vector3 (colParent.transform.position.x + tileWidth/2,
						                                            colParent.transform.position.y,
						                                            colParent.transform.position.z);
						col.size = new Vector2 (col.size.x + tileWidth, col.size.y);
						tiles.RemoveAt (i);
						break;
					}
					else
						cont = false;
				}
			}

			// Add the finished collider to the list of colliders
			cols.Add (col);
		}

		int colID = 0;

		// Merge colliders
		// Operates very similarly to how tiles were handled
		while (cols.Count > 0)
		{
			// Get first collider
			BoxCollider2D col = cols[0];
			cols.RemoveAt (0);

			// Name colliders gameObject
			col.gameObject.name = "Tile Collider " + colID;

			bool cont = true;

			while (cont && cols.Count > 0)
			{
				Vector3 selfPos = col.transform.position;

				for (int i = 0; i < cols.Count; i++)
				{
					Vector3 pos = cols[i].transform.position;
					// Positions to check for colliders at
					float checkUp = ((selfPos.y+col.size.y/2)+cols[i].size.y/2);
					float checkDown = ((selfPos.y-col.size.y/2)-cols[i].size.y/2);
						
					if (eMethods.inRange (pos.y, checkUp-0.01f, checkUp+0.01f)
				    	&& eMethods.inRange (pos.x, selfPos.x-0.01f, selfPos.x+0.01f)
				    	&& cols[i].size.x == col.size.x)
					{
						// Up
						col.transform.position = new Vector3 (selfPos.x,
						                                      selfPos.y + tileHeight*cols[i].size.y,
						                                      selfPos.z);
						col.size = new Vector2 (col.size.x, cols[i].size.y + col.size.y);
						// Remove other collider
						Destroy (cols[i].gameObject);
						cols.RemoveAt (i);
						cont = true;
						break;
					}
					else if (eMethods.inRange (pos.y, checkDown-0.01f, checkDown+0.01f)
					         && eMethods.inRange (pos.x, selfPos.x-0.01f, selfPos.x+0.01f)
					         && cols[i].size.x == col.size.x)
					{
						// Down
						col.transform.position = new Vector3 (selfPos.x,
						                                      selfPos.y - tileHeight*cols[i].size.y,
						                                      selfPos.z);
						col.size = new Vector2 (col.size.x, cols[i].size.y + col.size.y);
						Destroy (cols[i].gameObject);
						cols.RemoveAt (i);
						cont = true;
						break;
					}
					else
						cont = false;
				}
			}

			// Increment collider ID
			colID++;
		}
	}
}