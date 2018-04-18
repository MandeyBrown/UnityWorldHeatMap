using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;

class NewGeohash
{
    /*static void Main(string[] args)
	{

		//NewGeohash gh = new NewGeohash();
		string ss = NewGeohash.Encode(31.22d, -101.3d, 4);
		Console.WriteLine(ss);
		double[] dd = NewGeohash.Decode(ss);
		Console.WriteLine(dd[0]+" "+dd[1]+" "+dd.Length);

		Console.Write(NewGeohash.CalculateAdjacent("9xtc", Direction.Right));

		ArrayList list = getInternalGeohashes("9xp","9zp","9tz","9vz");
		Console.WriteLine(" ");
		foreach (var item in list)
		{
			Console.Write(item+" ");
		}
		System.Threading.Thread.Sleep(100000);
	}*/



    public static void parseJsonRsp(string rsp, Dictionary<string, float> gMap)
    {

        Dictionary<string, Int32> gMapCount = new Dictionary<string, Int32>();
        //String rsp = "[{\"_id\":{\"$oid\":\"5acf89733cf24e6ae604b651\"},\"point\":{\"geohash\":\"9xh1958dkfzw\"},\"severity\":2,\"toPoint\":{\"geohash\":\"9xh1958dkfzw\"}},{\"_id\":{\"$oid\":\"5acf89743cf24e6ae604b659\"},\"point\":{\"geohash\":\"9xh1958dkfz11\"},\"severity\":3,\"toPoint\":{\"geohash\":\"9xh8b8kwfvgr\"}},{\"_id\":{\"$oid\":\"5acf89743cf24e6ae604b65e\"},\"point\":{\"geohash\":\"9xh3e2ugg69y\"},\"severity\":2,\"toPoint\":{\"geohash\":\"9xh1958evw96\"}},{\"_id\":{\"$oid\":\"5acf89743cf24e6ae604b669\"},\"point\":{\"geohash\":\"9xh8b8kmsc1r\"},\"severity\":2,\"toPoint\":{\"geohash\":\"9xh2zh8jjwg1\"}}]";
        Console.Write(rsp);

        rsp = rsp.Substring(1, rsp.Length - 2);

        Dictionary<string, float> gMapTemp = JsonConvert.DeserializeObject<Dictionary<string, float>>(rsp);
        //JObject o = JObject.Parse(rsp);
        foreach (var sss in gMapTemp.Keys)
        {

            string geohash = sss;
            float sev = gMapTemp[sss];

            if (geohash.Length > 7)
                geohash = geohash.Substring(0, 7);


            if (!gMap.ContainsKey(geohash))
            {
                gMap.Add(geohash, sev);
                gMapCount.Add(geohash, 1);
            }
            else
            {
                float sevOld = gMap[geohash];
                Int32 countOld = gMapCount[geohash];

                float newSev = (sevOld * countOld + sev) / (countOld + 1);
                gMap.Remove(geohash);
                gMap.Add(geohash, newSev);

                gMapCount.Remove(geohash);
                gMapCount.Add(geohash, countOld + 1);
            }
        }






    }
    public static long getTimeStamp(string s)
    {

        DateTime MyDateTime;
        MyDateTime = new DateTime();
        MyDateTime = DateTime.ParseExact(s, "yyyy-MM-dd h:mm tt", null);

        long timestamp = (long)(MyDateTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
        Console.Write(timestamp);
		return timestamp;
    }
	public static string queryJSON(string geohashes){
		return "";
	}

	public static string queryJSON(string geohashes, string date, string type)
	{
		long ts = getTimeStamp (date);
		string rt = "";
		try
		{
			var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://dover.cs.colostate.edu:27018/heliosServlet/helios");
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";

			using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				string json1 = "{\"geohash\":[\"9xh2zh8\",\"9xh1958\",\"dr5\"]," +
					//"\"timestamp\":{\"from\":\"1522617934000\",\"to\":\"1523568335000\"}," +
					"\"timestamp\":{\"from\":\"1523110008000\"}," +
					"\"zoomLevel\":\"heatPoints\"," +
					"\"type\":\"historical\"" +
					"}";

				string json2 = "{\"geohash\":["+ geohashes +"]," +
					//"\"timestamp\":{\"from\":\"1522617934000\",\"to\":\"1523568335000\"}," +
					"\"timestamp\":{\"from\":\"1523110008000\"}," +
					"\"zoomLevel\":\"number\"," +
					"\"type\":\"historical\"" +
					"}";

				string json = "{\"geohash\":["+ geohashes +"]," +
					//"\"timestamp\":{\"from\":\"1522617934000\",\"to\":\"1523568335000\"}," +
					"\"timestamp\":{\"from\":\""+ ts +"\"}," +
					"\"zoomLevel\":\"number\"," +
					"\"type\":\""+ type +"\"" +
					"}";
				

				Debug.Log(json);
				streamWriter.Write(json);
				streamWriter.Flush();
				streamWriter.Close();
			}


			Console.WriteLine(httpWebRequest.RequestUri.ToString());
			var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			Console.WriteLine("GOT IT");
			string result = "";
			using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
			{
				result = streamReader.ReadToEnd();
				return result;

			}
			Console.WriteLine("RSP: " + result);
		} catch(Exception e)
		{
			Console.WriteLine("EXCEPTION "+e.ToString());
		}
		return rt;
	}

	public enum Direction
	{
		Top = 0,
		Right = 1,
		Bottom = 2,
		Left = 3
	}

	private const string Base32 = "0123456789bcdefghjkmnpqrstuvwxyz";
	private static readonly int[] Bits = new[] { 16, 8, 4, 2, 1 };

