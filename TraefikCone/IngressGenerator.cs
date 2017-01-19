using System.Collections.Generic;

namespace TraefikCone
{
	/// <summary>
	/// This class is responsible for creating a structure which represents an ingress in a træfik cluster. 
	/// This is one of multiple services, designed for a pipeline, so that a kubernetes cluster namespace can be spun up for testing with services/ingress/pods or replicationcontroller.
	/// </summary>
	public class IngressGenerator
	{
		/// <summary>
		/// Creates an ingress rule for a given pipeline.
		/// Choices that have been made implicitly, are that træfik is running, and that the service is tested in 
		/// context of its correct placement (ie access goes through root on / )
		/// </summary>
		/// <param name="path">Path to save file</param>
		/// <param name="name">Ingress name</param>
		public List<string> CreateIngress(string name)
		{
			#region Conventions for ingress - kept to allow change of convention easily ( has a dependency in kubernetes to the service!)
			string ingressName = string.Format("{0}-ing", name);
			string host = string.Format("{0}.local", name);
			string svcName = string.Format("{0}-svc", name);
			#endregion

			List<string> ingress = new List<string>();
			ingress = WriteIngressMetadata(ingress, ingressName);
			ingress = WriteIngressSpec(ingress, host, svcName);

			return ingress;
		}

		/// <summary>
		/// Writes in this format to file: 
		/// 
		/// apiVersion: extensions/v1beta1
		/// kind: Ingress
		/// metadata:
		///   name: ingress-name
		///   
		/// </summary>
		/// <param name="file">The structure of the file to write to, in this context its an ingress yml.</param>
		/// <param name="ingressName">Name for the ingress in the kubernetes cluster</param>
		List<string> WriteIngressMetadata(List<string> file, string ingressName)
		{
			SharedMethods indent = new SharedMethods(); 
			file.Add("apiVersion: extensions/v1beta1");
			file.Add("kind: Ingress");
			file.Add("metadata:");
			file.Add(indent.FixIndention(1, string.Format("name: {0}", ingressName)));
			return file;
		}

		/// <summary>
		/// Writes in this format to file: 
		/// 
		/// spec:
		///   rules:
		///   - host: host-name
		/// 	http:
		/// 	  paths:
		///       - path: /
		///         backend:
		///           serviceName: name-svc
		/// 		  servicePort: 80
		/// 		  
		/// </summary>
		/// <param name="file">The structure of the file to write to, in this context its an ingress yml.</param>
		/// <param name="hostName">The address to access the cluster on, here 'name'.local.</param>
		/// <param name="serviceName">The name of the service. This is directly dependant on the ServiceGenerator class.</param>
		List<string> WriteIngressSpec(List<string> file, string hostName, string serviceName)
		{
			SharedMethods indent = new SharedMethods();
			file.Add("spec:");
			file.Add(indent.FixIndention(1, "rules:"));
			file.Add(indent.FixIndention(1, string.Format("- host: {0}", hostName)));
			file.Add(indent.FixIndention(2, "http:"));
			file.Add(indent.FixIndention(3, "paths:"));
			file.Add(indent.FixIndention(3, "- path: /"));
			file.Add(indent.FixIndention(4, "backend:"));
			file.Add(indent.FixIndention(5, string.Format("serviceName: {0}", serviceName)));
			file.Add(indent.FixIndention(5, "servicePort: 80"));
			return file;
		}
	}
}