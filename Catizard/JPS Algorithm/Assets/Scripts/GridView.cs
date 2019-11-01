using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class GridView : MonoBehaviour 
{
	public GameObject blockPrefab = null, seroPrefab = null, garoPrefab = null, roroPrefab = null;

	[RangeAttribute(0, 481)]
	public int numBlocks = 1;
	public float blockSize = 0.64f;
	public int rowSize = 37;
	[RangeAttribute(0.0f, 1.0f)]
	public float blockBuffer = 0.0f;

	[SerializeField] private PathLineRenderer _pathRenderer;

	private int previousNumBlocks = 0;
	private float previousBuffer = 0;

	private GameObject[] childObjects = new GameObject[1];

	private Grid grid = new Grid();

	private Queue< BlockScript > selectedPathPoints = new Queue< BlockScript >();

	private IEnumerator findPath = null;

    // 필요 변수 선언
    private bool isPrint = false;
    public int start_x = 0, start_y = 6, end_x = 36, end_y = 6;

	void Start()
	{
		Debug.Assert( _pathRenderer != null, "Path Renderer isn't set!" );

		_pathRenderer._gridView = this;
	}

	// Update is called once per frame
	void Update () 
	{
		// If no one has given us a prefab to use, then don't make anything as we'll just get null pointer exception nonsense
		if ( blockPrefab == null )
			return;

		// If we need to resize then do
		if ( previousNumBlocks != numBlocks || previousBuffer != blockBuffer )
		{
			resize();
			previousNumBlocks = numBlocks;
			previousBuffer = blockBuffer;
		}
	}

#region Helper Functions

	public void Reset()
	{
        isPrint = false;
		JPSState.state = eJPSState.ST_OBSTACLE_BUILDING;
		_pathRenderer.disablePath();
		findPath = null;
		selectedPathPoints.Clear();
		resize();
	}

	// Resize the grid based off the new values
	public void resize()
	{
		// clear the queue
		selectedPathPoints.Clear();

		// Kill all my children
		foreach ( GameObject child in childObjects )
		{
			DestroyImmediate( child );
		}

		// realloc the grids
		grid.gridNodes = new Node[numBlocks];
		grid.pathfindingNodes = new PathfindingNode[numBlocks];
		childObjects   = new GameObject[numBlocks];

		for ( int i = 0; i < numBlocks ; ++i )
		{
			int column = i % rowSize;
			int row    = i / rowSize;
            bool isColumn = column % 2 == 1? true : false;
            bool isRow = row % 2 == 1? true : false;
            float xSize = 0, ySize = 0;

            // Create a new Child object
            // 크기 다르게 만들기
            GameObject child;
            if (isColumn && isRow)
            {
                child = Instantiate(roroPrefab);
            }
            else if (isColumn)
            {
                child = Instantiate(seroPrefab);
            }
            else if (isRow)
            {
                child = Instantiate(garoPrefab);
            }
            else
            {
                child = Instantiate(blockPrefab);
            }
			child.GetComponent<Transform>().parent = GetComponent<Transform>();  // Set as parent of this new child
            if (isColumn)
            {
                xSize = (column + 1) * 0.5f * (blockSize * 1.2f + blockBuffer) + blockSize * 0.36f;
            }
            else
            {
                xSize = column * 0.5f * (blockSize * 1.2f + blockBuffer) + blockSize;
            }
            if (isRow)
            {
                ySize = (row + 1) * 0.5f * -(blockSize * 1.2f + blockBuffer) - blockSize * 0.36f;
            }
            else
            {
                ySize = row * 0.5f * -(blockSize * 1.2f + blockBuffer) - blockSize;
            }
            child.GetComponent<Transform>().localPosition = new Vector3(
				xSize, ySize, 0.0f);

			grid.gridNodes[ i ] = new Node();
			grid.gridNodes[ i ].pos  = new Point( row, column );

			grid.pathfindingNodes[ i ] = new PathfindingNode();
			grid.pathfindingNodes[ i ].pos = new Point( row, column );
            
            // 회색지역 만들기
            if ((column * row) % 2 == 1)
            {
                grid.gridNodes[i].isObstacle = true;
            }

            grid.rowSize = this.rowSize;
			child.GetComponent<BlockScript>().nodeReference = grid.gridNodes[ i ]; // give the child a shared_ptr reference to the node it needs to act on
			child.GetComponent<BlockScript>().gridView = this;

            // 목적지 만들기
            if ((column == end_x) && (row == end_y))
            {
                child.GetComponent<BlockScript>().isPathEndPoint = true;
                selectedPathPoints.Enqueue(child.GetComponent<BlockScript>());
            }
            // 출발지 만들기
            else if ((column == start_x) && (row == start_y))
            {
                child.GetComponent<BlockScript>().isPathEndPoint = true;
                selectedPathPoints.Enqueue(child.GetComponent<BlockScript>());
            }

            childObjects[ i ] = child;

        }
	}

    // JPS 실행하기
    public void JPS()
    {
        isPrint = true;
        CalcPrimaryJumpPoints();
    }

	// Return the World Position of these grid points, relative to this object
	public Vector3 getNodePosAsWorldPos( Point point )
	{
		var trans = GetComponent<Transform>();
		
		return new Vector3(
			trans.localPosition.x + point.column *  ( blockSize + blockBuffer ),
			trans.localPosition.y + point.row    * -( blockSize + blockBuffer ),
			0.0f
		);
	}

    /*
	public void markNodeAsPathPoint( BlockScript block_script )
	{
		if ( selectedPathPoints.Contains( block_script ) )
		{
			return;
		}

		// max size has to be 2
		while ( selectedPathPoints.Count >= 2 )
		{
			selectedPathPoints.Dequeue().removePathMarker();   // remove the oldest element
		}

		// enqueue the new postition
		selectedPathPoints.Enqueue( block_script );
	}
    */

#endregion

#region Button Callbacks

	public void CalcPrimaryJumpPoints()
	{
		grid.buildPrimaryJumpPoints();    // Build primary Jump Points
		JPSState.state = eJPSState.ST_PRIMARY_JPS_BUILDING; // transition state to Primary Jump Point Building State

		// Tell each child object to re-evaulte their rendering info
		/*foreach ( GameObject child in childObjects )
		{
			BlockScript block_component = child.GetComponent<BlockScript>();
			block_component.setupDisplay();	
		}*/

        // 다음 단계로
        if (isPrint)
            CalcStraightJPDistances();
    }

	public void CalcStraightJPDistances()
	{
		grid.buildStraightJumpPoints();    // Build primary Jump Points
		JPSState.state = eJPSState.ST_STRAIGHT_JPS_BUILDING; // transition state to Primary Jump Point Building State

		// Tell each child object to re-evaulte their rendering info
		/*foreach ( GameObject child in childObjects )
		{
			BlockScript block_component = child.GetComponent<BlockScript>();
			block_component.setupDisplay();	
		}*/

        // 다음 단계로
        if (isPrint)
            CalcDiagonalJPDistances();
    }

	public void CalcDiagonalJPDistances()
	{
		grid.buildDiagonalJumpPoints();    // Build primary Jump Points
		JPSState.state = eJPSState.ST_DIAGONAL_JPS_BUILDING; // transition state to Primary Jump Point Building State

		// Tell each child object to re-evaulte their rendering info
		/*foreach ( GameObject child in childObjects )
		{
			BlockScript block_component = child.GetComponent<BlockScript>();
			block_component.setupDisplay();	
		}*/

        // 다음 단계로
        if (isPrint)
            CalcWallDistances();
    }

	public void CalcWallDistances()
	{
		//grid.buildDiagonalJumpPoints();    // Build primary Jump Points
		JPSState.state = eJPSState.ST_WALL_DISTANCES_BUILT; // transition state to Primary Jump Point Building State

		// Tell each child object to re-evaulte their rendering info
		/*foreach ( GameObject child in childObjects )
		{
			BlockScript block_component = child.GetComponent<BlockScript>();
			block_component.setupDisplay();	
		}*/

        // 다음 단계로
        if (isPrint)
        {
            BeginPathFind();
        }
	}

    /*
	// This button just enters the path search mode where the user can select the start and end points
	public void PlaceSearchEndPoints()
	{
		JPSState.state = eJPSState.ST_PLACE_SEARCH_ENDPOINTS; // transition state to Primary Jump Point Building State

		// Disable existing paths if we are restarting
        // 출발지, 목적지 초기화 부분
		foreach ( var block_script in selectedPathPoints )
		{
			block_script.isPathEndPoint = false;
		}

		selectedPathPoints.Clear();

		// Disable path view
		_pathRenderer.disablePath();
		findPath = null;

		// Tell each child object to re-evaulte their rendering info
		foreach ( GameObject child in childObjects )
		{
			BlockScript block_component = child.GetComponent<BlockScript>();
			block_component.setupDisplay();	
		}
	}
    */
    
	public void BeginPathFind()
	{
		// Verify at least TWO END POINTS ARE SET!
		if ( this.selectedPathPoints.Count != 2 ) return;

		JPSState.state = eJPSState.ST_FIND_PATH; // transition state to Primary Jump Point Building State

		// Tell each child object to re-evaulte their rendering info
		foreach ( GameObject child in childObjects )
		{
			BlockScript block_component = child.GetComponent<BlockScript>();
			block_component.setupDisplay();	
		}

		BlockScript[] points = this.selectedPathPoints.ToArray();

		Point start = points[ 0 ].nodeReference.pos;
		Point stop  = points[ 1 ].nodeReference.pos;

		List<Point> path = grid.getPath( start, stop );

        if (path != null && path.Count != 0)
        {
            _pathRenderer.drawPath(path);    // Draw Path on Screen
            print("길을 찾았습니다.");
            for (int i = 0; i < path.Count; i++)
            {
                print((i + 1) + "번째 노드 : (" + path[i].column + ", " + path[i].row + ")");
            }
        }
        else
            print("길을 찾을 수 없습니다.");
	}

	public void StepThroughPath()
	{
		// Verify at least TWO END POINTS ARE SET!
		if ( this.selectedPathPoints.Count != 2 ) return;
		JPSState.state = eJPSState.ST_FIND_PATH;
		JPSState.LastPathFound = true;

		if ( findPath == null )
		{
			BlockScript[] points = this.selectedPathPoints.ToArray();

			Point start = points[ 0 ].nodeReference.pos;
			Point stop  = points[ 1 ].nodeReference.pos;

			// Get enumerator path finding
			findPath = grid.getPathAsync( start, stop );

            // Tell each child object to re-evaulte their rendering info
            foreach (GameObject child in childObjects)
            {
                BlockScript block_component = child.GetComponent<BlockScript>();
                block_component.setupDisplay();
            }

            findPath.MoveNext();  // First iteration doesn't really do anything, so just skip it
		}

		// step through path finding process
		if ( findPath.MoveNext() )
		{
			PathfindReturn curr_return = (PathfindReturn) findPath.Current;

			switch ( curr_return._status )
			{
				case PathfindReturn.PathfindStatus.SEARCHING:
					// render path up to this point
					List< Point > path_so_far = grid.reconstructPath( 
						curr_return._current, 
						selectedPathPoints.Peek().nodeReference.pos 
					);
					_pathRenderer.drawPath( path_so_far );
					break;
				case PathfindReturn.PathfindStatus.FOUND:
					// render path
					_pathRenderer.drawPath( curr_return.path );
					findPath = null;
					JPSState.state = eJPSState.ST_PATH_FIND_COMPLETE;
					break;
				case PathfindReturn.PathfindStatus.NOT_FOUND:
					// disable rendering, ya blew it
					_pathRenderer.disablePath();
					findPath = null;
					JPSState.state = eJPSState.ST_PATH_FIND_COMPLETE;
					JPSState.LastPathFound = false;   // tell everyone that we failed to find a path
					break;
			}
		}
		else
		{
			Debug.Log("WE ARRIVED AT THE END!");
			findPath = null;
			JPSState.state = eJPSState.ST_PATH_FIND_COMPLETE;
		}
	}

#endregion
}
