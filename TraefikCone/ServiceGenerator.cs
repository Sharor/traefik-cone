using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TraefikCone
{
	/// <summary>
	/// This class is responsible for creating a structure which represents a service. 
	/// This is one of multiple services, designed for a pipeline, so that a kubernetes cluster namespace can be spun up for testing with services/ingress/pods or replicationcontroller.
	/// </summary>
	public class ServiceGenerator
	{
		/// <summary>
		/// Creates an ingress rule for a given pipeline.
		/// Choices that have been made implicitly, are that træfik is running, and that the service is tested in 
		/// context of its correct placement (ie access goes through root on / )
		/// </summary>
		/// <param name="path">Path to save file</param>
		/// <param name="name">Ingress name</param>
		public List<string> CreateService(string name, string deploymentPort)
		{
			#region Conventions for service - this has a dependency in kubernetes to the ingress!
			string serviceName = string.Format("{0}-svc", name);
			string deploymentName = string.Format("{0}-deploy", name);
			#endregion

			List<string> service = new List<string>();
			service = WriteServiceMetadata(service, serviceName);
			service = WriteServiceSpec(service, deploymentPort, deploymentName);

			return service;
		}

		/// <summary>
		/// Writes in this format to file: 
		/// 
		/// apiVersion: v1
		/// kind: Service
		/// metadata:
		///   name: service-name
		///   
		/// </summary>
		/// <param name="file">The structure of the file to write to, in this context its an service yml.</param>
		/// <param name="serviceName">Name for the service in the kubernetes cluster</param>
		List<string> WriteServiceMetadata(List<string> file, string serviceName)
		{
			SharedMethods indent = new SharedMethods();
			file.Add("apiVersion: v1");
			file.Add("kind: Service");
			file.Add("metadata:");
			file.Add(indent.FixIndention(1, string.Format("name: {0}", serviceName)));
			return file;
		}

		/// <summary>
		/// Writes in this format to file: 
		/// 
		/// spec:
		///   ports:
		///     - port: 80
		///     targetPort: 80
		///   selector:
		///     run: deployment-name
		///   type: ClusterIP
		///  
		/// Note that the targetPort can be any chosen port, it just needs to map to the pod exposed port.
		/// </summary>
		/// <param name="file">The structure of the file to write to, in this context its an service yml.</param>
		/// <param name="targetPort">The pod exposed on the deployed pods.</param>
		/// <param name="deploymentName">Name for the deployment in kubernetes.</param>
		List<string> WriteServiceSpec(List<string> file, string targetPort, string deploymentName)
		{
			SharedMethods indent = new SharedMethods();
			file.Add("spec:");
			file.Add(indent.FixIndention(1, "ports:"));
			file.Add(indent.FixIndention(2, "- port: 80"));
			file.Add(indent.FixIndention(2, string.Format("targetPort: {0}", targetPort)));
			file.Add(indent.FixIndention(1, "selector:"));
			file.Add(indent.FixIndention(2, string.Format("run: {0}", deploymentName)));
			file.Add(indent.FixIndention(1, "type: ClusterIP"));
			return file;
		}
	}
}