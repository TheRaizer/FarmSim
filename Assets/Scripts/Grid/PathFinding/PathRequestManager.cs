using System;
using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Grid 
{
    public class PathRequestManager : Singleton<PathRequestManager>
    {
        private AStar aStar;
        private bool isProcessing;
        private PathRequest currentPathProcess;

        private readonly Queue<PathRequest> requests = new Queue<PathRequest>();

        private void Awake()
        {
            NodeGrid grid = GetComponent<NodeGrid>();
            aStar = new AStar(grid);
        }

        public void RequestPath(string guid, Node start, Node end, Action<Vector2[], bool> callBack)
        {
            PathRequest pathRequest = new PathRequest(guid, start, end, callBack);
            requests.Enqueue(pathRequest);
        }

        private void TryProcessingNextPath()
        {
            if (requests.Count > 0 && !isProcessing)
            {
                isProcessing = true;
                currentPathProcess = requests.Dequeue();
                aStar.PathFind(currentPathProcess.start, currentPathProcess.end, currentPathProcess.id);
            }
        }

        public void OnFinishProcess(Vector2[] path, bool foundPath)
        {
            isProcessing = false;
            currentPathProcess.callBack(path, foundPath);
            TryProcessingNextPath();
        }

        public void StopSearch(string guid)
        {
            aStar.StopSearch(guid);
        }
    }

    public struct PathRequest
    {
        public readonly Node start;
        public readonly Node end;
        public readonly string id;

        /// <summary>
        ///     Callback function that takes in the retraced path and a bool of whether it was succesful.
        /// </summary>
        public readonly Action<Vector2[], bool> callBack;

        public PathRequest(string _id, Node _start, Node _end, Action<Vector2[], bool> _callBack)
        {
            id = _id;
            start = _start;
            end = _end;
            callBack = _callBack;
        }
    }
}