Some notes:

1. There is only basic logging. On real production service detailed logged 
   are mandatory and a proper logger should be used.
2. Only Basic error handling implemented.
2. The service should include healthcheck Ep and broadcast metric for monitoring.
3. Only one unit test implemeted for type checking, but on real world more tests should be written
4. Models definitions store is implemented by a micro service in memory cache.
   Production solution will need to support high scale and low latency solution such as redis,
   backed up by a persistent DB for reload, for example. 
   Depending on the size of the data and system architecture, maybe a dedicated micro service should
   be considered for central models management, allowing each micro service getting the models
   it needs and store them in an in memory cache with an option to update on demand.

 Smaple valid response:

{
  "status": true,
  "abnormal_fields": []
}

Sample invalid response:

{
  "status": false,
  "abnormal_fields": [
    {
      "field": "Authorization",
      "section": "headers",
      "reason": "Type mismatch",
      "provided_value": "Bearer 56ee9b7a-da8e45a1aadea57761b912c4",
      "expected_types": [
        "Auth-Token"
      ]
    }
  ]
}

{
  "status": false,
  "abnormal_fields": [
    {
      "field": "endpoint",
      "section": "general",
      "reason": "No definition found for this endpoint and method combination",
      "provided_value": "GET /users/check",
      "expected_types": []
    }
  ]
}