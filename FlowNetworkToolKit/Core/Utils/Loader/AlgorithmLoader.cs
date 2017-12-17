using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FlowNetworkToolKit.Core.Base.Algorithm;
using FlowNetworkToolKit.Core.Utils.Logger;

namespace FlowNetworkToolKit.Core.Utils.Loader
{
    class AlgorithmLoader
    {
        public AlgorithmLoader()
        {
        }
        public List<AlgorithmInfo> LoadFromSource(DirectoryInfo directory)
        {
            var list = new List<AlgorithmInfo>();
            Log.Write($"Loading algorithms files from {directory.FullName}");
            foreach (FileInfo file in directory.GetFiles("*.cs"))
            {
                Log.Write($"Found file {file.Name}");
                var asm = CompileExecutable(file);
                if (asm != null)
                {
                    foreach (var type in asm.GetTypes())
                    {
                        if (type.IsSubclassOf(typeof(BaseMaxFlowAlgorithm)))
                        {
                            BaseMaxFlowAlgorithm var = (BaseMaxFlowAlgorithm)asm.CreateInstance(type.FullName);
                            Log.Write($"  Found alghoritm class {type.Name}: {var.GetName()}");
                            list.Add(new AlgorithmInfo(var.GetName(), var.GetDescription(), var.GetUrl(), type.FullName, var));
                        }
                    }
                }

            }
            return list;
        }

        public List<AlgorithmInfo> LoadFromNamespace(string assemblyNamespace, string assemblyFile = null)
        {
            var list = new List<AlgorithmInfo>();
            Assembly asm;

            if (assemblyFile == null)
            {
                asm = Assembly.GetCallingAssembly();
                Log.Write($"Searching alghoritms in namespace {assemblyNamespace} from {asm.EscapedCodeBase}.");
            }
            else
            {
                asm = Assembly.LoadFrom(assemblyFile);
                Log.Write($"Searching alghoritms in namespace {assemblyNamespace} from {assemblyFile}.");
            }
            Type[] typelist = asm.GetTypes();

            foreach (Type type in typelist)
            {
                try
                {
                    if (type.Namespace == assemblyNamespace && type.IsSubclassOf(typeof(BaseMaxFlowAlgorithm)))
                    {
                        BaseMaxFlowAlgorithm var = (BaseMaxFlowAlgorithm)asm.CreateInstance(type.FullName);
                        Log.Write($"  Found alghoritm class {type.Name}: {var.GetName()}");
                        list.Add(new AlgorithmInfo(var.GetName(), var.GetDescription(), var.GetUrl(), type.FullName, var));
                    }
                }
                catch (Exception e)
                {
                    Log.Write($"  {e.Message}", Log.ERROR);
                }
            }
            return list;
        }

        private Assembly CompileExecutable(FileInfo sourceFile)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters cp = new CompilerParameters();

            cp.GenerateInMemory = true;
            cp.TreatWarningsAsErrors = false;

            cp.ReferencedAssemblies.Add("System.Core.dll");
            cp.ReferencedAssemblies.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GraphToolKit.Core.dll"));
            cp.ReferencedAssemblies.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GraphToolKit.Alghoritms.dll"));


            var cr = provider.CompileAssemblyFromFile(cp, sourceFile.FullName);
            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                Log.Write($"Errors building {sourceFile.FullName} into {cr.PathToAssembly}", Log.ERROR);
                foreach (CompilerError ce in cr.Errors)
                {
                    Log.Write($"  {ce}", Log.ERROR);
                }
                return null;
            }
            return cr.CompiledAssembly;


        }
    }
}
