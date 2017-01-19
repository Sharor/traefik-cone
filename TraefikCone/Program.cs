﻿using System;
using System.IO;
using TraefikCone;

namespace ConsoleApplication
{
    public class Program
    {
		public static void Main(string[] args)
		{
			string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
			SharedMethods access = new SharedMethods();

			Console.WriteLine("This is intended as a generator for kubernetes pipeline deployment.");
			Console.WriteLine("Author and maintainer: David Johannes Christensen");
			Console.WriteLine();
			Console.WriteLine();

			#region Properties
			Console.WriteLine(string.Format("The generator assumes you put a 'properties.yml' file in the rootfolder of this project: {0}", path));
			string[] properties = File.ReadAllLines(string.Format("{0}/{1}", path, "properties.yml"));
			string name = properties[0];
			string port = properties[1];
			string replicas = properties[2];
			string image = properties[3]; 			
			Console.WriteLine();
			Console.WriteLine();
			#endregion

			#region Ingress
			string ing = string.Format("{0}-ing.yml", name);
			Console.WriteLine(string.Format("Generating ingress - {0}", ing));
			IngressGenerator ingress = new IngressGenerator();
			access.WriteOut(ingress.CreateIngress(name), string.Format("{0}/{1}", path, ing));
			Console.WriteLine();
			#endregion

			#region Service
			string svc = string.Format("{0}-svc.yml", name);
			Console.WriteLine(string.Format("Generating service - {0}", svc));
			ServiceGenerator service = new ServiceGenerator();
			access.WriteOut(service.CreateService(name, port), string.Format("{0}/{1}", path, svc));
			Console.WriteLine();
			#endregion

			#region Deployment
			string deploy = string.Format("{0}-deploy.yml", name);
			Console.WriteLine(string.Format("Generating deployment - {0}", deploy));
			DeploymentGenerator deployment = new DeploymentGenerator();
			access.WriteOut(deployment.CreateDeployment(name, port, replicas, image), string.Format("{0}/{1}", path, deploy));
			Console.WriteLine();
			#endregion

			Console.WriteLine("Finished generation, good testing!");
		}
	}
}
