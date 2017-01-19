using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TraefikCone
{
	/// <summary>
	/// This class is responsible for creating a structure which represents a deployment. 
	/// This is one of multiple services, designed for a pipeline, so that a kubernetes cluster namespace can be spun up for testing with services/ingress/pods or replicationcontroller.
	/// </summary>
	public class DeploymentGenerator
	{
		/// <summary>
		/// Creates an ingress rule for a given pipeline.
		/// Choices that have been made implicitly, are that træfik is running, and that the service is tested in 
		/// context of its correct placement (ie access goes through root on / )
		/// </summary>
		/// <param name="path">Path to save file</param>
		/// <param name="name">Ingress name</param>
		public List<string> CreateDeployment(string name, string deploymentPort, string replicas, string image)
		{
			#region Conventions for service - this has a dependency in kubernetes to the ingress!
			string deploymentName = string.Format("{0}-deploy", name);
			string serviceName = string.Format("{0}-svc", name);
			
			#endregion

			List<string> deployment = new List<string>();
			deployment = WriteDeploymentMetadata(deployment, deploymentName);
			deployment = WriteDeploymentSpec(deployment, deploymentPort, name, replicas, image);

			return deployment;
		}

		/// <summary>
		/// Writes in this format to file: 
		/// 
		/// apiVersion: extensions/v1beta1
		/// kind: Deployment
		/// metadata:
		///   name: deployment-name
		///   
		/// </summary>
		/// <param name="file">The structure of the file to write to, in this context its an service yml.</param>
		/// <param name="name">Name for the deployment in the kubernetes cluster</param>
		List<string> WriteDeploymentMetadata(List<string> file, string name)
		{
			SharedMethods indent = new SharedMethods();
			file.Add("apiVersion: extensions/v1beta1");
			file.Add("kind: Deployment");
			file.Add("metadata:");
			file.Add(indent.FixIndention(1, string.Format("name: {0}", name)));
			return file;
		}

		/// <summary>
		/// Writes in this format to file: 
		/// 
		/// spec:
		///   replicas: 3
		///   template:
		///     metadata:
		///       labels:
		///         run: nginx
		///    	spec:
		///    	  containers:
		///    	  - name: nginx
		///    	    image: user/image:tag
		///    	    ports:
		///    		- containerPort: 80 
		///     		
		/// </summary>
		/// <param name="file">The structure of the file to write to, in this context its an service yml.</param>
		/// <param name="targetPort">The pod exposed on the deployed pods.</param>
		/// <param name="deploymentName">Name for the deployment in kubernetes.</param>
		/// <param name="replicas">Amount of replicas to produce</param>
		/// <param name="image">Image from hub, in the format of user/image:tag </param>
		List<string> WriteDeploymentSpec(List<string> file, string targetPortstring, string name, string replicas, string image)
		{
			SharedMethods indent = new SharedMethods();
			file.Add("spec:");
			file.Add(indent.FixIndention(1, string.Format("replicas: {0}", replicas))); 
			file.Add(indent.FixIndention(1, "template:"));
			file.Add(indent.FixIndention(2, "metadata:"));
			file.Add(indent.FixIndention(3, "labels:"));
			file.Add(indent.FixIndention(4, string.Format("run: {0}", name)));
			file.Add(indent.FixIndention(2, "spec:"));
			file.Add(indent.FixIndention(3, "containers:"));
			file.Add(indent.FixIndention(3, string.Format("- name: {0}", name)));
			file.Add(indent.FixIndention(4, string.Format("image: {0}", image)));
			file.Add(indent.FixIndention(4, "ports:"));
			file.Add(indent.FixIndention(4, string.Format("- containerPort: {0}", targetPortstring)));
			return file;
		}
	}
}