# Pokedex

## Instructions

1. Install [git](https://git-scm.com)
2. Install [docker desktop](https://www.docker.com/products/docker-desktop)
3. To download the repository, execute `git clone https://github.com/prasadtad/Pokedex.git`
3. To build the docker image, execute `docker build -t prasadtad/pokedex .`
4. To run the application container, execute `docker run -p 80:80 prasadtad/pokedex`

## Production instructions

We want to run the application securely over port 443 in production. The steps involved are 
* Follow the [instructions](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-5.0&tabs=visual-studio) to configure the dotnet application for https
* Get a certificate for the domain on which the app will run
* Follow the [instructions](https://docs.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-5.0) to configure the docker file to use the certificate 

Other things to consider are putting an api gateway for security using an api key, rate limiting or geographic limiting for public apis and a cdn for serving frequent requests. If we want to make the app more available, we can setup a kubernetes cluster with ingress and load balancing to run the app over multiple pods. This would also allow us to do rolling updates. We definitely want to use a continuous integration/continuous deployment system to run the tests and then deploy the application to a uat environment that can then be promoted to the production environment.

