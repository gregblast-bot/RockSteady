using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[Flags]
public enum WallState
{
    /**********************************
     * Walls
     **********************************/
    // 0000 -> No Walls
    // 1111 -> Left, Top, Right, Bottom
    LeftWall = 8,   // 1000
    TopWall = 4,    // 0100
    RightWall = 2,  // 0010
    BottomWall = 1, // 0001

    /**********************************
     * Recursive Backtracker
     **********************************/
    Visited = 128, // 1000 0000
}

public struct Position
{
    public int X, Y;   
}

public struct Neighbor
{
    public Position Position;
    public WallState SharedWall;
}

public static class MazeGenerator
{
    private static WallState GetOppositeWall(WallState wall)
    {
        switch (wall)
        {
            case WallState.LeftWall: return WallState.RightWall;
            case WallState.TopWall: return WallState.BottomWall;
            case WallState.RightWall: return WallState.LeftWall;
            case WallState.BottomWall: return WallState.TopWall;
            default: return WallState.LeftWall;
        }  
    }

    private static WallState[,] ApplyRecursiveBacktracker(WallState[,] maze, int width, int height)
    {
        var rng = new Random();
        var positionStack = new Stack<Position>(); 
        var position = new Position { X = rng.Next(0, width), Y = rng.Next(0, height) };

        maze[position.X, position.Y] |= WallState.Visited; // 1000 1111
        positionStack.Push(position);

        while (positionStack.Count > 0)
        {
            // Start at parent node and find its neighbor
            var current = positionStack.Pop();
            var neighbors = GetUnvisitedNeighbors(current, maze, width, height);

            if (neighbors.Count > 0)
            {
                positionStack.Push(current);

                // Go to a random neighbor
                var randIndex = rng.Next(0, neighbors.Count);
                var randNeighbor = neighbors[randIndex];
                var nPosition = randNeighbor.Position;

                // Remove walls
                maze[current.X, current.Y] &= ~randNeighbor.SharedWall;
                maze[nPosition.X, nPosition.Y] &= ~GetOppositeWall(randNeighbor.SharedWall);

                // Mark as visited
                maze[nPosition.X, nPosition.Y] |= WallState.Visited;

                // Push onto stack and repeat
                positionStack.Push(nPosition);
            }
        }

        return maze;
    }

    private static List<Neighbor> GetUnvisitedNeighbors(Position pos, WallState[,] maze, int width, int height)
    {
        var list = new List<Neighbor>();

        // Check left
        if (pos.X > 0)
        {
            if (!maze[pos.X - 1, pos.Y].HasFlag(WallState.Visited))
            {
                list.Add(new Neighbor
                {
                    Position = new Position()
                    {
                        X = pos.X - 1,
                        Y = pos.Y
                    },
                    SharedWall = WallState.LeftWall
                }
                );;               
            }
        }

        // Check top
        if (pos.Y < height - 1)
        {
            if (!maze[pos.X, pos.Y + 1].HasFlag(WallState.Visited))
            {
                list.Add(new Neighbor
                {
                    Position = new Position()
                    {
                        X = pos.X,
                        Y = pos.Y + 1
                    },
                    SharedWall = WallState.TopWall
                }
                ); ;
            }
        }

        // Check right
        if (pos.X < width - 1)
        {
            if (!maze[pos.X + 1, pos.Y].HasFlag(WallState.Visited))
            {
                list.Add(new Neighbor
                {
                    Position = new Position()
                    {
                        X = pos.X + 1,
                        Y = pos.Y
                    },
                    SharedWall = WallState.RightWall
                }
                ); ;
            }
        }

        // Check bottom
        if (pos.Y > 0)
        {
            if (!maze[pos.X, pos.Y - 1].HasFlag(WallState.Visited))
            {
                list.Add(new Neighbor
                {
                    Position = new Position()
                    {
                        X = pos.X,
                        Y = pos.Y - 1
                    },
                    SharedWall = WallState.BottomWall
                }
                ); ;
            }
        }

        return list;
    }
    public static WallState[,] Generate(int width, int height, int randEntrancePosY, int randExitPosY)
    {
        WallState[,] maze = new WallState[width, height];
        WallState initial = WallState.LeftWall | WallState.TopWall | WallState.RightWall | WallState.BottomWall;

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                maze[i, j] = initial; // initial -> 1111
            }
        }

        maze[0, randEntrancePosY] &= ~WallState.LeftWall; // Remove left wall of leftmost cell for entrance
        maze[width - 1, randExitPosY] &= ~WallState.RightWall; // Remove right wall of rightmost cell for exit
        return ApplyRecursiveBacktracker(maze, width, height);
    }
}