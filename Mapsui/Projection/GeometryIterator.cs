﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mapsui.Geometries;

namespace Mapsui.Projection
{
    public static class GeometryIterator
    {
        public static IEnumerable<Point> AllVertices(this IGeometry geometry)
        {
            if (geometry == null)
                return new Point[0];
            var point = geometry as Point;
            if (point != null)
                return new[] { point };
            var lineString = geometry as LineString;
            if (lineString != null)
                return AllVertices(lineString);
            var polygon = geometry as Polygon;
            if (polygon != null)
                return AllVertices(polygon);
            var geometrys = geometry as IEnumerable<Geometry>;
            if (geometrys != null)
                return AllVertices(geometrys);
            
            var format = String.Format("unsupported geometry: {0}", geometry.GetType().Name);
            throw new NotSupportedException(format);
        }

        private static IEnumerable<Point> AllVertices(LineString lineString)
        {
            if (lineString == null)
                throw new ArgumentNullException("lineString");

            return lineString.Vertices;
        }

        private static IEnumerable<Point> AllVertices(Polygon polygon)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon");

            foreach (var point in polygon.ExteriorRing.Vertices)
                yield return point;
            foreach (var ring in polygon.InteriorRings)
                foreach (var point in ring.Vertices)
                    yield return point;
        }

        private static IEnumerable<Point> AllVertices(IEnumerable<Geometry> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            foreach (var geometry in collection)
                foreach (var point in AllVertices(geometry))
                    yield return point;
        }
    }
}
