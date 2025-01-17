using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts
{
    [RequireComponent(typeof(PlaySpeedModifier))]
    public class GridGenerator : MonoBehaviour
    {
        [Tooltip("Grid Width (Number of Columns)")]
        public int Width;
        
        [Tooltip("Grid Height (Number of Rows)")]
        public int Height;
        
        [Tooltip("Grid Depth")]
        public int Depth;
        
        [Tooltip("Toggle to set 3d on and off. If off depth will be one always")]
        public bool Is3D;

        [Range(1, 10)]
        [Tooltip("Multiplier for distance between all cells in 3D mode")]
        public int DistanceMultiplier;

        public event Action CellRevived;

        private static readonly Vector3 CellScale = Vector3.one * 0.8f;

        private Cell[,,] _cells;
        private int _width, _height, _depth;
        private bool _is3D;
        private int _distanceMultiplier;
        private bool _hasRunCoroutineFinished;

        private PlaySpeedModifier _playSpeedModifier;
        private float _generationGap;
        
        
        public void Initialize()
        {
            _width = Width;
            _height = Height;
            _depth = Depth;
            _is3D = Is3D;
            _distanceMultiplier = DistanceMultiplier;
            _hasRunCoroutineFinished = true;
            
            SetupPlaySpeedModifier();

            Assert.IsTrue(_width > 2, "Width should be greater than 2 for proper simulation to occur");
            Assert.IsTrue(_height > 2, "Height should be greater than 2 for proper simulation to occur");
            Assert.IsTrue(_depth > 2, "Depth should be greater than 2 for proper simulation to occur");

            if (_width < 3 || _height < 3 || _depth < 3)
            {
                Manager.GameState = GameStateEnum.Invalid;
                Debug.LogError("Invalid width or height or depth");
                return;
            }

            if (!_is3D)
            {
                _depth = 1;
            }

            if (Manager.Initialize())
            {
                Manager.GameState = GameStateEnum.Wait;
            }
            
            TryPopulateGrid();
        }

        private void SetupPlaySpeedModifier()
        {
            _playSpeedModifier = GetComponent<PlaySpeedModifier>();
            _generationGap = _playSpeedModifier.PlaybackSpeed;

            _playSpeedModifier.PlaybackSpeedUpdated += () =>
            {
                _generationGap = _playSpeedModifier.PlaybackSpeed;
            };
        }

        private void TryPopulateGrid()
        {
            if (Manager.GameState == GameStateEnum.Invalid)
            {
                return;
            }
            
            _cells = new Cell[_depth, _height, _width];
            PopulateGrid();
        }

        private void PopulateGrid()
        {
            var offset = new Vector3Int
            {
                x = _width - Mathf.FloorToInt(0.5f * (_width - 1) + 1.0f),
                y = _height - Mathf.FloorToInt(0.5f * (_height - 1) + 1.0f),
                z = _depth - Mathf.FloorToInt(0.5f * (_depth - 1) + 1.0f)
            };

            for (var d = 0; d < _depth; d++)
            {
                for (var h = 0; h < _height; h++)
                {
                    for (var w = 0; w < _width; w++)
                    {

                        _cells[d, h, w] = Instantiate(Manager.CellPrefab, transform).GetComponent<Cell>();

#if UNITY_EDITOR
                        _cells[d, h, w].gameObject.name = "Cell (" + d + "," + h + "," + w + ")";
#endif

                        var cellTransform = _cells[d, h, w].transform;
                        cellTransform.position = new Vector3(w - offset.x, h - offset.y, d - offset.z) * (_is3D
                            ? _distanceMultiplier
                            : 1.0f);
                        cellTransform.rotation = Quaternion.identity;
                        cellTransform.localScale = CellScale;

                        _cells[d, h, w].Initialize(d, h, w, _width, _height, _depth);
                    }
                }
            }

            transform.eulerAngles = new Vector3(90, 0, 0);
            Manager.GameState = GameStateEnum.Run; // 
        }

        private void UpdateCells()
        {
            for (var d = 0; d < _depth; d++)
            {
                for (var h = 0; h < _height; h++)
                {
                    for (var w = 0; w < _width; w++)
                    {
                        var sum = _is3D
                            ? _cells[d, h, w].CalculateCellSum3D(_cells)
                            : _cells[d, h, w].CalculateCellSum(_cells);

                        _cells[d, w, h].LoopCheck();
                        if (_is3D)
                        {
                            if (sum >= 8 && sum <= 12)
                            {
                                _cells[d, h, w].NextCellState = _cells[d, h, w].IsAlive
                                    ? NextCellStateEnum.NoChange
                                    : NextCellStateEnum.MakeAlive;
                            }
                            else if (sum < 8 || sum > 14)
                            {
                                _cells[d, h, w].NextCellState = _cells[d, h, w].IsAlive
                                    ? NextCellStateEnum.MakeDead
                                    : NextCellStateEnum.NoChange;
                            }
                            else
                            {
                                _cells[d, h, w].NextCellState = NextCellStateEnum.NoChange;
                            }
                        }
                        else
                        {
                            switch (sum)
                            {
                                case 3:
                                    _cells[d, h, w].NextCellState = _cells[d, h, w].IsAlive
                                        ? NextCellStateEnum.NoChange
                                        : NextCellStateEnum.MakeAlive;
                                    break;
                                case 4:
                                    _cells[d, h, w].NextCellState = NextCellStateEnum.NoChange;
                                    break;
                                default:
                                    _cells[d, h, w].NextCellState = _cells[d, h, w].IsAlive
                                        ? NextCellStateEnum.MakeDead
                                        : NextCellStateEnum.NoChange;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void ApplyCellUpdates()
        {
            for (var d = 0; d < _depth; d++)
            {
                for (var h = 0; h < _height; h++)
                {
                    for (var w = 0; w < _width; w++)
                    {
                        if (_cells[d, h, w].NextCellState == NextCellStateEnum.MakeDead)
                        {
                            _cells[d, h, w].IsAlive = false;
                        }
                        else if (_cells[d, h, w].NextCellState == NextCellStateEnum.MakeAlive)
                        {
                            _cells[d, h, w].IsAlive = true;
                            CellRevived?.Invoke();
                        }

                        _cells[d, h, w].IsSumSet = false;
                    }
                }
            }
        }

        private void Update()
        {
            if (Manager.GameState == GameStateEnum.Run && _hasRunCoroutineFinished)
            {
                Manager.GameState = GameStateEnum.Run;
                _hasRunCoroutineFinished = false;
                StartCoroutine(Run());
            }
        }

        private IEnumerator Run()
        {
            while (Manager.GameState == GameStateEnum.Run)
            {
                UpdateCells();
                ApplyCellUpdates();
                yield return new WaitForSeconds(1 / _generationGap);
            }
            _hasRunCoroutineFinished = true;
        }

        public void SetupFieldSize(string width, string height)
        {
            Width = int.Parse(width);
            Height = int.Parse(height);
            Width = Mathf.Clamp(Width, 22, 50);
            Height = Width;
            // Height = Mathf.Clamp(Height, 22, 50);
        }
    }
}
