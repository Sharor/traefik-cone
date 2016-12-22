using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TraefikCone
{
    public class IngressGenerator
    {
		/// <summary>
		/// Ensures proper indention of methods in the yml. 
		/// </summary>
		/// <param name="indent">indent is the amount of indention. '1' gives 2 spaces, '2' gives 4 spaces and so on.</param>
		/// <param name="message">message is the string after the indention.</param>
		/// <returns></returns>
		public string FixIndention(int indent, string message)
		{
			string spaceIndent = "";
			for (int i = 0; i < indent; i++)
			{
				spaceIndent += "  ";
			}

			return string.Format("{0}{1}", spaceIndent, message);
		}

		/// <summary>
		/// Creates an ingress rule for a given pipeline.
		/// Choices that have been made implicitly, are that træfik is running, and that the service is tested in 
		/// context of its correct placement (ie access goes through root on / )
		/// </summary>
		/// <param name="path">Path to save file</param>
		/// <param name="name">Ingress name</param>
		public void CreateIngress(string path, string name)
		{
			#region Conventions for ingress
			string ingressName = string.Format("{0}-ing", name);
			string host = string.Format("{0}.local", name);
			string svcName = string.Format("{0}-svc", name); 
			#endregion

			Stream fileStream = File.Create(path);
			using (StreamWriter file = new StreamWriter(fileStream))
			{
				WriteIngressMetadata(file, ingressName);
				WriteIngressSpec(file, host, svcName); 

			}
		}

		/// <summary>
		/// Writes in this format to file: 
		/// apiVersion: extensions/v1beta1
		/// kind: Ingress
		/// metadata:
		///   name: ingress-name
		/// </summary>
		/// <param name="file">The file being written to, in this context its an ingress yml.</param>
		/// <param name="ingressName">Name for the ingress in the kubernetes cluster</param>
		void WriteIngressMetadata(StreamWriter file, string ingressName)
		{
			file.WriteLine("apiVersion: extensions / v1beta1");
			file.WriteLine("kind: Ingress");
			file.WriteLine("metadata:");
			file.WriteLine(FixIndention(1, string.Format("name: {0}", ingressName)));
		}

		/// <summary>
		/// Writes in this format to file: 
		/// spec:
		///   rules:
		///   - host: host-name
		/// 	http:
		/// 	  paths:
		///       - path: /
		///         backend:
		///           serviceName: name-svc
		/// 		  servicePort: 80
		/// </summary>
		/// <param name="file"></param>
		/// <param name="hostName"></param>
		/// <param name="serviceName"></param>
		void WriteIngressSpec(StreamWriter file, string hostName, string serviceName)
		{
			file.WriteLine("spec:");
			file.WriteLine(FixIndention(1, "rules:"));
			file.WriteLine(FixIndention(1, string.Format("- host: {0}", hostName)));
			file.WriteLine(FixIndention(2, "http:"));
			file.WriteLine(FixIndention(3, "paths:"));
			file.WriteLine(FixIndention(3, "- path: /"));
			file.WriteLine(FixIndention(4, "backend:"));
			file.WriteLine(FixIndention(5, string.Format("serviceName: {0}", serviceName)));
			file.WriteLine(FixIndention(5, "servicePort: 80"));
		}
	}
}