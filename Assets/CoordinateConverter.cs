using System;

public class CoordinateConverter
{
    public MeterCoordinate FindMeterCoordinateFromOrigin(GeoCoordinate origin, GeoCoordinate point)
    {
        MeterCoordinate result = new MeterCoordinate();

        double dlon = point.Longitude - origin.Longitude;
        double dlat = point.Latitude - origin.Latitude;

        double latitudeCircumference = 40075160 * Math.Cos(ToRad(origin.Latitude));
        result.X = dlon * latitudeCircumference / 360;
        result.Y = dlat * 40008000 / 360;

        return result;
    }


    /*
    /// <summary>
    /// pass in origin and unknown point, return its location in 3D space
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="point"></param>
    /// <returns>MeterCoordinate</returns>
    public MeterCoordinate FindMeterCoordinateFromOrigin2(GeoCoordinate origin, GeoCoordinate point)
    {
        MeterCoordinate result = new MeterCoordinate();
        // find the distance and bearing from the origin to the point
        double distance = DistanceBetweenGeoPoints(origin, point), bearing = FindBearing(origin, point);

        // if result is on the x or y axis, set and return
        if (bearing == 0)
        {
            result.X = distance;
        }
        else if (bearing == 90)
        {
            result.Y = distance;
        }
        else if (bearing == 180)
        {
            result.X = distance * -1;
        }
        else if(bearing == 270)
        {
            result.Y = distance * -1;
        }
        else // bearing is not directly on the axis
        {
            double angleA = 0;
            // find the inside angle based off the bearing. Think of it like a unit circle i suppose.
            if (bearing < 90)
            {
                angleA = bearing;
            }
            else if (bearing > 90 && bearing < 180)
            {
                angleA = bearing - 90;
            }
            else if (bearing > 180 && bearing < 270)
            {
                angleA = bearing - 180;
            }
            else if (bearing > 270)
            {
                angleA = bearing - 270;
            }
            //angle side angle computation. euclidian trig introduces some error!
            double angleC = 90 - bearing, angleB = 90;
            double sideB = distance;

            double sideA = ((sideB * Math.Sin(angleA)) / Math.Sin(angleB));

            double sideC = ((sideB * Math.Sin(angleC)) / Math.Sin(angleB));

            // set result to correct lengths based off the calculations
            if (bearing < 90)
            {
                result.X = sideC;
                result.Y = sideA;
                return result;
            }
            else if (bearing > 90 && bearing < 180)
            {
                result.X = sideC * -1;
                result.Y = sideA;
            }
            else if (bearing > 180 && bearing < 270)
            {
                result.X = sideC * -1;
                result.Y = sideA * -1;
            }
            else if (bearing > 270)
            {
                result.X = sideC;
                result.Y = sideA * -1;
            }
        }


        return result;
    }

    /// <summary>
    /// finds the distance between the lat long coords
    /// </summary>
    /// <param name="end"></param>
    /// <param name="start"></param>
    /// <returns>distance in meters between the two points, double</returns>
    private double DistanceBetweenGeoPoints(GeoCoordinate point1, GeoCoordinate point2)
    {
        // check for equal points
        if ((point1.Latitude == point2.Latitude) && (point1.Longitude == point2.Longitude))
        {
            return 0;
        }
        else
        {
            // I found this math online and translated it into c#. appears to be correct
            double dlon = ToRad(point2.Longitude - point1.Longitude);
            double dlat = ToRad(point2.Latitude - point1.Latitude);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(ToRad(point1.Latitude)) * Math.Cos(ToRad(point2.Latitude)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * 6371e3;
        }
    }

    /// <summary>
    /// returns the angle of the bearing.
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns>double angle of bearing</returns>
    private double FindBearing (GeoCoordinate point1, GeoCoordinate point2)
    {
        // I also found this math online and converted it into c#. appears to work

        double dLon = ToRad(point2.Longitude - point1.Longitude);

        double y = Math.Sin(dLon) * Math.Cos(ToRad(point2.Latitude));
        double x = Math.Cos(ToRad(point1.Latitude)) * Math.Sin(ToRad(point2.Latitude)) - Math.Sin(ToRad(point1.Latitude))
                * Math.Cos(ToRad(point2.Latitude)) * Math.Cos(dLon);

        double brng = Math.Atan2(y, x);

        brng = ToDegrees(brng);
        brng = (brng + 360) % 360;
        brng = 360 - brng; // count degrees counter-clockwise - remove to make clockwise

        return brng;



    }
    */

