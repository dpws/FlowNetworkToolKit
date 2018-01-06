using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowNetworkToolKit.Core.Utils.Visualizer
{
    public static class Animator
    {
        private static Queue<Animation> Animations = new Queue<Animation>();

        public static Animation GetAnimation()
        {
            return Animations.Dequeue();
        }

        public static void AddAnimation(Animation animation)
        {
            Animations.Enqueue(animation);
        }

    }
}
