# traefik-cone
This whole project is intended as a tool for continuous integration/delivery pipelines, by generating a service, ingress and deployment file for a kubernetes cluster.
The project is highly inspired by [Træfik.io](traefik.io), and assumes that your kubernetes cluster can make use of ingress rules. 

The vision is to give a given pipeline the tool to automate deployment into a kubernetes cluster, which in turn allows end to end testing for an app without having to worry about running commands directly on the kubernetes cluster. 
An advantage of this approach is that the .yml files are kept, which serves as documentation to how the deployment happened. 

## Using the Træfic-cone
Simply make a 'properties.yml' file in the root of the project and run the project. 
It needs the following four parameters: 
```properties.yml
name: your-app-name
port: the-port-your-app-exposes
replicas: replicas-in-kubernetes
image: image-pulled-by-kubernetes
``` 

### Dependencies
It depends on dotnet core libraries.
