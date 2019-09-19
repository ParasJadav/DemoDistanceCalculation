using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DistanceCalculation.Models;
using System.Web;
using System.Net;
using System.Data;
using System.Text;
using System.Configuration;

namespace DistanceCalculation.Repository
{
    public class DistanceRepository
    {
        public static IEnumerable<Locations> GetLocations()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\address list australia.txt");
            if (System.IO.File.Exists(path))
            {
                List<string> lines = System.IO.File.ReadLines(path).ToList();
                if (lines != null && lines.Count > 0)
                {
                    List<Locations> locations = new List<Locations>();
                    for (int i = 0; i < lines.Count; i++)
                    {
                        Locations location = new Locations();
                        location.Id = i;
                        location.Name = lines[i];
                        locations.Add(location);
                    }
                    return locations;
                }
                return null;
            }
            return null;
        }

        public static List<DistanceModels> Calculate(string originaddress, string destinationaddress)
        {
            List<DistanceModels> lstdistanceForAddress = new List<DistanceModels>();
            string url = "https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + originaddress + "&destinations=" + destinationaddress + "&key=" + ConfigurationManager.AppSettings["GoogleAPIKey"]; ;
            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    DataSet dsResult = new DataSet();
                    dsResult.ReadXml(reader);
                    DataSet addressDataSet = new DataSet();
                    addressDataSet = dsResult.Tables[0].Rows[0].Table.DataSet;
                    for (int i = 0; i < addressDataSet.Tables[1].Select().Count(); i++)
                    {
                        DistanceModels addressDistance = new DistanceModels();
                        addressDistance.LocationName = addressDataSet.Tables[1].Rows[i].ItemArray[0].ToString();
                        addressDistance.DurationToTravel = addressDataSet.Tables[4].Rows[i].ItemArray[1].ToString();
                        addressDistance.Distance = Convert.ToDouble(addressDataSet.Tables[5].Rows[i].ItemArray[1].ToString().Replace("km","").Trim());
                        lstdistanceForAddress.Add(addressDistance);
                    }
                    if (lstdistanceForAddress != null && lstdistanceForAddress.Count() > 0)
                    {
                        lstdistanceForAddress = lstdistanceForAddress.OrderBy(x => x.Distance).Take(5).ToList();
                    }
                }
            }
            return lstdistanceForAddress;
        }

    }
}