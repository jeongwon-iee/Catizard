using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class GridView : MonoBehaviour 
{
	public GameObject blockPrefab = null, seroPrefab = null, garoPrefab = null, roroPrefab = null;

	[RangeAttribute(0, 481)]
	public int numBlocks = 1;
	public float blockSize = 0.3f;
	public int rowSize = 37;
	[RangeAttribute(0.0f, 1.0f)]
	public float blockBuffer = 0.0f;

	[SerializeField] private PathLineRenderer _pathRenderer = null;

	private int previousNumBlocks = 0;
	private float previousBuffer = 0;

	private GameObject[] childObjects = new GameObject[1];

	public Grid grid = new Grid();

	public Queue< BlockScript > selectedPathPoints = new Queue< BlockScript >();

	private IEnumerator findPath = null;

    private BlockScript[] GoalPoint = new BlockScript[7];

    // 필요 변수 선언
    public bool isGame = true, isPrint = false, isPath = true, isChange = false;
    public int start_x = 0, start_y=0, temp_x = 0, temp_y = 6, end_x = 36, end_y;
    public int[] last_column = new int[7] { 0, 1, 2, 3, 4, 5, 6 };
    public List<Point>[] CatPath = new List<Point>[7];
    public int minIndex = -1, CatIndex = 0;

    public int[] rand_x = new int[9];
    public int[] rand_y = new int[9]; //총 9개 이동 불가 (x,y)좌표
    public GameObject Dots;
    public GameObject[] dots;

    void Start()
	{
		Debug.Assert( _pathRenderer != null, "Path Renderer isn't set!" );

        for (int i = 0; i < 7; i++)
        {
            CatPath[i] = new List<Point>();
        }

		_pathRenderer._gridView = this;
        resize();
        if (N_CardDeckSys.isTest)
        {
            JPS();
        }
    }

	// Update is called once per frame
	/*void Update () 
	{
		// If no one has given us a prefab to use, then don't make anything as we'll just get null pointer exception nonsense
		if ( blockPrefab == null )
			return;
        
			JPS();
		
	}*/

    // JPS 리셋
    public void Reset()
    {
        isPrint = false;
        JPSState.state = eJPSState.ST_OBSTACLE_BUILDING;
        _pathRenderer.disablePath();
        CatIndex = 0;
        minIndex = -1;
        for (int i = 0; i < 7; i++)
        {
            CatPath[i] = null;
        }
        findPath = null;
        // selectedPathPoints.Clear();
        // resize();
    }

    // JPS 실행하기
    public void JPS()
    {
        isPrint = true;
        CalcPrimaryJumpPoints();
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
            int blockType = 0;
            bool isColumn = column % 2 == 1? true : false;
            bool isRow = row % 2 == 1? true : false;
            float xSize = 0, ySize = 0;

            // Create a new Child object
            // 크기 다르게 만들기
            GameObject child;
            if (isColumn && isRow)
            {
                child = Instantiate(roroPrefab);
                blockType = 0;
            }
            else if (isColumn)
            {
                child = Instantiate(seroPrefab);
                blockType = 1;
            }
            else if (isRow)
            {
                child = Instantiate(garoPrefab);
                blockType = 2;
            }
            else
            {
                child = Instantiate(blockPrefab);
                blockType = 3;
            }
			child.GetComponent<Transform>().parent = GetComponent<Transform>();  // Set as parent of this new child
            // 블럭 위치 지정
            if (isColumn)
            {
                xSize = (column + 1) * 0.5f * (blockSize * 7f + blockBuffer) - blockSize * 3f;
            }
            else
            {
                xSize = column * 0.5f * (blockSize * 7f + blockBuffer) + blockSize;
            }
            if (isRow)
            {
                ySize = (row + 1) * 0.5f * -(blockSize * 7f + blockBuffer) + blockSize * 3f;
            }
            else
            {
                ySize = row * 0.5f * -(blockSize * 7f + blockBuffer) - blockSize;
            }
            child.GetComponent<Transform>().localPosition = new Vector3(
				xSize, ySize, 10.0f);

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
            child.GetComponent<BlockScript>().BlockType = blockType;

            // 목적지 만들기
            if ((column == end_x) && (row % 2 == 0))
            {
                child.GetComponent<BlockScript>().isPathEndPoint = true;

                GoalPoint[row/2] = child.GetComponent<BlockScript>();
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
        //랜덤지역 만들기
        Random_pos();

        JPS();
    }
    public void Random_pos()
    {
        int x_district = 1; //첫번째 열(column==0)빼려고 1부터 초기화
        int y_district = 0;
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                rand_x[i + 3 * j] = Random.Range(x_district, x_district + 4); //고양이가 움직이는 보드의 정수형 x좌표 = n 임. +4는 다음구역으로 넘어가는거고 마지막 열 빼기위해 4까지만 이동함ㅠ
                rand_y[i + 3 * j] = Random.Range(y_district, y_district + 2); //고양이가 움직이는 보드의 정수형 y좌표 = m ->문제는 마지막행은 절대 못가는 칸이 안만들어짐ㅠ
                x_district += 6;
            }
            x_district = 1;
            if (j == 1) { y_district += 3; }
            else { y_district += 2; }
        }
        for (int k = 0; k < rand_x.Length; k++)
        {
            grid.gridNodes[2 * rand_y[k] * 37 + 2 * rand_x[k]].isObstacle = true; // 코드 보드판에서 좌표는 (2n ,2m)임. 그걸 인덱스화해서 장애물로 만듦!
        }
        Dots.SetActive(true);
        for(int i = 0; i < 9; i++)
        { ///dots 인덱스는 세로로 증가함
            dots[6 * rand_x[i] + rand_y[i]].SetActive(false);
            if (6 * rand_x[i] + rand_y[i]-1 >= 0 && 6 * rand_x[i] + rand_y[i - 1] < 108) dots[6 * rand_x[i] + rand_y[i]-1].SetActive(false);
            if (5 * rand_x[i] + rand_y[i] - 1 >= 0 && 5 * rand_x[i] + rand_y[i] - 1 < 108) dots[5 * rand_x[i] + rand_y[i]-1].SetActive(false);
            if (5 * rand_x[i] + rand_y[i] >= 0 && 5 * rand_x[i] + rand_y[i] < 108) dots[5 * rand_x[i] + rand_y[i]].SetActive(false);
        }
    }

    // Return the World Position of these grid points, relative to this object
    public Vector3 getNodePosAsWorldPos( Point point )
	{
		var trans = GetComponent<Transform>();
        float x, y;
        if (point.column%2 == 1)
        {
            x = (point.column + 1) * 0.5f * (blockSize * 7f + blockBuffer) - blockSize * 3f;
        }
        else
        {
            x = point.column * 0.5f * (blockSize * 7f + blockBuffer) + blockSize;
        }
        if (point.row%2 == 1)
        {
            y = (point.row + 1) * 0.5f * -(blockSize * 7f + blockBuffer) + blockSize * 3f;
        }
        else
        {
            y = point.row * 0.5f * -(blockSize * 7f + blockBuffer) - blockSize;
        }

        return new Vector3(x - 7.2f, y + 2.3f, 0.0f);
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

    // 길 찾기 부분
    public void BeginPathFind()
    {
        // Verify at least TWO END POINTS ARE SET!
        //if (this.selectedPathPoints.Count != 2) return;

        JPSState.state = eJPSState.ST_FIND_PATH; // transition state to Primary Jump Point Building State

        BlockScript start_point= null;

        int min = int.MaxValue;

        // Tell each child object to re-evaulte their rendering info
 
        foreach (GameObject child in childObjects)
        {
            Point block_point = child.GetComponent<BlockScript>().nodeReference.pos;
            if (block_point.column == start_x && block_point.row == start_y)
            {
                child.GetComponent<BlockScript>().isPathEndPoint = false;
            }
            if (block_point.column == temp_x && block_point.row == temp_y)
            {
                child.GetComponent<BlockScript>().isPathEndPoint = true;
                start_point = child.GetComponent<BlockScript>();
            }
            child.GetComponent<BlockScript>().setupDisplay();
        }
        for (int i = 0; i < 7; i++)
        {
            selectedPathPoints.Clear();
            selectedPathPoints.Enqueue(start_point);
            selectedPathPoints.Enqueue(GoalPoint[i]);
            GoalPoint[i].isPathEndPoint = true;
            

            start_x = temp_x;
            start_y = temp_y;

            BlockScript[] points = this.selectedPathPoints.ToArray();

            Point start, stop;

            start = points[0].nodeReference.pos;
            stop = points[1].nodeReference.pos;

            CatPath[i] = grid.getPath(start, stop);

            int tempCount = CatPath[i].Count;

            if (CatPath[i] != null && tempCount != 0)
            {
                isPath = true;
                if (min > tempCount)
                {
                    min = tempCount;
                    minIndex = i;
                }
            }
            else
            {
                isPath = false;
            }
            GoalPoint[i].isPathEndPoint = false;
        }
        if (minIndex < 0)
        {
            print("길이 존재하지 않습니다.");
        }
        // 테스트 중이거나 게임이 끝났다면 길 표시
        else if (N_CardDeckSys.isTest || !isGame)
        {
            _pathRenderer.drawPath(CatPath[minIndex]);    // Draw Path on Screen
        }
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
    
}
