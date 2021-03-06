# Knetik Gaming Service API 

## Unity SDK


##1. Knetik Service API

  This document would provide a very brief information to how to use the API to develop software for Knetik SAPI

  The Knetik SAPI provides game portal improvement for games and facilitates the integration with the main web portal. Games could use Login/Registration feature of SAPI to authenticate users. After successful authentication, games could send metric updates to main server. 

  Knetik SAPI integration could be learned by an easy sample provided too. The simple game has Login/Registration and sends user scores to main server too.

##2. Integration Tutorial

  First you need to initialize API with the hostname and Key information you have received from the website:

EXAMPLE:
```
ApiUtil.setApiHost("your.sapi.server.com");
ApiUtil.setClientKey("some_client_key");
ApiUtil.setClientSecret("some_secret_key");
```

  Using these three calls, SAPI would know to whom it needs to talk to and what is the key/secret for communication. This should be done before any calls made to the API. In the sample, this has been done in StartMenuController::Start function with placeholders.  The real information should be provided to you by Knetik Media and the admin panel.

  After initializing the API, you could call different functions in the API to login/register. Please note that, other functions would not be available before having an authenticated session to SAPI.

###2.1 Login
Login requires valid username/password to proceed. The sample request username/password by a Unity form. You could do this by each way you like, but the strings should be passed to LoginRequest object. Then, by calling doLogin method, you could get the response from server. This C# sample shows how to use the API:

EXAMPLE:
```
	LoginRequest login = new LoginRequest(loginView.data.login, loginView.data.password);
	if (login.doLogin()) {
		m_Key = login.getKey();
		UserSessionUtils.setUserSession(login.getUserId(), login.getUsername(), login.getKey());
		Application.LoadLevel(1);
	} else {
		loginView.error = true;
		loginView.errorMessage = login.getErrorMessage();			
	}		
```

###2.2 Registration
The registration would more information. You should provide Username, Password, Email and Fullname. The API returns true or false to notify caller if it was successful or not. In case of error, getErrorMessage() method could be called for complete description.

EXAMPLE:
```
RegisterRequest ur = new RegisterRequest(lr.getKey(), 
		registrationView.data.login, 
		registrationView.data.password, 
		registrationView.data.email, 
		registrationView.data.fullname);
if(!ur.doRegister()) {
		registrationView.error = true;
		registrationView.errorMessage = ur.getErrorMessage();			
		return;
}
m_Key = lr.getKey();
UserSessionUtils.setUserSession(0, registrationView.data.login, lr.getKey());
Application.LoadLevel(1);
```

###2.3 Metric update
The game should inform the main server about user points using the Metric Update call. The call requires active session created before. Besides active session, caller should provider Metric-ID and value. Metric-ID should be grabbed from website. You should create metric_id in the Product information in the administration panel.

EXAMPLE:
```
	PostMetricRequest ur = new PostMetricRequest(UserSessionUtils.getApiKey(), 13, 1);
	if(ur.doPostMetric()) {
		Debug.Log("Metric post successful");
	} else {
		Debug.Log("METRIC POST FAILED!!!");
	}
```

To post a metric update with a specific level ID (NOTE: Levels are created on Admin Panel for each game), the PostMetricRequest simply needs an additional parameter passed to it, another int that represents the Level ID to be included as the part of the metric post.

EXAMPLE:
```
	PostMetricRequest ur = new PostMetricRequest(UserSessionUtils.getApiKey(), 13, 1, 2);
	if(ur.doPostMetric()) {
		Debug.Log("Metric post successful");
	} else {
		Debug.Log("METRIC POST FAILED!!!");
	}
```
	
###2.4 Product Game Option Retrieval
Product Game Options that are set in the Admin panel can be retrieved with a provided Product ID (which can also be seen on the Admin Panel).  Product ID should be passed as a string.  An active session is required as well.  Upon retrieving the game options, the option information is available as a look-up Dictionary, so option values can be looked up by name.  

EXAMPLE:
```
	ProductInfoRequest product = new ProductInfoRequest(UserSessionUtils.getApiKey(), productId);
	bool product_result = product.doGetInfo();
	string optionValue = null;
	string optionName = "my_game_option_name";
	if (product_result && product.game_options.TryGetValue(optionName, out optionValue))
	{
		Debug.Log("Option Name: " + optionName + " Option Value: " + optionValue);
	}
```
	
###2.5 User Game Option Retrieval
User Game Options that are set in the Admin panel or in the SDK (see 2.6) can be retrieved with a Product ID (which can also be seen on the Admin Panel).  Product ID and User ID should be passed as strings.  An active session is required as well.  Upon retrieving the user's game options, the option information is available as a look-up Dictionary, so option values can be looked up by name.  Note that this also retrieves several other user information values.

