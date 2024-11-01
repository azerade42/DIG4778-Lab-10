- A* pathfinding calculates it's projected paths by labeling it's start node and target end node and calculating a cost path based on the distance to the target.
  A* goes off of a priority queue meaning that it prioritizes the most promising paths first to try and find the best route

- While dynamically allocating in real time, there are a couple problems that can occur.
  Based on the obstacles in the scene, recalculation times and synchronization can potentially cause issues with A* and can cause performance issues if it is trying to scan large areas.

- To adapt this code to larger grids or open-world settings you can do a few techniques to optimize A*;
  Hierarchical Pathfinding can split the world into pieces and your A* can calculate it's path through one piece at a time until it reaches it's goal.
  Sampling the area can also help if you use something like a Rapidly-exploring random tree method to quickly find and reduce the size of the area to search with A*.
  You could also use multithreading or memory optimization to allow paths to be cached or calculated asynchronously to help the scalability.

- If we were to add weighted cells to the map we would probably need to make a cost function to accurately calculate how the cells would gain their weighted value.
  We would also need to adjust how the A* calculates the best paths to make sure it doesn't overestimate the costs of paths with the added weights.
  Lastly if we would want to dynamically adjust the weights of a path we would need to create a method to create a weight of a node based on local variables for A* to calculate.
