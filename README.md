# MarchingCubeTerrainGeneration

This is a project exploring the the marching cube algorithm. Algorithms have always been interesting to me and I have been wanting to learn a new terrain generation algorithm.



## How it works
The marching cube algorithm relies on a grid of points, with weight values. When below a certain threshold, the point will either be white (positive, inside the mesh) or black (negative, outside the mesh).
By using a noise generator consisting of perlin noise and a Y cuttoff point, the following grid is made:

![grid-values-marchingcube-terrain](https://github.com/Mightylight/MarchingCubeTerrainGeneration/assets/59601297/f5fa5135-334d-4fb7-911f-822ef9d24aae)


After this grid has been made, cubes are assembled from these points and their configuration is calculated. This is done by combining the values of the corners (either 0 or 1) into a binary number and converting that
to a decimal number. This number is than put into the Marchingcubes table, to know what triangles need to be formed. The triangulation table was not made by me, but aqquired from https://polycoding.net/assets/downloads/marching-cubes/MarchingCubesTables.cs .

![cube-configuration-marchingcube-terrain](https://github.com/Mightylight/MarchingCubeTerrainGeneration/assets/59601297/5e0ce07e-32e5-4445-8b4b-2744b2fb3f7c)


With the configuration calculated, we make the triangles like this:

![TriangleCalculation-marchingcube-terrain](https://github.com/Mightylight/MarchingCubeTerrainGeneration/assets/59601297/dc1c84d9-9289-4bed-a931-6133cdf42fc3)

The triangles are sent to a meshbuilder, which combines all cubes to make a singular mesh inside the MarchingCubeManager.

This leads to this product:

### Side view:
![side-view-marchingcube-terrain](https://github.com/Mightylight/MarchingCubeTerrainGeneration/assets/59601297/e0b53568-6384-40f8-ab9d-b9ea9a0c0bcc)

### Top down view:
![top-down-marchingcube-terrain](https://github.com/Mightylight/MarchingCubeTerrainGeneration/assets/59601297/69210925-0150-4d8f-94ba-5e88e8b19f6e)

### Adjustable variables
Adjustable variables thus far include:
- Width, height and depth of the (voxel) grid
- Height Cutoff (amplitude of the mountains)


## Todo's
This project is still a work in progress and sees constant work.
Further updates to this project include:
- Adding an improved noise generator for other applications than flat terrain
- Different ways to interpolate the vertices for the mesh
- Adding colours based on height levels (adding snow at the tops, water below the surface level etc.)
- Editor tooling for editor time terrain generation and ease of use.
- Terrain decorations
- Converting the algorithm (right now written in normal unity C#) to a compute shader for better performance
