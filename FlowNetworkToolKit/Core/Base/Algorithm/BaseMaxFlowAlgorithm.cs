using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FlowNetworkToolKit.Core.Base.Exceptions;
using FlowNetworkToolKit.Core.Base.Network;
using FlowNetworkToolKit.Core.Utils.Logger;

namespace FlowNetworkToolKit.Core.Base.Algorithm
{
    

    public abstract class BaseMaxFlowAlgorithm : ICloneable, IStatable
    {

        protected string Name;
        protected string Description;
        protected string Url;

        public TimeSpan Elapsed { get; protected set; }
        public int Ticks { get; private set; } = 0;
        protected FlowNetwork graph;

        public double MaxFlow { get; protected set; } = Double.Epsilon;

        
        #region Events

        public delegate void TicksChanged(BaseMaxFlowAlgorithm sender, int Ticks);
        public delegate void Start(BaseMaxFlowAlgorithm sender);
        public delegate void Finish(BaseMaxFlowAlgorithm sender);
        public delegate void BeforeInit(BaseMaxFlowAlgorithm sender);
        public delegate void AfterInit(BaseMaxFlowAlgorithm sender);

        public event TicksChanged OnTick;
        public event Start OnStart;
        public event Finish OnFinish;
        public event BeforeInit OnBeforeInit;
        public event AfterInit OnAfterInit;

        #endregion

        #region Helpers

        protected void Tick()
        {
            Ticks++;
            OnTick?.Invoke(this, Ticks);
        }

        #endregion

        public BaseMaxFlowAlgorithm()
        {
            
        }

        public virtual void SetGraph(FlowNetwork g)
        {
            graph = new FlowNetwork(g);
        }

        protected abstract void Init();

        protected abstract void Logic();

        public void Run()
        {
            try
            {
                Reset();
                var errors = graph.Validate();
                if (errors.Count > 0)
                    throw new FLowNetworkValidationException(errors);
                if (graph == null)
                {
                    throw new InvalidConfigurationException("Can't run algorithm without graph.");
                }
                OnBeforeInit?.Invoke(this);
                Init();
                OnAfterInit?.Invoke(this);
                var timer = new Stopwatch();
                OnStart?.Invoke(this);
                timer.Start();
                Logic();
                timer.Stop();
                Elapsed = timer.Elapsed;
                Log.Write($"Algorithm {Name}. Max flow: {MaxFlow}. From: {graph.Source}, To: {graph.Target}, Time: {Elapsed}. Ticks: {Ticks}");
                OnFinish?.Invoke(this);
            }
            catch (Exception e)
            {
                Log.Write(e.Message, Log.ERROR);
            }
            
        }

        public void RunAsync()
        {
            var thread = new Thread(Run);
            thread.Start();
        }

        public virtual void Reset()
        {
            MaxFlow = Double.Epsilon;
            Ticks = 0;
            Elapsed = new TimeSpan(0);
            foreach (var edge in graph.Edges)
            {
                edge.Flow = 0;
            }
        }

        #region Information

        public string GetName()
        {
            if (Name != null)
                return Name;
            else
                throw new NotImplementedException("Specify algorithm's name");
        }

        public string GetDescription()
        {
            if (Description != null)
                return Description;
            else
                throw new NotImplementedException("Specify algorithm's description");
        }

        public string GetUrl()
        {
            if (Url != null)
                return Url;
            else
                throw new NotImplementedException("Specify algorithm's url");
        }

        #endregion


        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public virtual string GetStats()
        {
            throw new NotImplementedException();
        }
    }
}
