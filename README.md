# Knetik Gaming Service API 

## Unity SDK


##1. Knetik Service API

  This document would provide a very brief information to how to use the API to develop software for Knetik SAPI

  The Knetik SAPI provides game portal improvement for games and facilitates the integration with the main web portal. Games could use Login/Registration feature of SAPI to authenticate users. After successful authentication, games could send metric updates to main server. 

  Knetik SAPI integration could be learned by an easy sample provided too. The simple game has Login/Registration and sends user scores to main server too.

##2. Integration Tutorial

  First you need to initialize API with the hostname and Key information you have received from the website:


```
ApiUtil.setApiHost("sapi.rsigaming.com");
ApiUtil.setClientKey("qxneBlYwEe");
ApiUtil.setClientSecret("v1X0CbBVbgpDXRqQ4aFU06rnl2Jjyn");
```

  Using these three calls, SAPI would know to whom it needs to talk to and what is the key/secret for communication. This should be done before any calls made to the API. In the sample, this has been done in StartMenuController::Start function.

  After initializing the API, you could call different functions in the API to login/register. Please note that, other functions would not be available before having an authenticated session to SAPI.

###2.1 Login
Login requires valid username/password to proceed. The sample request username/password by a Unity form. You could do this by each way you like, but the strings should be passed to LoginRequest object. Then, by calling doLogin method, you could get the response from server. This C# sample shows how to use the API:

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

```
	PostMetricRequest ur = new PostMetricRequest(UserSessionUtils.getApiKey(), 13, 1);
	if(ur.doPostMetric()) {
		Debug.Log("Metric post successful");
	} else {
		Debug.Log("METRIC POST FAILED!!!");
	}
```