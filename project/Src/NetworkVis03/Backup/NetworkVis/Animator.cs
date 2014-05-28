using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace NetworkVis
{
    public class Animator
    {
        #region Tween Helpers


        public static AnimationClock AnimatePenner(
            DependencyObject element,
            DependencyProperty prop,
            Tween.Equations type,
            double to,
            int durationMS,
            EventHandler callbackFunc,
            EventHandler durationFunc
            )
        {
            return AnimatePenner(element, prop, type, null, to, durationMS, callbackFunc, durationFunc);
        }

        public static AnimationClock AnimatePenner(
            DependencyObject element,
            DependencyProperty prop,
            Tween.Equations type,
            double? from,
            double to,
            int durationMS,
            EventHandler callbackFunc,
            EventHandler durationFunc)
        {
            double defaultFrom = double.IsNaN((double)element.GetValue(prop)) ?
                                 0 :
                                 (double)element.GetValue(prop);

            Tween anim = new Tween(type, from.GetValueOrDefault(defaultFrom), to);
            return Animate(element, prop, anim, durationMS, null, null, callbackFunc, durationFunc);
        }

        #endregion

        #region DoubleAnimation Helpers


        public static AnimationClock AnimateDouble(
            DependencyObject element,
            DependencyProperty prop,
            double? from,
            double to,
            int durationMS,
            double? accel,
            double? decel,
            EventHandler callbackFunc,
            EventHandler durationFunc)
        {
            double defaultFrom = double.IsNaN((double)element.GetValue(prop)) ?
                                 0 :
                                 (double)element.GetValue(prop);

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = from.GetValueOrDefault(defaultFrom);
            anim.To = to;

            return Animate(element, prop, anim, durationMS, null, null, callbackFunc, durationFunc);
        }

        #endregion

        /// <summary>
        /// Method to configure and start an animation.
        /// </summary>
        private static AnimationClock Animate(
            DependencyObject animatable,
            DependencyProperty prop,
            AnimationTimeline anim,
            int duration,
            double? accel,
            double? decel,
            EventHandler func,
            EventHandler durationfunc
            )
        {
            anim.AccelerationRatio = accel.GetValueOrDefault(0);
            anim.DecelerationRatio = decel.GetValueOrDefault(0);
            anim.Duration = TimeSpan.FromMilliseconds(duration);
            anim.Freeze();

            AnimationClock animClock = anim.CreateClock();

            // When animation is complete, remove animation and set the animation's "To" 
            // value as the new value of the property.
            EventHandler eh = null;
            eh = delegate(object sender, EventArgs e)
            {
                animatable.SetValue(prop, animatable.GetValue(prop));

                ((IAnimatable)animatable).ApplyAnimationClock(prop, null);

                animClock.Completed -= eh;
                
            };

            animClock.Completed += eh;

            // assign completed eventHandler, if defined
            if (func != null)
            {
                animClock.Completed += func;
            }
            else
            {
                durationfunc(null,null);
            }

            animClock.Controller.Begin();

            // goferit
            ((IAnimatable)animatable).ApplyAnimationClock(prop, animClock);

            return animClock;
        }
    }
}
