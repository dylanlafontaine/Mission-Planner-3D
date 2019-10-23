using System;

public class CoordinateConverter
{
    /// <summary>
    /// this method takes two sets of coordinates and converts them to 
    /// </summary>
    /// <param name="end"></param>
    /// <param name="start"></param>
    /// <returns>distance in meters between the two points, double</returns>
    private double DistanceBetweenGeoPoints(GeoCoordinate point1, GeoCoordinate point2)
    {
        if ((point1.Latitude == point2.Latitude) && (point1.Longitude == point2.Longitude))
        {
            return 0;
        }
        else
        {
            double theta = point1.Longitude - point2.Longitude;
            double dist = Math.Sin(deg2rad(point1.Latitude)) * Math.Sin(deg2rad(point2.Latitude)) + Math.Cos(deg2rad(point1.Latitude)) * Math.Cos(deg2rad(point2.Latitude)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            dist = (dist * 1.609344) / 1000;

            return dist;
        }
    }

    private double deg2rad(double deg)
    {
        return (deg * Math.PI / 180.0);
    }

    private double rad2deg(double rad)
    {
        return (rad / Math.PI * 180.0);
    }

    public static double MeterCoordtoGeoCoord(GeoCoordinate originGeo, MeterCoordinate originMeter, MeterCoordinate end)
    {
        /*
        δ = distance r / Earth radius(both in the same units)

        lat_P2 = asin(sin lat_O ⋅ cos δ + cos lat_O ⋅ sin δ ⋅ cos θ)
        lon_P2 = lon_O + atan((sin θ ⋅ sin δ ⋅ cos lat_O) / (cos δ − sin lat_O ⋅ sin lat_P2))
        */

        return 0;
    }
}

public class GeoCoordinate
{
    private double latitude;
    private double longitude;

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