EXAMPLE:
```
	UserInfoRequest user = new UserInfoRequest(UserSessionUtils.getApiKey(), productId);
	bool user_result = user.doGetInfo();
	string optionValue = null;
	string optionName = "my_user_option_name";
	if (user_result && user.user_options.TryGetValue(optionName, out optionValue))
	{
		Debug.Log("Option Name: " + optionName + " Option Value: " + optionValue);
	}
```

###2.6 User Game Option Storage
User Game Options can be created or updated from the SDK.  This requires the following fields sent as strings: User ID (which can be retrieved with UserInfoRequest followed by a doGetInfo() on that object), Product ID (which can also be seen on the Admin Panel), the Option Name, the Option Value, as well as an active session.

####2.6.1 User Game Option Insert
Creates a new user option (name and value) based on the user ID and product ID.

EXAMPLE:
```
	PostUserOptionsRequest userInsert = new PostUserOptionsRequest(UserSessionUtils.getApiKey(), userId, productId, optionName, optionValue);
	bool user_result = userInsert.postUserInfo("insert");
```

####2.6.1 User Game Option Update
Updates an existing user option (value) based on the user ID, product ID, and option name.

EXAMPLE:
```
PostUserOptionsRequest userInsert = new PostUserOptionsRequest(UserSessionUtils.getApiKey(), userId, productId, optionName, optionValue);
bool user_result = userInsert.postUserInfo("update");
```

###2.7 Changing User Settings
A number of settings for the user can be changed within the Unity SDK.  These assume that the user information is already known as shown below.

```
UserInfoRequest userGet = new UserInfoRequest(UserSessionUtils.getApiKey(), productId);
	bool user_info_result = userGet.doGetInfo();
```

####2.7.1 Changing The User Avatar
The avatar of the user can be changed using an existing URL that must contain an image type (i.e. .jpg, .png, .gif, etc.).  Note that userGet.id is the ID of the user as pulled from the above userGet request.

EXAMPLE:
```
if (user_info_result)
{
string avatar_url = "http://www.mywebsite/this_new_avatar.png";
UserInfoRequest userPut = new UserInfoRequest(UserSessionUtils.getApiKey(), userGet.id, avatar_url, null);
string mode = "avatar";
bool userPut_result = userPut.putUserInfo(mode);
Debug.Log ("User Put Result for Avatar: " + userPut_result);
}
```

####2.7.2 Changing The User Language
The language of the user can be changed using an existing set of 2 letter names of languages that the system is expecting to handle (example: en for English, ar for Arabic).  Note that userGet.id is the ID of the user as pulled from the above userGet request.

EXAMPLE:
```
if (user_info_result)
{
string lang = "en";
UserInfoRequest userPut = new UserInfoRequest(UserSessionUtils.getApiKey(), userGet.id, null, lang);
string mode = "lang";
bool userPut_result = userPut.putUserInfo(mode);
Debug.Log ("User Put Result for Lang: " + userPut_result);
}
```

###2.8 Leaderboard Information Retrieval
Leaderboards are created on the Admin Panel outside of the Unity SDK.  They each have a unique ID associated with them, are only attached to a single game and a single metric.  Optionally they can be tied to a single game level as well.  Retrieving the leaderboard information requires knowing the leaderboard ID.  This will give the option of a number of fields, as well as the Game Leaderboard, which is the users' results based on this leaderboard.

EXAMPLE:
```
	int leaderboardId = 10;
	LeaderboardRequest leaderboard = new LeaderboardRequest(UserSessionUtils.getApiKey(), leaderboardId);
	bool leaderboard_result = leaderboard.doGetInfo();
	Debug.Log ("Leaderboard Result: " + leaderboard_result);
	Debug.Log ("Leaderboard Product Title: " + leaderboard.product_title);  // The title of the product associated with this leaderboard\
```

The Game Leaderboard is known as user_results and is a searchable dictionary, by user ID.  The user_results field requires a key of the user_id, (which can be requested from the User Results), and will return the following values:

```	
	public string currentscore;  // Value 0 of the user_results array
	public string username;		 // Value 1 of the user_results array
	public string avatar_url;	 // Value 2 of the user_results array
```

	EXAMPLE continued from above:
	
```
UserInfoRequest userGet = new UserInfoRequest(UserSessionUtils.getApiKey(), productId);
bool userGetResult = userGet.doGetInfo();
if (userGetResult)
{
	if(leaderboard.user_results.ContainsKey(userGet.id))
	{
		string[] user1_results = leaderboard.user_results[userGet.id];
		if (user1_results != null || user1_results.Length >= 1)
		{
			Debug.Log("User 1 Current Score: " + user1_results[0]);
			Debug.Log("User 1 Username: " + user1_results[1]);
			Debug.Log("User 1 Avatar URL: " + user1_results[2]);
		}
	}
	else
	{
		Debug.Log ("User " + userGet.id + " does not have results for this leaderboard");
	}
}
```

The following fields are currently retrievable from LeaderboardRequest objects:

```
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
```
