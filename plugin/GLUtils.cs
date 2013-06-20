using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// GL utils. Part of this code is from MechJeb2.
/// </summary>
namespace MissionController
{
    public class GLUtils
    {
        static Material _material;

        static Material material {
            get {
                if (_material == null)
                    _material = new Material (Shader.Find("Particles/Additive"));
                return _material;
            }
        }

        public static void drawLandingArea (CelestialBody body, double minLatitude, double maxLatitude, 
                                            double minLongitude, double maxLongitude, 
                                            Color c, double rotation = 0)
        {
            double dlat = (maxLatitude - minLatitude) / 10.0;
            double dlog = (maxLongitude - minLongitude) / 10.0;

            List<Vector3d[]> quads = new List<Vector3d[]> ();

            for (double lat = minLatitude; lat + dlat < maxLatitude; lat += dlat) {
                for (double log = minLongitude; log + dlog < maxLongitude; log += dlog) {
                    Vector3d up1 = body.GetSurfaceNVector (lat, log);
                    Vector3d center1 = body.position + body.Radius * up1;

                    Vector3d up2 = body.GetSurfaceNVector (lat, log + dlog);
                    Vector3d center2 = body.position + body.Radius * up2;

                    Vector3d up3 = body.GetSurfaceNVector (lat + dlat, log + dlog);
                    Vector3d center3 = body.position + body.Radius * up3;

                    Vector3d up4 = body.GetSurfaceNVector (lat + dlat, log);
                    Vector3d center4 = body.position + body.Radius * up4;

                    if (!IsOccluded (center1, body)) {
                        quads.Add (new Vector3d[] { center1, center2, center3, center4});
                    }
                }
            }

            GLQuadMap (quads, c);
        }

        public static void GLQuadMap (List<Vector3d[]> list, Color c)
        {
            GL.PushMatrix ();
            material.SetPass (0);
            GL.LoadOrtho ();
            GL.Begin (GL.QUADS);
            GL.Color (c);
            foreach(Vector3d[] vertices in list) {
                GLVertexMap (vertices[0]);
                GLVertexMap (vertices[1]);
                GLVertexMap (vertices[2]);
                GLVertexMap (vertices[3]);
            }
            GL.End ();
            GL.PopMatrix ();
        }

        public static void GLVertexMap (Vector3d worldPosition)
        {
            Vector3 screenPoint = PlanetariumCamera.Camera.WorldToScreenPoint (ScaledSpace.LocalToScaledSpace(worldPosition));
            GL.Vertex3 (screenPoint.x / Camera.main.pixelWidth, screenPoint.y / Camera.main.pixelHeight, 0);
        }
            //Tests if byBody occludes worldPosition, from the perspective of the planetarium camera
        public static bool IsOccluded (Vector3d worldPosition, CelestialBody byBody)
        {
            if (Vector3d.Distance (worldPosition, byBody.position) < byBody.Radius - 100)
                return true;

            Vector3d camPos = ScaledSpace.ScaledToLocalSpace (PlanetariumCamera.Camera.transform.position);

            if (Vector3d.Angle (camPos - worldPosition, byBody.position - worldPosition) > 90)
                return false;

            double bodyDistance = Vector3d.Distance (camPos, byBody.position);
            double separationAngle = Vector3d.Angle (worldPosition - camPos, byBody.position - camPos);
            double altitude = bodyDistance * Math.Sin (Math.PI / 180 * separationAngle);
            return (altitude < byBody.Radius);
        }
    }
}

