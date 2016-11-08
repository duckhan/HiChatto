using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace HiChatto.Universal.View.Behaviors
{
    public class AutoScrollBehavior:Behavior<ScrollViewer>
    {
        private double height=0;
        private ScrollViewer scroll=null;
        protected override void OnAttached()
        {
            base.OnAttached();
           scroll = base.AssociatedObject;
            scroll.LayoutUpdated += Scroll_LayoutUpdated;
        }

        private void Scroll_LayoutUpdated(object sender, object e)
        {
            if (Math.Abs(height - scroll.ExtentHeight) > 1)
            {
                scroll.ChangeView(null,scroll.ExtentHeight,null);
                height = scroll.ExtentHeight;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

    }
}
