# BPDTS Test API

An API built for the BPDTS Test. This task was completed by Richard Meara.

## Tools and Environments used

The language used for the task was C#, and the framework was .NET Core 3.1.

The IDE used was Visual Studio 2019.

The build and releases were produced using Azure DevOps, with the source code hosted by GitHub.

## Building, testing and deploying

The .NET Core solution contains 2 projects: An API and a test project. The test project contains basic functional tests to ensure the controller logic is correct. It does not use the live API, and instead uses fake data.

The build and test definition is in the file at the root directory:

```
azure-pipelines.yml
```
The build uses the .NET SDK, restores the solution, builds it in a deployable package, and then tests the binaries. Finally it uploads the produced deployable package as an artifact.

A release has been produced that will deploy to a free tier app service created by me on Azure. The link to which is below:

https://bpdts-test-api-richard-meara.azurewebsites.net

Below is a status badge for the build of the master branch. The project is public for the purpose of being transparent.

[![Build Status](https://dev.azure.com/richardmeara/BPDTS%20Test%20API/_apis/build/status/richardmeara.bpdts-test-api?branchName=master)](https://dev.azure.com/richardmeara/BPDTS%20Test%20API/_build/latest?definitionId=9&branchName=master)

## Using the API

As mentioned above, the API has been hosted on a free tier. There are 3 API methods written to satisfy the test.

/TestAnswer/users/london - This will provide a complete list of users who live in London and who are within 60 miles of the central London location.

/TestAnswer/users/london/citynameonly - This will provide a list of users who live in London by using the API call provided: 

```
/city/{city}/users
```

/TestAnswer/users/london/coordinatesonly - This will provide a list of users who are within 60 miles of the central London location.

This has been done by using the API method:


```
/users
```

Once a JSON list of users has been received, they are parsed into a list of objects known as User.cs. From here they are looped over, and those with coordinates within the bounds of 60 miles are added to a separate list. This new list is then returned by my API.

The calculation done first checked that the longitude and latitude coordinates are both valid, then uses the GeoCoordinate class to parse the coordinates and then use the GetDistanceTo method to calculate the distance in metres. This is then converted to miles and compared against the 60 mile limit.

The GeoCoordinate class is inherited from the publicly available NuGet package GeoCoordinate.NetCore by cormaltes.
The NuGet feed can be found below:
https://www.nuget.org/packages/GeoCoordinate.NetCore/1.0.0.1?_src=template

## Scaling the system and deploying in different approaches

The API is currently running in a free tier App Service plan and therefore cannot be scaled out properly. However with an upgraded plan, autoscaling could be used which would automatically spin up new App Services on demand depending on current load on the server. 

Depending on the traffic and what the API would be processing, the service could be scaled out to handle more requests quickly, or scaled up to increase the processing power of a single server.

For a more manual version which would grant more control, the app could be hosted via Azure or AWS.

On AWS, a clean way would be to use AWS Route53 which would point to a Elastic Load Balancer. This load balancer would have multiple EC2 instances in a target group which the app would be deployed to. This is a more complex method, as autoscaling is very simple.

A final method would be to use Docker. The API could very easily be containerised and hosted using Docker Swarm, AWS ECS or Azure Kubernetes Service. I am personally learning the tool in my spare time to benefit myself and my current company. I considered adding a docker-compose file so that the api could be hosted locally, however as per the original email, I didn't want to spend more time on the task.

## Securing the API

Currently there is little security from the API. I am not using any monitoring tools, however I am ensuring the app is HTTPS only.

If using Azure, Application Insights could be attached to the code so that the performance can be monitored and automatically detect threats. Other tools such as NewRelic and DataDog could be used to automatically detect threats. Other tools like CloudFlare could also be used to prevent DDOS attacks.

If the API was allowed to accept user input, then I would sanitise the request. For example, in the test API there is a /city/{city}/users call. I would ensure that the supplied city name was valid and did not contain any malicious characters or symbols. This would be even more important if the API allowed POST requests and allowed the user to add data.

Currently, the API is hosted using Azure without using my own domain name, and therefore has HTTPS enabled for free, using Azure's own SSL Cert. The app has been configured to not allow HTTP and will redirect to HTTPS.

## Note from me

Thank you for the opportunity to complete this task. I have a strong passion for development, devops processes and communication. I am happy to accept any criticism and am looking forward to hearing back from you. 
