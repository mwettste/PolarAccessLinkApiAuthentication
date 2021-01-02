# Authentication to Polar AccessLink API with OAuth2 and C#

This is a very basic implementation in C# to get access to the Polar AccessLink API.

1. Follow the directions from Polar to get registered: https://www.polar.com/accesslink-api/#polar-accesslink-api
2. When registering the application details, set the "Authorization callback domain" to: http://localhost:5005/oauth2_callback
(that's where the ASP.NET controller will be running on the local machine)
3. Download the repository and replace client id & secret in the PolarAuthenticationController
4. Start the project, the browser will open and automatically star the process.
5. If you configured everything correctly, the controller will display the response from the Polar Authentication Server => save this information in a secure location and use it to access the API
6. Hint: if you get an Unauthorized response while trying to access user data, make sure that you initially registered the user: https://www.polar.com/accesslink-api/#users (Post Request with a random/custom user ID and the bearer token received from the authentication process)