using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace CornControls.CustomControl
{
    public class AnimatedContentControlOpacity : AnimatedContentControl
    {
        protected override void BeginAnimateContentReplacement()
        {
            m_paintArea.Visibility = Visibility.Visible;

            m_mainContent.BeginAnimation(ContentPresenter.OpacityProperty, CreateAnimation(0, 1, 0.0, EasingMode.EaseInOut));
            m_paintArea.BeginAnimation(Shape.OpacityProperty, CreateAnimation(1, 0, 0.0, EasingMode.EaseInOut, (s, e) => m_paintArea.Visibility = Visibility.Hidden));
        }
    }
}
