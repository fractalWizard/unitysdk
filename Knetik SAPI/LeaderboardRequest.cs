using System;
using SimpleJSON;
using UnityEngine;
using System.Collections.Generic;

namespace Knetik
{
	public class LeaderboardRequest : ApiRequest
	{
		public string leaderboard_id;
		public string copyright;
		public string date_created;
		public string date_updated;
		public string deleted;
		public string description;
		public string developer_id;
		public string image_url;
		public string lang;
		public string languages;
		public string metric_id;
		public string name;
		public string product_description;
		public string product_id;
		public string product_summary;
		public string product_title;
		public string product_translation_id;
		public string publisher_id;
		public string qualifying_value;
		public string rating_id;
		public string size;
		public string sort_style;
		public string update_date;
		public string level_id;
		public string active;
		public string create_date;
		public string level_name;
		public string metric_name;
		public Dictionary<string, string[]> user_results = new Dictionary<string, string[]>();

		public LeaderboardRequest (string api_key, int leaderboardId)
		{
			m_Key = api_key;
			m_clientSecret = ApiUtil.API_CLIENT_SECRET;
			m_leaderboardId = leaderboardId;
			m_method = "get";
			Debug.Log ("Leaderboard by ID");
		}

		string getLeaderboardRequest()
		{
			string leaderboard_request = "{";
			leaderboard_request += "\"leaderboard_id\":\"" + m_leaderboardId.ToString() + "\"";
			leaderboard_request += "}";
			
			Debug.Log ("Leaderboard Request String: " + leaderboard_request);
			return leaderboard_request;    
		}
		
		public bool doGetInfo()
		{
			string postBody = getLeaderboardRequest();
			
			JSONNode jsonDict = null;
			
			m_url = ApiUtil.API_URL + "/rest/api/latest/gameleaderboard";
			
			if(sendSignedRequest(null, postBody, ref jsonDict) == false) {
				return false;
			}
			
			if (jsonDict["result"] == null) {
				return false;
			}
			
			Debug.Log("Leaderboard Result: " + jsonDict["result"].ToString());
			
			leaderboard_id = jsonDict["result"]["id"];
			active = jsonDict["result"]["active"];
			copyright = jsonDict["result"]["copyright"];
			date_created = jsonDict["result"]["date_created"];
			date_updated = jsonDict["result"]["date_updated"];
			deleted = jsonDict["result"]["deleted"];
			developer_id = jsonDict["result"]["developer_id"];
			lang = jsonDict["result"]["lang"];
			languages = jsonDict["result"]["languages"];
			level_id = jsonDict["result"]["level_id"];
			level_name = jsonDict["result"]["level_name"];
			metric_id = jsonDict["result"]["metric_id"];
			metric_name = jsonDict["result"]["metric_name"];
			product_description = jsonDict["result"]["product_description"];
			product_id = jsonDict["result"]["product_id"];
			product_summary = jsonDict["result"]["product_summary"];
			product_title = jsonDict["result"]["product_title"];
			product_translation_id = jsonDict["result"]["product_translation_id"];
			publisher_id = jsonDict["result"]["publisher_id"];
			qualifying_value = jsonDict["result"]["qualifying_value"];
			rating_id = jsonDict["result"]["rating_id"];
			size = jsonDict["result"]["size"];
			sort_style = jsonDict["result"]["sort_style"];
			update_date = jsonDict["result"]["update_date"];
			create_date = jsonDict["result"]["create_date"];

			var gameLeaderboards = jsonDict["result"]["gameleaderboards"];
				
			int gameLeaderboardsCount = gameLeaderboards.Count;
			for(int i = 0; i < gameLeaderboardsCount; i++)
			{
				string userid = gameLeaderboards[i]["userid"];
				string current_score = gameLeaderboards[i]["current_score"];
				string username = gameLeaderboards[i]["username"];
				string avatar_url = gameLeaderboards[i]["avatar_url"];
				string[] results = {current_score, username, avatar_url};
				user_results.Add(userid, results);
			}

			return true;
		}
	}	
}