using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowNetworkToolKit.Core.Base.Algorithm;
using FlowNetworkToolKit.Core.Base.Network;

namespace FlowNetworkToolKit.Core.Utils.Visualizer
{
    public class Animator
    {
        public int Delay = 50; //Delay between animation steps in ms
        public bool AlgorithmFinished = false;
        private Timer AnimationStepTimer;

        private Queue<Animation> Animations = new Queue<Animation>();

        public delegate void AnimationTick(Animator sender);
        public delegate void AnimationFinished(Animator sender);
        public delegate void AnimationStarted(Animator sender);

        public event AnimationTick OnAnimationTick;
        public event AnimationFinished OnAnimationFinished;
        public event AnimationStarted OnAnimationStarted;

        public Animation GetAnimation()
        {
            if (Animations.Count > 0)
            {
                return Animations.Dequeue();
            }
            else
            {
                if (AlgorithmFinished)
                {
                    Stop();
                }
                return null;
            }

        }

        private void Stop()
        {
            AnimationStepTimer.Enabled = false;
            OnAnimationFinished?.Invoke(this);
            Runtime.StopAnimation = false;
        }

        private void AddAnimation(Animation animation)
        {
            Animations.Enqueue(animation);
        }

        public void EdgeMarked(BaseMaxFlowAlgorithm algorithm, FlowNetwork network, FlowEdge edge)
        {
            AddAnimation(new Animation(edge, AnimationType.EdgeMarked));
        }

        public void EdgeUnmarked(BaseMaxFlowAlgorithm algorithm, FlowNetwork network, FlowEdge edge)
        {
            AddAnimation(new Animation(edge, AnimationType.EdgeUnmarked));
        }

        public void EdgeFlowChanged(BaseMaxFlowAlgorithm algorithm, FlowNetwork network, FlowEdge edge)
        {
            AddAnimation(new Animation(edge, AnimationType.EdgeFlowChanged));
        }

        public void Run()
        {
            AnimationStepTimer = new Timer { Interval = Delay};
            AnimationStepTimer.Tick += (sender, args) =>
            {
                if (Runtime.StopAnimation)
                {
                    Stop();
                    return;
                }
                OnAnimationTick?.Invoke(this);
            };
            AnimationStepTimer.Start();
            OnAnimationStarted?.Invoke(this);
        }

        public void Reset()
        {
            Animations.Clear();
            AnimationStepTimer = null;
            OnAnimationTick = null;
        }

    }
}
