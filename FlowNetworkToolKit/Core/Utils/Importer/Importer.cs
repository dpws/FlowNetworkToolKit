using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowNetworkToolKit.Core.Base.Network;
using FlowNetworkToolKit.Core.Utils.Logger;
using Microsoft.VisualBasic.FileIO;

namespace FlowNetworkToolKit.Core.Utils.Importer
{
    class Importer
    {

        public Importer()
        {
            
        }

        public FlowNetwork Import(FileInfo file)
        {
            try
            {
                switch (file.Extension)
                {
                    case ".csv":
                        return FromCSV(file);
                    case ".fn":
                        return FromFn(file);
                    case ".dimacs":
                        return FromDimac(file);
                }
                    
            }
            catch (Exception e)
            {
                Log.Write(e.Message, Log.ERROR);
            }
            return null;
        }

        private FlowNetwork FromFn(FileInfo file)
        {
            FlowNetwork g = new FlowNetwork();
            int source = -1, target = -1;
            try
            {
                System.IO.StreamReader fs =
                    new System.IO.StreamReader(file.FullName);
                string line;
                while ((line = fs.ReadLine()) != null)
                {
                    if (line.StartsWith("SOURCE"))
                    {
                        var s = line.Split(':');
                        source = int.Parse(s[1]) - 1;
                    }
                    if (line.StartsWith("TARGET"))
                    {
                        var s = line.Split(':');
                        target = int.Parse(s[1]) - 1;
                    }
                    if (char.IsLetter(line[0]))
                        continue;
                    var d = line.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                    int n0 = int.Parse(d[0]) - 1; // My numbering starts with 0, not 1
                    if (n0 == -2)
                        break;
                    int n1 = int.Parse(d[1]) - 1; // My numbering starts with 0, not 1
                    int w = int.Parse(d[2]); // weight
                    g.AddEdge(n0, n1, w);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message, Log.ERROR);
            }

            g.Source = source;
            g.Target = target;
            var validation = g.Validate();
            if (validation.Count > 0)
            {
                foreach (var error in validation)
                {
                    Log.Write(error, Log.ERROR);
                }
                return null;
            }
            return g;
        }

        private FlowNetwork FromCSV(FileInfo file)
        {
            FlowNetwork g = new FlowNetwork();
            int source = -1, target = -1;
            using (TextFieldParser parser = new TextFieldParser(file.FullName))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                int line = 0;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    line++;
                    if (line == 1)
                    {
                        //skip header
                        continue;
                    }
                    
                    if (line == 2)
                    {
                        int.TryParse(fields[0], out source);
                        int.TryParse(fields[1], out target);
                        //parse source and target
                        continue;
                    }
                    //Process row
                    
                    int from, to;
                    double capacity;
                    int.TryParse(fields[0], out from);
                    int.TryParse(fields[1], out to);
                    double.TryParse(fields[2], out capacity);
                    g.AddEdge(from, to, capacity);
                }
            }
            g.Source = source;
            g.Target = target;
            var validation = g.Validate();
            if (validation.Count > 0)
            {
                foreach (var error in validation)
                {
                    Log.Write(error, Log.ERROR);
                }
                return null;
            }
            return g;
        }

        private FlowNetwork FromDimac(FileInfo file)
        {
            FlowNetwork g = new FlowNetwork();
            var srs = -1;
            var trg = -1;
            try
            {
                System.IO.StreamReader fs =
                    new System.IO.StreamReader(file.FullName);
                string line;
                while ((line = fs.ReadLine()) != null)
                {
                    if (line.EndsWith("s"))
                    {
                        var s = line.Split(' ');
                        srs = int.Parse(s[1])-1;
                    }
                    if (line.EndsWith("t"))
                    {
                        var t = line.Split(' ');
                        trg = int.Parse(t[1])-1;
                    }
                    if (line.StartsWith("a"))
                    {
                        var a = line.Split(' ');
                        int from = int.Parse(a[1])-1; 

                        int to = int.Parse(a[2])-1; 
                        int weight = int.Parse(a[3]); 
                        g.AddEdge(from, to, weight);
                    }


                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message, Log.ERROR);
            }
            g.Source = srs;
            g.Target = trg;
            var validation = g.Validate();
            if (validation.Count > 0)
            {
                foreach (var error in validation)
                {
                    Log.Write(error, Log.ERROR);
                }
                return null;
            }
            return g;
        }
    }
}
