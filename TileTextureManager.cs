using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileTextureManager : MonoBehaviour {

	/*
	 * This script is for detecting tiles next to this one and then adjusting
	 * the texture to match the occupied sides. For example, a tile with tiles
	 * above and to the left of it will change to a corner tile sprite, then rotate
	 * so that it appears correctly.
	 * 
	 * Currently checks 8 areas - the four directly next to it, and four diagonally from it.
	 */

	// TODO: Add sprites and options for more situations of tiles, such as a side-block
	// that is half t-junction

	// All gameobjects with one of these tags will be counted when checking for adjacent tiles
	public string[] tagsOfTiles;

	// Sprites to use
	// Full block. All (non diagonal) sides are free.
	public Sprite fullBlock;
	// Straight block. Blocks on left & right or top & bottom.
	public Sprite straightBlock;
	// End block. Only one side occupied.
	public Sprite endBlock;
	// Empty block. All (including diagonal) sides are occupied.
	public Sprite emptyBlock;
	// 'Thin' empty block. All non-diagonal sides are occupied.
	public Sprite thinEmpty;
	// Corner block. Should be self-explanatory.
	public Sprite cornerBlock;
	// 'Thin' corner.
	public Sprite thinCorner;
	// Side block. Only one side isn't occupied.
	public Sprite sideBlock;
	// T-Intersection block.
	public Sprite tBlock;

	/*
	 * Tile rotations - typically, these represent which sides are the outside edges of the tile
	 * For example, a corner facing blocks on it's right and bottom should be set to LeftTop so
	 * it rotates correctly. This prevents needing 4 sprites for every possible direction.
	 */
	public enum CornerRotation
	{
		LeftTop = 0,
		TopRight = 90,
		RightBottom = 180,
		BottomLeft = 270
	};
	public enum JunctionRotation
	{
		Top = 0,
		Right = 90,
		Bottom = 180,
		Left = 270
	};
	public enum SideRotation
	{
		Top = 0,
		Right = 90,
		Bottom = 180,
		Left = 270
	};
	public enum StraightRotation
	{
		Horizontal = 0,
		Vertical = 90
	};
	public enum EndRotation
	{
		Top = 0,
		Right = 90,
		Bottom = 180,
		Left = 270
	};

	public CornerRotation cornerRotation;
	public JunctionRotation junctionRotation;
	public SideRotation sideRotation;
	public StraightRotation straightRotation;
	public EndRotation endRotation;

	// Occupied sides
	private bool top = false;
	private bool bottom = false;
	private bool left = false;
	private bool right = false;

	private bool topLeft = false;
	private bool topRight = false;
	private bool bottomLeft = false;
	private bool bottomRight = false;

	// Other
	public float tileSize;
	//public string Tag;
	//public int layerMask = 8;

	private List<GameObject> tiles = new List<GameObject> ();

	// Initialization
	void Start ()
	{
		// Get all tiles
		foreach (string tag in tagsOfTiles) 
		{
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag (tag))
			{
				tiles.Add (obj);
			}
		}

		Vector3 selfPos = transform.position;

		// Check for adjacent blocks
		for (int i = 0; i < tiles.Count; i++)
		{
			Vector3 pos = tiles[i].transform.position;

			Vector2 sPos = new Vector2 (selfPos.x+tileSize, selfPos.y+tileSize);
			Vector2 sNeg = new Vector2 (selfPos.x-tileSize, selfPos.y-tileSize);

			// When checking for neighboring tiles, searching a small range prevents both
			// human error and rounding errors from causing problems.
			if (inRange (pos.x, sPos.x-0.01f, sPos.x+0.01f))
			{
				if (inRange (pos.y, sPos.y-0.01f, sPos.y+0.01f))
				{
					topRight = true;
					continue;
				}
				else if (inRange (pos.y, sNeg.y-0.01f, sNeg.y+0.01f))
				{
					bottomRight = true;
					continue;
				}
				else if (inRange (pos.y, selfPos.y-0.01f, selfPos.y+0.01f))
				{
					right = true;
					continue;
				}
			}
			else if (inRange (pos.x, sNeg.x-0.01f, sNeg.x+0.01f))
			{
				if (inRange (pos.y, sPos.y-0.01f, sPos.y+0.01f))
				{
					topLeft = true;
					continue;
				}
				else if (inRange (pos.y, sNeg.y-0.01f, sNeg.y+0.01f))
				{
					bottomLeft = true;
					continue;
				}
				else if (inRange (pos.y, selfPos.y-0.01f, selfPos.y+0.01f))
				{
					left = true;
					continue;
				}
			}
			else if (inRange (pos.y, sPos.y-0.01f, sPos.y+0.01f)
			         && inRange (pos.x, selfPos.x-0.01f, selfPos.x+0.01f))
			{
				top = true;
				continue;
			}
			else if (inRange (pos.y, sNeg.y-0.01f, sNeg.y+0.01f)
			         && inRange (pos.x, selfPos.x-0.01f, selfPos.x+0.01f))
				bottom = true;
		}
		
		SpriteRenderer renderer = GetComponent<SpriteRenderer> ();

		// Change sprite to fit based on found objects
		if (top)
		{
			if (bottom)
			{
				if (left)
				{
					if (right)
					{
						// All sides occupied
						if (!topLeft && !topRight && !bottomLeft && !bottomRight)
							renderer.sprite = thinEmpty;
						else
							renderer.sprite = emptyBlock;
					}
					else
					{
						// Top, bottom, left, occupied
						if (!topLeft && !bottomLeft)
						{
							renderer.sprite = tBlock;
							transform.Rotate (new Vector3 (0, 0, (float)junctionRotation - 90));
						}
						else
						{
							renderer.sprite = sideBlock;
							transform.Rotate (new Vector3 (0, 0, (float)sideRotation - 90));
						}
					}
				}
				else if (right)
				{
					// Top, bottom, right occupied
					if (!topRight && !bottomRight)
					{
						renderer.sprite = tBlock;
						transform.Rotate (new Vector3 (0, 0, (float)junctionRotation - 270));
					}
					else
					{
						renderer.sprite = sideBlock;
						transform.Rotate (new Vector3 (0, 0, (float)sideRotation - 270));
					}
				}
				else
				{
					// Top, bottom occupied
					renderer.sprite = straightBlock;
					transform.Rotate (new Vector3 (0, 0, (float)straightRotation - 90));
				}
			}
			else if (left)
			{
				if (right)
				{
					// Top, left, right occupied
					if (!topLeft && !topRight)
					{
						renderer.sprite = tBlock;
						transform.Rotate (new Vector3 (0, 0, (float)junctionRotation - 180));
					}
					else
					{
						renderer.sprite = sideBlock;
						transform.Rotate (new Vector3 (0, 0, (float)sideRotation - 180));
					}
				}
				else
				{
					// Top, left occupied
					if (topLeft)
						renderer.sprite = cornerBlock;
					else
						renderer.sprite = thinCorner;
					transform.Rotate (new Vector3 (0, 0, (float)cornerRotation - 180));
				}
			}
			else if (right)
			{
				// Top, right occupied
				if (topRight)
					renderer.sprite = cornerBlock;
				else
					renderer.sprite = thinCorner;
				transform.Rotate (new Vector3 (0, 0, (float)cornerRotation - 270));
			}
			else
			{
				// Top occupied
				renderer.sprite = endBlock;
				transform.Rotate (new Vector3 (0, 0, (float)endRotation - 180));
			}
		}
		else if (bottom)
		{
			if (left)
			{
				if (right)
				{
					// Bottom, left, right occupied
					if (!bottomLeft && !bottomRight)
					{
						renderer.sprite = tBlock;
						transform.Rotate (new Vector3 (0, 0, (float)junctionRotation));
					}
					else
					{
						renderer.sprite = sideBlock;
						transform.Rotate (new Vector3 (0, 0, (float)sideRotation));
					}
				}
				else
				{
					// Bottom, left occupied
					if (bottomLeft)
						renderer.sprite = cornerBlock;
					else
						renderer.sprite = thinCorner;
					transform.Rotate (new Vector3 (0, 0, (float)cornerRotation - 90));
				}
			}
			else if (right)
			{
				// Bottom, right occupied
				if (bottomRight)
					renderer.sprite = cornerBlock;
				else
					renderer.sprite = thinCorner;
				transform.Rotate (new Vector3 (0, 0, (float)cornerRotation));
			}
			else
			{
				// Bottom occupied
				renderer.sprite = endBlock;
				transform.Rotate (new Vector3 (0, 0, (float)endRotation));
			}
		}
		else if (left)
		{
			if (right)
			{
				// Left, right occupied
				renderer.sprite = straightBlock;
				transform.Rotate (new Vector3 (0, 0, (float)straightRotation));
			}
			else
			{
				// Left occupied
				renderer.sprite = endBlock;
				transform.Rotate (new Vector3 (0, 0, (float)endRotation - 90));
			}
		}
		else if (right)
		{
			// Right occupied
			renderer.sprite = endBlock;
			transform.Rotate (new Vector3 (0, 0, (float)endRotation - 270));
		}
		else
		{
			// None occupied
		}
	}
	
	public bool inRange (float num, float first, float second)
	{
		if (first > second)
		{
			if (num >= second && num <= first)
				return true;
		}
		else
		{
			if (num >= first && num <= second)
				return true;
		}
		return false;
	}
}