	private static readonly string[][] Neighbors = {
		new[]
		{
			"p0r21436x8zb9dcf5h7kjnmqesgutwvy", // Top
			"bc01fg45238967deuvhjyznpkmstqrwx", // Right
			"14365h7k9dcfesgujnmqp0r2twvyx8zb", // Bottom
			"238967debc01fg45kmstqrwxuvhjyznp", // Left
		}, new[]
		{
			"bc01fg45238967deuvhjyznpkmstqrwx", // Top
			"p0r21436x8zb9dcf5h7kjnmqesgutwvy", // Right
			"238967debc01fg45kmstqrwxuvhjyznp", // Bottom
			"14365h7k9dcfesgujnmqp0r2twvyx8zb", // Left
		}
	};

	private static readonly string[][] Borders = {
		new[] {"prxz", "bcfguvyz", "028b", "0145hjnp"},
		new[] {"bcfguvyz", "prxz", "0145hjnp", "028b"}
	};


	public static ArrayList getInternalGeohashes(String nw, String ne, String sw, String se)
	{
		ArrayList list = new ArrayList();
		Int32 precision = nw.Length;
		String currentG = nw;
		String start = nw;
		String endG = ne;
		Boolean keepGoing = true;
		Boolean lastRow = false;
		//Console.WriteLine("======");
		//Console.Write(start+" ");
		list.Add(start);
		while (keepGoing)
		{
			String neighbor = CalculateAdjacent(currentG, Direction.Right);

			currentG = neighbor;
			list.Add(currentG);
			//Console.Write(currentG + " ");

			if (neighbor.Equals(endG) && !lastRow)
			{
				start = CalculateAdjacent(start, Direction.Bottom);
				currentG = start;
				list.Add(currentG);
				//Console.Write(currentG + " ");

				endG = CalculateAdjacent(endG, Direction.Bottom);
				if (currentG.Equals(sw))
				{
					lastRow = true;
				}
			} else if (neighbor.Equals(endG) && lastRow) {
				keepGoing = false;
			}
		}
		return list;

	}

	public static string getInternalGeohashesAsString(String nw, String ne, String sw, String se)
	{
		Int32 precision = nw.Length;
		String currentG = nw;
		String start = nw;
		String endG = ne;
		Boolean keepGoing = true;
		Boolean lastRow = false;
		string internals = "";
		//Console.WriteLine("======");
		//Console.Write(start+" ");
		internals+="\""+start+"\",";
		while (keepGoing)
		{
			String neighbor = CalculateAdjacent(currentG, Direction.Right);

			currentG = neighbor;
			internals+="\""+currentG+"\",";
			//Console.Write(currentG + " ");

			if (neighbor.Equals(endG) && !lastRow)
			{
				start = CalculateAdjacent(start, Direction.Bottom);
				currentG = start;
				internals+="\""+currentG+"\",";
				//Console.Write(currentG + " ");

				endG = CalculateAdjacent(endG, Direction.Bottom);
				if (currentG.Equals(sw))
				{
					lastRow = true;
				}
			} else if (neighbor.Equals(endG) && lastRow) {
				keepGoing = false;
			}
		}
		return internals.Substring(0, internals.Length - 1);

	}

	public static String CalculateAdjacent(String hash, Direction direction)
	{
		hash = hash.ToLower();

		char lastChr = hash[hash.Length - 1];
		int type = hash.Length % 2;
		var dir = (int)direction;
		string nHash = hash.Substring(0, hash.Length - 1);

		if (Borders[type][dir].IndexOf(lastChr) != -1)
		{
			nHash = CalculateAdjacent(nHash, (Direction)dir);
		}
		return nHash + Base32[Neighbors[type][dir].IndexOf(lastChr)];
	}

	public static void RefineInterval(ref double[] interval, int cd, int mask)
	{
		if ((cd & mask) != 0)
		{
			interval[0] = (interval[0] + interval[1]) / 2;
		}
		else
		{
			interval[1] = (interval[0] + interval[1]) / 2;
		}
	}

	// Returns lat1, lat2, long1, long2
	public static double[] Decode(String geohash)
	{
		bool even = true;
		double[] lat = { -90.0, 90.0 };
		double[] lon = { -180.0, 180.0 };

		foreach (char c in geohash)
		{
			int cd = Base32.IndexOf(c);
			for (int j = 0; j < 5; j++)
			{
				int mask = Bits[j];
				if (even)
				{
					RefineInterval(ref lon, cd, mask);
				}
				else
				{
					RefineInterval(ref lat, cd, mask);
				}
				even = !even;
			}
		}

		//return new[] { (lat[0] + lat[1]) / 2, (lon[0] + lon[1]) / 2 };
		return new[] { lat[0], lat[1], lon[0],lon[1]};
	}

	public static String Encode(double latitude, double longitude, int precision = 12)
	{
		bool even = true;
		int bit = 0;
		int ch = 0;
		string geohash = "";

		double[] lat = { -90.0, 90.0 };
		double[] lon = { -180.0, 180.0 };

		if (precision < 1 || precision > 20) precision = 12;

		while (geohash.Length < precision)
		{
			double mid;

			if (even)
			{
				mid = (lon[0] + lon[1]) / 2;
				if (longitude > mid)
				{
					ch |= Bits[bit];
					lon[0] = mid;
				}
				else
					lon[1] = mid;
			}
			else
			{
				mid = (lat[0] + lat[1]) / 2;
				if (latitude > mid)
				{
					ch |= Bits[bit];
					lat[0] = mid;
				}
				else
					lat[1] = mid;
			}

			even = !even;
			if (bit < 4)
				bit++;
			else
			{
				geohash += Base32[ch];
				bit = 0;
				ch = 0;
			}
		}
		return geohash;
	}
}


