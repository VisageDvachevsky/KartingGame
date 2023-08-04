using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

namespace ARTEX.Procedural.Racing
{
    using Random = UnityEngine.Random;
    using Debug = UnityEngine.Debug;

    public class PathGenerator : MonoBehaviour
    {
        [Header("Grid Size")]
        [Min(1)]
        [SerializeField] private int _length = 10;
        [Min(1)]
        [SerializeField] private int _width = 10;

        [Header("Points Generation")]
        [Min(3)]
        [SerializeField] private int _pointsAmount = 10;
        [SerializeField] private int _minLength = 20;

        [Header("Angle Cutting")]
        [SerializeField] private float _minAngle = 30f;
        [SerializeField] private int _iterationCount = 2;

        [Header("Debug")]
        [SerializeField] private bool _drawPoints = false;
        [SerializeField] private bool _drawRawPath = false;
        [SerializeField] private bool _drawFullPath = false;
        [SerializeField] private bool _drawCleanedPath = false;

        private bool _hasPath = false;
        private List<Vector3Int> _points = new List<Vector3Int>();
        private Vector3 _leftRaw;
        private Vector3 _rightRaw;
        private List<int> _rawPath = new List<int>();
        private List<Vector3Int> _path = new List<Vector3Int>();
        private HashSet<Vector3Int> _pathConflicts = new HashSet<Vector3Int>();
        private List<Vector3Int> _cleanedPath = new List<Vector3Int>();
        private int _firstPath = 0;

        public List<Vector3Int> GeneratePath()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            _hasPath = false;
            while (!_hasPath)
            {
                try
                {
                    GeneratePoints();
                    GenerateRawPath();
                    TracePaths();

                    if (_cleanedPath.Count < _minLength || !IsPathValid(_cleanedPath))
                    {
                        continue;
                    }

                    _hasPath = true;
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
            }

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                               ts.Hours, ts.Minutes, ts.Seconds,
                                               ts.Milliseconds / 10);
            Debug.Log($"Path generated in {elapsedTime}");

            return _cleanedPath;
        }

        private bool IsPathValid(List<Vector3Int> path)
        {
            for (int i = 1; i < path.Count; i++)
            {
                if (((Vector3)path[i] - (Vector3)path[i - 1]).sqrMagnitude != 1f)
                {
                    return false;
                }
            }
            return true;
        }

        private void GenerateRawPath()
        {
            _rawPath.Clear();

            int leftPoint = 0;
            int rightPoint = 0;

            for (int i = 0; i < _points.Count; i++)
            {
                if (_points[i].x < _points[leftPoint].x) leftPoint = i;
                if (_points[i].x > _points[rightPoint].x) rightPoint = i;
            }

            List<int> abovePoints = new List<int>();
            List<int> belowPoints = new List<int>();

            Vector3 l = _points[leftPoint];
            Vector3 r = _points[rightPoint];
            _leftRaw = l;
            _rightRaw = r;

            float k = (r.z - l.z) / (r.x - l.x);
            float b = l.z - k * l.x;

            for (int i = 0; i < _points.Count; i++)
            {
                if (i == leftPoint || i == rightPoint) continue;

                Vector3 point = _points[i];

                if (point.z >= (k * point.x + b))
                {
                    abovePoints.Add(i);
                }
                else
                {
                    belowPoints.Add(i);
                }
            }

            var belowSorted = belowPoints.OrderBy(p => _points[p].x).ThenByDescending(p => _points[p].y);
            var aboveSorted = abovePoints.OrderByDescending(p => _points[p].x).ThenBy(p => _points[p].y).ToList();
            _rawPath.Add(leftPoint);
            _rawPath.AddRange(belowSorted);
            _rawPath.Add(rightPoint);
            _rawPath.AddRange(aboveSorted);
            _rawPath.Add(leftPoint);

            for (int it = 0; it < _iterationCount; it++)
            {
                List<int> toDelete = new List<int>();
                for (int i = 1; i < _rawPath.Count - 1; i++)
                {
                    Vector3 previous = _points[_rawPath[i - 1]];
                    Vector3 point = _points[_rawPath[i]];
                    Vector3 next = _points[_rawPath[i + 1]];

                    float angle = Vector3.Angle(next - point, previous - point);

                    if (angle < _minAngle)
                    {
                        toDelete.Add(_rawPath[i]);
                    }
                }

                for (int i = 0; i < toDelete.Count; i++)
                {
                    _rawPath.Remove(toDelete[i]);

                    if (_rawPath.Count < 3) break;
                }

                if (_rawPath.Count < 3) break;
            }
        }
        private void TracePaths()
        {
            _path.Clear();

            GridGraph map = new GridGraph(_width, _length);
            map.Walls = new List<Vector2>();
            map.Forests = new List<Vector2>();

            _path.Add(_points[_rawPath[0]]);
            for (int e = 1; e < _rawPath.Count; e++)
            {
                Vector3Int startPoint = _points[_rawPath[e - 1]];
                Vector3Int endPoint = _points[_rawPath[e]];

                List<Node> path = AStar.Search(map, map.Grid[startPoint.x, startPoint.z], map.Grid[endPoint.x, endPoint.z]);

                for (int i = 0; i < path.Count; i++)
                {
                    Node node = path[i];
                    Vector2 position = node.Position;
                    _path.Add(new Vector3Int((int)position.x, 0, (int)position.y));
                }
            }

            HashSet<Vector3Int> previousPoints = new HashSet<Vector3Int>();
            _pathConflicts.Clear();
            for (int i = 1; i < _path.Count; i++)
            {
                Vector3Int current = _path[i];
                if (previousPoints.Contains(current))
                {
                    _pathConflicts.Add(current);
                }
                else
                {
                    previousPoints.Add(current);
                }
            }

            _firstPath = 1;
            while (true)
            {
                if (!_pathConflicts.Contains(_path[_firstPath]))
                {
                    break;
                }
                _firstPath++;
            }

            _cleanedPath.Clear();
            Vector3Int? conflict = null;
            for (int i = 0; i < _path.Count; i++)
            {
                Vector3Int current = _path[(_firstPath + i) % _path.Count];

                if (conflict != null && current == conflict.Value)
                {
                    conflict = null;
                }
                else if (conflict == null && _pathConflicts.Contains(current))
                {
                    conflict = current;
                }

                if (conflict == null)
                {
                    _cleanedPath.Add(current);
                }
            }

            _cleanedPath.Add(_cleanedPath[0]);
        }

