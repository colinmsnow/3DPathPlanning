# 3DPathPlanning

Implementation of three graph creation algorithms for path planning in 3D.

1. Grid-based graph
2. Visibility Graph
3. Navigation Mesh with Delaunay Triangulation

Usage:

1. Open up a scene in Unity
2. Assign any objects you want to be obstacles to the tag Obstacle.
3. Add the correct script for your preferred method to any game object.
    - Object DetectionGrid and Path Planner for grid-based
    - Bounding Box Graph Creator for visibility graph
    - Nav Mesh Creator for navigation mesh
4. Run the scene with debug rays on to see generated path.

Special thanks to June Rhodes for a circumsphere calculator and Nikhil Joshi for a priority queue implementation.
