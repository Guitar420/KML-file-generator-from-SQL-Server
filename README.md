#KML file generator from SQL Server
A simple project for generating a kml file from a sqlserver table.
Just change the connection string, look at the query (should be modified to match your own table columns) and it's done.

#Current query
```
SELECT location.ToString(), name FROM entity
````
Just selects any row and gets the location (geography column) from SqlServer

##Filtering points inside an area
Let say that we have to include the point only inside Dubai area, we need to define a polygon (just go to google maps and touch any point then copy the latitude and longitude) once you have all the point you can modify the query like that
````
DECLARE @g geometry;
SET @g = geometry::STGeomFromText('POLYGON((24.911425 54.897508, 24.794290 55.078782, 25.233591 55.560807, 25.355271 55.291642, 24.911425 54.897508))', 4326);
SELECT location.ToString(), name from entity
where @g.STIntersects( geometry::STGeomFromText('POINT(' + CONVERT(varchar(10), location.Lat) +' ' + CONVERT(varchar(10), location.Long) + ')', 4326) ) = 1
````
I'm not a SqlServer expert (as you can see) this query just defines a polygon (just four point, you can add more if you want to) the determines if your location is inside, and here you are, the file generate will only includes point inside Dubai.

Enjoy.
