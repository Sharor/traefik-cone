using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TraefikCone
{
	/// <summary>
	/// This class is responsible for writing out a file that represents an ingress in a træfik cluster, when prompted from a pipeline. 
	/// </summary>
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
		/// Loops through file structure and writes out each line to a physical file. 
		/// </summary>
		/// <param name="fileStructure">List of strings, each list element represents a line in the file.</param>
		/// <param name="path">The path to save the file.</param>
		public void WriteOut(List<string> fileStructure, string path)
		{

			Stream fileStream = File.Create(path);
			using (StreamWriter file = new StreamWriter(fileStream))
			{
				foreach (string line in fileStructure)
					file.WriteLine(line); 
			}
		}

		/// <summary>
		/// Creates an ingress rule for a given pipeline.
		/// Choices that have been made implicitly, are that træfik is running, and that the service is tested in 
		/// context of its correct placement (ie access goes through root on / )
		/// </summary>
		/// <param name="path">Path to save file</param>
		/// <param name="name">Ingress name</param>
		public List<string> CreateIngress(string name)
		{
			#region Conventions for ingress
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
		/// apiVersion: extensions/v1beta1
		/// kind: Ingress
		/// metadata:
		///   name: ingress-name
		/// </summary>
		/// <param name="file">The structure of the file to write to, in this context its an ingress yml.</param>
		/// <param name="ingressName">Name for the ingress in the kubernetes cluster</param>
		List<string> WriteIngressMetadata(List<string> file, string ingressName)
		{
			file.Add("apiVersion: extensions/v1beta1");
			file.Add("kind: Ingress");
			file.Add("metadata:");
			file.Add(FixIndention(1, string.Format("name: {0}", ingressName)));
			return file; 
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
		List<string> WriteIngressSpec(List<string> file, string hostName, string serviceName)
		{
			file.Add("spec:");
			file.Add(FixIndention(1, "rules:"));
			file.Add(FixIndention(1, string.Format("- host: {0}", hostName)));
			file.Add(FixIndention(2, "http:"));
			file.Add(FixIndention(3, "paths:"));
			file.Add(FixIndention(3, "- path: /"));
			file.Add(FixIndention(4, "backend:"));
			file.Add(FixIndention(5, string.Format("serviceName: {0}", serviceName)));
			file.Add(FixIndention(5, "servicePort: 80"));
			return file; 
		}
	}
}