        private void GeneratePoints()
        {
            _points.Clear();

            for (int i = 0; i < _pointsAmount; i++)
            {
                Vector3Int pointPosition = GetRandomPosition();
                _points.Add(pointPosition);
            }
        }

        private Vector3Int GetRandomPosition()
        {
            while (true)
            {
                Vector3Int position = new Vector3Int(
                    Random.Range(0, _width),
                    0,
                    Random.Range(0, _length)
                );

                if (!_points.Contains(position))
                {
                    return position;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!_hasPath) return;

            switch (GetCurrentDrawMode())
            {
                case DrawMode.Points:
                    DrawPoints();
                    break;
                case DrawMode.RawPath:
                    DrawRawPath();
                    break;
                case DrawMode.FullPath:
                    DrawFullPath();
                    break;
                case DrawMode.CleanedPath:
                    DrawCleanedPath();
                    break;
            }
        }

        private enum DrawMode
        {
            Points,
            RawPath,
            FullPath,
            CleanedPath
        }

        private DrawMode GetCurrentDrawMode()
        {
            if (_drawPoints) return DrawMode.Points;
            if (_drawRawPath) return DrawMode.RawPath;
            if (_drawFullPath) return DrawMode.FullPath;
            if (_drawCleanedPath) return DrawMode.CleanedPath;

            // If no mode is selected, default to DrawMode.Points
            return DrawMode.Points;
        }

        private void DrawPoints()
        {
            Gizmos.color = Color.green;
            foreach (var point in _points)
            {
                Gizmos.DrawWireSphere(point, .1f);
            }
        }

        private void DrawRawPath()
        {
            Gizmos.color = Color.red;
            for (int i = 1; i < _rawPath.Count; i++)
            {
                Gizmos.DrawLine(_points[_rawPath[i - 1]], _points[_rawPath[i]]);
            }
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_leftRaw, _rightRaw);
        }

        private void DrawFullPath()
        {
            Gizmos.color = Color.yellow;
            for (int i = 1; i < _path.Count; i++)
            {
                Gizmos.DrawLine(_path[i - 1], _path[i]);
            }
            Gizmos.color = Color.red;
            foreach (var p in _pathConflicts)
            {
                Gizmos.DrawSphere(p, .2f);
            }

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_path[0], .2f);
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(_path[_firstPath], .2f);
        }

        private void DrawCleanedPath()
        {
            Gizmos.color = Color.cyan;
            for (int i = 1; i < _cleanedPath.Count; i++)
            {
                Gizmos.DrawLine(_cleanedPath[i - 1], _cleanedPath[i]);
            }
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_cleanedPath[0], .2f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_cleanedPath[1], .2f);
        }
    }
}