    /// <summary>
    /// a version of the Math.atan2 found in javascript and converted to c#
    /// </summary>
    /// <param name="y"></param>
    /// <param name="x"></param>
    /// <returns>arc tan between two radians</returns>
    private double atan2(double y, double x)
    {
        if (x < 0)
        {
            return (Math.Atan(y / x) + 3 * Math.PI / 2); // subst 270 for 3*pi/2 if degrees
        }
        else
        {
            return (Math.Atan(y / x) + Math.PI / 2); // subst 90 for pi/2 if degrees
        }
    }

    /// <summary>
    /// degrees -> rads
    /// </summary>
    /// <param name="degrees"></param>
    /// <returns></returns>
    private static double ToRad(double degrees)
    {
        return degrees * (Math.PI / 180);
    }

    /// <summary>
    /// rads -> degrees
    /// </summary>
    /// <param name="radians"></param>
    /// <returns></returns>
    private static double ToDegrees(double radians)
    {
        return radians * 180 / Math.PI;
    }

    /// <summary>
    /// rads to bearing
    /// </summary>
    /// <param name="radians"></param>
    /// <returns></returns>
    private static double ToBearing(double radians)
    {
        // convert radians to degrees (as bearing: 0...360)
        return (ToDegrees(radians) + 360) % 360;
    }

    /// <summary>
    /// ryals special sauce. not sure if its working or not
    /// </summary>
    /// <param name="end"></param>
    /// <param name="originMeter"></param>
    /// <param name="originGeo"></param>
    /// <returns></returns>
    private double FindLatitude(MeterCoordinate end, MeterCoordinate originMeter, GeoCoordinate originGeo)
    {
        int earthRadius = 63781370; //meters
        double complimentAngle = 90 - Math.Abs(originGeo.Longitude);
        double radius = Math.Sin(complimentAngle) * earthRadius;
        double circumference = Math.PI * 2 * radius;
        double deltaX = end.X - originMeter.X;
        return (deltaX / circumference) * 360 + originGeo.Latitude;

    }

    /// <summary>
    /// ryals special sauce. not sure if its working or not.
    /// </summary>
    /// <param name="end"></param>
    /// <param name="originMeter"></param>
    /// <param name="originGeo"></param>
    /// <returns></returns>
    private double FindLongitude(MeterCoordinate end, MeterCoordinate originMeter, GeoCoordinate originGeo)
    {
        int earthRadius = 63781370; //meters
        double complimentAngle = 90 - Math.Abs(originGeo.Latitude);
        double radius = Math.Sin(complimentAngle) * earthRadius;
        double circumference = Math.PI * 2 * radius;
        double deltaY = end.Y - originMeter.Y;
        return (deltaY / circumference) * 360 + originGeo.Longitude;

    }

    /// <summary>
    /// the main function to find lat long from a geo coordinate
    /// </summary>
    /// <param name="originGeo"></param>
    /// <param name="originMeter"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public GeoCoordinate MeterCoordtoGeoCoord(GeoCoordinate originGeo, MeterCoordinate originMeter, MeterCoordinate end)
    {
        /*
        δ = distance r / Earth radius(both in the same units)

        lat_P2 = asin(sin lat_O ⋅ cos δ + cos lat_O ⋅ sin δ ⋅ cos θ)
        lon_P2 = lon_O + atan((sin θ ⋅ sin δ ⋅ cos lat_O) / (cos δ − sin lat_O ⋅ sin lat_P2))
        */
        GeoCoordinate result = new GeoCoordinate();
        result.Latitude = FindLatitude(end, originMeter, originGeo);
        result.Longitude = FindLongitude(end, originMeter, originGeo);
        return result;
    }
}

public class GeoCoordinate
{
    private double latitude;
    private double longitude;

    /// <summary>
    /// constructor that takes lat lon points
    /// </summary>
    /// <param name="v1">lat</param>
    /// <param name="v2">lon</param>
    public GeoCoordinate(double v1, double v2)
    {
        this.latitude = v1;
        this.longitude = v2;
    }

    public GeoCoordinate()
    {
        this.latitude = 0;
        this.longitude = 0;
    }

    public double Latitude
    {
        get
        {
            return this.latitude;
        }
        set
        {
            this.latitude = value;
        }
    }

    public double Longitude
    {
        get
        {
            return this.longitude;
        }

        set
        {
            this.longitude = value;
        }
    }
}

public class MeterCoordinate
{
    private double x;
    private double y;

    public MeterCoordinate()
    {
        this.x = 0;
        this.y = 0;
    }

    /// <summary>
    /// constructor which takes x and y arguments
    /// </summary>
    /// <param name="v1">x</param>
    /// <param name="v2">y</param>
    public MeterCoordinate(double v1, double v2)
    {
        this.x = v1;
        this.y = v2;
    }

    public double X
    {
        get
        {
            return this.x;
        }

        set
        {
            this.x = value;
        }
    }

    public double Y
    {
        get
        {
            return this.y;
        }
        set
        {
            this.y = value;
        }
    }
}